using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZMap.GNGDataGenerator
{
    public class PointsCollector : IPointLatLngCollector
    {
        private Stack<PointLatLng> stack = new Stack<PointLatLng>();

        public PointsCollector() { }

        #region IPointLatLngCollector Members

        public void Add(double lng, double lat)
        {
            stack.Push(new PointLatLng(lng, lat));
        }

        public void Add(PointLatLng ptll)
        {
            stack.Push(ptll);
        }

        public void Undo()
        {
            if (stack.Count == 0)
                return;
            stack.Pop();
        }

        public virtual void Save(string path)
        {
            //PointCollectorHelper.Save(stack, InfoFilePath);
            PointCollectorHelper.SaveBusStop(stack, path);
        }

        public void Clear()
        {
            stack.Clear();
        }

        public List<PointLatLng> ToList()
        {
            return stack.ToList<PointLatLng>();
        }

        public int Count
        {
            get { return stack.Count; }
        }

        #endregion

        IEnumerator<PointLatLng> IEnumerable<PointLatLng>.GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        public IEnumerable<PointLatLng> GetEnumerator()
        {
            return stack;
        }
    }
}