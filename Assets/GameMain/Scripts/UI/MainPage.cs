using DG.Tweening;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class MainPageModel : UGuiFormModel<MainPage, MainPageModel>
    {
        #region ScoreProperty

        private readonly Property<int> _privateScoreProperty = new Property<int>();

        public Property<int> ScoreProperty
        {
            get { return _privateScoreProperty; }
        }

        public int Score
        {
            get { return _privateScoreProperty.GetValue(); }
            set { _privateScoreProperty.SetValue(value); }
        }

        #endregion
    }

    public class MainPage : UGuiFormPage<MainPage, MainPageModel>
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Model.Score = GameEntry.Achieve.Score;
            GameEntry.Event.Subscribe(JumpEventArgs.EventId, OnJump);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(JumpEventArgs.EventId, OnJump);
        }

        private void OnJump(object sender, GameEventArgs e)
        {
            JumpEventArgs ne = e as JumpEventArgs;
            if (ne == null)
            {
                return;
            }

            Model.Score = GameEntry.Achieve.Score;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}