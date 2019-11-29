using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class AchieveComponent : GameFrameworkComponent
    {
        private int m_Score;
        private int m_HighestScore;
//        [SerializeField] private Sprite[] m_Sprites;
//        [SerializeField] private Sprite[] m_Obstacle;
        [SerializeField] public EventSystem EventSystem;
        public int MapGridId;

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public int HighestScore
        {
            get { return m_HighestScore; }
            set { m_HighestScore = value; }
        }

        public void RandomMap()
        {
            MapGridId = GameFramework.Utility.Random.GetRandom(0, 4);
        }

        public int RandomObstacle()
        {
            return GameFramework.Utility.Random.GetRandom(0, 5);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
            }
        }
    }
}