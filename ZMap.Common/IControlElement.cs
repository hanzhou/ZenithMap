using System;

namespace ZMap
{
    public interface IControlElement
    {
        PointLatLng Location { get; }
        PointD Offset { get; set; }
    }
}
