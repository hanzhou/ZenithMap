
namespace ZMap
{
    public struct MapArea
    {
        public RectInt Area;
        public int Level;

        public MapArea(RectInt area, int level)
        {
            Area = area;
            Level = level;
        }

        public MapArea(PointInt point, SizeInt size, int level)
        {
            Area = new RectInt(point, size);
            Level = level;
        }
    }
}
