//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace StarForce
{
    public struct MapGridInfo
    {
        public int GridId { get; set; }

        public bool Left { get; set; }

        public MapGridInfo(int gridId, bool left)
        {
            GridId = gridId;
            Left = left;
        }
    }
}