using System.Windows.Media;

namespace ZMap
{
    public interface ILayer
    {
        /// <summary>
        /// 获取或设置图层是否显示
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// 图层可显示的最小比例，比例是当前大小与最小地图(Level = 0, ZoomRate = 1)的比值
        /// </summary>
        double MinVisible { get; }

        /// <summary>
        /// 图层可显示的最大比例，比例是当前大小与最小地图(Level = 0, ZoomRate = 1)的比值
        /// </summary>
        double MaxVisible { get; }

        int ZIndex { get; set; }

        event LayerVisibleChanged VisibleChanged;
        /// <summary>
        /// 图层需要重新绘制
        /// </summary>
        event LayerNeedRendered NeedRendered;
        /// <summary>
        /// 图层绘制完毕
        /// </summary>
        event LayerRendered Rendered;
        
        bool HitTest(MapLocation pos, InputEventType inputtype);

        void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate);
    }
}
