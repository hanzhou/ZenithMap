using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZMap
{
    public static partial class MapHelper
    {
        /// <summary>
        /// distance (in km) between two points specified by latitude/longitude
        /// The Haversine formula, http://www.movable-type.co.uk/scripts/latlong.html
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistance(PointLatLng p1, PointLatLng p2)
        {
            double Lat1 = p1.Lat * (Math.PI / 180);
            double Long1 = p1.Lng * (Math.PI / 180);
            double Lat2 = p2.Lat * (Math.PI / 180);
            double Long2 = p2.Lng * (Math.PI / 180);
            double dLongitude = Long2 - Long1;
            double dLatitude = Lat2 - Lat1;
            double a = Math.Pow(Math.Sin(dLatitude / 2), 2) + Math.Cos(Lat1) * Math.Cos(Lat2) * Math.Pow(Math.Sin(dLongitude / 2), 2);
            return Constants.EarthRadius * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }
    }
}
