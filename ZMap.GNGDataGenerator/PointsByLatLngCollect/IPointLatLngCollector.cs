using System.Collections.Generic;

namespace ZMap.GNGDataGenerator
{
    public interface IPointLatLngCollector : IEnumerable<PointLatLng>
    {
        void Add(double lng, double lat);
        void Add(PointLatLng ptll);
        void Undo();
        void Save(string path);
        void Clear();
        List<PointLatLng> ToList();

        int Count { get; }
    }
}
