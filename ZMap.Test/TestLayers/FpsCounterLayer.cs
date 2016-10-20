using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows;

namespace ZMap.Test
{
    public class FpsCounterLayer : Layer
    {
        #region Fields

        private double elapsed;
        private int lastTick = Environment.TickCount;
        private int currentTick;
        private int frameCount;
        private double frameCountTime;
        private int fps;

        private Typeface typeface = new Typeface("微软雅黑");
        Point pos = new Point(10, 10);

        #endregion

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            drawingContext.DrawText(
                new FormattedText(
                    GetFps().ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 20, Brushes.Black),
                pos);
        }

        public int GetFps()
        {
            this.currentTick = Environment.TickCount;
            this.elapsed = (double)(this.currentTick - this.lastTick) / 1000.0;
            this.lastTick = this.currentTick;
            frameCount++;
            frameCountTime += elapsed;
            if (frameCountTime >= 1.0)
            {
                frameCountTime -= 1.0;
                fps = frameCount;
                frameCount = 0;
            }
            return fps;
        }
    }
}
