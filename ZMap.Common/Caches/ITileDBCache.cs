using System.IO;

namespace ZMap
{
    public interface ITileDBCache
    {
        bool PutTileToCache(RawTile key, MemoryStream data);
        MemoryStream GetTileFromCache(RawTile key);
    }
}
