using System.Collections.Generic;

namespace ZMap
{
    public class GeoRoute : List<PointLatLng>
    {
        public GeoRoute()
        { }

        public GeoRoute(IEnumerable<PointLatLng> route)
            : base(route)
        { }

        /// <summary>
        /// route start point
        /// </summary>
        public PointLatLng? From
        {
            get
            {
                if (this.Count > 0) return this[0];
                else return null;
            }
        }

        /// <summary>
        /// route end point
        /// </summary>
        public PointLatLng? To
        {
            get
            {
                if (this.Count > 1) return this[this.Count - 1];
                else return null;
            }
        }

        /// <summary>
        /// route distance (in km)
        /// </summary>
        public double Distance
        {
            get
            {
                double distance = 0;
                if (From.HasValue && To.HasValue)
                    for (int i = 1; i < this.Count; i++)
                        distance += MapHelper.GetDistance(this[i - 1], this[i]);
                return distance;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GeoRoute))
                return false;
            GeoRoute route = (GeoRoute)obj;
            for (int i = 0; i < this.Count; i++)
                if (route[i] != this[i])
                    return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
