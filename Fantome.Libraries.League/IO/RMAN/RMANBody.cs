using System;
using System.Collections.Generic;
using FlatSharp.Attributes;

namespace Fantome.Libraries.League.IO.RMAN
{
    [FlatBufferTable]
    public class RMANBody : object
    {
        [FlatBufferItem(0)] public virtual IList<RMANBundle> Bundles { get; set; }
        [FlatBufferItem(1)] public virtual IList<RMANLanguage> Langs { get; set; }
        [FlatBufferItem(2)] public virtual IList<RMANFileEntry> Files { get; set; }
        [FlatBufferItem(3)] public virtual IList<RMANDirectory> Dirs { get; set; }
        [FlatBufferItem(4)] public virtual IList<RMANEncryptionKey> EncryptionKeys { get; set; }
        [FlatBufferItem(5)] public virtual IList<RMANChunkingParameters> Parameters { get; set; }
    }
}
