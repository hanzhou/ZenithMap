
namespace ZMap.Provider.SingleImageMap
{
    public static class SingleMapTransformSystem
    {
        public static SizeInt MapSize = SizeInt.Empty;
        public static SizeD ModelSize = SizeD.Empty;

        public static void Init(SizeInt mapsize, SizeD modelsize)
        {
            MapSize = mapsize;
            ModelSize = modelsize;
        }

        public static MapRoute ToSingleMapRoute(this GeoRoute route)
        {
            MapRoute maproute = new MapRoute(0);
            foreach (PointLatLng pos in route)
                maproute.Add(SingleMapTransformSystem.ModelXYToPixelXY(pos));
            return maproute;
        }

        public static GeoRoute SingleMapToGeoRoute(this MapRoute route)
        {
            GeoRoute georoute = new GeoRoute();
            foreach (PointInt pos in route)
                georoute.Add(SingleMapTransformSystem.PixelXYToModelXY(pos));
            return georoute;
        }

        public static void ModelXYToPixelXY(double modelLng, double modelLat, out int pixelX, out int pixelY)
        {
            pixelX = (int)(modelLng / ModelSize.Width * MapSize.Width);
            pixelY = (int)(modelLat / ModelSize.Height * MapSize.Height);
        }

        public static PointInt ModelXYToPixelXY(PointLatLng modelLocation)
        {
            int x, y;
            ModelXYToPixelXY(modelLocation.Lng, modelLocation.Lat, out x, out y);
            return new PointInt(x, y);
        }

        public static void PixelXYToModelXY(int pixelX, int pixelY, out double modelLng, out double modelLat)
        {
            modelLng = ModelSize.Width * pixelX / MapSize.Width;
            modelLat = ModelSize.Height * pixelY / MapSize.Height;
        }

        public static PointLatLng PixelXYToModelXY(PointInt pixelPosition)
        {
            double lng, lat;
            PixelXYToModelXY(pixelPosition.X, pixelPosition.Y, out lng, out lat);
            return new PointLatLng(lng, lat);
        }
    }
}
