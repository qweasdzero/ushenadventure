using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 生成地图块。
    /// </summary>
    public sealed class ShowMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowMapEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public override void Clear()
        {
        }

        public ShowMapEventArgs Fill()
        {
            return this;
        }
    }
}