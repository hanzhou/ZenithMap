using System;
using System.Windows.Media;

namespace ZMap
{
    public class Layer : ILayer
    {
        #region fields

        private string name = "";
        private object tag = null;

        private bool isVisible = true;
        private double minVisible = 0;
        private double maxVisible = Double.MaxValue;
        private int z_Index = 0;

        private event LayerVisibleChanged visibleChanged;
        private event LayerNeedRendered viewNeedRendered;
        private event LayerRendered viewRendered;

        protected object eventLock = new object();

        public MapBase MapCore { protected get; set; }

        #endregion

        protected virtual void OnVisibleChanged(bool visible)
        {
            LayerVisibleChanged temp = visibleChanged;
            if (temp != null) temp(this, visible);
        }

        protected virtual void OnViewNeedRendered()
        {
            LayerNeedRendered temp = viewNeedRendered;
            if (temp != null) temp(this);
        }

        protected virtual void OnViewRendered()
        {
            LayerRendered temp = viewRendered;
            if (temp != null) temp(this);
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        #region ILayer Members

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnVisibleChanged(value);
            }
        }

        public double MinVisible
        {
            get { return minVisible; }
            protected set { minVisible = value; }
        }

        public double MaxVisible
        {
            get { return maxVisible; }
            protected set { maxVisible = value; }
        }

        public int ZIndex
        {
            get { return z_Index; }
            set { z_Index = value; }
        }

        public event LayerVisibleChanged VisibleChanged
        {
            add { lock (eventLock) { visibleChanged += value; } }
            remove { lock (eventLock) { visibleChanged -= value; } }
        }

        public event LayerNeedRendered NeedRendered
        {
            add { lock (eventLock) { viewNeedRendered += value; } }
            remove { lock (eventLock) { viewNeedRendered -= value; } }
        }

        public event LayerRendered Rendered
        {
            add { lock (eventLock) { viewRendered += value; } }
            remove { lock (eventLock) { viewRendered -= value; } }
        }

        public virtual bool HitTest(MapLocation pos, InputEventType inputtype)
        {
            return false;
        }

        public virtual void Draw(DrawingContext drawingContext, MapArea viewarea, double zoomRate)
        {
            OnViewRendered();
        }

        #endregion
    }
}
