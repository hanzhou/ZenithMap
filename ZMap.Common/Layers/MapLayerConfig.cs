using System;

namespace ZMap
{
    public struct MapLayerConfig
    {
        public MapLayerConfig(int minlevel, int maxlevel, double minzoomrate, double maxzoomrate, MapType maptype)
        {
            isVisible = true;
            minVisible = 0;
            maxVisible = Double.MaxValue;
            minLevel = minlevel;
            maxLevel = maxlevel;
            minZoomRate = minzoomrate;
            maxZoomRate = maxzoomrate;
            zIndex = 0;
            mapType = maptype;
        }

        public static MapLayerConfig DefaultConfig
        {
            get { return new MapLayerConfig(0, 0, 0, Double.MaxValue, MapType.Invalid); }
        }

        private bool isVisible;
        /// <summary>
        /// 获取或设置图层是否显示
        /// </summary>
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = true; }
        }

        private double minVisible;
        /// <summary>
        /// 图层可显示的最小比例，比例是当前大小与最小地图(Level = 0, ZoomRate = 1)的比值
        /// </summary>
        public double MinVisible
        {
            get { return minVisible; }
            set { minVisible = value; }
        }

        private double maxVisible;
        /// <summary>
        /// 图层可显示的最大比例，比例是当前大小与最小地图(Level = 0, ZoomRate = 1)的比值
        /// </summary>
        public double MaxVisible
        {
            get { return maxVisible; }
            set { maxVisible = value; }
        }

        private int minLevel;
        public int MinLevel
        {
            get { return minLevel; }
            set { minLevel = value; }
        }

        private int maxLevel;
        public int MaxLevel
        {
            get { return maxLevel; }
            set { maxLevel = value; }
        }

        private double minZoomRate;
        public double MinZoomRate
        {
            get { return minZoomRate; }
            set { minZoomRate = value; }
        }

        private double maxZoomRate;
        public double MaxZoomRate
        {
            get { return maxZoomRate; }
            set { maxZoomRate = value; }
        }

        private int zIndex;
        public int ZIndex
        {
            get { return zIndex; }
            set { zIndex = value; }
        }

        private MapType mapType;
        public MapType MapType
        {
            get { return mapType; }
            set { mapType = value; }
        }
    }
}
