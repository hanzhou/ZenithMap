using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ZMap.Provider.BingMap;
using System.Diagnostics;

namespace ZMap.Test
{
    public class TileFileCache : ITileDBCache
    {
        private string directory = Environment.CurrentDirectory + "/Cache";//ConfigurationManager.AppSettings["CacheDirectory"];
        //private string dataInfoFileName = "TilesInfo.xml";
        private string infoFilePath = Environment.CurrentDirectory + "/Cache/TilesInfo.xml";
        private List<string> QuadKeyList = new List<string>();
        XDocument infofile;

        public TileFileCache()
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (!File.Exists(infoFilePath))
            {
                StreamWriter sw = File.CreateText(infoFilePath);
                sw.Write("<TileSet></TileSet>");
                sw.Flush();
                sw.Close();
            }

            using (FileStream fsInfo = File.OpenRead(infoFilePath))
            {
                infofile = XDocument.Load(fsInfo);
            }
            //infofile = XDocument.Load(infoFilePath);
            var tResult = from t in infofile.Descendants("Tile")
                          select t;
            foreach (var item in tResult)
            {
                QuadKeyList.Add(item.Value);
            }
        }

        #region ITileDBCache Members

        public bool PutTileToCache(RawTile key, MemoryStream data)
        {
            string quadkey = BingMapTileSystem.TileXYToQuadKey(key.TileXY, key.Level);
            if (QuadKeyList.Contains(quadkey))
                return false;
            string path = string.Format("{0}/{1}.png", directory, quadkey);
            FileStream fs = File.OpenWrite(path);
            data.CopyTo(fs);
            fs.Flush();
            fs.Close();
            QuadKeyList.Add(quadkey);
            infofile.Element("TileSet").Add(new XElement("Tile") { Value = quadkey });

            try
            {
                using (FileStream fsInfo = File.OpenWrite(infoFilePath))
                {
                    infofile.Save(fsInfo);
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Error in PutTileToCache: " + e.ToString());
            }

            return true;
        }

        public MemoryStream GetTileFromCache(RawTile key)
        {
            string quadkey = BingMapTileSystem.TileXYToQuadKey(key.TileXY, key.Level);
            if (!QuadKeyList.Contains(quadkey))
                return null;
            string path = string.Format("{0}/{1}.png", directory, quadkey);
            FileStream fs = File.OpenRead(path);
            MemoryStream ms = new MemoryStream();
            fs.CopyTo(ms);
            fs.Dispose();
            fs.Close();
            return ms;
        }

        #endregion
    }
}
