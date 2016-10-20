using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ZMap.Provider.BingMap;

namespace ZMap.GNGDataGenerator
{
    public class BusStopCollectLayer : Layer
    {
        #region fields

        private PointLatLng TempPoint = new PointLatLng();
        private List<PointLatLng> buslinepoints;
        private const int radius = 5;
        private string InfoFilePath = Environment.CurrentDirectory + "/BusLine702.xml";
        private bool HasAdded = false;

        private IPointLatLngCollector CollectorUp = new PointsCollector();
        private IPointLatLngCollector CollectorDown = new PointsCollector();
        private IPointLatLngCollector CollectorLeft = new PointsCollector();
        private IPointLatLngCollector CollectorRight = new PointsCollector();
        private string InfoFilePathUp = Environment.CurrentDirectory + "/PointInfoUp.xml";
        private string InfoFilePathDown = Environment.CurrentDirectory + "/PointInfoDown.xml";
        private string InfoFilePathLeft = Environment.CurrentDirectory + "/PointInfoStart.xml";
        private string InfoFilePathRight = Environment.CurrentDirectory + "/PointInfoEnd.xml";

        #endregion

        public BusStopCollectLayer()
        {
            buslinepoints = PointCollectorHelper.Load(InfoFilePath);
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            for (int i = 1; i < buslinepoints.Count; i++)
                drawingContext.DrawLine(new Pen(Brushes.Blue, 2),
                    LocationToWindow(buslinepoints[i - 1], viewarea, zoomRate),
                    LocationToWindow(buslinepoints[i], viewarea, zoomRate));
            foreach (PointLatLng pt in CollectorUp)
                drawingContext.DrawEllipse(Brushes.Red, new Pen(),
                    LocationToWindow(pt, viewarea, zoomRate), radius, radius);
            foreach (PointLatLng pt in CollectorDown)
                drawingContext.DrawEllipse(Brushes.Blue, new Pen(),
                    LocationToWindow(pt, viewarea, zoomRate), radius, radius);
            foreach (PointLatLng pt in CollectorLeft)
                drawingContext.DrawEllipse(Brushes.DarkRed, new Pen(),
                    LocationToWindow(pt, viewarea, zoomRate), radius, radius);
            foreach (PointLatLng pt in CollectorRight)
                drawingContext.DrawEllipse(Brushes.DarkBlue, new Pen(),
                    LocationToWindow(pt, viewarea, zoomRate), radius, radius);
            if (!HasAdded)
                drawingContext.DrawEllipse(Brushes.Yellow, new Pen(),
                    LocationToWindow(TempPoint, viewarea, zoomRate), radius, radius);
            base.Draw(drawingContext, viewarea, zoomRate);
        }

        private Point LocationToWindow(PointLatLng position, MapArea viewarea, double zoomrate)
        {
            PointInt pt = BingMapTileSystem.LatLngToPixelXY(position, viewarea.Level);
            return new Point((pt.X - viewarea.Area.X) * zoomrate, (pt.Y - viewarea.Area.Y) * zoomrate);
        }

        public void SetTempPoint(PointLatLng temppoint)
        {
            HasAdded = false;
            TempPoint = temppoint;
        }

        public void AddUp()
        {
            if (HasAdded)
                return;
            CollectorUp.Add(TempPoint);
            HasAdded = true;
        }

        public void AddDown()
        {
            if (HasAdded)
                return;
            CollectorDown.Add(TempPoint);
            HasAdded = true;
        }

        public void AddStart()
        {
            if (CollectorLeft.Count > 0 || HasAdded)
                return;
            CollectorLeft.Add(TempPoint);
            HasAdded = true;
        }

        public void AddEnd()
        {
            if (CollectorRight.Count > 0 || HasAdded)
                return;
            CollectorRight.Add(TempPoint);
            HasAdded = true;
        }

        public void Save()
        {
            CollectorLeft.Save(InfoFilePathLeft);
            CollectorUp.Save(InfoFilePathUp);
            CollectorRight.Save(InfoFilePathRight);
            CollectorDown.Save(InfoFilePathDown);
        }
    }
}
