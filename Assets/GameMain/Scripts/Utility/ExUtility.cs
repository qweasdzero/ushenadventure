//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace StarForce
{
    public static class ExUtility
    {
        public static float GetRandom(float min, float max)
        {
            return (float) (GameFramework.Utility.Random.GetRandomDouble() * (max - min) + min);
        }
    }
}