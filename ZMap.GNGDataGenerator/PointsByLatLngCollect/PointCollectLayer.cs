using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ZMap.Provider.BingMap;

namespace ZMap.GNGDataGenerator
{
    public class PointCollectLayer : Layer
    {
        private IPointLatLngCollector collector;
        private const int radius = 3;

        public PointCollectLayer(IPointLatLngCollector collector)
        {
            this.collector = collector;
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            List<PointInt> positions = collector.ToList().ConvertAll<PointInt>(
                new Converter<PointLatLng, PointInt>(ptll => { return BingMapTileSystem.LatLngToPixelXY(ptll, MapCore.Level); }));
            foreach (PointInt pt in positions)
                drawingContext.DrawEllipse(Brushes.Black, new Pen(), 
                    new Point((pt.X - viewarea.Area.X) * zoomRate, (pt.Y - viewarea.Area.Y) * zoomRate), radius, radius);
            base.Draw(drawingContext, viewarea, zoomRate);
        }
    }
}
