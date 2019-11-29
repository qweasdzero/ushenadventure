using UnityEngine;

namespace StarForce
{
    [SelectionBase]
    public class MapData : EntityData
    {
        private bool m_Move;
        private bool m_Left;
        private float m_Target;

        public bool Move
        {
            get { return m_Move; }
            set { m_Move = value; }
        }

        public bool Left
        {
            get { return m_Left; }
            set { m_Left = value; }
        }

        public float Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }


        public MapData(int entityId, int typeId) : base(entityId, typeId)
        {
        }
    }
}