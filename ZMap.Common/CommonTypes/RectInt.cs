using System;
using System.Globalization;

namespace ZMap
{
    public struct RectInt
    {
        public static readonly RectInt Empty = new RectInt();

        private int x;
        private int y;
        private int width;
        private int height;

        public RectInt(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public RectInt(PointInt location, SizeInt size)
        {
            this.x = location.X;
            this.y = location.Y;
            this.width = size.Width;
            this.height = size.Height;
        }

        public static bool operator ==(RectInt left, RectInt right)
        {
            return (left.X == right.X
                       && left.Y == right.Y
                       && left.Width == right.Width
                       && left.Height == right.Height);
        }

        public static bool operator !=(RectInt left, RectInt right)
        {
            return !(left == right);
        }

        #region methods

        public bool Contains(int x, int y)
        {
            return this.X <= x &&
               x < this.X + this.Width &&
               this.Y <= y &&
               y < this.Y + this.Height;
        }

        public bool Contains(PointInt pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(RectInt rect)
        {
            return (this.X <= rect.X) &&
               ((rect.X + rect.Width) <= (this.X + this.Width)) &&
               (this.Y <= rect.Y) &&
               ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        /// <summary>
        /// 求交集
        /// </summary>
        /// <param name="rect"></param>
        public void Intersect(RectInt rect)
        {
            RectInt result = RectInt.GetIntersect(rect, this);
            this.X = result.X;
            this.Y = result.Y;
            this.Width = result.Width;
            this.Height = result.Height;
        }

        /// <summary>
        /// 是否有公共区域
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool HasIntersectsWith(RectInt rect)
        {
            return rect.X < this.X + this.Width &&
               this.X < rect.X + rect.Width &&
               rect.Y < this.Y + this.Height &&
               this.Y < rect.Y + rect.Height;
        }

        #endregion

        #region public properties

        /// <summary>
        /// get or set the top left point of the rectangle or the Position of the rectangle
        /// </summary>
        public PointInt TopLeft
        {
            get
            {
                return new PointInt(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        /// <summary>
        /// get the top left point of the rectangle
        /// </summary>
        public PointInt BottomRight
        {
            get
            {
                return new PointInt(x + width, y + height);
            }
        }

        /// <summary>
        /// get or set the size of the rectangle
        /// </summary>
        public SizeInt Size
        {
            get
            {
                return new SizeInt(width, height);
            }
            set
            {
                width = value.Width;
                height = value.Height;
            }
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

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Left
        {
            get
            {
                return x;
            }
        }

        public int Top
        {
            get
            {
                return y;
            }
        }

        public int Right
        {
            get
            {
                return x + width;
            }
        }

        public int Bottom
        {
            get
            {
                return x + height;
            }
        }

        public PointInt Center
        {
            get { return new PointInt(x + width / 2, y + height / 2); }
        }

        public bool IsEmpty
        {
            get
            {
                return height == 0 || width == 0;
            }
        }

        #endregion

        #region static methods

        public static RectInt FromLTRB(int left, int top, int right, int bottom)
        {
            return new RectInt(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// 获取指定的两个 RectU 的公共区域
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public static RectInt GetIntersect(RectInt rect1, RectInt rect2)
        {
            int left = Math.Max(rect1.X, rect2.X);
            int right = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width);
            int top = Math.Max(rect1.Y, rect2.Y);
            int bottom = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height);
            if (right > left && bottom > top)
                return RectInt.FromLTRB(left, top, right, bottom);
            return RectInt.Empty;
        }

        /// <summary>
        /// 获取包含指定的两个 Rect 的最小 Rect
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public static RectInt GetUnionRect(RectInt rect1, RectInt rect2)
        {
            int left = Math.Min(rect1.X, rect2.X);
            int right = Math.Max(rect1.X + rect1.Width, rect2.X + rect2.Width);
            int top = Math.Min(rect1.Y, rect2.Y);
            int bottom = Math.Max(rect1.Y + rect1.Height, rect2.Y + rect2.Height);
            return RectInt.FromLTRB(left, top, right, bottom);
        }

        #endregion

        #region overrides

        public override bool Equals(object obj)
        {
            if (!(obj is RectInt))
                return false;
            RectInt comp = (RectInt)obj;
            return comp.X == x && comp.Y == y && comp.Width == width && comp.Height == height;
        }

        public override int GetHashCode()
        {
            return (int)((UInt32)X ^
                (((UInt32)Y << 13) | ((UInt32)Y >> 19)) ^
                (((UInt32)Width << 26) | ((UInt32)Width >> 6)) ^
                (((UInt32)Height << 7) | ((UInt32)Height >> 25)));
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ", Y=" + Y.ToString(CultureInfo.CurrentCulture) +
               ", Width=" + Width.ToString(CultureInfo.CurrentCulture) +
               ", Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }

        #endregion
    }
}
