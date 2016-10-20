
namespace ZMap
{
    public struct MapLocation
    {
        public PointInt Position;
        public int Level;

        public MapLocation(PointInt position, int level)
        {
            Position = position;
            Level = level;
        }
    }
}
