using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 返回菜单。
    /// </summary>
    public sealed class BackToMenuEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BackToMenuEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public override void Clear()
        {
        }

        public BackToMenuEventArgs Fill()
        {
            return this;
        }
    }
}