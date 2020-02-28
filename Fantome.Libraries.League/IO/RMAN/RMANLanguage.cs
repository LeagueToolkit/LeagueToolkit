using System;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.RMAN
{
    [FlatBufferTable]
    public class RMANLanguage : object
    {
        [FlatBufferItem(0)] public virtual byte Id { get; set; }
        [FlatBufferItem(1)] public virtual string Name { get; set; }
    }
}
