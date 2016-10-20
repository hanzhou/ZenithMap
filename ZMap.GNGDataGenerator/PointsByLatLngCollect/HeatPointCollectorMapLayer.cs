using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZMap.Provider.BingMap;
using ZMap.WPFControl;

namespace ZMap.GNGDataGenerator
{
    public class HeatPointCollectorMapLayer : MapLayer
    {
        private int[] palette;
        private List<PointInt> positions = new List<PointInt>();
        private MapControl map;
        private Image image = new Image();
        private Random rand = new Random();
        private string paletteImagePath = Environment.CurrentDirectory + ConfigurationManager.AppSettings["PaletteImage"];

        private IPointLatLngCollector collector;

        public HeatPointCollectorMapLayer(MapControl map, IPointLatLngCollector collector)// Stack<PointLatLng> pointstack
        {
            this.map = map;
            this.collector = collector;
            palette = HeatMapHelper.GetPalette(paletteImagePath);

            Canvas.SetZIndex(image, -100);
            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);
            image.IsHitTestVisible = false;
            map.Children.Add(image);
        }
        
        public override void OnUpdateMap(MapArea viewarea)
        {
            if (MapCore.IsDragging)
                image.Visibility = Visibility.Collapsed;
            else
                image.Visibility = Visibility.Visible;
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            int height = viewarea.Area.Height;
            int width = viewarea.Area.Width;
            if (height == 0 || width == 0)
                return;
            if (MapCore.IsDragging)
                return;
            positions = collector.ToList().ConvertAll<PointInt>(
                new Converter<PointLatLng, PointInt>(ptll => { return BingMapTileSystem.LatLngToPixelXY(ptll, MapCore.Level); }));

            DrawingVisual visual = new DrawingVisual();
            DrawingContext dc = visual.RenderOpen();
            for (int i = 0; i < positions.Count; i++)
                dc.DrawEllipse(HeatMapHelper.CreateBrush((byte)Density, GradientStop), null,
                    new Point(positions[i].X - viewarea.Area.X, positions[i].Y - viewarea.Area.Y), CircleRadius, CircleRadius);
            dc.Close();
            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(visual);
            WriteableBitmap bmp = new WriteableBitmap(rtb);
            bmp.Lock();
            for (int i = 0; i < bmp.PixelHeight; i++)
                for (int j = 0; j < bmp.PixelWidth; j++)
                    bmp.SetPixelWithoutLock(i, j, palette);
            bmp.AddDirtyRect(new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));
            bmp.Unlock();
            image.Source = bmp;

            image.Width = viewarea.Area.Width * zoomRate;
            image.Height = viewarea.Area.Height * zoomRate;
            base.Draw(drawingContext, viewarea, zoomRate);
        }

        public int CircleRadius = 20;

        public double GradientStop = 0.4;

        public byte Density = 100;
    }
}