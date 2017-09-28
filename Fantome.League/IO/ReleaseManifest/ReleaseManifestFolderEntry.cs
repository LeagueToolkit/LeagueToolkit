using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Fantome.Libraries.League.IO.ReleaseManifest
{
    /// <summary>
    /// Contains information about folders (name, files, subfolders).
    /// </summary>
    public class ReleaseManifestFolderEntry
    {
        /// <summary>
        /// Name of the current <see cref="ReleaseManifestFolderEntry"/>.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Position of the <see cref="Name"/> in <see cref="Names"/>.
        /// </summary>
        internal int NameIndex { get; private set; }

        /// <summary>
        /// Position of the first subfolder of the current <see cref="ReleaseManifestFolderEntry"/> in the folders buffer (used for reading and writing the manifest).
        /// </summary>
        internal int SubFolderStartIndex { get; set; }

        /// <summary>
        /// Number of subfolders of the current <see cref="ReleaseManifestFolderEntry"/> (used for reading and writing the manifest).
        /// </summary>
        internal int SubFolderCount { get; private set; }

        /// <summary>
        /// Position of the first file in the current <see cref="ReleaseManifestFolderEntry"/> in the files buffer (used for reading and writing the manifest).
        /// </summary>
        internal int FileListStartIndex { get; set; }

        /// <summary>
        /// Number of files in the current <see cref="ReleaseManifestFolderEntry"/> (used for reading and writing the manifest).
        /// </summary>
        internal int FileCount { get; private set; }

        /// <summary>
        /// Parent folder of the current <see cref="ReleaseManifestFolderEntry"/>.
        /// </summary>
        public ReleaseManifestFolderEntry Parent { get; internal set; }

        /// <summary>
        /// Editable list of subfolders of the current <see cref="ReleaseManifestFolderEntry"/> used internally.
        /// </summary>
        internal readonly List<ReleaseManifestFolderEntry> _folders = new List<ReleaseManifestFolderEntry>();

        /// <summary>
        /// List of subfolders of the current <see cref="ReleaseManifestFolderEntry"/>.
        /// </summary>
        public readonly ReadOnlyCollection<ReleaseManifestFolderEntry> Folders;

        /// <summary>
        /// Editable list of files of the current <see cref="ReleaseManifestFolderEntry"/> used internally.
        /// </summary>
        internal readonly List<ReleaseManifestFileEntry> _files = new List<ReleaseManifestFileEntry>();

        /// <summary>
        /// List of files of the current <see cref="ReleaseManifestFolderEntry"/>.
        /// </summary>
        public readonly ReadOnlyCollection<ReleaseManifestFileEntry> Files;


        private ReleaseManifestFolderEntry()
        {
            Folders = _folders.AsReadOnly();
            Files = _files.AsReadOnly();
        }

        /// <summary>
        /// Parses Release Manifest Folder Entry content from a previously initialized <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br"><see cref="BinaryReader"/> instance holding Release Manifest File content.</param>
        public ReleaseManifestFolderEntry(BinaryReader br) : this()
        {
            this.NameIndex = br.ReadInt32();
            this.SubFolderStartIndex = br.ReadInt32();
            this.SubFolderCount = br.ReadInt32();
            this.FileListStartIndex = br.ReadInt32();
            this.FileCount = br.ReadInt32();
        }

        /// <summary>
        /// Creates a new <see cref="ReleaseManifestFolderEntry"/> from its name and its folder.
        /// </summary>
        /// <param name="name">Name of the folder.</param>
        /// <param name="nameIndex">Position of the name of the folder in <see cref="Names"/></param>
        /// <param name="folder"><see cref="ReleaseManifestFolderEntry"/> the new folder belongs to.</param>
        public ReleaseManifestFolderEntry(string name, int nameIndex, ReleaseManifestFolderEntry parent) : this()
        {
            this.Name = name;
            this.NameIndex = nameIndex;
            this.Parent = parent;
        }

        /// <summary>
        /// Writes content from the current <see cref="ReleaseManifestFolderEntry"/> in a previously initialized <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="bw"><see cref="BinaryWriter"/> instance where to write data.</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.NameIndex);
            bw.Write(this.SubFolderStartIndex);
            bw.Write(this.Folders.Count);
            bw.Write(this.FileListStartIndex);
            bw.Write(this.Files.Count);
        }

        /// <summary>
        /// Returns the full path (with parent folders) of the current <see cref="ReleaseManifestFolderEntry"/>.
        /// </summary>
        /// <returns></returns>
        public string GetFullPath()
        {
            if (this.Parent?.Parent != null)
            {
                return this.Parent.GetFullPath() + "/" + this.Name;
            }
            else
            {
                return this.Name;
            }
        }

        /// <summary>
        /// Removes the current <see cref="ReleaseManifestFolderEntry"/> from its parent folder.
        /// </summary>
        public void Remove()
        {
            if (this.Parent._folders.Contains(this))
            {
                this.Parent._folders.Remove(this);
            }
        }
    }
}
