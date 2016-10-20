using System.Collections.Generic;
using System.IO;

namespace ZMap
{
    public class TileMemoryCache
    {
        private Queue<RawTile> queue = new Queue<RawTile>();
        private Dictionary<RawTile, MemoryStream> resourses = new Dictionary<RawTile, MemoryStream>();
        private double cacheCapacity = 5120;
        private object syncRoot = new object();
        private static TileMemoryCache instance = new TileMemoryCache();

        private TileMemoryCache() { }

        ~TileMemoryCache()
        {
            foreach (RawTile tile in resourses.Keys)
            {
                resourses[tile].Dispose();
            }
            resourses.Clear();
            resourses = null;
        }

        private void Refresh()
        {
            while (CacheSize > cacheCapacity)
            {
                lock (syncRoot)
                {
                    RawTile first = queue.Dequeue();
                    using (MemoryStream ms = resourses[first])
                    {
                        resourses.Remove(first);
                        CacheSize -= ms.Length / 1024.0;
                    }
                }
                TileCount--;
                //GC.Collect();
            }
        }

        #region public

        public void Add(RawTile key, MemoryStream value)
        {
            lock (syncRoot)
            {
                if (resourses.ContainsKey(key))
                    return;
                queue.Enqueue(key);
                resourses.Add(key, value);
                CacheSize += value.Length / 1024.0;
                TileCount++;
                Refresh();
            }
        }

        public MemoryStream Find(RawTile key)
        {
            lock (syncRoot)
            {
                if (!queue.Contains(key))
                    return null;
                return resourses[key];
            }
        }

        private double cacheSize = 0;
        /// <summary>
        /// 当前使用的缓存大小
        /// </summary>
        public double CacheSize
        {
            get { return cacheSize; }
            set { cacheSize = value; }
        }

        private int tileCount = 0;
        /// <summary>
        /// 缓存中图片的数量
        /// </summary>
        public int TileCount
        {
            get { return tileCount; }
            set { tileCount = value; }
        }

        /// <summary>
        /// the amount of tiles in MB to keep in memmory, default: 10MB, if each ~100Kb it's ~100 tiles
        /// </summary>
        public int CacheCapacity
        {
            get { return (int)(cacheCapacity / 1024); }
            set
            {
                cacheCapacity = value * 1024.0;
                Refresh();
            }
        }

        public static TileMemoryCache Instance
        {
            get { return instance; }
        }

        #endregion
    }
}
