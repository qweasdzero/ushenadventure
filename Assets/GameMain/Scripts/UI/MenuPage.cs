using DG.Tweening;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class MenuPageModel : UGuiFormModel<MenuPage, MenuPageModel>
    {
        public void GameStart()
        {
            Page.GameStart();
        }

        #region TipsProperty

        private readonly Property<bool> _privateTipsProperty = new Property<bool>();

        public Property<bool> TipsProperty
        {
            get { return _privateTipsProperty; }
        }

        public bool Tips
        {
            get { return _privateTipsProperty.GetValue(); }
            set { _privateTipsProperty.SetValue(value); }
        }

        #endregion
    }

    public class MenuPage : UGuiFormPage<MenuPage, MenuPageModel>
    {
        [SerializeField] private RectTransform m_Star = null;
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private GameObject m_GameStart;
        private float m_TipsTime;

        public override bool FadeToAlpha
        {
            get { return false; }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Model.Tips = true;
            m_TipsTime = 0;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Star.DOLocalJump(new Vector3(m_Star.localPosition.x, m_Star.localPosition.y - 24, 0), 3, 1, 2)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            GameEntry.Achieve.EventSystem.SetSelectedGameObject(m_GameStart);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            m_Star.DOKill();
            m_RectTransform.DOKill();
            m_RectTransform.anchoredPosition = Vector3.zero;
        }

        public void GameStart()
        {
            m_RectTransform.DOLocalMoveY(750f, 0.1f).OnComplete(() =>
            {
                GameEntry.Event.Fire(this, ReferencePool.Acquire<StartGameEventArgs>().Fill());
            });
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_TipsTime < 3)
            {
                m_TipsTime += realElapseSeconds;
                if (m_TipsTime > 3)
                {
                    Model.Tips = false;
                }
            }
        }
    }
}