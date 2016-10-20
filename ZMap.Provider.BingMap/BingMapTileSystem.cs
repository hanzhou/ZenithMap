using System;
using System.Text;

namespace ZMap.Provider.BingMap
{
    public static class BingMapTileSystem
    {
        public const double EarthRadius = 6378137;
        public const double MinLatitude = -85.05112878;
        public const double MaxLatitude = 85.05112878;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;

        /// <summary>
        /// Clips a number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="n">The number to clip.</param>
        /// <param name="minValue">Minimum allowable value.</param>
        /// <param name="maxValue">Maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        internal static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        /// <summary>
        /// Determines the map width and height (in pixels) at a specified level of detail.
        /// </summary>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>The map width and height in pixels.</returns>
        public static uint MapSize(int level)
        {
            return (uint)256 << level;
        }

        /// <summary>
        /// Determines the ground resolution (in meters per pixel) at a specified latitude and level of detail.
        /// </summary>
        /// <param name="latitude">Latitude (in degrees) at which to measure the ground resolution.</param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>The ground resolution, in meters per pixel.</returns>
        public static double GroundResolution(double latitude, int level)
        {
            latitude = Clip(latitude, MinLatitude, MaxLatitude);
            return Math.Cos(latitude * Math.PI / 180) * 2 * Math.PI * EarthRadius / MapSize(level);
        }

        /// <summary>
        /// Determines the map scale at a specified latitude, level of detail, and screen resolution.
        /// </summary>
        /// <param name="latitude">Latitude (in degrees) at which to measure the map scale.</param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <param name="screenDpi">Resolution of the screen, in dots per inch.</param>
        /// <returns>The map scale, expressed as the denominator N of the ratio 1 : N.</returns>
        public static double MapScale(double latitude, int level, int screenDpi)
        {
            return GroundResolution(latitude, level) * screenDpi / 0.0254;
        }

        /// <summary>
        /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees) into pixel XY coordinates at a specified level of detail.
        /// </summary>
        /// <param name="latitude">Latitude of the point, in degrees.</param>
        /// <param name="longitude">Longitude of the point, in degrees.</param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <param name="pixelX">Output parameter receiving the X coordinate in pixels.</param>
        /// <param name="pixelY">Output parameter receiving the Y coordinate in pixels.</param>
        public static void LatLngToPixelXY(double latitude, double longitude, int level, out int pixelX, out int pixelY)
        {
            latitude = Clip(latitude, MinLatitude, MaxLatitude);
            longitude = Clip(longitude, MinLongitude, MaxLongitude);

            double x = (longitude + 180) / 360;
            double sinLatitude = Math.Sin(latitude * Math.PI / 180);
            double y = 0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI);

            uint mapSize = MapSize(level);
            pixelX = (int)Clip(x * mapSize + 0.5, 0, mapSize - 1);
            pixelY = (int)Clip(y * mapSize + 0.5, 0, mapSize - 1);
        }

        /// <summary>
        /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees) into pixel XY coordinates at a specified level of detail.
        /// </summary>
        /// <param name="location">a point, latitude/longitude WGS-84 coordinates, in degrees.</param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>the point in pixels.</returns>
        public static PointInt LatLngToPixelXY(PointLatLng location, int level)
        {
            int x, y;
            LatLngToPixelXY(location.Lat, location.Lng, level, out x, out y);
            return new PointInt(x, y);
        }

        /// <summary>
        /// Converts a pixel from pixel XY coordinates at a specified level of detail into latitude/longitude WGS-84 coordinates (in degrees).
        /// </summary>
        /// <param name="pixelX">X coordinate of the point, in pixels.</param>
        /// <param name="pixelY">Y coordinates of the point, in pixels.</param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <param name="latitude">Output parameter receiving the latitude in degrees.</param>
        /// <param name="longitude">Output parameter receiving the longitude in degrees.</param>
        public static void PixelXYToLatLong(int pixelX, int pixelY, int level, out double latitude, out double longitude)
        {
            double mapSize = MapSize(level);
            double x = (Clip(pixelX, 0, mapSize - 1) / mapSize) - 0.5;
            double y = 0.5 - (Clip(pixelY, 0, mapSize - 1) / mapSize);

            latitude = 90 - 360 * Math.Atan(Math.Exp(-y * 2 * Math.PI)) / Math.PI;
            longitude = 360 * x;
        }

        /// <summary>
        /// Converts a pixel from pixel XY coordinates at a specified level of detail into latitude/longitude WGS-84 coordinates (in degrees).
        /// </summary>
        /// <param name="pixelXY">the point in pixels</param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>the location, latitude/longitude WGS-84 coordinates</returns>
        public static PointLatLng PixelXYToLatLong(PointInt pixelXY, int level)
        {
            double lat, lng;
            PixelXYToLatLong(pixelXY.X, pixelXY.Y, level, out lat, out lng);
            return new PointLatLng(lng, lat);
        }

        /// <summary>
        /// Converts pixel XY coordinates into tile XY coordinates of the tile containingthe specified pixel.
        /// </summary>
        /// <param name="pixelX">Pixel X coordinate.</param>
        /// <param name="pixelY">Pixel Y coordinate.</param>
        /// <param name="tileX">Output parameter receiving the tile X coordinate.</param>
        /// <param name="tileY">Output parameter receiving the tile Y coordinate.</param>
        public static void PixelXYToTileXY(int pixelX, int pixelY, out int tileX, out int tileY)
        {
            tileX = pixelX / 256;
            tileY = pixelY / 256;
        }

        /// <summary>
        /// Converts pixel XY coordinates into tile XY coordinates of the tile containingthe specified pixel.
        /// </summary>
        /// <param name="pixelXY"></param>
        /// <returns></returns>
        public static PointInt PixelXYToTileXY(PointInt pixelXY)
        {
            return new PointInt(pixelXY.X / 256, pixelXY.Y / 256);
        }

        /// <summary>
        /// Converts tile XY coordinates into pixel XY coordinates of the upper-left pixel of the specified tile.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="tileY">Tile Y coordinate.</param>
        /// <param name="pixelX">Output parameter receiving the pixel X coordinate.</param>
        /// <param name="pixelY">Output parameter receiving the pixel Y coordinate.</param>
        public static void TileXYToPixelXY(int tileX, int tileY, out int pixelX, out int pixelY)
        {
            pixelX = tileX * 256;
            pixelY = tileY * 256;
        }

        /// <summary>
        /// Converts tile XY coordinates into pixel XY coordinates of the upper-left pixel of the specified tile.
        /// </summary>
        /// <param name="tileXY"></param>
        /// <returns></returns>
        public static PointInt TileXYToPixelXY(PointInt tileXY)
        {
            return new PointInt(tileXY.X * 256, tileXY.Y * 256);
        }

        /// <summary>
        /// Converts tile XY coordinates into a QuadKey at a specified level of detail.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="tileY">Tile Y coordinate.</param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>A string containing the QuadKey.</returns>
        public static string TileXYToQuadKey(int tileX, int tileY, int level)
        {
            StringBuilder quadKey = new StringBuilder();
            for (int i = level; i > 0; i--)
            {
                char digit = '0';
                int mask = 1 << (i - 1);
                if ((tileX & mask) != 0)
                {
                    digit++;
                }
                if ((tileY & mask) != 0)
                {
                    digit++;
                    digit++;
                }
                quadKey.Append(digit);
            }
            return quadKey.ToString();
        }

        /// <summary>
        /// Converts tile XY coordinates into a QuadKey at a specified level of detail.
        /// </summary>
        /// <param name="tileXY"></param>
        /// <param name="level">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns></returns>
        public static string TileXYToQuadKey(PointInt tileXY, int level)
        {
            return TileXYToQuadKey(tileXY.X, tileXY.Y, level);
        }

        /// <summary>
        /// Converts a QuadKey into tile XY coordinates.
        /// </summary>
        /// <param name="quadKey">QuadKey of the tile.</param>
        /// <param name="tileX">Output parameter receiving the tile X coordinate.</param>
        /// <param name="tileY">Output parameter receiving the tile Y coordinate.</param>
        /// <param name="level">Output parameter receiving the level of detail.</param>
        public static void QuadKeyToTileXY(string quadKey, out int tileX, out int tileY, out int level)
        {
            tileX = tileY = 0;
            level = quadKey.Length;
            for (int i = level; i > 0; i--)
            {
                int mask = 1 << (i - 1);
                switch (quadKey[level - i])
                {
                    case '0':
                        break;
                    case '1':
                        tileX |= mask;
                        break;
                    case '2':
                        tileY |= mask;
                        break;
                    case '3':
                        tileX |= mask;
                        tileY |= mask;
                        break;
                    default:
                        throw new ArgumentException("Invalid QuadKey digit sequence.");
                }
            }
        }

        /// <summary>
        /// Converts a QuadKey into tile XY coordinates.
        /// </summary>
        /// <param name="quadKey">QuadKey of the tile.</param>
        /// <param name="tileXY"></param>
        /// <param name="level">Output parameter receiving the level of detail.</param>
        public static void QuadKeyToTileXY(string quadKey, out PointInt tileXY, out int level)
        {
            int x, y;
            QuadKeyToTileXY(quadKey, out x, out y, out level);
            tileXY = new PointInt(x, y);
        }
    }
}
