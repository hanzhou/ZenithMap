using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace ZMap.Provider.BingMap
{
    public class BingMapTileLayer : MapLayer
    {
        private const int TileLength = 256;

        private TileLoadManager tileLoadManager = null;
        private Dictionary<RawTile, ImageSource> tileViewDict = null;//////

        public BingMapTileLayer(TileLoadProxy tileloadProxy)
        {
            tileLoadManager = new TileLoadManager(tileloadProxy);
            
            tileViewDict = new Dictionary<RawTile, ImageSource>();
            tileLoadManager.TileLoadCompleted += (o, e) =>
            {
                if (e.Cancelled || e.Error != null)
                    return;
                if (!tileViewDict.ContainsKey(e.TileKey))
                    return;
                tileViewDict[e.TileKey] = Stuff.GetImageSourceFromStream(e.Source);
                this.OnViewNeedRendered();
            };
        }
        
        public override void OnUpdateMap(MapArea viewarea)
        {
            Stopwatch watch = Stopwatch.StartNew();
            PointInt topleftTileXY = BingMapTileSystem.PixelXYToTileXY(viewarea.Area.TopLeft);
            PointInt bottomrightTileXY = BingMapTileSystem.PixelXYToTileXY(viewarea.Area.BottomRight);
            topleftTileXY.Offset(-2, -2);
            bottomrightTileXY.Offset(2, 2);
            if (topleftTileXY.X < 0) topleftTileXY.X = 0;
            if (topleftTileXY.Y < 0) topleftTileXY.Y = 0;
            int len = 1 << viewarea.Level;
            if (bottomrightTileXY.X >= len) bottomrightTileXY.X = len - 1;
            if (bottomrightTileXY.Y >= len) bottomrightTileXY.Y = len - 1;
            
            List<PointInt> pointlist = new List<PointInt>();
            for (int i = topleftTileXY.X; i <= bottomrightTileXY.X; i++)
                for (int j = topleftTileXY.Y; j <= bottomrightTileXY.Y; j++)
                {
                    pointlist.Add(new PointInt(i, j));
                }
            List<RawTile> toBeDel = new List<RawTile>();
            foreach (RawTile tile in tileViewDict.Keys)
            {
                if (tile.Level == viewarea.Level)
                    pointlist.Remove(tile.TileXY);
                RectInt tilerect = new RectInt(
                    (tile.TileXY.X - 1) * TileLength, (tile.TileXY.Y - 1) * TileLength, TileLength * 2, TileLength * 2);
                if (tile.Level != viewarea.Level)// || !tilerect.HasIntersectsWith(viewarea.Area))
                    toBeDel.Add(tile);
            }
            foreach (RawTile tile in toBeDel)
                tileViewDict.Remove(tile);

            foreach (PointInt p in pointlist)
            {
                RawTile tileKey = new RawTile(this.MapType, p, viewarea.Level);
                tileViewDict.Add(tileKey, null);
                tileLoadManager.LoadTileAsync(tileKey);
            }
            watch.Stop();
            //Debug.Write(" GetTiles: " + watch.Elapsed.TotalMilliseconds);
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            double len = TileLength * zoomRate;
            double viewX = viewarea.Area.X * zoomRate;
            double viewY = viewarea.Area.Y * zoomRate;
            Rect realview = new Rect(0, 0, viewarea.Area.Width * zoomRate, viewarea.Area.Height * zoomRate);
            int count = 0;
            lock (tileViewDict)
            {
                foreach (RawTile tile in tileViewDict.Keys)
                {
                    ImageSource img = tileViewDict[tile];
                    if (img != null)
                    {
                        Rect rect = new Rect(
                            tile.TileXY.X * len - viewX, tile.TileXY.Y * len - viewY, len, len);
                        if (realview.IntersectsWith(rect))
                        {
                            drawingContext.DrawImage(img, rect);
                            count++;
                        }
                    }
                }
            }
            //Debug.Write(" #Draw tile count: " + count);
            base.Draw(drawingContext, viewarea, zoomRate);
        }
    }
}