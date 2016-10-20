using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ZMap.WPFControl
{
    public class MapControl : Canvas, IMapControl
    {
        private object eventLock = new object();
        private event MapLevelChanged levelChanged;
        private event MapRendered mapRendered;

        private Map map;

        public MapControl()
        {
            map = new Map();
            map.NeedRepainted += () => { this.InvalidateVisual(); };
            map.LevelChanged += level => { OnLevelChanged(level); };
            map.Repainted += (area, rate) => { OnMapRendered(area, rate); };
            //
            this.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(MapControl_MouseLeftButtonDown);
            this.MouseRightButtonDown += new MouseButtonEventHandler(MapControl_MouseRightButtonDown);
            this.MouseRightButtonUp += new MouseButtonEventHandler(MapControl_MouseRightButtonUp);
            this.MouseMove += new MouseEventHandler(MapControl_MouseMove);
            this.MouseWheel += new MouseWheelEventHandler(MapControl_MouseWheel);
            this.SizeChanged += new SizeChangedEventHandler(MapControl_SizeChanged);

            this.ClipToBounds = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Stopwatch watch = Stopwatch.StartNew();
            map.DrawMap(drawingContext);
            watch.Stop();
            //Debug.Write("#Draw: " + watch.Elapsed.TotalMilliseconds + "\n");
            base.OnRender(drawingContext);
        }

        public Brush BackgroundColor
        {
            get { return map.BackgroundColor; }
            set { map.BackgroundColor = value; }
        }

        void MapControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            map.OnUpdateMapViewSize(e.NewSize.Width, e.NewSize.Height);
        }

        #region Drag, Zoom and Raise Events

        void MapControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            map.StartZoom(e.GetPosition(this).ToPointD(), 0.05 * Math.Sign(e.Delta));
        }

        void MapControl_MouseMove(object sender, MouseEventArgs e)
        {
            PointD clickpos = e.GetPosition(this).ToPointD();
            map.HitTest(new MapLocation(map.GetCurrentPosition(clickpos), map.Level), InputEventType.MouseMove);
            if (e.RightButton != MouseButtonState.Pressed)
                return;
            map.Drag(e.GetPosition(this).ToPointD());
        }

        void MapControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            PointD clickpos = e.GetPosition(this).ToPointD();
            map.HitTest(new MapLocation(map.GetCurrentPosition(clickpos), map.Level), InputEventType.MouseRightButtonUp);
            map.EndDrag();
        }

        void MapControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointD clickpos = e.GetPosition(this).ToPointD();
            map.HitTest(new MapLocation(map.GetCurrentPosition(clickpos), map.Level), InputEventType.MouseRightButtonDown);
            map.BeginDrag(clickpos);
        }

        void MapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointD clickpos = e.GetPosition(this).ToPointD();
            map.HitTest(new MapLocation(map.GetCurrentPosition(clickpos), map.Level), InputEventType.MouseLeftButtonDown);
            ClickPosition = new MapLocation(map.GetCurrentPosition(clickpos), map.Level);
        }

        #endregion

        protected void OnLevelChanged(int level)
        {
            MapLevelChanged temp = levelChanged;
            if (temp != null) temp(level);
        }

        protected void OnMapRendered(MapArea viewarea, double zoomrate)
        {
            MapRendered temp = mapRendered;
            if (temp != null) temp(viewarea, zoomrate);
        }

        #region IMapControl Members

        public int Level
        {
            get { return map.Level; }
        }

        public MapLocation ClickPosition
        {
            get;
            private set;
        }

        public int RepaintTimeDelay
        {
            get { return map.RepaintTimeDelay; }
            set { map.RepaintTimeDelay = value; }
        }

        public void InitMap(MapLayer maproot)
        {
            map.rootLayer = maproot;
            map.rootLayer.ZIndex = Int32.MinValue;
            map.layerItems.Clear();
            map.maplayerItems.Clear();
        }

        public void Config(MapLayerConfig config)
        {
            map.rootLayer.InitMapLayer(config);
        }

        public void AddMapLayer(MapLayer maplayer)
        {
            map.maplayerItems.Add(maplayer);
            maplayer.MapCore = this.map;
        }

        public void RemoveMapLayer(MapLayer maplayer)
        {
            map.maplayerItems.Remove(maplayer);
            maplayer.MapCore = null;
        }

        public void AddLayer(Layer layer)
        {
            map.layerItems.Add(layer);
            layer.MapCore = map;
        }

        public void RemoveLayer(Layer layer)
        {
            map.layerItems.Remove(layer);
            layer.MapCore = null;
        }

        public void GotoPosition(PointInt centerPosition, int level)
        {
            map.GoToPosition(new MapLocation(centerPosition, level));
        }

        public event MapLevelChanged LevelChanged
        {
            add { lock (eventLock) { levelChanged += value; } }
            remove { lock (eventLock) { levelChanged -= value; } }
        }

        public event MapRendered MapRendered
        {
            add { lock (eventLock) { mapRendered += value; } }
            remove { lock (eventLock) { mapRendered -= value; } }
        }

        #endregion
    }
}