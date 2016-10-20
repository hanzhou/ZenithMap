using System;

namespace ZMap
{
    [Flags]
    public enum AccessMode : byte
    {
        None = 0x00,
        /// <summary>
        /// memory cache only
        /// </summary>
        Memory = 0x01,
        /// <summary>
        /// DB cache only
        /// </summary>
        DBCache = 0x02,
        /// <summary>
        /// server only
        /// </summary>
        Server = 0x04,
        MemoryandDB = 0x03,
        MemoryandServer = 0x05,
        DBandServer = 0x06,
        All = 0x07
    }
}
