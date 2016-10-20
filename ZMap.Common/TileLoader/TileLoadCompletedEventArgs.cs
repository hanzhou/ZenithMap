using System;
using System.ComponentModel;
using System.IO;

namespace ZMap
{
    public class TileLoadCompletedEventArgs : AsyncCompletedEventArgs
    {
        private RawTile tile;
        private MemoryStream source;

        public TileLoadCompletedEventArgs(RawTile tileKey, MemoryStream stream, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            tile = tileKey;
            this.source = stream;
        }

        public RawTile TileKey
        {
            get { return new RawTile(tile.Type, tile.TileXY, tile.Level); }
        }

        public MemoryStream Source
        {
            get
            {
                RaiseExceptionIfNecessary();
                MemoryStream s = source;
                return s;
            }
        }
    }
}
