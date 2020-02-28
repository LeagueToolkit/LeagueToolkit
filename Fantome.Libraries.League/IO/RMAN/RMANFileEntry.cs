using System;
using System.Collections.Generic;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.RMAN
{
    [FlatBufferTable]
    public class RMANFileEntry : object
    {
        [FlatBufferItem(0)] public virtual ulong Id { get; set; }
        [FlatBufferItem(1)] public virtual ulong ParentId { get; set; }
        [FlatBufferItem(2)] public virtual uint Size { get; set; }
        [FlatBufferItem(3)] public virtual string Name { get; set; }
        [FlatBufferItem(4)] public virtual ulong LangFlags { get; set; }
        [FlatBufferItem(7)] public virtual IList<ulong> ChunkIds { get; set; }
        [FlatBufferItem(9)] public virtual string Link { get; set; }
    }
}
