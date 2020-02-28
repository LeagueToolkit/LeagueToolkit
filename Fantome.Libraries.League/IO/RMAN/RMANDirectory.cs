using System;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.RMAN
{
    [FlatBufferTable]
    public class RMANDirectory : object
    {
        [FlatBufferItem(0)] public virtual ulong Id { get; set; }
        [FlatBufferItem(1)] public virtual ulong ParentId { get; set; }
        [FlatBufferItem(2)] public virtual string Name { get; set; }
    }
}
