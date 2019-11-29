using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 跳跃事件。
    /// </summary>
    public sealed class JumpEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(JumpEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public bool Left;

        public override void Clear()
        {
        }

        public JumpEventArgs Fill(bool left)
        {
            Left = left;
            return this;
        }
    }
}