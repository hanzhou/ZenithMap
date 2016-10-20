
namespace ZMap
{
    public enum InputEventType : int
    {
        MouseLeftButtonDown = 0x0001,
        MouseLeftButtonUp = 0x0002,
        MouseLeftButtonClick = 0x0004,
        MouseLeftButtonDoubleClick = 0x0008,

        MouseRightButtonDown = 0x0010,
        MouseRightButtonUp = 0x0020,
        MouseRightButtonClick = 0x0040,
        MouseRightButtonDoubleClick = 0x0080,

        MouseDown = 0x0011,
        MouseUp = 0x0022,
        MouseClick = 0x0044,
        MouseDoubleClick = 0x0088,

        MouseMove = 0x0100,

        TouchDown = 0x1000,
        TouchUp = 0x2000,
        TouchMove = 0x4000,
    }
}
