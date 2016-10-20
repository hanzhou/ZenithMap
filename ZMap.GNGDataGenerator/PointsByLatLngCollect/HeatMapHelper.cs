using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZMap.GNGDataGenerator
{
    public static class HeatMapHelper
    {

        public static void SetPixelWithoutLock(this WriteableBitmap bmp, int row, int column, int[] palette)
        {
            unsafe
            {
                // Get a pointer to the back buffer.
                int pBackBuffer = (int)bmp.BackBuffer;
                // Find the address of the pixel to draw.
                pBackBuffer += row * bmp.BackBufferStride;
                pBackBuffer += column * 4;
                // Assign the color data to the pixel.
                *((int*)pBackBuffer) = palette[(byte)~(((byte*)pBackBuffer)[3])];
            }
        }

        public static void SetPixel(this WriteableBitmap bmp, int row, int column, int[] palette)
        {
            bmp.Lock();
            bmp.SetPixelWithoutLock(row, column, palette);
            // Specify the area of the bitmap that changed.
            bmp.AddDirtyRect(new Int32Rect(column, row, 1, 1));
            bmp.Unlock();
        }
        
        public static int[] GetPalette(string paletteimagepath)
        {
            int[] palette = new int[256];
            Bitmap paletteImage = (Bitmap)Bitmap.FromFile(paletteimagepath);
            for (int i = 0; i < palette.Length - 1; i++)
                palette[i] = paletteImage.GetPixel(i, 0).ToArgb();
            palette[palette.Length - 1] = 0;
            return palette;
        }

        public static Ellipse CreateCircle(RadialGradientBrush fillbrush, int radius)
        {
            return new Ellipse()
            {
                Height = 2 * radius,
                Width = 2 * radius,
                Fill = fillbrush
            };
        }

        /// <summary>
        /// Creates the brush for ellipse.
        /// </summary>
        /// <returns></returns>
        public static RadialGradientBrush CreateBrush(byte density, double gradientStop)
        {
            var brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(density, 0, 0, 0), 0));
            brush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(density, 0, 0, 0), gradientStop));
            brush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(0, 0, 0, 0), 1));
            return brush;
        }
    }
}
