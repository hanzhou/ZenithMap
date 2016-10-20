using System;
using System.Globalization;

namespace ZMap
{
    public struct RectD
    {
        public static readonly RectD Empty = new RectD();

        private double x;
        private double y;
        private double width;
        private double height;

        public RectD(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public RectD(PointD location, RectD size)
        {
            this.x = location.X;
            this.y = location.Y;
            this.width = size.Width;
            this.height = size.Height;
        }

        public static bool operator ==(RectD left, RectD right)
        {
            return (left.X == right.X
                       && left.Y == right.Y
                       && left.Width == right.Width
                       && left.Height == right.Height);
        }

        public static bool operator !=(RectD left, RectD right)
        {
            return !(left == right);
        }

        public bool Contains(double x, double y)
        {
            return this.X <= x &&
               x < this.X + this.Width &&
               this.Y <= y &&
               y < this.Y + this.Height;
        }

        public bool Contains(PointD pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(RectD rect)
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
        public void Intersect(RectD rect)
        {
            RectD result = RectD.GetIntersect(rect, this);
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
        public bool HasIntersectsWith(RectD rect)
        {
            return rect.X < this.X + this.Width &&
               this.X < rect.X + rect.Width &&
               rect.Y < this.Y + this.Height &&
               this.Y < rect.Y + rect.Height;
        }

        #region public properties

        /// <summary>
        /// get or set the top left point of the rectangle or the Position of the rectangle
        /// </summary>
        public PointD TopLeft
        {
            get
            {
                return new PointD(x, y);
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
        public PointD BottomRight
        {
            get
            {
                return new PointD((int)(x + width), (int)(y + height));
            }
        }

        /// <summary>
        /// get or set the size of the rectangle
        /// </summary>
        public SizeD Size
        {
            get
            {
                return new SizeD(width, height);
            }
            set
            {
                width = value.Width;
                height = value.Height;
            }
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public double Left
        {
            get
            {
                return x;
            }
        }

        public double Top
        {
            get
            {
                return y;
            }
        }

        public double Right
        {
            get
            {
                return (int)(x + width);
            }
        }

        public double Bottom
        {
            get
            {
                return (int)(x + height);
            }
        }

        public PointD Center
        {
            get { return new PointD(x + width / 2, y + height / 2); }
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

        public static RectD FromLTRB(double left, double top, double right, double bottom)
        {
            return new RectD(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// 获取指定的两个 RectU 的公共区域
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public static RectD GetIntersect(RectD rect1, RectD rect2)
        {
            double left = Math.Max(rect1.X, rect2.X);
            double right = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width);
            double top = Math.Max(rect1.Y, rect2.Y);
            double bottom = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height);
            if (right > left && bottom > top)
                return RectD.FromLTRB(left, top, right, bottom);
            return RectD.Empty;
        }

        /// <summary>
        /// 获取包含指定的两个 Rect 的最小 Rect
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public static RectD GetUnionRect(RectD rect1, RectD rect2)
        {
            double left = Math.Min(rect1.X, rect2.X);
            double right = Math.Max(rect1.X + rect1.Width, rect2.X + rect2.Width);
            double top = Math.Min(rect1.Y, rect2.Y);
            double bottom = Math.Max(rect1.Y + rect1.Height, rect2.Y + rect2.Height);
            return RectD.FromLTRB(left, top, right, bottom);
        }

        #endregion

        #region overrides

        public override bool Equals(object obj)
        {
            if (!(obj is RectD))
                return false;
            RectD comp = (RectD)obj;
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
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) +
                ", Y=" + Y.ToString(CultureInfo.CurrentCulture) +
                ", Width=" + Width.ToString(CultureInfo.CurrentCulture) +
                ", Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }

        #endregion
    }
}
