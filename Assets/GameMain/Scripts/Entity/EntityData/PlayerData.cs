using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class PlayerData : EntityData
    {
        private bool m_Jump;
        private bool m_Die;

        public bool Jump
        {
            get { return m_Jump; }
            set { m_Jump = value; }
        }

        public bool Die
        {
            get { return m_Die; }
            set { m_Die = value; }
        }

        public PlayerData(int entityId, int typeId) : base(entityId, typeId)
        {
        }
    }
}

