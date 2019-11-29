using UnityEngine;
using UnityGameFramework.Runtime;
using DG.Tweening;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine.U2D;

namespace StarForce
{
    public class Player : Entity
    {
        private PlayerData m_PlayerData;
        private SpriteRenderer m_Renderer;
        private Tweener m_Tweener1;
        private Tweener m_Tweener2;
//        private LoadAssetCallbacks m_LoadAssetCallbacks;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Renderer = GetComponent<SpriteRenderer>();
//            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailure);
        }

//        private void OnLoadAssetFailure(string assetname, LoadResourceStatus status, string errormessage,
//            object userdata)
//        {
//            Log.Error(status + ":" + errormessage);
//        }
//
//        private void OnLoadAssetSuccess(string assetname, object asset, float duration, object userdata)
//        {
//            SpriteAtlas spriteAtlas = asset as SpriteAtlas;
//            if (spriteAtlas == null)
//            {
//                return;
//            }
//
//            m_Renderer.sprite = spriteAtlas.GetSprite("characters_0a");
//        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("data is invalid");
                return;
            }

            m_Renderer.sortingOrder = 0;
            m_Tweener1 = DOTween.To(() => m_PlayerData.Position, x => m_PlayerData.Position = x, Vector3.up, 0.2f)
                .SetRelative().SetAutoKill(false).Pause();
            m_Tweener2 = DOTween.To(() => m_PlayerData.Position, x => m_PlayerData.Position = x, -Vector3.up, 0.2f)
                .SetRelative().SetAutoKill(false).Pause();
            m_Tweener1.OnComplete(() => { m_Tweener2.Restart(); });
            m_Tweener2.OnComplete(() =>
            {
                m_PlayerData.Jump = false;
                GameEntry.Event.Fire(this, ReferencePool.Acquire<ShowMapEventArgs>().Fill());
            });

//            GameEntry.Resource.LoadAsset("Assets/GameMain/Textures/Characters.spriteatlas", m_LoadAssetCallbacks);
            GameEntry.Event.Subscribe(PlayerDieEventArgs.EventId, OnDie);
        }

        private void OnDie(object sender, GameEventArgs e)
        {
            PlayerDieEventArgs ne = e as PlayerDieEventArgs;
            if (ne == null)
            {
                return;
            }

            m_PlayerData.Die = true;
            m_Renderer.sortingOrder = -1000;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(PlayerDieEventArgs.EventId, OnDie);
            m_Tweener1.Kill();
            m_Tweener2.Kill();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_PlayerData.Die)
            {
                m_PlayerData.Position += Vector3.down * 5 * realElapseSeconds;
                if (m_PlayerData.Position.y < -5)
                {
                    GameEntry.Event.Fire(this, ReferencePool.Acquire<GameOverEventArgs>().Fill());
                }

                return;
            }

            if (!m_PlayerData.Jump)
            {
                int left = 0;
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    left = 2;
                }

                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    left = 1;
                }

                if (left != 0)
                {
                    Jump(left == 1);
                }
            }
        }

        private void Jump(bool left)
        {
            m_PlayerData.Scale = new Vector3(left ? -1 : 1, 1, 1);
            CachedTransform.localScale = m_PlayerData.Scale;
            m_PlayerData.Jump = true;
            m_Tweener1.Restart();
            GameEntry.Achieve.Score += 1;
            GameEntry.Event.Fire(this, ReferencePool.Acquire<JumpEventArgs>().Fill(left));
        }
    }
}