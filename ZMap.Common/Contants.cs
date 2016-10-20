using System;

namespace ZMap
{
    public static class Constants
    {
        /// <summary>
        /// Radius of the Earth, in Kilometers
        /// </summary>
        public const double EarthRadius = 6378.137;// WGS-84

        /// <summary>
        /// 显示图像相对于原始图像的最大比例: sqrt(2);
        /// </summary>
        public const double MaxZoomRate = 1.4142135623730950488;
        /// <summary>
        /// 显示图像相对于原始图像的最大比例: sqrt(1/2);
        /// </summary>
        public const double MinZoomRate = 0.7071067811865475244;

        public static int RepaintTimeDelay = 40;
    }
}
