using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 开始游戏事件。
    /// </summary>
    public sealed class PlayerDieEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerDieEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public override void Clear()
        {
        }

        public PlayerDieEventArgs Fill()
        {
            return this;
        }
    }
}