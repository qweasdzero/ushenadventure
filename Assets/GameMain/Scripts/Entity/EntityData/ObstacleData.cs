using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace StarForce
{
    [SelectionBase]
    public class ObstacleData : EntityData
    {
        public bool Left;
        public int FatherId;

        public ObstacleData(int entityId, int typeId, bool left,int fatherId) : base(entityId, typeId)
        {
            Left = left;
            FatherId = fatherId;
        }
    }
}