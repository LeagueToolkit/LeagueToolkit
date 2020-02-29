using System;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.ReleaseManifest
{
    [FlatBufferTable]
    public class ReleaseManifestLanguage
    {
        [FlatBufferItem(0)] public virtual byte ID { get; set; }
        [FlatBufferItem(1)] public virtual string Name { get; set; }
    }
}
