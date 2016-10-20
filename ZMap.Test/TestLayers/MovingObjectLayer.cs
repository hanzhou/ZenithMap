using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZMap.Provider.BingMap;

namespace ZMap.Test
{
    public class MovingObjectLayer : MapLayer
    {
        #region fields

        private const int busNum = 40;
        private PointLatLng[] starts = new PointLatLng[busNum];
        private PointLatLng[] ends = new PointLatLng[busNum];
        private PointLatLng[] tempLocations = new PointLatLng[busNum];
        private double[] deltaLngs = new double[busNum];
        private double[] deltaLats = new double[busNum];

        private List<GeoRoute> routes = new List<GeoRoute>(busNum);

        private TimeSpan totalInterval = TimeSpan.FromMilliseconds(5000);
        private TimeSpan deltaTime = TimeSpan.FromMilliseconds(100);
        private int[] totalCounts = new int[busNum];
        private int[] currCounts = new int[busNum];
        private int[] currPt = new int[busNum];

        BitmapImage bus;
        Random rand = new Random();

        #endregion

        public MovingObjectLayer()
        {
            GeoRoute rA = new GeoRoute(r1);
            rA.Reverse();
            GeoRoute rB = new GeoRoute(r2);
            rB.Reverse();
            //GeoRoute rC = new GeoRoute(r3);
            //rC.Reverse();
            //GeoRoute rD = new GeoRoute(r4);
            //rD.Reverse();
            for (int i = 0; i < 10; i++)
            {
                routes.Add(new GeoRoute(r1));
                routes.Add(new GeoRoute(rA));
                routes.Add(new GeoRoute(r2));
                routes.Add(new GeoRoute(rB));
                //routes.Add(new GeoRoute(r3));
                //routes.Add(new GeoRoute(rC));
                //routes.Add(new GeoRoute(r4));
                //routes.Add(new GeoRoute(rD));
            }
            for (int i = 0; i < busNum; i++)
            {
                totalCounts[i] = (int)(1000 * (i + 1) / deltaTime.TotalMilliseconds);
                currPt[i] = 0;
                starts[i] = routes[i][0];
                ends[i] = routes[i][1];
                tempLocations[i] = starts[i];
                deltaLats[i] = (ends[i].Lat - starts[i].Lat) / totalCounts[i];
                deltaLngs[i] = (ends[i].Lng - starts[i].Lng) / totalCounts[i];
            }
            bus = new BitmapImage(new Uri(Environment.CurrentDirectory + "/Bus.png"));
            if (bus.CanFreeze)
                bus.Freeze();
        }

        public override void OnUpdateMap(MapArea viewarea)
        {
            for (int i = 0; i < busNum; i++)
            {
                tempLocations[i].Offset(deltaLats[i], deltaLngs[i]);
                currCounts[i]++;
                if (currCounts[i] >= totalCounts[i])
                {
                    currPt[i]++;
                    if (currPt[i] >= routes[i].Count - 1)
                    {
                        routes[i].Reverse();
                        currPt[i] = 0;
                    }
                    starts[i] = routes[i][currPt[i]];
                    ends[i] = routes[i][currPt[i] + 1];
                    tempLocations[i] = starts[i];
                    totalCounts[i] = (int)(TimeSpan.FromMilliseconds(rand.Next(2000, 6000)).TotalMilliseconds / deltaTime.TotalMilliseconds);
                    deltaLats[i] = (ends[i].Lat - starts[i].Lat) / totalCounts[i];
                    deltaLngs[i] = (ends[i].Lng - starts[i].Lng) / totalCounts[i];
                    currCounts[i] = 0;
                }
            }
        }

        public override void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            for (int i = 0; i < busNum; i++)
            {
                PointInt point;
                int x, y;
                BingMapTileSystem.LatLngToPixelXY(tempLocations[i].Lng, tempLocations[i].Lat, viewarea.Level, out x, out y);
                point = new PointInt(x, y);
                Point viewpoint = new Point((point.X - viewarea.Area.X) * zoomRate, (point.Y - viewarea.Area.Y) * zoomRate);
                drawingContext.DrawImage(bus, new Rect(viewpoint.X - 5, viewpoint.Y - 5, 20, 20));
            }
            base.Draw(drawingContext, viewarea, zoomRate);
        }

        public override bool HitTest(MapLocation pos, InputEventType inputtype)
        {
            return base.HitTest(pos, inputtype);
        }

        #region mock data

        GeoRoute r1 = new GeoRoute(new PointLatLng[]{
            new PointLatLng(30.63097, 114.38736),
            new PointLatLng(30.63097, 114.39176),
            new PointLatLng(30.63097, 114.39621),
            new PointLatLng(30.62878, 114.39621),
            new PointLatLng(30.62539, 114.39621),
            new PointLatLng(30.62539, 114.40073),
            new PointLatLng(30.62341, 114.40073),
            new PointLatLng(30.62341, 114.40524),
            new PointLatLng(30.62132, 114.40524),
            new PointLatLng(30.61936, 114.40524),
            });
        GeoRoute r2 = new GeoRoute(new PointLatLng[] {
            new PointLatLng(30.63097, 114.38736),
            new PointLatLng(30.62878, 114.38736),
            new PointLatLng(30.62539, 114.38736),
            new PointLatLng(30.62539, 114.39176),
            new PointLatLng(30.62341, 114.39176),
            new PointLatLng(30.62341, 114.39621),
            new PointLatLng(30.62132, 114.39621),
            new PointLatLng(30.62132, 114.40073),
            new PointLatLng(30.61936, 114.40073),
            new PointLatLng(30.61936, 114.40524),
        });
        //GeoRoute r3 = new GeoRoute(new PointLatLng[] {
        //    new PointLatLng(30.61936, 114.38736),
        //    new PointLatLng(30.62132, 114.38736),
        //    new PointLatLng(30.62341, 114.38736),
        //    new PointLatLng(30.62341, 114.39176),
        //    new PointLatLng(30.62539, 114.39176),
        //    new PointLatLng(30.62539, 114.39621),
        //    new PointLatLng(30.62878, 114.39621),
        //    new PointLatLng(30.62878, 114.40073),
        //    new PointLatLng(30.63097, 114.40073),
        //    new PointLatLng(30.63097, 114.40524),
        //});
        //GeoRoute r4 = new GeoRoute(new PointLatLng[] {
        //    new PointLatLng(30.61936, 114.38736),
        //    new PointLatLng(30.61936, 114.39176),
        //    new PointLatLng(30.62132, 114.39176),
        //    new PointLatLng(30.62132, 114.39621),
        //    new PointLatLng(30.62341, 114.39621),
        //    new PointLatLng(30.62341, 114.40073),
        //    new PointLatLng(30.62539, 114.40073),
        //    new PointLatLng(30.62878, 114.40073),
        //    new PointLatLng(30.62878, 114.40524),
        //    new PointLatLng(30.63097, 114.40524),
        //});

        #endregion
    }
}
