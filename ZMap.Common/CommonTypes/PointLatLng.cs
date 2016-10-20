using System.Globalization;

namespace ZMap
{
    public struct PointLatLng
    {
        private double lat;
        private double lng;

        public PointLatLng(double lng, double lat)
        {
            this.lat = lat;
            this.lng = lng;
        }

        public static bool operator ==(PointLatLng left, PointLatLng right)
        {
            return ((left.Lng == right.Lng) && (left.Lat == right.Lat));
        }

        public static bool operator !=(PointLatLng left, PointLatLng right)
        {
            return !(left == right);
        }

        public void Offset(double offsetLat, double offsetLng)
        {
            this.lat += offsetLat;
            this.lng += offsetLng;
        }

        #region public properties

        /// <summary>
        /// 表示的地点是否有效
        /// </summary>
        public bool IsInvalid
        {
            get { return this.lng >= -180 && this.lng <= 180 && this.lat >= -90 && this.lat <= 90; }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat
        {
            get { return this.lat; }
            set { this.lat = value; }
        }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lng
        {
            get { return this.lng; }
            set { this.lng = value; }
        }

        /// <summary>
        /// 纬度（度分秒表示）
        /// </summary>
        public DegMinSec LatDMS
        {
            get { return DegMinSec.DecimaltoDMS(lat); }
            set { this.lat = DegMinSec.DMStoDecimal(value); }
        }

        /// <summary>
        /// 经度（度分秒表示）
        /// </summary>
        public DegMinSec LngDMS
        {
            get { return DegMinSec.DecimaltoDMS(lng); }
            set { this.lng = DegMinSec.DMStoDecimal(value); }
        }

        #endregion

        #region overrides

        public override bool Equals(object obj)
        {
            if (!(obj is PointLatLng))
                return false;
            PointLatLng tf = (PointLatLng)obj;
            return tf.Lng == this.Lng && tf.Lat == this.Lat;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Lat={0}, Lng={1}}}", this.Lat, this.Lng);
        }

        #endregion
    }
}