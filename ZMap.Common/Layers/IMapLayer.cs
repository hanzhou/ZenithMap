using System;

namespace ZMap
{
    public interface IMapLayer : ILayer
    {
        MapType MapType { get; set; }
        int MinLevel { get; }
        int MaxLevel { get; }
        double MinZoomRate { get; }
        double MaxZoomRate { get; }

        event MapTypeChanged MapTypeChanged;

        void OnUpdateMap(MapArea viewarea);
    }
}