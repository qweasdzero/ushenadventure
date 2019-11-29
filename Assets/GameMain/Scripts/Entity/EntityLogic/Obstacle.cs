using System.Collections.Generic;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.U2D;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class Obstacle : Entity
    {
        [SerializeField] private ObstacleData m_obstacleData;
        private SpriteRenderer m_Renderer;
        private List<string> m_MapGrid;
        private LoadAssetCallbacks m_LoadAssetCallbacks;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Renderer = GetComponent<SpriteRenderer>();
            m_MapGrid = new List<string>()
            {
                "Obstacle_0", "Obstacle_1", "Obstacle_2", "Obstacle_3", "Obstacle_5", "Obstacle_6",
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

            m_Renderer.sprite = spriteAtlas.GetSprite(m_MapGrid[GameEntry.Achieve.RandomObstacle()]);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_obstacleData = userData as ObstacleData;
            if (m_obstacleData == null)
            {
                Log.Error("data is invalid");
                return;
            }

            GameEntry.Resource.LoadAsset("Assets/GameMain/Textures/Obstacle.spriteatlas", m_LoadAssetCallbacks);
            GameEntry.Entity.AttachEntity(this, m_obstacleData.FatherId);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            Vector3 vector3;

            if (m_obstacleData.Left)
            {
                vector3 = new Vector3(0.5f, 0.5f);
                m_Renderer.sortingOrder = -1000;
            }
            else
            {
                vector3 = new Vector3(-0.5f, 0.5f);
                m_Renderer.sortingOrder = -1000;
            }

            m_obstacleData.Position = vector3;
            CachedTransform.localPosition = m_obstacleData.Position;
        }
    }
}