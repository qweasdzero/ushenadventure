using GameFramework.Event;

namespace StarForce
{
    /// <summary>
    /// 砖块下落事件。
    /// </summary>
    public sealed class GridDownEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GridDownEventArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public int GridId;
        
        public override void Clear()
        {
        }

        
        public GridDownEventArgs Fill(int gridid)
        {
            GridId = gridid;
            return this;
        }
    }
}