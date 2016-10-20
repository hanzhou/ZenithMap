
namespace ZMap
{
    //public delegate void MapLoadStarted();
    //public delegate void MapLoadCompleted();

    //public delegate void MapDragged();
    public delegate void MapLevelChanged(int newLevel);
    public delegate void MapTypeChanged(MapType newType);
    public delegate void MapNeedRepainted();
    public delegate void MapRepainted(MapArea viewarea, double zoomrate);
    public delegate void MapViewPositionChanged(PointInt newPosition);

    public delegate void LayerVisibleChanged(ILayer layer, bool newVisible);
    public delegate void LayerNeedRendered(ILayer layer);
    public delegate void LayerRendered(ILayer layer);
}
