using System.IO;
using System;
using static Fantome.Libraries.League.IO.ReleaseManifest.ReleaseManifestFile;

namespace Fantome.Libraries.League.IO.ReleaseManifest
{
    /// <summary>
    /// Contains information about files (name, version, size).
    /// </summary>
    public class ReleaseManifestFileEntry
    {
        /// <summary>
        /// Name of the current <see cref="ReleaseManifestFileEntry"/>.
        /// </summary>
        /// <example>room.wgeo</example>
        public string Name { get; internal set; }

        /// <summary>
        /// Position of the <see cref="Name"/> in <see cref="Names"/>.
        /// </summary>
        internal int NameIndex { get; private set; }

        /// <summary>
        /// File version in number.
        /// </summary>
        public uint Version { get; set; }

        private byte[] _MD5;

        /// <summary>
        /// MD5 checksum of the current <see cref="ReleaseManifestFileEntry"/> (uncompressed).
        /// </summary>
        public byte[] MD5
        {
            get { return _MD5; }
            set
            {
                if (value?.Length == 16)
                {
                    if (_MD5 == null)
                    {
                        _MD5 = value;
                    }
                    else
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            _MD5[i] = value[i];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deploy Mode of the current <see cref="ReleaseManifestFileEntry"/>.
        /// </summary>
        public DeployMode DeployMode { get; set; }

        /// <summary>
        /// Size of the current <see cref="ReleaseManifestFileEntry"/>.
        /// </summary>
        public int SizeRaw { get; set; }

        /// <summary>
        /// Size of the current <see cref="ReleaseManifestFileEntry"/> compressed in zlib.
        /// </summary>
        public int SizeCompressed { get; set; }

        /// <summary>
        /// Last write time of the current <see cref="ReleaseManifestFileEntry"/>.
        /// </summary>
        public DateTime LastWriteTime { get; set; } = new DateTime();

        /// <summary>
        /// The <see cref="ReleaseManifestFolderEntry"/> the current <see cref="ReleaseManifestFileEntry"/> belongs to.
        /// </summary>
        public ReleaseManifestFolderEntry Folder { get; internal set; }

        /// <summary>
        /// Parses Release Manifest File Entry content from a previously initialized <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br"><see cref="BinaryReader"/> instance holding Release Manifest File content</param>.
        public ReleaseManifestFileEntry(BinaryReader br)
        {
            this.NameIndex = br.ReadInt32();
            this.Version = br.ReadUInt32();
            this.MD5 = br.ReadBytes(16);
            this.DeployMode = (DeployMode)br.ReadUInt32();
            this.SizeRaw = br.ReadInt32();
            this.SizeCompressed = br.ReadInt32();
            long dateValue = br.ReadInt64();
            if (dateValue != 0)
            {
                // The date is definitely a date!
                this.LastWriteTime = DateTime.FromBinary(dateValue).AddYears(1600);
            }
        }

        /// <summary>
        /// Creates a new <see cref="ReleaseManifestFileEntry"/> from its name and its folder.
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <param name="nameIndex">Position of the name of the file in <see cref="Names"/>.</param>
        /// <param name="folder"><see cref="ReleaseManifestFolderEntry"/> the new file belongs to.</param>
        public ReleaseManifestFileEntry(string name, int nameIndex, ReleaseManifestFolderEntry folder)
        {
            this.Name = name;
            this.NameIndex = nameIndex;
            this.MD5 = new byte[16];
            this.Folder = folder;
        }

        /// <summary>
        /// Writes content from the current <see cref="ReleaseManifestFileEntry"/> in a previously initialized <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="bw"><see cref="BinaryWriter"/> instance where to write data.</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.NameIndex);
            bw.Write(this.Version);
            bw.Write(this.MD5);
            bw.Write((uint)this.DeployMode);
            bw.Write(this.SizeRaw);
            bw.Write(this.SizeCompressed);
            if (this.LastWriteTime.ToBinary() == 0)
            {
                bw.Write((long)0);
            }
            else
            {
                bw.Write(this.LastWriteTime.AddYears(-1600).ToBinary());
            }
        }

        /// <summary>
        /// Returns the full path (with folders) of the current <see cref="ReleaseManifestFileEntry"/>.
        /// </summary>
        public string GetFullPath()
        {
            if (this.Folder.Parent != null)
            {
                return this.Folder.GetFullPath() + "/" + this.Name;
            }
            else
            {
                return this.Name;
            }
        }

        /// <summary>
        /// Removes the current <see cref="ReleaseManifestFileEntry"/> from its folder.
        /// </summary>
        public void Remove()
        {
            if (this.Folder._files.Contains(this))
            {
                this.Folder._files.Remove(this);
            }
        }
    }
}
