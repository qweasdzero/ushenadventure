using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 开始游戏事件。
    /// </summary>
    public sealed class MapDownEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(MapDownEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public int MapId;
        public float Time;

        public override void Clear()
        {
        }

        public MapDownEventArgs Fill(int mapId, float time)
        {
            MapId = mapId;
            Time = time;
            return this;
        }
    }
}