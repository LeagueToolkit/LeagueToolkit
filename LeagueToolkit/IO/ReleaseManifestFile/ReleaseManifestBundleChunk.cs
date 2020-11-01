using System;
using FlatSharp.Attributes;

namespace LeagueToolkit.IO.ReleaseManifestFile
{
    [FlatBufferTable]
    public class ReleaseManifestBundleChunk
    {
        [FlatBufferItem(0)] public virtual ulong ID { get; set; }
        [FlatBufferItem(1)] public virtual uint CompressedSize { get; set; }
        [FlatBufferItem(2)] public virtual uint UncompressedSize { get; set; }
    }
}
