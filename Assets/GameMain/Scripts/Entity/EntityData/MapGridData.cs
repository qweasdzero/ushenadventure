using UnityEngine;

namespace StarForce
{
    [SelectionBase]
    public class MapGridData : EntityData
    {
        private bool m_Left;
        private bool m_Down;
        private float m_DownTime;
        public bool ChildEntity;
        public int FatherId;

        public bool Left
        {
            get { return m_Left; }
            set { m_Left = value; }
        }


        public bool Down
        {
            get { return m_Down; }
            set { m_Down = value; }
        }

        public float DownTime
        {
            get { return m_DownTime; }
            set { m_DownTime = value; }
        }

        public MapGridData(int entityId, int typeId,int fatherId,bool left,bool childEntity=false) : base(entityId, typeId)
        {
            ChildEntity = childEntity;
            Left = left;
            FatherId = fatherId;
        }
    }
}

