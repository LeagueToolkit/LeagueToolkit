using System;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.RMAN
{
    [FlatBufferTable]
    public class RMANBundleChunk : object
    {
        [FlatBufferItem(0)] public virtual ulong Id { get; set; }
        [FlatBufferItem(1)] public virtual uint CompressedSize { get; set; }
        [FlatBufferItem(2)] public virtual uint UncompressedSize { get; set; }
    }
}
