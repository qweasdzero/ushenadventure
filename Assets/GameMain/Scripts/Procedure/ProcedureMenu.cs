//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        public void StartGame()
        {
            m_StartGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_StartGame = false;
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(StartGameEventArgs.EventId, OnStartGame);

            GameEntry.UI.OpenUIForm(UIFormId.MenuPage, this);
        }


        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(StartGameEventArgs.EventId, OnStartGame);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId,
                    GameEntry.Config.GetInt("Scene.Main"));
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.GameMode, (int) GameMode.Test);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }
        }

        private void OnStartGame(object sender, GameEventArgs e)
        {
            StartGameEventArgs ne = (StartGameEventArgs) e;
            if (ne == null)
            {
                return;
            }

            StartGame();
        }
    }
}