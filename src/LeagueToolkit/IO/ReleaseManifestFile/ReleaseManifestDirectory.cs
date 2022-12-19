using System;
using FlatSharp.Attributes;

namespace LeagueToolkit.IO.ReleaseManifestFile
{
    [FlatBufferTable]
    public class ReleaseManifestDirectory
    {
        [FlatBufferItem(0)] public virtual ulong ID { get; set; }
        [FlatBufferItem(1)] public virtual ulong ParentID { get; set; }
        [FlatBufferItem(2)] public virtual string Name { get; set; }
    }
}
