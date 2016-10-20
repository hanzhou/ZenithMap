using System.Windows.Input;
using System.Windows.Media;
using ZMap.Provider.BingMap;

namespace ZMap.Test
{
    public delegate void MouseMoveOn();
    public delegate void MouseMoveNotOn();

    public class MouseMoveTestLayer : Layer
    {
        private event MouseMoveOn mouseMoveOn;
        private event MouseMoveNotOn mouseMoveNotOn;

        PointLatLng pos1 = new PointLatLng(114.387373924, 30.6309699587);
        PointLatLng pos2 = new PointLatLng(114.391665458, 30.6287912278);

        public MouseMoveTestLayer()
        {
            this.MouseMoveOn += () => { Mouse.OverrideCursor = Cursors.Hand; };
            this.MouseMoveNotOn += () => { Mouse.OverrideCursor = Cursors.Arrow; };
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            PointInt p1 = BingMapTileSystem.LatLngToPixelXY(pos1, viewarea.Level);
            PointInt p2 = BingMapTileSystem.LatLngToPixelXY(pos2, viewarea.Level);
            RectInt rect = RectInt.FromLTRB(p1.X, p1.Y, p2.X, p2.Y);
            drawingContext.DrawRectangle(Brushes.Blue, new Pen(),
                new System.Windows.Rect((rect.X - viewarea.Area.X) * zoomRate, (rect.Y - viewarea.Area.Y) * zoomRate, rect.Width * zoomRate, rect.Height * zoomRate));
            base.Draw(drawingContext, viewarea, zoomRate);
        }

        public override bool HitTest(MapLocation pos, InputEventType inputtype)
        {
            PointInt p1 = BingMapTileSystem.LatLngToPixelXY(pos1, pos.Level);
            PointInt p2 = BingMapTileSystem.LatLngToPixelXY(pos2, pos.Level);
            switch (inputtype)
            {
                case InputEventType.MouseMove:
                    RectInt rect = RectInt.FromLTRB(p1.X, p1.Y, p2.X, p2.Y);
                    if (rect.Contains(pos.Position))
                    {
                        OnMouseMoveOn();
                        return true;
                    }
                    OnMouseMoveNotOn();
                    return false;
            }
            return false;
        }

        protected void OnMouseMoveOn()
        {
            MouseMoveOn temp = mouseMoveOn;
            if (temp != null) temp();
        }

        protected void OnMouseMoveNotOn()
        {
            MouseMoveNotOn temp = mouseMoveNotOn;
            if (temp != null) temp();
        }

        public event MouseMoveOn MouseMoveOn
        {
            add { lock (eventLock) { mouseMoveOn += value; } }
            remove { lock (eventLock) { mouseMoveOn -= value; } }
        }

        public event MouseMoveNotOn MouseMoveNotOn
        {
            add { lock (eventLock) { mouseMoveNotOn += value; } }
            remove { lock (eventLock) { mouseMoveNotOn -= value; } }
        }
    }
}
