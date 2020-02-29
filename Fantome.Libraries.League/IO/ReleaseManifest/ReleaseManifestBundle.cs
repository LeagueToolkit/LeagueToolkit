using System;
using System.Collections.Generic;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.ReleaseManifest
{
    [FlatBufferTable]
    public class ReleaseManifestBundle
    {
        [FlatBufferItem(0)] public virtual ulong ID { get; set; }
        [FlatBufferItem(1)] public virtual IList<ReleaseManifestBundleChunk> Chunks { get; set; }
    }
}
