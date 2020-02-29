using System;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.RMAN
{
    [FlatBufferTable]
    public class RMANChunkingParameters : object
    {
        [FlatBufferItem(0)] public virtual ushort Unknown0 { get; set; }
        [FlatBufferItem(1)] public virtual byte Unknown1 { get; set; }
        [FlatBufferItem(2)] public virtual byte Unknown2 { get; set; }
        [FlatBufferItem(3)] public virtual int Unknown3 { get; set; }
        [FlatBufferItem(4)] public virtual int MaxUncompressedSize { get; set; }
    }
}
