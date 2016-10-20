using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ZMap
{
    public class MapBase : IZMapBasicControl, IZMap
    {
        #region fields

        public Brush BackgroundColor = Brushes.AliceBlue;
        private DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);
        public int RepaintTimeDelay = Constants.RepaintTimeDelay;

        public MapLayer rootLayer = MapLayer.Null;
        public List<MapLayer> maplayerItems = new List<MapLayer>();
        public List<Layer> layerItems = new List<Layer>();

        #region fields on dragging map

        private PointD drawingPoint = new PointD();
        protected bool isDragging = false;
        protected bool isDown = false;

        #endregion

        #region fields on Projection

        //projection of the logical map(the total size only depends on the Level of Details, no zoom)
        //on the screen(the map your eyes see)
        //virtual:
        //  mapViewRect         defines the Position and Size of the logical view rect on the whole logical map
        //  CurrentPosition     the Position of the center of the view on the logical map
        //real:
        //  [RenderOffsetRealView]
        //  mapRealViewSize

        /// <summary>
        /// the center of the logical rect of the map view on the whole map, in pixels
        /// </summary>
        protected PointInt mapViewCenter = new PointInt();
        /// <summary>
        /// the logical rect of the map view, in pixels
        /// </summary>
        protected RectInt mapViewRect = new RectInt();
        /// <summary>
        /// set only in the method OnUpdateMapViewSize after the map view size changed, a backup size of the real window
        /// </summary>
        protected RectD mapViewRealRect = new RectD();

        private int level = 0;
        private double zoomRate = 1.0;

        #endregion

        #region fields on event

        protected object eventLock = new object();
        private event MapLevelChanged levelChanged;
        private event MapNeedRepainted needRepainted;
        private event MapRepainted repainted;
        private event MapViewPositionChanged currentPositionChanged;

        #endregion

        #endregion

        public MapBase()
        {
            timer.Interval = TimeSpan.FromMilliseconds(RepaintTimeDelay);
            timer.Tick += (o, e) => { this.OnNeedRepainted(); };
            timer.Start();
        }
        
        /// <summary>
        /// 相对于原始图像(同一级别)的大小比例
        /// range from MinZoomRate to MaxZoomRate in same Level
        /// </summary>
        public double ZoomRate
        {
            get { return zoomRate; }
            protected set
            {
                if (value >= rootLayer.MinZoomRate && value <= rootLayer.MaxZoomRate)
                    zoomRate = value;
                else if (value > rootLayer.MaxZoomRate)
                {
                    if (Level < rootLayer.MaxLevel)
                    {
                        zoomRate = value / 2;
                        Level++;
                    }
                    else
                        zoomRate = rootLayer.MaxZoomRate;
                }
                else if (value < rootLayer.MinZoomRate)
                {
                    if (Level > rootLayer.MinLevel)
                    {
                        zoomRate = value * 2;
                        Level--;
                    }
                    else
                        zoomRate = rootLayer.MinZoomRate;
                }
                this.mapViewRect.Width = (int)(mapViewRealRect.Width / ZoomRate);
                this.mapViewRect.Height = (int)(mapViewRealRect.Height / ZoomRate);
            }
        }

        protected PointD renderOffsetRealView = new PointD();
        /// <summary>
        /// 地图显示区域中心在整个地图中的位置
        /// </summary>
        protected PointD RenderOffsetRealView
        {
            get { return renderOffsetRealView; }
            set
            {
                renderOffsetRealView = value;
                mapViewCenter.X = (int)(value.X / ZoomRate);
                mapViewCenter.Y = (int)(value.Y / ZoomRate);
                mapViewRect.X = (int)(mapViewCenter.X - mapViewRect.Width / 2);
                mapViewRect.Y = (int)(mapViewCenter.Y - mapViewRect.Height / 2);
                //DateTime start = DateTime.Now;
                rootLayer.OnUpdateMap(new MapArea(mapViewRect, Level));
                OnNeedRepainted();
                //DateTime end = DateTime.Now;
                //Debug.WriteLine("Update: " + (end - start).TotalMilliseconds);
            }
        }

        public MapArea MapArea
        {
            get { return new MapArea(mapViewRect, Level); }
        }

        #region raise event

        protected void OnLevelChanged(int newLevel)
        {
            MapLevelChanged temp = levelChanged;
            if (temp != null) temp(newLevel);
        }

        protected void OnNeedRepainted()
        {
            foreach (MapLayer maplayer in maplayerItems)
                maplayer.OnUpdateMap(new MapArea(mapViewRect, Level));
            MapNeedRepainted temp = needRepainted;
            if (temp != null) temp();
        }

        protected void OnRepainted()
        {
            MapRepainted temp = repainted;
            if (temp != null) temp(new MapArea(mapViewRect, Level), ZoomRate);
        }

        protected void OnCurrentPositionChanged(PointInt newPosition)
        {
            MapViewPositionChanged temp = currentPositionChanged;
            if (temp != null) temp(newPosition);
        }

        #endregion

        #region IMapBasicControl Members

        public bool IsDragging
        {
            get { return isDragging; }
        }

        public void BeginDrag(PointD startPos)
        {
            isDown = true;
            drawingPoint.X = startPos.X + RenderOffsetRealView.X;
            drawingPoint.Y = startPos.Y + RenderOffsetRealView.Y;
        }

        public void Drag(PointD newPos)
        {
            if (!isDown)
                return;
            isDragging = true;
            double offsetX = drawingPoint.X - newPos.X;
            double offsetY = drawingPoint.Y - newPos.Y;
            RenderOffsetRealView = new PointD(offsetX, offsetY);

            //double offsetX = drawingPoint.X - newPos.X + mapViewRealRect.Width / 2;
            //double offsetY = drawingPoint.Y - newPos.Y + mapViewRealRect.Height / 2;
            //this.GoToPosition(Level, new PointInt((int)(offsetX / ZoomRate), (int)(offsetY / ZoomRate)));
        }

        public void EndDrag()
        {
            if (isDragging)
            {
                isDragging = false;
                isDown = false;
            }
        }

        public void StartZoom(PointD zoomCenter, double relativeZoomRate)
        {
            //double delX = 0, delY = 0;
            //ZoomRate *= (relativeZoomRate + 1);
            //if (ZoomRate == rootLayer.MaxZoomRate || ZoomRate == rootLayer.MinZoomRate)
            //    return;
            //if (relativeZoomRate > 0)
            //{
            //    delX = zoomCenter.X -mapViewRealRect.Width / 2;
            //    delY = zoomCenter.Y -mapViewRealRect.Height / 2;
            //}
            //double deltaX = (mapViewRealRect.Center.X + delX) * relativeZoomRate;
            //double deltaY = (mapViewRealRect.Center.Y + delY) * relativeZoomRate;
            //GoToPosition(Level,
            //    new PointInt((int)(deltaX / ZoomRate + mapViewRect.Center.X), (int)(deltaY / ZoomRate + mapViewRect.Center.Y)));

            double delX = 0, delY = 0;
            ZoomRate *= (relativeZoomRate + 1);
            if (ZoomRate == rootLayer.MaxZoomRate || ZoomRate == rootLayer.MinZoomRate)
                return;
            if (relativeZoomRate > 0)
            {
                delX = zoomCenter.X - mapViewRealRect.Width / 2;
                delY = zoomCenter.Y - mapViewRealRect.Height / 2;
            }
            double deltaX = (RenderOffsetRealView.X + delX) * relativeZoomRate;
            double deltaY = (RenderOffsetRealView.Y + delY) * relativeZoomRate;
            RenderOffsetRealView = new PointD(deltaX + RenderOffsetRealView.X, deltaY + RenderOffsetRealView.Y);
        }

        #endregion

        #region IZMap Members

        /// <summary>
        /// Level of detail
        /// </summary>
        public int Level
        {
            get { return level; }
            protected set
            {
                level = value;
                OnLevelChanged(value);
                OnNeedRepainted();
            }
        }

        /// <summary>
        /// the center of the logical rect of the map view on the whole map, in pixels
        /// </summary>
        public PointInt CurrentPosition
        {
            get { return mapViewRect.Center; }
        }

        public void OnUpdateMapViewSize(double width, double height)
        {
            this.mapViewRealRect.Width = width;
            this.mapViewRealRect.Height = height;
            this.mapViewRect.Width = (int)(width / ZoomRate);
            this.mapViewRect.Height = (int)(height / ZoomRate);
            GoToPosition(new MapLocation(mapViewCenter, Level));
        }
        
        public void DrawMap(DrawingContext drawingContext)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            drawingContext.DrawRectangle(BackgroundColor, new Pen(), new Rect(0, 0, mapViewRealRect.Width, mapViewRealRect.Height));
            rootLayer.Draw(drawingContext, new MapArea(mapViewRect, Level), ZoomRate);
            TimeSpan drawmap = stopwatch.Elapsed;
            //Debug.Write(" Draw Map: " + drawmap.TotalMilliseconds);
            double rate = (1 << Level) * ZoomRate;
            foreach (MapLayer maplayer in maplayerItems)
                maplayer.Draw(drawingContext, new MapArea(mapViewRect, Level), ZoomRate);
            foreach (Layer layer in layerItems)
                layer.Draw(drawingContext, new MapArea(mapViewRect, Level), ZoomRate);
            OnRepainted();
            stopwatch.Stop();
            TimeSpan total = stopwatch.Elapsed;
            //Debug.Write("\tDraw Layer: " + (total - drawmap).TotalMilliseconds);
            //Debug.Write("\t!Draw: " + total.TotalMilliseconds);
        }

        public void HitTest(MapLocation pos, InputEventType inputtype)
        {
            foreach (MapLayer maplayer in maplayerItems)
                maplayer.HitTest(pos, inputtype);
            foreach (Layer layer in layerItems)
                layer.HitTest(pos, inputtype);
        }

        public event MapNeedRepainted NeedRepainted
        {
            add { lock (eventLock) { needRepainted += value; } }
            remove { lock (eventLock) { needRepainted -= value; } }
        }

        public event MapRepainted Repainted
        {
            add { lock (eventLock) { repainted += value; } }
            remove { lock (eventLock) { repainted -= value; } }
        }

        public event MapViewPositionChanged CurrentPositionChanged
        {
            add { lock (eventLock) { currentPositionChanged += value; } }
            remove { lock (eventLock) { currentPositionChanged -= value; } }
        }

        public event MapLevelChanged LevelChanged
        {
            add { lock (eventLock) { levelChanged += value; } }
            remove { lock (eventLock) { levelChanged -= value; } }
        }

        public void GoToPosition(MapLocation pos)
        {
            //mapViewRect.X = (int)(position.X - mapViewRect.Width / 2);
            //mapViewRect.Y = (int)(position.Y - mapViewRect.Height / 2);
            ////mapViewRealRect.X = mapViewRect.X * ZoomRate;
            ////mapViewRealRect.Y = mapViewRect.Y * ZoomRate;
            //Level = level;
            //rootLayer.OnUpdateMap(mapViewRect, Level);
            //foreach (MapLayer maplayer in maplayerItems)
            //    maplayer.OnUpdateMap(mapViewRect, Level);
            Level = pos.Level;
            RenderOffsetRealView = new PointD(pos.Position.X * ZoomRate, pos.Position.Y * ZoomRate);
            mapViewCenter = pos.Position;
        }

        #endregion
    }
}