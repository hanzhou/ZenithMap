using System;

namespace ZMap
{
    /// <summary>
    /// 用来表示经纬度的度分秒
    /// </summary>
    public struct DegMinSec
    {
        private int degrees;
        private int minites;
        private double seconds;

        public DegMinSec(int degrees, int minites, double seconds)
        {
            this.degrees = degrees;
            this.minites = minites;
            this.seconds = seconds;
        }

        public static double DMStoDecimal(DegMinSec dms)
        {
            return dms.Degrees + dms.Minites / 60.0 + dms.Seconds / 3600.0;
        }

        public static DegMinSec DecimaltoDMS(double dec)
        {
            int deg = (int)dec;
            double ms = (dec - deg) * 60.0;
            int min = (int)ms;
            return new DegMinSec(deg, min, (ms - min) * 60.0);
        }

        public int Degrees
        {
            get { return this.degrees; }
            set { this.degrees = value; }
        }

        public int Minites
        {
            get { return this.minites; }
            set { this.minites = value; }
        }

        public double Seconds
        {
            get { return this.seconds; }
            set { this.seconds = value; }
        }

        public override string ToString()
        {
            return String.Format("{0}°{1}'{2}\"", degrees, minites, seconds);
        }
    }
}
