using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ZMap
{
    public struct RawTile : IEquatable<RawTile>
    {
        private static string regextext = ",";

        public MapType Type;
        public PointInt TileXY;
        public int Level;

        public RawTile(MapType type, PointInt tileXY, int level)
        {
            this.Type = type;
            this.TileXY = tileXY;
            this.Level = level;
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(Type.ToString())
                .Append(regextext).Append(Level)
                .Append(regextext).Append(TileXY.X)
                .Append(regextext).Append(TileXY.Y);
            return ret.ToString();
        }

        public static RawTile FromString(string tile)
        {
            string[] info = Regex.Split(tile, regextext);
            MapType type = (MapType)Enum.Parse(typeof(MapType), info[0]);
            int level = Convert.ToInt32(info[1]);
            int x = Convert.ToInt32(info[2]);
            int y = Convert.ToInt32(info[3]);
            return new RawTile(type, new PointInt(x, y), level);
        }

        #region IEquatable<RawTile> Members

        public bool Equals(RawTile other)
        {
            return this.Type == other.Type
                && this.Level == other.Level
                && this.TileXY == other.TileXY;
        }

        #endregion
    }
}
