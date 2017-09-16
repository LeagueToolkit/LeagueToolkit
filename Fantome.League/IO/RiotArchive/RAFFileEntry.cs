using System;
using System.IO;
using System.IO.Compression;

namespace Fantome.Libraries.League.IO.RiotArchive
{
    /// <summary>
    /// Contains information about files stored in Riot Archive Files
    /// </summary>
    public class RAFFileEntry : IComparable<RAFFileEntry>
    {
        private string _path;

        /// <summary>
        /// Hash of <see cref="Path"/>.
        /// </summary>
        private uint _pathHash;

        /// <summary>
        /// Game path of the current <see cref="RAFFileEntry"/>.
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                _pathHash = GetPathHash();
            }
        }

        /// <summary>
        /// Start offset of the current <see cref="RAFFileEntry"/> in the data file of the current <see cref="RAF"/>.
        /// </summary>
        public uint Offset { get; private set; }

        /// <summary>
        /// Data length of the current <see cref="RAFFileEntry"/>.
        /// </summary>
        public uint Length { get; private set; }

        /// <summary>
        /// Position of <see cref="Path"/> in the <see cref="RAF.PathList"/>.
        /// </summary>
        /// <remarks>Used only when reading and writing the RAF.</remarks>
        internal int PathListIndex { get; set; }

        /// <summary>
        /// Instance of <see cref="RAF"/> the current file belongs to.
        /// </summary>
        private RAF _raf;

        /// <summary>
        /// Parses the content of a <see cref="RAFFileEntry"/> from a previously initialized <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="raf"><see cref="RAF"/> instance the current <see cref="RAFFileEntry"/> belongs to.</param>
        /// <param name="br"><see cref="BinaryReader"/> instance containing data from a this <see cref="RAFFileEntry"/>.</param>
        public RAFFileEntry(RAF raf, BinaryReader br)
        {
            this._raf = raf;
            this._pathHash = br.ReadUInt32();
            this.Offset = br.ReadUInt32();
            this.Length = br.ReadUInt32();
            this.PathListIndex = br.ReadInt32();
        }

        /// <summary>
        /// Creates a new <see cref="RAFFileEntry"/>.
        /// </summary>
        /// <param name="raf"><see cref="RAF"/> instance the current <see cref="RAFFileEntry"/> belongs to.</param>
        /// <param name="path">Game path of the new file.</param>
        /// <param name="offset">Start offset of the new file in the data file of <paramref name="raf"/>.</param>
        /// <param name="length">Data length of the new file.</param>
        public RAFFileEntry(RAF raf, string path, uint offset, uint length)
        {
            this._raf = raf;
            this.Path = path;
            this.Offset = offset;
            this.Length = length;
        }

        /// <summary>
        /// Returns raw data of the current <see cref="RAFFileEntry"/>.
        /// </summary>
        /// <param name="decompress">Whether the data needs to be decompressed.</param>
        public byte[] GetContent(bool decompress)
        {
            this._raf.InitDataStream();
            byte[] data;
            if (decompress)
            {
                this._raf._dataStream.Seek((int)this.Offset + 2, SeekOrigin.Begin);
                data = new byte[this.Length - 6];
                this._raf._dataStream.Read(data, 0, (int)this.Length - 6);
                return Inflate(data);
            }
            else
            {
                this._raf._dataStream.Seek((int)this.Offset, SeekOrigin.Begin);
                data = new byte[this.Length];
                this._raf._dataStream.Read(data, 0, (int)this.Length);
                return data;
            }
        }

        /// <summary>
        /// Writes content from the current <see cref="RAFFileEntry"/> to a previously initialized <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="bw">Instance of <see cref="BinaryWriter"/> where to write data.</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this._pathHash);
            bw.Write(this.Offset);
            bw.Write(this.Length);
            bw.Write(this.PathListIndex);
        }

        /// <summary>
        /// Compares two <see cref="RAFFileEntry"/> instances.
        /// </summary>
        /// <remarks>Used to generate a valid list of files in a <see cref="RAF"/>.</remarks>
        public int CompareTo(RAFFileEntry other)
        {
            if (this._pathHash > other._pathHash)
            {
                return 1;
            }
            else if (this._pathHash < other._pathHash)
            {
                return -1;
            }
            else
            {
                // Hash collision
                return String.Compare(this.Path, other.Path, true);
            }
        }

        /// <summary>
        /// Calculates the hash of <see cref="Path"/>.
        /// </summary>
        private uint GetPathHash()
        {
            uint hash = 0;
            uint temp = 0;
            string path = this.Path.ToLower(new System.Globalization.CultureInfo("en-US", false));
            foreach (char chr in path)
            {
                hash = (hash << 4) + chr;
                temp = hash & 0xf0000000;
                if (temp != 0)
                {
                    hash = hash ^ (temp >> 24);
                    hash = hash ^ temp;
                }
            }
            return hash;
        }

        /// <summary>
        /// Decompresses the specified data.
        /// </summary>
        /// <param name="compressedData">Data to decompress.</param>
        /// <returns>The decompressed data.</returns>
        private static byte[] Inflate(byte[] compressedData)
        {
            byte[] decompressedData = null;
            using (MemoryStream compressedStream = new MemoryStream(compressedData))
            {
                using (MemoryStream rawStream = new MemoryStream())
                {
                    using (DeflateStream decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(rawStream);
                    }
                    decompressedData = rawStream.ToArray();
                }
            }
            return decompressedData;
        }
    }
}