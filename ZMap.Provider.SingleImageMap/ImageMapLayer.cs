using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace ZMap.Provider.SingleImageMap
{
    public class ImageMapLayer : MapLayer
    {
        private int mapSourceWidth = SingleMapTransformSystem.MapSize.Width;
        private int mapSourceHeight = SingleMapTransformSystem.MapSize.Height;
        private RenderTargetBitmap rtmap;
        //Image map = new Image();

        public ImageMapLayer(Canvas mapcontrol)
        {
            rtmap = new RenderTargetBitmap(mapSourceWidth, mapSourceHeight, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual drawingvisual = new DrawingVisual();
            DrawingContext drawingcontext = drawingvisual.RenderOpen();
            drawingcontext.DrawImage(
                new BitmapImage(new Uri(Environment.CurrentDirectory + ConfigurationManager.AppSettings["MapPath"])),
                new Rect(0, 0, mapSourceWidth, mapSourceHeight));
            drawingcontext.Close();
            rtmap.Render(drawingvisual);
            if (rtmap.CanFreeze)
                rtmap.Freeze();
            //map.Source = rtmap;
            //mapcontrol.Children.Add(map);
        }

        public override void OnUpdateMap(MapArea viewarea) { }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            //map.Width = mapSourceWidth * zoomRate;
            //map.Height = mapSourceHeight * zoomRate;
            //Canvas.SetLeft(map, -viewarea.Area.X * zoomRate);
            //Canvas.SetTop(map, -viewarea.Area.Y * zoomRate);
            drawingContext.DrawImage(rtmap, new Rect(
                -viewarea.Area.X * zoomRate,
                -viewarea.Area.Y * zoomRate,
                mapSourceWidth * zoomRate,
                mapSourceHeight * zoomRate));
            base.Draw(drawingContext, viewarea, zoomRate);
        }
    }
}
