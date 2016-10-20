using System;

namespace ZMap
{
    public interface IZMapBasicControl
    {
        bool IsDragging { get; }
        
        void BeginDrag(PointD startPos);
        void Drag(PointD newPos);
        void EndDrag();

        void StartZoom(PointD zoomCenter, double relativeZoomRate);
    }
}