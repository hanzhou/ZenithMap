using System;
using System.Windows.Media;

namespace ZMap
{
    public interface IZMap
    {
        /// <summary>
        /// Level of detail
        /// </summary>
        int Level { get; }
        /// <summary>
        /// get or set the center of the logical rect of the map view on the whole map, in pixels[viewCenterOnMap]
        /// 设置时，视图转移到当前 Level 下的指定像素位置
        /// </summary>
        PointInt CurrentPosition { get; }
        /// <summary>
        /// 更新地图显示区域大小，与界面大小保持一致
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void OnUpdateMapViewSize(double width, double height);
        void DrawMap(DrawingContext drawingContext);
        void HitTest(MapLocation pos, InputEventType inputtype);//功能扩展
        /// <summary>
        /// 将地图显示到指定 Level 下的指定位置
        /// </summary>
        /// <param name="level"></param>
        /// <param name="position"></param>
        void GoToPosition(MapLocation pos);

        event MapNeedRepainted NeedRepainted;
        event MapRepainted Repainted;
        event MapViewPositionChanged CurrentPositionChanged;
        event MapLevelChanged LevelChanged;
    }
}