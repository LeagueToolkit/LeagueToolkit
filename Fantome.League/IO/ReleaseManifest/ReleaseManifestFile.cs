using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.ReleaseManifest
{
    /// <summary>
    /// League of Legends Release Manifest File containing data (files, folders) from a specific release.
    /// </summary>
    public class ReleaseManifestFile
    {
        /// <summary>
        /// File path of the parsed <see cref="ReleaseManifest"/>.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Major version of the parsed <see cref="ReleaseManifest"/>.
        /// </summary>
        public short MajorVersion { get; private set; }

        /// <summary>
        /// Minor version of the parsed <see cref="ReleaseManifest"/>.
        /// </summary>
        public short MinorVersion { get; private set; }

        /// <summary>
        /// League of Legends release version the current <see cref="ReleaseManifest"/> contains info from.
        /// </summary>
        /// <example>0.0.1.25</example>
        public uint ReleaseVersion { get; private set; }

        /// <summary>
        /// League of Legends project name the current <see cref="ReleaseManifest"/> contains info from.
        /// </summary>
        /// <example>lol_game_client</example>
        public string ProjectName { get; private set; }

        /// <summary>
        /// List of names the current <see cref="ReleaseManifest"/> uses.
        /// </summary>
        private readonly List<string> Names = new List<string>();

        /// <summary>
        /// Base <see cref="ReleaseManifestFolderEntry"/> containing files and folders from the project release.
        /// </summary>
        public ReleaseManifestFolderEntry Project { get; private set; }

        /// <summary>
        /// Parses a League of Legends Release Manifest File at <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">Path to the Release Manifest</param>
        public ReleaseManifestFile(string filePath)
        {
            this.FilePath = filePath;
            using (BinaryReader br = new BinaryReader(File.OpenRead(filePath), Encoding.ASCII))
            {
                this.Read(br);
            }
        }

        /// <summary>
        /// Finds a <see cref="ReleaseManifestFolderEntry"/> in the current <see cref="ReleaseManifest"/>.
        /// </summary>
        /// <param name="path">Path to the folder you want to get.</param>
        /// <param name="createIfNotFound">Create the folder if it was not found.</param>
        /// <example>GetFolder("LEVELS/map11/scene", false)</example>
        public ReleaseManifestFolderEntry GetFolder(string path, bool createIfNotFound)
        {
            if (path == "")
            {
                return this.Project;
            }
            string[] folders = path.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            ReleaseManifestFolderEntry baseFolder = this.Project;
            for (int i = 0; i < folders.Length; i++)
            {
                ReleaseManifestFolderEntry foundSubFolder = baseFolder._folders.Find(x => String.Equals(x.Name, folders[i], StringComparison.InvariantCultureIgnoreCase));
                if (foundSubFolder == null)
                {
                    if (createIfNotFound)
                    {
                        ReleaseManifestFolderEntry newFolderEntry = new ReleaseManifestFolderEntry(folders[i], this.GetNameIndex(folders[i]), baseFolder);
                        baseFolder._folders.Add(newFolderEntry);
                        baseFolder = newFolderEntry;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    baseFolder = foundSubFolder;
                }
            }
            return baseFolder;
        }

        /// <summary>
        /// Finds a <see cref="ReleaseManifestFileEntry"/> in the current <see cref="ReleaseManifest"/>.
        /// </summary>
        /// <param name="path">Path to the file you want to get.</param>
        /// <param name="createIfNotFound">Create the folder if it was not found.</param>
        /// <example>GetFile("LEVELS/map11/scene/room.wgeo", false)</example>
        /// <returns>The <see cref="ReleaseManifestFileEntry"/> you asked for.</returns>
        public ReleaseManifestFileEntry GetFile(string path, bool createIfNotFound)
        {
            string[] folders = path.Split('/');
            string folderPath = path.Substring(0, path.Length - folders[folders.Length - 1].Length);
            ReleaseManifestFolderEntry gotFolder = this.GetFolder(folderPath, createIfNotFound);
            if (gotFolder == null)
            {
                return null;
            }
            else
            {
                ReleaseManifestFileEntry foundFile = gotFolder._files.Find(x => String.Equals(x.Name, folders[folders.Length - 1], StringComparison.InvariantCultureIgnoreCase));
                if (foundFile == null && createIfNotFound)
                {
                    foundFile = new ReleaseManifestFileEntry(folders[folders.Length - 1], this.GetNameIndex(folders[folders.Length - 1]), gotFolder);
                    gotFolder._files.Add(foundFile);
                }
                return foundFile;
            }
        }

        /// <summary>
        /// Saves the current <see cref="ReleaseManifest"/> at the initially specified path.
        /// </summary>
        public void Save()
        {
            this.Save(this.FilePath);
        }

        /// <summary>
        /// Saves the current <see cref="ReleaseManifest"/> at the specified path.
        /// </summary>
        /// <param name="filePath">Path where to save the file.</param>
        public void Save(string filePath)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
            {
                this.Write(bw);
            }
        }

        /// <summary>
        /// Parses Release Manifest File content from a previously initialized <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br"><see cref="BinaryReader"/> instance holding Release Manifest File content.</param>
        private void Read(BinaryReader br)
        {
            string readMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (!String.Equals(readMagic, "RLSM"))
            {
                throw new InvalidMagicNumberException(readMagic);
            }
            this.MajorVersion = br.ReadInt16();
            this.MinorVersion = br.ReadInt16();

            int projectNameIndex = br.ReadInt32();
            this.ReleaseVersion = br.ReadUInt32();

            int folderCount = br.ReadInt32();

            List<ReleaseManifestFolderEntry> folders = new List<ReleaseManifestFolderEntry>();
            for (int i = 0; i < folderCount; i++)
            {
                folders.Add(new ReleaseManifestFolderEntry(br));
            }

            int fileCount = br.ReadInt32();

            List<ReleaseManifestFileEntry> files = new List<ReleaseManifestFileEntry>();
            for (int i = 0; i < fileCount; i++)
            {
                files.Add(new ReleaseManifestFileEntry(br));
            }

            int nameCount = br.ReadInt32();
            int nameSectionLength = br.ReadInt32();
            this.Names.AddRange(Encoding.ASCII.GetString(br.ReadBytes(nameSectionLength)).Split('\0'));
            if (nameCount != this.Names.Count)
            {
                this.Names.RemoveRange(nameCount, this.Names.Count - nameCount);
            }

            this.ProjectName = this.Names[projectNameIndex];

            // Assigning names and parent/sub entries to all file and folder entries
            foreach (ReleaseManifestFolderEntry folderEntry in folders)
            {
                folderEntry.Name = this.Names[folderEntry.NameIndex];
                for (int i = 0; i < folderEntry.SubFolderCount; i++)
                {
                    folders[folderEntry.SubFolderStartIndex + i].Parent = folderEntry;
                    folderEntry._folders.Add(folders[folderEntry.SubFolderStartIndex + i]);
                }
                for (int i = 0; i < folderEntry.FileCount; i++)
                {
                    files[folderEntry.FileListStartIndex + i].Folder = folderEntry;
                    folderEntry._files.Add(files[folderEntry.FileListStartIndex + i]);
                }
            }
            foreach (ReleaseManifestFileEntry fileEntry in files)
            {
                fileEntry.Name = this.Names[fileEntry.NameIndex];
            }
            this.Project = folders[0];
        }

        /// <summary>
        /// Writes the current <see cref="ReleaseManifest"/> in a previously initialized <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="bw"><see cref="BinaryWriter"/> instance where to write file data.</param>
        private void Write(BinaryWriter bw)
        {
            int folderCount = 1 + GetFolderCount(this.Project);
            int fileCount = GetFileCount(this.Project);
            int nameSectionLength = GetNameSectionLength();
            SetFolderIndexes(this.Project, 1 + this.Project.Folders.Count);
            SetFileIndexes(this.Project, this.Project.Files.Count);

            bw.Write(Encoding.ASCII.GetBytes("RLSM"));
            bw.Write(this.MajorVersion);
            bw.Write(this.MinorVersion);
            bw.Write(this.Names.IndexOf(this.ProjectName));
            bw.Write(this.ReleaseVersion);
            bw.Write(folderCount);
            this.Project.Write(bw);
            WriteSubFolderEntries(this.Project, bw);
            bw.Write(fileCount);
            WriteFileEntries(this.Project, bw);
            bw.Write(this.Names.Count);
            bw.Write(nameSectionLength);
            foreach (string name in this.Names)
            {
                bw.Write(Encoding.ASCII.GetBytes(name));
                bw.Write((byte)0);
            }
        }

        /// <summary>
        /// Returns the position of the specified name in <see cref="Names"/>. If the specified name was not found in the list, it is added to it.
        /// </summary>
        private int GetNameIndex(string name)
        {
            int gotIndex = this.Names.IndexOf(name);
            if (gotIndex == -1)
            {
                gotIndex = this.Names.Count;
                this.Names.Add(name);
            }
            return gotIndex;
        }

        /// <summary>
        /// Returns the number of folders and subfolders included in the specified <see cref="ReleaseManifestFolderEntry"/>.
        /// </summary>
        private static int GetFolderCount(ReleaseManifestFolderEntry folderEntry)
        {
            int folderCount = folderEntry.Folders.Count;
            foreach (ReleaseManifestFolderEntry subFolderEntry in folderEntry.Folders)
            {
                folderCount += GetFolderCount(subFolderEntry);
            }
            return folderCount;
        }

        /// <summary>
        /// Returns the number of files found in the specified <see cref="ReleaseManifestFolderEntry"/> and all of its subfolders.
        /// </summary>
        private static int GetFileCount(ReleaseManifestFolderEntry folderEntry)
        {
            int fileCount = folderEntry.Files.Count;
            foreach (ReleaseManifestFolderEntry subFolderEntry in folderEntry.Folders)
            {
                fileCount += GetFileCount(subFolderEntry);
            }
            return fileCount;
        }

        /// <summary>
        /// Sets the appropriate <see cref="ReleaseManifestFolderEntry.SubFolderStartIndex"/> for the specified <see cref="ReleaseManifestFolderEntry"/> and all of its subfolders.
        /// </summary>
        /// <param name="index">Start index.</param>
        private static int SetFolderIndexes(ReleaseManifestFolderEntry baseFolder, int index)
        {
            foreach (ReleaseManifestFolderEntry subFolderEntry in baseFolder.Folders)
            {
                subFolderEntry.SubFolderStartIndex = index;
                index = SetFolderIndexes(subFolderEntry, index + subFolderEntry.Folders.Count);
            }
            return index;
        }

        /// <summary>
        /// Sets the appropriate <see cref="ReleaseManifestFolderEntry.FileListStartIndex"/> for the specified <see cref="ReleaseManifestFolderEntry"/> and all of its subfolders.
        /// </summary>
        /// <param name="index">Start index.</param>
        private static int SetFileIndexes(ReleaseManifestFolderEntry baseFolder, int index)
        {
            foreach (ReleaseManifestFolderEntry subFolderEntry in baseFolder.Folders)
            {
                subFolderEntry.FileListStartIndex = index;
                index = SetFileIndexes(subFolderEntry, index + subFolderEntry.Files.Count);
            }
            return index;
        }

        /// <summary>
        /// Returns the length in bytes of the <see cref="Names"/> list.
        /// </summary>
        /// <returns></returns>
        private int GetNameSectionLength()
        {
            int length = 0;
            foreach (string name in this.Names)
            {
                length += 1 + name.Length;
            }
            return length;
        }

        /// <summary>
        /// Writes the content of the specified <see cref="ReleaseManifestFolderEntry"/> and all of its subfolders.
        /// </summary>
        /// <param name="bw">Previously initialized <see cref="BinaryWriter"/> where to write <paramref name="baseFolder"/> content.</param>
        private static void WriteSubFolderEntries(ReleaseManifestFolderEntry baseFolder, BinaryWriter bw)
        {
            foreach (ReleaseManifestFolderEntry folderEntry in baseFolder.Folders)
            {
                folderEntry.Write(bw);
            }
            foreach (ReleaseManifestFolderEntry folderEntry in baseFolder.Folders)
            {
                WriteSubFolderEntries(folderEntry, bw);
            }
        }

        /// <summary>
        /// Writes the content of the files from the specified <see cref="ReleaseManifestFolderEntry"/> and all of its subfolders.
        /// </summary>
        /// <param name="bw">Previously initialized <see cref="BinaryWriter"/> where to write <paramref name="baseFolder"/> files content.</param>
        private static void WriteFileEntries(ReleaseManifestFolderEntry baseFolder, BinaryWriter bw)
        {
            foreach (ReleaseManifestFileEntry fileEntry in baseFolder.Files)
            {
                fileEntry.Write(bw);
            }
            foreach (ReleaseManifestFolderEntry folderEntry in baseFolder.Folders)
            {
                WriteFileEntries(folderEntry, bw);
            }
        }

        /// <summary>
        /// Indicates how a file is deployed in the League of Legends installation.
        /// </summary>
        public enum DeployMode : uint
        {
            /// <summary>
            /// The file is in the 'deploy' folder.
            /// </summary>
            /// <example>C:/Riot Games/League of Legends/RADS/projects/league_client/releases/0.0.0.87/deploy/LeagueClient.exe</example>
            Deployed0 = 0,
            Deployed4 = 4,
            Managed = 5,
            RAFRaw = 6,
            RAFCompressed = 22
        }

        /// <summary>
        /// Occurs when the read magic number is not correct.
        /// </summary>
        public class InvalidMagicNumberException : Exception
        {
            public InvalidMagicNumberException(string readMagic) : base(String.Format("Invalid magic number (\"{0}\"), expected: \"RLSM\".", readMagic)) { }
        }
    }
}