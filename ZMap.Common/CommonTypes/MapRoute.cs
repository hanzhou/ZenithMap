using System.Collections.Generic;

namespace ZMap
{
    public class MapRoute : List<PointInt>
    {
        private int level;
        
        public MapRoute(int level)
        {
            this.level = level;
        }

        public MapRoute(IEnumerable<PointInt> route, int level)
            : base(route)
        {
            this.level = level;
        }

        public int Level
        {
            get { return level; }
        }

        #region overrides

        public override bool Equals(object obj)
        {
            if (!(obj is MapRoute))
                return false;
            MapRoute route = (MapRoute)obj;
            if (route.Level != Level)
                return false;
            for (int i = 0; i < this.Count; i++)
                if (route[i] != this[i])
                    return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
