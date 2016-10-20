
namespace ZMap
{
    public delegate void MapRendered(MapArea viewarea, double zoomrate);
    
    public interface IMapControl
    {
        int Level { get; }
        MapLocation ClickPosition { get; }
        int RepaintTimeDelay { get; set; }
        
        void InitMap(MapLayer maproot);
        void Config(MapLayerConfig config);
        void GotoPosition(PointInt centerPosition, int level);
        void AddMapLayer(MapLayer maplayer);
        void RemoveMapLayer(MapLayer maplayer);
        void AddLayer(Layer layer);
        void RemoveLayer(Layer layer);

        event MapLevelChanged LevelChanged;
        event MapRendered MapRendered;
    }
}