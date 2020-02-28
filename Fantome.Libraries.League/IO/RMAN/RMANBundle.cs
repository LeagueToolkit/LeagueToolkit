using System;
using System.Collections.Generic;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.RMAN
{
    [FlatBufferTable]
    public class RMANBundle : object
    {
        [FlatBufferItem(0)] public virtual ulong Id { get; set; }
        [FlatBufferItem(1)] public virtual IList<RMANBundleChunk> Chunks { get; set; }
    }
}
