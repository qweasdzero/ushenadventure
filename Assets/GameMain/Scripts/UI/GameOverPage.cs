using GameFramework;
using UnityEngine;

namespace StarForce
{
    public class GameOverPageModel : UGuiFormModel<GameOverPage, GameOverPageModel>
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

        #region HighestScoreProperty

        private readonly Property<string> _privateHighestScoreProperty = new Property<string>();

        public Property<string> HighestScoreProperty
        {
            get { return _privateHighestScoreProperty; }
        }

        public string HighestScore
        {
            get { return _privateHighestScoreProperty.GetValue(); }
            set { _privateHighestScoreProperty.SetValue(value); }
        }

        #endregion

        public void GameOver()
        {
            Page.GameOver();
        }
    }

    public class GameOverPage : UGuiFormPage<GameOverPage, GameOverPageModel>
    {
        [SerializeField] private GameObject m_ReturnMenu;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Model.Score = GameEntry.Achieve.Score;
            if (Model.Score > GameEntry.Achieve.HighestScore)
            {
                GameEntry.Achieve.HighestScore = Model.Score;
            }

            Model.HighestScore = "最佳 " + GameEntry.Achieve.HighestScore;
            GameEntry.Achieve.EventSystem.SetSelectedGameObject(m_ReturnMenu);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public void GameOver()
        {
            GameEntry.Event.Fire(this, ReferencePool.Acquire<BackToMenuEventArgs>().Fill());
        }
    }
}