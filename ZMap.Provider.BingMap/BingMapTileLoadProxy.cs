using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace ZMap.Provider.BingMap
{
    public class BingMapTileLoadProxy : TileLoadProxy
    {
        /// <summary>
        /// the version of BingMap
        /// </summary>
        public string VersionBingMaps = "387";
        /// <summary>
        /// Bing Maps Customer Identification, more info here
        /// http://msdn.microsoft.com/en-us/library/bb924353.aspx
        /// </summary>
        public string BingMapsClientToken = null;
        /// <summary>
        /// Gets or sets the value of the User-agent HTTP header.
        /// </summary>
        public string UserAgent = "Opera/9.62 (Windows NT 5.1; U; en) Presto/2.1.1";
        /// <summary>
        /// timeout for map connections
        /// </summary>
        public int Timeout = 30 * 1000;
        /// <summary>
        /// proxy for net access
        /// </summary>
        public IWebProxy Proxy = null;

        string LanguageStr;
        LanguageType language = LanguageType.English;

        /// <summary>
        /// map language
        /// </summary>
        public LanguageType Language
        {
            get { return language; }
            set
            {
                language = value;
                LanguageStr = language.GetEnumDescription();
            }
        }

        public BingMapTileLoadProxy(AccessMode mode)
            : base(mode)
        { }

        public override MemoryStream GetTileFromServer(RawTile key)
        {
            if ((mode & AccessMode.Server) != AccessMode.Server)
                return null;
            MemoryStream ms = null;
            string url = MakeImageUrl(key, LanguageStr);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (Proxy != null)
                {
                    request.Proxy = Proxy;
                    request.PreAuthenticate = true;
                }
                else
                    request.Proxy = WebRequest.DefaultWebProxy;

                request.UserAgent = UserAgent;
                request.Timeout = Timeout;
                request.ReadWriteTimeout = Timeout * 6;
                request.KeepAlive = true;
                request.Referer = "http://www.bing.com/maps/";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    ms = Stuff.CopyStream(response.GetResponseStream(), false);
                    if (ms != null)
                    {
                        if ((mode & AccessMode.DBCache) == AccessMode.DBCache)
                            dbCache.PutTileToCache(key, ms);
                        else
                            TileMemoryCache.Instance.Add(key, ms);
                    }
                    response.Close();
                    //GC.Collect();
                }
            }
            catch (Exception ex)
            {
                ms = null;
                Debug.WriteLine("Error in GetTileFromServer: " + ex.ToString());
            }
            return ms;
        }

        private string MakeImageUrl(RawTile tile, string language)
        {
            string key = BingMapTileSystem.TileXYToQuadKey(tile.TileXY, tile.Level);
            switch (tile.Type)
            {
                case MapType.BingMap:
                    return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/r{1}.png?g={2}&mkt={3}{4}",
                        GetServerNum(tile.TileXY, 4), key, VersionBingMaps, language, !string.IsNullOrEmpty(BingMapsClientToken) ? "&token=" + BingMapsClientToken : string.Empty);
                case MapType.BingSatellite:
                    return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/a{1}.jpeg?g={2}&mkt={3}{4}",
                        GetServerNum(tile.TileXY, 4), key, VersionBingMaps, language, !string.IsNullOrEmpty(BingMapsClientToken) ? "&token=" + BingMapsClientToken : string.Empty);
                case MapType.BingHybrid:
                    return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/h{1}.jpeg?g={2}&mkt={3}{4}",
                        GetServerNum(tile.TileXY, 4), key, VersionBingMaps, language, !string.IsNullOrEmpty(BingMapsClientToken) ? "&token=" + BingMapsClientToken : string.Empty);
                case MapType.BingMapChinese:
                    return string.Format("http://r2.tiles.ditu.live.com/tiles/r{0}.png?g=41", key);
            }
            return null;
        }

        /// <summary>
        /// gets server num based on position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int GetServerNum(PointInt pos, int max)
        {
            return (pos.X + 2 * pos.Y) % max;
        }
    }
}
