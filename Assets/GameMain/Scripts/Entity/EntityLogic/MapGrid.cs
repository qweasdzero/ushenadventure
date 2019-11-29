using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.U2D;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class MapGrid : Entity
    {
        [SerializeField] private MapGridData m_MapGridData;
        private SpriteRenderer m_Renderer;
        private Obstacle m_Obstacle;
        private LoadAssetCallbacks m_LoadAssetCallbacks;
        private List<string> m_MapGrid;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Renderer = GetComponent<SpriteRenderer>();
            m_MapGrid = new List<string>()
            {
                "fire", "grass", "ice", "normal",
            };
            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailure);
        }

        private void OnLoadAssetFailure(string assetname, LoadResourceStatus status, string errormessage,
            object userdata)
        {
            Log.Error(status + ":" + errormessage);
        }

        private void OnLoadAssetSuccess(string assetname, object asset, float duration, object userdata)
        {
            SpriteAtlas spriteAtlas = asset as SpriteAtlas;
            if (spriteAtlas == null)
            {
                return;
            }

            m_Renderer.sprite = spriteAtlas.GetSprite(m_MapGrid[GameEntry.Achieve.MapGridId]);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_MapGridData = userData as MapGridData;
            if (m_MapGridData == null)
            {
                Log.Error("data is invalid");
                return;
            }

//            m_Renderer.sprite = GameEntry.Achieve.MapGrid;
            m_Renderer.sortingOrder = Id;
            if (m_MapGridData.ChildEntity)
            {
                GameEntry.Entity.ShowObstacle(new ObstacleData(GameEntry.Entity.GenerateSerialId(), 21,
                    m_MapGridData.Left, Id));
            }

            GameEntry.Resource.LoadAsset("Assets/GameMain/Textures/MapGrid.spriteatlas", m_LoadAssetCallbacks);

            GameEntry.Entity.AttachEntity(this, m_MapGridData.FatherId);
            GameEntry.Event.Subscribe(MapDownEventArgs.EventId, OnMapDown);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(MapDownEventArgs.EventId, OnMapDown);
        }

        private void OnMapDown(object sender, GameEventArgs e)
        {
            MapDownEventArgs ne = e as MapDownEventArgs;
            if (ne == null)
            {
                return;
            }

            if (ne.MapId == Id)
            {
                m_MapGridData.DownTime = ne.Time;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_MapGridData.DownTime > float.Epsilon)
            {
                m_MapGridData.DownTime -= realElapseSeconds;
                if (m_MapGridData.DownTime < float.Epsilon)
                {
                    m_MapGridData.Down = true;
                    GameEntry.Event.Fire(this, ReferencePool.Acquire<GridDownEventArgs>().Fill(Id));
                }
            }

            if (m_MapGridData.Down)
            {
                m_MapGridData.Position += Vector3.down * 5 * realElapseSeconds;
                if (m_MapGridData.Position.y < -5)
                {
                    GameEntry.Entity.HideEntity(this);
                }

                return;
            }
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            if (childEntity is Obstacle)
            {
                m_Obstacle = (Obstacle) childEntity;
            }
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
            if (childEntity is Obstacle)
            {
                m_Obstacle = null;
            }
        }
    }
}