using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class Map : Entity
    {
        [SerializeField] private MapData m_mapData;


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_mapData = userData as MapData;
            if (m_mapData == null)
            {
                Log.Error("data is invalid");
                return;
            }

            m_mapData.Target = m_mapData.Position.y;

            GameEntry.Event.Subscribe(JumpEventArgs.EventId, OnJump);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(JumpEventArgs.EventId, OnJump);
        }

        private void OnJump(object sender, GameEventArgs e)
        {
            JumpEventArgs ne = e as JumpEventArgs;
            if (ne == null)
            {
                return;
            }

            m_mapData.Move = true;
            m_mapData.Left = ne.Left;
            m_mapData.Target -= 0.5f;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_mapData.Move)
            {
                m_mapData.Position += new Vector3(m_mapData.Left ? 0.5f : -0.5f, -0.5f) * 5 * realElapseSeconds;
                if (m_mapData.Position.y <= m_mapData.Target)
                {
                    m_mapData.Move = false;
                }
            }
        }
    }
}