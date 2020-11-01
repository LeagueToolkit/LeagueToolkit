using System;
using System.Collections.Generic;
using FlatSharp.Attributes;

namespace LeagueToolkit.IO.ReleaseManifestFile
{
    [FlatBufferTable]
    public class ReleaseManifestBody
    {
        [FlatBufferItem(0)] public virtual IList<ReleaseManifestBundle> Bundles { get; set; }
        [FlatBufferItem(1)] public virtual IList<ReleaseManifestLanguage> Languages { get; set; }
        [FlatBufferItem(2)] public virtual IList<ReleaseManifestFile> Files { get; set; }
        [FlatBufferItem(3)] public virtual IList<ReleaseManifestDirectory> Directories { get; set; }
        [FlatBufferItem(4)] public virtual IList<ReleaseManifestEncryptionKey> EncryptionKeys { get; set; }
        [FlatBufferItem(5)] public virtual IList<ReleaseManifestChunkingParameter> ChunkingParameters { get; set; }
    }
}
