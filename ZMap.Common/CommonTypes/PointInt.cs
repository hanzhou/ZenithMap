using System;
using System.Globalization;

namespace ZMap
{
    public struct PointInt : IEquatable<PointInt>
    {
        public static readonly PointInt Empty = new PointInt(0, 0);

        private int x;
        private int y;

        public PointInt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsEmpty
        {
            get { return x == 0 && y == 0; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public static bool operator ==(PointInt left, PointInt right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(PointInt left, PointInt right)
        {
            return !(left == right);
        }

        public PointInt Offset(int offsetX, int offsetY)
        {
            this.x += offsetX;
            this.y += offsetY;
            return this;
        }

        #region overrides

        public override bool Equals(object obj)
        {
            if (!(obj is PointInt))
                return false;
            PointInt comp = (PointInt)obj;
            return comp.X == this.X && comp.Y == this.Y;
        }

        public override int GetHashCode()
        {
            return (int)x ^ (int)y;
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ", Y=" + Y.ToString(CultureInfo.CurrentCulture) + "}";
        }

        #endregion

        #region IEquatable<PointInt> Members

        public bool Equals(PointInt other)
        {
            return this == other;
        }

        #endregion
    }
}
