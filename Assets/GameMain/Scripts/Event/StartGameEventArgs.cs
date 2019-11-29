using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 开始游戏事件。
    /// </summary>
    public sealed class StartGameEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(StartGameEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public override void Clear()
        {
        }

        public StartGameEventArgs Fill()
        {
            return this;
        }
    }
}