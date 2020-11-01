using System;
using System.Collections.Generic;
using FlatSharp.Attributes;

namespace LeagueToolkit.IO.ReleaseManifestFile
{
    [FlatBufferTable]
    public class ReleaseManifestFile
    {
        [FlatBufferItem(0)] public virtual ulong ID { get; set; }
        [FlatBufferItem(1)] public virtual ulong ParentID { get; set; }
        [FlatBufferItem(2)] public virtual uint Size { get; set; }
        [FlatBufferItem(3)] public virtual string Name { get; set; }
        [FlatBufferItem(4)] public virtual ulong LanguageFlags { get; set; }
        [FlatBufferItem(5)] public virtual byte Unknown5 { get; set; } // Type/Size unknown
        [FlatBufferItem(6)] public virtual byte Unknown6 { get; set; } // Type/Size unknown
        [FlatBufferItem(7)] public virtual IList<ulong> ChunkIDs { get; set; }
        [FlatBufferItem(8)] public virtual byte Unk8 { get; set; }
        [FlatBufferItem(9)] public virtual string Link { get; set; }
        [FlatBufferItem(10)] public virtual byte Unknown10 { get; set; } // Type/Size unknown
        [FlatBufferItem(11)] public virtual byte ChunkingParametersIndex { get; set; }
        [FlatBufferItem(12)] public virtual byte Permissions { get; set; }
    }
}
