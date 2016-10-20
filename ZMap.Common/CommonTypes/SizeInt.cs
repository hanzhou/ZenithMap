using System;
using System.Globalization;

namespace ZMap
{
    public struct SizeInt
    {
        public static readonly SizeInt Empty = new SizeInt(0, 0);

        private int width;
        private int height;

        public SizeInt(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public static SizeInt operator +(SizeInt s1, SizeInt s2)
        {
            return SizeInt.Add(s1, s2);
        }

        public static SizeInt operator -(SizeInt s1, SizeInt s2)
        {
            return SizeInt.Sub(s1, s2);
        }

        public static bool operator ==(SizeInt s1, SizeInt s2)
        {
            return s1.Width == s2.Width && s1.Height == s2.Height;
        }

        public static bool operator !=(SizeInt s1, SizeInt s2)
        {
            return !(s1 == s2);
        }

        public static SizeInt Add(SizeInt s1, SizeInt s2)
        {
            return new SizeInt(s1.Width + s2.Width, s1.Height + s2.Height);
        }

        public static SizeInt Sub(SizeInt s1, SizeInt s2)
        {
            return new SizeInt(s1.Width - s2.Width, s1.Height - s2.Height);
        }

        public bool IsEmpty
        {
            get { return width == 0 && height == 0; }
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

        #region overrides

        public override bool Equals(object obj)
        {
            if (!(obj is SizeInt))
                return false;
            SizeInt comp = (SizeInt)obj;
            return (comp.width == this.width) && (comp.height == this.height);
        }

        public override int GetHashCode()
        {
            return width ^ height;
        }

        public override string ToString()
        {
            return "{Width=" + width.ToString(CultureInfo.CurrentCulture) + ", Height=" + height.ToString(CultureInfo.CurrentCulture) + "}";
        }

        #endregion
    }
}
