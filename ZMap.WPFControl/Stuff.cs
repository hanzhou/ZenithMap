using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZMap.WPFControl
{
    public static class Stuff
    {
        public static PointD ToPointD(this System.Windows.Point point)
        {
            return new PointD(point.X, point.Y);
        }

        public static System.Windows.Point ToWindowsPoint(this PointD point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }
    }
}
