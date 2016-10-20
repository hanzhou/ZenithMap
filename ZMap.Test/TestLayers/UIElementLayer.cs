using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZMap.Provider.BingMap;
using ZMap.WPFControl;

namespace ZMap.Test
{
    public class UIElementLayer : Layer
    {
        private List<ControlElement> controlItems = new List<ControlElement>();

        private PointLatLng pos1 = new PointLatLng(114.387373924, 30.6309699587);
        private PointLatLng pos2 = new PointLatLng(114.391665458, 30.6287912278);

        public UIElementLayer(MapControl control)
        {
            controlItems.Add(
                new ControlElement(pos1) { Width = 80, Height = 30, Content = "A" });
            controlItems.Add(
                new ControlElement(pos2) { Width = 80, Height = 30, Content = "Hello World!~" });
            foreach (UIElement ui in controlItems)
                control.Children.Add(ui);
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            foreach (ControlElement element in controlItems)
            {
                PointInt pos = BingMapTileSystem.LatLngToPixelXY(element.Location, viewarea.Level);
                Canvas.SetLeft(element, (pos.X - viewarea.Area.X) * zoomRate);
                Canvas.SetTop(element, (pos.Y - viewarea.Area.Y) * zoomRate);
            }
            base.Draw(drawingContext, viewarea, zoomRate);
        }
    }
}
