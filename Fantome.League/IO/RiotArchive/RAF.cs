using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Fantome.Libraries.League.IO.RiotArchive
{
    /// <summary>
    /// Riot Archive File (RAF) containing files from LoL.
    /// </summary>
    public class RAF : IDisposable
    {
        /// <summary>
        /// Number every RAF starts with.
        /// </summary>
        private const uint Magic = 0x18be0ef0;

        /// <summary>
        /// File path of the current <see cref="RAF"/>.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// File version of the current <see cref="RAF"/>.
        /// </summary>
        /// <remarks>There have been only version 1 archives so far.</remarks>
        public int Version { get; private set; }

        /// <summary>
        /// ManagerIndex value of the current <see cref="RAF"/> (no real meaning found, since many new RAFs have a "0" <see cref="ManagerIndex"/>).
        /// </summary>
        public int ManagerIndex { get; private set; }

        /// <summary>
        /// List of files included in the current <see cref="RAF"/>.
        /// </summary>
        public List<RAFFileEntry> Files { get; private set; } = new List<RAFFileEntry>();

        /// <summary>
        /// <see cref="FileStream"/> of the opened data file of the current <see cref="RAF"/>.
        /// </summary>
        internal FileStream _dataStream { get; private set; }

        /// <summary>
        /// Opens or create a <see cref="RAF"/> at the specified path.
        /// </summary>
        /// <param name="filePath">The file path to open or create a <see cref="RAF"/> at.</param>
        /// <remarks>If an existing RAF is opened, its data file is opened as well.</remarks>
        public RAF(string filePath)
        {
            this.FilePath = filePath;
            if (File.Exists(filePath))
            {
                // Open an existing RAF and check if data file exists
                if (!File.Exists(filePath + ".dat"))
                {
                    throw new MissingDataFileException();
                }
                using (BinaryReader br = new BinaryReader(File.OpenRead(filePath), Encoding.ASCII))
                {
                    this.Read(br);
                }
            }
            else
            {
                // Create a new RAF file
                this.Version = 1;
                this.ManagerIndex = 0;
                InitDataStream();
            }
        }

        /// <summary>
        /// Loads the <see cref="FileStream"/> of the data file of the current <see cref="RAF"/>.
        /// </summary>
        /// <remarks>Creates the data file if it doesn't exist.</remarks>
        public void InitDataStream()
        {
            if (this._dataStream != null)
            {
                // Already initialized!
                return;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(this.FilePath));
            this._dataStream = File.Open(this.FilePath + ".dat", FileMode.OpenOrCreate);
        }

        /// <summary>
        /// Saves the content of the current <see cref="RAF"/> at <see cref="FilePath"/>.
        /// </summary>
        public void Save()
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(this.FilePath, FileMode.Create)))
            {
                this.Write(bw);
            }
        }

        /// <summary>
        /// Adds a new file to the current <see cref="RAF"/>.
        /// </summary>
        /// <param name="gamePath">Game path of the file to add.</param>
        /// <param name="data">Raw data of the file to add.</param>
        /// <param name="compress">Whether the file data needs to be ZLIB compressed.</param>
        public void AddFile(string gamePath, byte[] data, bool compress)
        {
            this.InitDataStream();
            this._dataStream.Seek(0, SeekOrigin.End);
            uint fileOffset = (uint)this._dataStream.Length;
            int fileLength = data.Length;
            if (compress)
            {
                this._dataStream.Write(BitConverter.GetBytes((ushort)40056), 0, 2);
                byte[] deflateData = Deflate(data);
                this._dataStream.Write(deflateData, 0, deflateData.Length);
                this._dataStream.Write(BitConverter.GetBytes(GetAdler32Hash(data)), 0, 4);
                fileLength = deflateData.Length + 2 + 4;
            }
            else
            {
                this._dataStream.Write(data, 0, data.Length);
            }
            this.Files.Add(new RAFFileEntry(this, gamePath, fileOffset, (uint)fileLength));
        }

        /// <summary>
        /// Adds a new file to the current <see cref="RAF"/>.
        /// </summary>
        /// <param name="gamePath">Game path of the file to add.</param>
        /// <param name="inputFilePath">Path (on your computer) of the file to add.</param>
        /// <param name="compress">Whether the file data needs to be ZLIB compressed.</param>
        public void AddFile(string gamePath, string inputFilePath, bool compress)
        {
            this.AddFile(gamePath, File.ReadAllBytes(inputFilePath), compress);
        }

        /// <summary>
        /// Closes the opened data file stream of the current <see cref="RAF"/>.
        /// </summary>
        public void Dispose()
        {
            if (this._dataStream != null)
            {
                this._dataStream.Dispose();
                this._dataStream = null;
            }
        }

        /// <summary>
        /// Parses the content of a RAF from a previously initialized <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br"><see cref="BinaryReader"/> instance containing data from a RAF.</param>
        private void Read(BinaryReader br)
        {
            uint magic = br.ReadUInt32();
            if (magic != Magic)
            {
                throw new InvalidMagicNumberException(magic);
            }
            this.Version = br.ReadInt32();
            this.ManagerIndex = br.ReadInt32();
            uint fileListOffset = br.ReadUInt32();
            uint pathListOffset = br.ReadUInt32();

            // Reading file list
            br.BaseStream.Seek(fileListOffset, SeekOrigin.Begin);
            int fileCount = br.ReadInt32();
            for (int i = 0; i < fileCount; i++)
            {
                this.Files.Add(new RAFFileEntry(this, br));
            }
            // Reading path list
            br.BaseStream.Seek(pathListOffset, SeekOrigin.Begin);
            PathList pathList = new PathList(br);
            foreach (RAFFileEntry fileEntry in this.Files)
            {
                fileEntry.Path = pathList.Paths[fileEntry.PathListIndex];
            }
        }

        /// <summary>
        /// Writes the content of the current <see cref="RAF"/> in a previously initialized <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="bw"></param>
        private void Write(BinaryWriter bw)
        {
            // Preparing file entries before writing
            this.Files.Sort();
            for (int i = 0; i < this.Files.Count; i++)
            {
                this.Files[i].PathListIndex = i;
            }

            bw.Write(Magic);
            bw.Write(this.Version);
            bw.Write(this.ManagerIndex);
            // File list offset
            bw.Write(20);
            // Path list offset
            int pathListOffset = 24 + (this.Files.Count * 16);
            bw.Write(pathListOffset);
            bw.Write(this.Files.Count);
            foreach (RAFFileEntry fileEntry in this.Files)
            {
                fileEntry.Write(bw);
            }
            PathList pathList = new PathList(this.Files);
            pathList.Write(bw);
        }

        /// <summary>
        /// Checksum used for ZLIB compression.
        /// </summary>
        /// <param name="data">Raw data to calculate the checksum from.</param>
        private static int GetAdler32Hash(byte[] data)
        {
            long MOD_ADLER = 65521;
            long s1 = 1;
            long s2 = 0;
            foreach (byte dataByte in data)
            {
                s1 = (s1 + dataByte) % MOD_ADLER;
                s2 = (s1 + s2) % MOD_ADLER;
            }
            int hash = (int)((s2 << 16) + s1);
            return (int)(hash & 0xFF000000) >> 24 | (hash & 0x00FF0000) >> 8 | (hash & 0x0000FF00) << 8 | (hash & 0x000000FF) << 24;
        }

        /// <summary>
        /// Deflate the passed data.
        /// </summary>
        /// <param name="rawData">Data to deflate.</param>
        /// <returns>The compressed data.</returns>
        private static byte[] Deflate(byte[] rawData)
        {
            byte[] compressedData = null;
            using (MemoryStream originalStream = new MemoryStream(rawData))
            {
                using (MemoryStream compressedStream = new MemoryStream())
                {
                    using (DeflateStream compressionStream = new DeflateStream(compressedStream, CompressionMode.Compress))
                    {
                        originalStream.CopyTo(compressionStream);
                    }
                    compressedData = compressedStream.ToArray();
                }
            }
            return compressedData;
        }

        /// <summary>
        /// Occurs when the read magic number is not <see cref="Magic"/>.
        /// </summary>
        public class InvalidMagicNumberException : Exception
        {
            public InvalidMagicNumberException(uint readMagic) : base(String.Format("Invalid magic number (\"{0}\"), expected: \"{1}\".", readMagic, 0xF00EBE18)) { }
        }

        /// <summary>
        /// Occurs when the data file was not found when opening an existing RAF.
        /// </summary>
        public class MissingDataFileException : Exception
        {
            public MissingDataFileException() : base("The data file wasn't found for the specified archive.") { }
        }

        /// <summary>
        /// Path list containing all paths to the files in the current <see cref="RAF"/>.
        /// </summary>
        /// <remarks>Is only used when reading and writing.</remarks>
        private class PathList
        {
            private uint _size;
            public List<string> Paths { get; private set; } = new List<string>();

            public PathList(BinaryReader br)
            {
                long offset = br.BaseStream.Position;
                this._size = br.ReadUInt32();
                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    uint pathOffset = br.ReadUInt32();
                    int pathLength = br.ReadInt32();
                    long currentOffset = br.BaseStream.Position;
                    br.BaseStream.Seek(offset + pathOffset, SeekOrigin.Begin);
                    this.Paths.Add(Encoding.ASCII.GetString(br.ReadBytes(pathLength - 1)));
                    br.BaseStream.Seek(currentOffset, SeekOrigin.Begin);
                }
            }

            public PathList(List<RAFFileEntry> files)
            {
                this._size = 0;
                foreach (RAFFileEntry fileEntry in files)
                {
                    Paths.Add(fileEntry.Path);
                    this._size += (uint)fileEntry.Path.Length + 1;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(this._size);
                bw.Write(this.Paths.Count);
                // 8 bytes for each path entry
                int currentOffset = 8 + this.Paths.Count * 8;
                foreach (string path in this.Paths)
                {
                    bw.Write(currentOffset);
                    bw.Write(path.Length + 1);
                    currentOffset += path.Length + 1;
                }
                foreach (string path in this.Paths)
                {
                    bw.Write(Encoding.ASCII.GetBytes(path));
                    bw.Write((byte)0);
                }
            }
        }
    }
}