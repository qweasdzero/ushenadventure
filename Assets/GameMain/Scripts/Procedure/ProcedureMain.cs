using System.Collections.Generic;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 0f;

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private float m_GotoMenuDelaySeconds = 0f;

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Games.Add(GameMode.Test, new TestGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
//            GameMode gameMode = (GameMode)procedureOwner.GetData<VarInt>(Constant.ProcedureData.GameMode).Value;
            m_CurrentGame = m_Games[GameMode.Test];
            m_CurrentGame.Initialize();
            GameEntry.UI.OpenUIForm(UIFormId.MainPage);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                return;
            }

            m_GotoMenuDelaySeconds += elapseSeconds;
            if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId,
                    GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
    }
}