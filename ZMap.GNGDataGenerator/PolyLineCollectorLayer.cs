using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ZMap.Provider.BingMap;

namespace ZMap.GNGDataGenerator
{
    public class PolyLineCollectorLayer : Layer
    {
        IPointLatLngCollector collector;
        private const int radius = 3;

        public PolyLineCollectorLayer(IPointLatLngCollector collector)
        {
            this.collector = collector;
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            List<PointInt> positions = collector.ToList().ConvertAll<PointInt>(
                new Converter<PointLatLng, PointInt>(ptll => { return BingMapTileSystem.LatLngToPixelXY(ptll, MapCore.Level); }));
            for (int i = 0; i < positions.Count; i++)
            {
                 drawingContext.DrawEllipse(Brushes.Black, new Pen(),
                    new Point((positions[i].X - viewarea.Area.X) * zoomRate, (positions[i].Y - viewarea.Area.Y) * zoomRate), radius, radius);
                 if (i == 0) continue;
                 drawingContext.DrawLine(new Pen(Brushes.Blue, 2),
                     new Point((positions[i - 1].X - viewarea.Area.X) * zoomRate, (positions[i - 1].Y - viewarea.Area.Y) * zoomRate),
                     new Point((positions[i].X - viewarea.Area.X) * zoomRate, (positions[i].Y - viewarea.Area.Y) * zoomRate));
            }
            base.Draw(drawingContext, viewarea, zoomRate);
        }
    }
}