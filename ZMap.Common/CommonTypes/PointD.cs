using System.Globalization;

namespace ZMap
{
    public struct PointD
    {
        public static PointD Empty = new PointD(0, 0);
        
        private double x;
        private double y;

        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsEmpty
        {
            get { return this.y == 0d && this.x == 0d; }
        }

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public static bool operator ==(PointD left, PointD right)
        {
            return (left.Y == right.Y) && (left.X == right.X);
        }

        public static bool operator !=(PointD left, PointD right)
        {
            return !(left == right);
        }

        public PointD Offset(double offsetX, double offsetY)
        {
            this.x += offsetX;
            this.y += offsetY;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PointD))
                return false;
            PointD tf = (PointD)obj;
            return tf.Y == this.Y && tf.X == this.X;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Lat={0}, Lng={1}}}", this.X, this.Y);
        }
    }
}
