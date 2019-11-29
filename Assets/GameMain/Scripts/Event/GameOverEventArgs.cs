using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 游戏结束事件。
    /// </summary>
    public sealed class GameOverEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameOverEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public override void Clear()
        {
        }

        public GameOverEventArgs Fill()
        {
            return this;
        }
    }
}