using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class TestGame : GameBase
    {
        public override GameMode GameMode
        {
            get { return GameMode.Test; }
        }

        private Vector2 m_Lastpos;
        private bool m_Left;
        private int m_NextCount;
        private List<MapGridInfo> m_MapId;
        private int m_Seat;
        private List<float> m_Level;
        private bool m_GameStart;
        private int m_MapFatherId;

        public TestGame()
        {
            m_Level = new List<float>() {2, 1.5f, 1f, 0.5f};
        }

        public override void Initialize()
        {
            base.Initialize();
            InitData();
            GameEntry.Event.Subscribe(JumpEventArgs.EventId, OnJump);
            GameEntry.Event.Subscribe(ShowMapEventArgs.EventId, OnShowMap);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId, OnOver);
            GameEntry.Event.Subscribe(BackToMenuEventArgs.EventId, OnBackToMenu);
            GameEntry.Event.Subscribe(GridDownEventArgs.EventId, OnGridDown);
        }

        private void InitData()
        {
            m_Left = false;
            m_Lastpos = new Vector2(0, -1.5f);
            m_MapId = new List<MapGridInfo>();
            m_NextCount = 0;
            m_Seat = 0;
            m_GameStart = false;
            GameEntry.Achieve.RandomMap();
            GameEntry.Achieve.Score = 0;
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 11)
            {
                Position = new Vector2(0, -1),
            });
            m_MapFatherId = GameEntry.Entity.GenerateSerialId();
            GameEntry.Entity.ShowMap(new MapData(m_MapFatherId, 22));
            int Id = GameEntry.Entity.GenerateSerialId();
            m_MapId.Add(new MapGridInfo(Id, m_Left));
            GameEntry.Entity.ShowMapGrid(new MapGridData(Id, 20, m_MapFatherId, m_Left)
            {
                Position = m_Lastpos,
            });
            for (int i = 0; i < 10; i++)
            {
                InitMap();
            }
        }

        private void InitMap()
        {
            if (m_NextCount > 0)
            {
                m_NextCount--;
            }
            else
            {
                m_Left = !m_Left;
                m_NextCount = GameFramework.Utility.Random.GetRandom(3, 6);
            }

            m_Lastpos += new Vector2(m_Left ? -0.5f : 0.5f, 0.5f);
            int Id = GameEntry.Entity.GenerateSerialId();
            m_MapId.Add(new MapGridInfo(Id, m_Left));
            GameEntry.Entity.ShowMapGrid(new MapGridData(Id, 20, m_MapFatherId, m_Left)
            {
                Position = m_Lastpos,
            });
        }

        private void ShowMap()
        {
            int Id = GameEntry.Entity.GenerateSerialId();
            bool haschild = false;
            if (m_NextCount > 0)
            {
                m_NextCount--;
                if (m_NextCount > 1 && Utility.Random.GetRandom(0, 100) > 80)
                {
                    haschild = true;
                }
            }
            else
            {
                m_Left = !m_Left;
                m_NextCount = GameFramework.Utility.Random.GetRandom(3, 6);
            }

            m_Lastpos += new Vector2(m_Left ? -0.5f : 0.5f, 0.5f);

            m_MapId.Add(new MapGridInfo(Id, m_Left));
            GameEntry.Entity.ShowMapGrid(new MapGridData(Id, 20, m_MapFatherId, m_Left, haschild)
            {
                Position = m_Lastpos,
            });
        }

        public override void Shutdown()
        {
            base.Shutdown();
            GameEntry.Event.Unsubscribe(JumpEventArgs.EventId, OnJump);
            GameEntry.Event.Unsubscribe(ShowMapEventArgs.EventId, OnShowMap);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, OnOver);
            GameEntry.Event.Unsubscribe(BackToMenuEventArgs.EventId, OnBackToMenu);
            GameEntry.Event.Unsubscribe(GridDownEventArgs.EventId, OnGridDown);
        }

        private void OnShowMap(object sender, GameEventArgs e)
        {
            ShowMapEventArgs ne = e as ShowMapEventArgs;
            if (ne == null)
            {
                return;
            }

            ShowMap();
            float downTime;
            if (GameEntry.Achieve.Score < 20)
            {
                downTime = m_Level[0];
            }
            else if (GameEntry.Achieve.Score < 50)
            {
                downTime = m_Level[1];
            }
            else if (GameEntry.Achieve.Score < 100)
            {
                downTime = m_Level[2];
            }
            else
            {
                downTime = m_Level[3];
            }

            GameEntry.Event.Fire(this,
                ReferencePool.Acquire<MapDownEventArgs>().Fill(m_MapId[m_Seat].GridId, downTime));
        }

        private void OnBackToMenu(object sender, GameEventArgs e)
        {
            BackToMenuEventArgs ne = e as BackToMenuEventArgs;
            if (ne == null)
            {
                return;
            }

            OnGameOver();
        }

        private void OnOver(object sender, GameEventArgs e)
        {
            GameOverEventArgs ne = e as GameOverEventArgs;
            if (ne == null)
            {
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.GameOverPage);
            GameEntry.Scene.MainCamera.gameObject.SetActive(false);
        }

        private void OnJump(object sender, GameEventArgs e)
        {
            JumpEventArgs ne = e as JumpEventArgs;
            if (ne == null)
            {
                return;
            }

            if (!m_GameStart)
            {
                m_GameStart = true;
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<MapDownEventArgs>().Fill(m_MapId[0].GridId, m_Level[0]));
            }

            m_Seat += 1;

            if (m_MapId[m_Seat].Left != ne.Left)
            {
                GameEntry.Event.Fire(this, ReferencePool.Acquire<PlayerDieEventArgs>().Fill());
            }
        }


        private void OnGridDown(object sender, GameEventArgs e)
        {
            GridDownEventArgs ne = e as GridDownEventArgs;
            if (ne == null)
            {
                return;
            }

            for (int i = 0; i < m_MapId.Count; i++)
            {
                if (ne.GridId == m_MapId[i].GridId)
                {
                    if (i == m_Seat)
                    {
                        GameEntry.Event.Fire(this, ReferencePool.Acquire<PlayerDieEventArgs>().Fill());
                    }

                    return;
                }
            }
        }


        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            if (!m_GameStart)
            {
                return;
            }
        }
    }
}