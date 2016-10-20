using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZMap.Provider.BingMap;
using ZMap.Provider.SingleImageMap;

namespace ZMap.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            int Binglevel = 16;
            map.InitMap(new BingMapTileLayer(new BingMapTileLoadProxy(AccessMode.All) { DBCache = new TileFileCache() }));
            map.Config(new MapLayerConfig(1, 17, Constants.MinZoomRate, Constants.MaxZoomRate, MapType.BingMapChinese));
            map.GotoPosition(BingMapTileSystem.LatLngToPixelXY(new PointLatLng(114.3961, 30.6254), Binglevel), Binglevel);
            map.AddLayer(new FpsCounterLayer());
            //map.AddLayer(new MouseMoveTestLayer());
            //map.AddLayer(new UIElementLayer(map));
            //map.AddMapLayer(new MovingObjectLayer());

            //int GNGlevel = 0;
            //SingleMapTransformSystem.Init(new SizeInt(800, 600), new SizeD(240, 180));
            //map.InitMap(new ImageMapLayer(map));
            //MapLayerConfig config = MapLayerConfig.DefaultConfig;
            //config.MapType = MapType.SingleImageMap;
            //map.Config(config);
            //map.GotoPosition(SingleMapTransformSystem.ModelXYToPixelXY(new PointLatLng(120, 90)), GNGlevel);
            //map.AddLayer(new FpsCounterLayer());

            SetBindings();
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

            //LatitudeText.DataContext = map;
            //LatitudeText.SetBinding(TextBlock.TextProperty, new Binding("CurrentPosition.Lat"));
            //LongitudeText.DataContext = map;
            //LongitudeText.SetBinding(TextBlock.TextProperty, new Binding("CurrentPosition.Lng"));
        }
    }
}
