using System.IO;

namespace ZMap
{
    public abstract class TileLoadProxy
    {
        protected AccessMode mode;
        protected ITileDBCache dbCache = null;

        protected TileLoadProxy(AccessMode mode)
        {
            this.mode = mode;
        }

        protected MemoryStream GetTileFromMemoryCache(RawTile key)
        {
            if ((mode & AccessMode.Memory) != AccessMode.Memory)
                return null;
            return TileMemoryCache.Instance.Find(key);
        }

        protected MemoryStream GetTileFromDBCache(RawTile key)
        {
            if ((mode & AccessMode.DBCache) != AccessMode.DBCache || dbCache == null)
                return null;
            MemoryStream ret = dbCache.GetTileFromCache(key);
            if (ret != null)
                TileMemoryCache.Instance.Add(key, ret);
            return ret;
        }

        #region public

        public static TileLoadProxy Null = new NullTileLoadProxy();

        public ITileDBCache DBCache
        {
            get { return dbCache; }
            set { dbCache = value; }
        }

        public abstract MemoryStream GetTileFromServer(RawTile key);

        public MemoryStream GetTile(RawTile key)
        {
            MemoryStream ret = GetTileFromMemoryCache(key);
            if (ret != null)
                return ret;
            ret = GetTileFromDBCache(key);
            if (ret != null)
                return ret;
            ret = GetTileFromServer(key);
            return ret;
        }

        #endregion
    }

    public sealed class NullTileLoadProxy : TileLoadProxy
    {

        public NullTileLoadProxy()
            : base(AccessMode.None)
        { }

        public override MemoryStream GetTileFromServer(RawTile key)
        {
            return null;
        }

        public new ITileDBCache DBCache
        {
            get { return null; }
            set { }
        }

        public new MemoryStream GetTile(RawTile key)
        {
            return null;
        }
    }
}
