
namespace ZMap.WPFControl
{
    public class Map : MapBase
    {

        public PointInt GetCurrentPosition(PointD pos)
        {
            return new PointInt((int)(mapViewRect.Left + pos.X / ZoomRate), (int)(mapViewRect.Top + pos.Y / ZoomRate));
        }
    }
}
