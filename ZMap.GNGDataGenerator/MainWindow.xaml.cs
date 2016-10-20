using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZMap.Provider.BingMap;

namespace ZMap.GNGDataGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPointLatLngCollector collector = new PointsCollector();

        BusStopCollectLayer stopcollectorlayer;

        public MainWindow()
        {
            InitializeComponent();

            int Binglevel = 16;
            map.InitMap(new BingMapTileLayer(new BingMapTileLoadProxy(AccessMode.MemoryandServer)));
            map.Config(new MapLayerConfig(1, 17, Constants.MinZoomRate, Constants.MaxZoomRate, MapType.BingMapChinese));
            map.GotoPosition(BingMapTileSystem.LatLngToPixelXY(new PointLatLng(114.3961, 30.6254), Binglevel), Binglevel);
            //map.AddMapLayer(new HeatPointCollectorMapLayer(map, collector));
            //map.AddLayer(new PointCollectorLayer(collector));
            //map.AddLayer(new PolyLineCollectorLayer(collector));
            stopcollectorlayer = new BusStopCollectLayer();
            map.AddLayer(stopcollectorlayer);

            map.MouseLeftButtonDown += new MouseButtonEventHandler(map_MouseLeftButtonDown);
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            savePointBtn.Click += new RoutedEventHandler(savePointBtn_Click);
        }

        void savePointBtn_Click(object sender, RoutedEventArgs e)
        {
            //collector.Save();
            stopcollectorlayer.Save();
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Key == Key.Z)
                    collector.Undo();
                if (e.Key == Key.Up)
                    stopcollectorlayer.AddUp();
                if (e.Key == Key.Down)
                    stopcollectorlayer.AddDown();
                if (e.Key == Key.Left)
                    stopcollectorlayer.AddStart();
                if (e.Key == Key.Right)
                    stopcollectorlayer.AddEnd();
            }
        }

        void map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = BingMapTileSystem.PixelXYToLatLong(map.ClickPosition.Position, map.ClickPosition.Level);
            //collector.Add(point.Lng, point.Lat);
            stopcollectorlayer.SetTempPoint(point);
        }

        private void SetBindings()
        {
            map.LevelChanged += level => { MapLevelText.Text = level.ToString(); };

            TileCountText.DataContext = TileMemoryCache.Instance;
            TileCountText.SetBinding(TextBlock.TextProperty, new Binding("TileCount"));

            MapWidthText.DataContext = map;
            MapWidthText.SetBinding(TextBlock.TextProperty, new Binding("ActualWidth"));
            MapHeightText.DataContext = map;
            MapHeightText.SetBinding(TextBlock.TextProperty, new Binding("ActualHeight"));

            LatitudeText.DataContext = map;
            LatitudeText.SetBinding(TextBlock.TextProperty, new Binding("CurrentPosition.Lat"));
            LongitudeText.DataContext = map;
            LongitudeText.SetBinding(TextBlock.TextProperty, new Binding("CurrentPosition.Lng"));
        }
    }
}