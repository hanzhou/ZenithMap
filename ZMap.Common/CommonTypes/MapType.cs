using System;

namespace ZMap
{
    /// <summary>
    /// 地图类型
    /// </summary>
    public enum MapType : int
    {
        Invalid = 0,
        SingleImageMap = 1,
        BingMap = 100,
        BingMapChinese = 110,
        BingSatellite = 200,
        BingHybrid = 300,
    }
}
