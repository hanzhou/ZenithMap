using System;
using System.Windows.Media;

namespace ZMap
{
    public abstract class MapLayer : Layer, IMapLayer
    {
        #region fields

        private MapType mapType = MapType.Invalid;
        private int minLevel = 0;
        private int maxLevel = 0;
        private double minZoomRate = 0;
        private double maxZoomRate = Double.MaxValue;

        private event MapTypeChanged mapTypeChanged;

        #endregion

        protected virtual void OnMapTypeChanged(MapType newType)
        {
            MapTypeChanged temp = mapTypeChanged;
            if (temp != null) temp(newType);
        }

        public void InitMapLayer(MapLayerConfig config)
        {
            this.IsVisible = config.IsVisible;
            this.MapType = config.MapType;
            this.MaxLevel = config.MaxLevel;
            this.MaxVisible = config.MaxVisible;
            this.MaxZoomRate = config.MaxZoomRate;
            this.MinLevel = config.MinLevel;
            this.MinVisible = config.MinVisible;
            this.MinZoomRate = config.MinZoomRate;
            this.ZIndex = config.ZIndex;
        }

        public static MapLayer Null
        {
            get { return new NullMapLayer(); }
        }

        #region IMapLayer Members

        public MapType MapType
        {
            get { return mapType; }
            set
            {
                mapType = value;
                OnMapTypeChanged(value);
            }
        }

        public int MinLevel
        {
            get { return minLevel; }
            protected set { minLevel = value; }
        }

        public int MaxLevel
        {
            get { return maxLevel; }
            protected set { maxLevel = value; }
        }

        public double MinZoomRate
        {
            get { return minZoomRate; }
            protected set { minZoomRate = value; }
        }

        public double MaxZoomRate
        {
            get { return maxZoomRate; }
            protected set { maxZoomRate = value; }
        }

        public event MapTypeChanged MapTypeChanged
        {
            add { lock (eventLock) { mapTypeChanged += value; } }
            remove { lock (eventLock) { mapTypeChanged -= value; } }
        }

        public abstract void OnUpdateMap(MapArea viewarea);

        #endregion
    }

    public class NullMapLayer : MapLayer
    {
        public override void OnUpdateMap(MapArea viewarea) { }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate) { }
    }
}
