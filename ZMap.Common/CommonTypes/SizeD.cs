using System;
using System.Globalization;

namespace ZMap
{
    public struct SizeD
    {
        public static readonly SizeD Empty = new SizeD();

        private double width;
        private double height;

        public SizeD(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        public static SizeD operator +(SizeD s1, SizeD s2)
        {
            return SizeD.Add(s1, s2);
        }

        public static SizeD operator -(SizeD s1, SizeD s2)
        {
            return SizeD.Sub(s1, s2);
        }

        public static bool operator ==(SizeD s1, SizeD s2)
        {
            return s1.Width == s2.Width && s1.Height == s2.Height;
        }

        public static bool operator !=(SizeD s1, SizeD s2)
        {
            return !(s1 == s2);
        }

        public static SizeD Add(SizeD s1, SizeD s2)
        {
            return new SizeD(s1.Width + s2.Width, s1.Height + s2.Height);
        }

        public static SizeD Sub(SizeD s1, SizeD s2)
        {
            return new SizeD(s1.Width - s2.Width, s1.Height - s2.Height);
        }

        public bool IsEmpty
        {
            get { return width == 0 && height == 0; }
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

        #region overrides

        public override bool Equals(object obj)
        {
            if (!(obj is SizeD))
                return false;
            SizeD comp = (SizeD)obj;
            return (comp.width == this.width) && (comp.height == this.height);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "{Width=" + width.ToString(CultureInfo.CurrentCulture) + ", Height=" + height.ToString(CultureInfo.CurrentCulture) + "}";
        }

        #endregion
    }
}
