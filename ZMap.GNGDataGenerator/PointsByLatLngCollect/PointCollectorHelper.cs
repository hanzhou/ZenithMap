using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace ZMap.GNGDataGenerator
{
    public static class PointCollectorHelper
    {

        public static void Save(IEnumerable<PointLatLng> points, string filepath)
        {
            using (StreamWriter sw = File.CreateText(filepath))
            {
                XDocument heatPointFile = new XDocument();
                XElement root = new XElement("Root");
                foreach (PointLatLng point in points)
                {
                    XElement pointlatlng = new XElement("PointLatLng");
                    pointlatlng.Add(new XElement("Lng", point.Lng));
                    pointlatlng.Add(new XElement("Lat", point.Lat));
                    root.Add(pointlatlng);
                }
                heatPointFile.Add(root);
                heatPointFile.Save(sw);
                sw.Close();
            }
        }

        public static void SaveBusStop(IEnumerable<PointLatLng> points, string filepath)
        {
            using (StreamWriter sw = File.CreateText(filepath))
            {
                XDocument heatPointFile = new XDocument();
                XElement root = new XElement("Root");
                foreach (PointLatLng point in points)
                {
                    XElement busstop = new XElement("BusStop");
                    XElement id = new XElement("ID");
                    id.Value = Guid.NewGuid().ToString();
                    XElement name = new XElement("Name");
                    name.Value = "";
                    XElement pointlatlng = new XElement("PointLatLng");
                    pointlatlng.Add(new XElement("Lng", point.Lng));
                    pointlatlng.Add(new XElement("Lat", point.Lat));
                    busstop.Add(id);
                    busstop.Add(name);
                    busstop.Add(pointlatlng);
                    root.Add(busstop);
                }
                heatPointFile.Add(root);
                heatPointFile.Save(sw);
                sw.Close();
            }
        }

        public static List<PointLatLng> Load(string path)
        {
            List<PointLatLng> points = new List<PointLatLng>();
            XDocument infofile = XDocument.Load(path);
            var tResult = from t in infofile.Descendants("PointLatLng")
                          select t;
            foreach (var item in tResult)
                points.Add(new PointLatLng(Convert.ToDouble(item.Element("Lng").Value), Convert.ToDouble(item.Element("Lat").Value)));
            return points;
        }
    }
}
