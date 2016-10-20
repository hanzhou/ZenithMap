using System.Windows.Controls;

namespace ZMap.WPFControl
{
    public class ControlElement : UserControl, IControlElement
    {
        private PointD offset = new PointD();

        public ControlElement(PointLatLng location)
        {
            Location = location;
        }
        
        #region IControlElement Members

        public PointLatLng Location
        {
            get;
            private set;
        }

        public PointD Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        #endregion
    }
}
