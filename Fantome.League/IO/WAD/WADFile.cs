using Fantome.Libraries.League.Helpers.Compression;
using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Fantome.Libraries.League.IO.WAD
{
    /// <summary>
    /// Represents a WAD File
    /// </summary>
    /// <remarks>WAD Files can only be read</remarks>
    public class WADFile : IDisposable
    {
        /// <summary>
        /// ECDSA Signature contained in the header of the file
        /// </summary>
        public byte[] ECDSA { get; private set; }

        /// <summary>
        /// A collection of <see cref="WADEntry"/>
        /// </summary>
        public List<WADEntry> Entries { get; private set; } = new List<WADEntry>();

        /// <summary>
        /// <see cref="Stream"/> of the currently opened <see cref="WADFile"/>.
        /// </summary>
        internal Stream _stream { get; private set; }

        /// <summary>
        /// Reads a <see cref="WADFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">The location to read from</param>
        public WADFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Reads a <see cref="WADFile"/> from the specified stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        public WADFile(Stream stream)
        {
            _stream = stream;
            using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(2));
                if (magic != "RW")
                {
                    throw new Exception("This is not a valid WAD file");
                }

                byte major = br.ReadByte();
                byte minor = br.ReadByte();
                if (major > 2 || minor > 0)
                {
                    throw new Exception("This version is not supported");
                }
                if (major == 2 && minor == 0)
                {
                    byte ecdsaLength = br.ReadByte();
                    this.ECDSA = br.ReadBytes(ecdsaLength);
                    br.ReadBytes(83 - ecdsaLength);
                }

                //XXHash Checksum of WAD Data (everything after TOC).
                ulong dataChecksum = br.ReadUInt64();

                ushort tocStartOffset = br.ReadUInt16();
                ushort tocFileEntrySize = br.ReadUInt16();
                uint fileCount = br.ReadUInt32();

                br.BaseStream.Seek(tocStartOffset, SeekOrigin.Begin);

                for (int i = 0; i < fileCount; i++)
                {
                    Entries.Add(new WADEntry(this, br, major, minor));
                }
            }
        }

        /// <summary>
        /// Adds a new <see cref="WADEntry"/> to this <see cref="WADFile"/>
        /// </summary>
        /// <param name="path">The virtual path of the file being added</param>
        /// <param name="data">Data of file being added</param>
        /// <param name="type">Type of file being added</param>
        public void AddEntry(string path, byte[] data, EntryType type)
        {
            using (XXHash64 xxHash = XXHash64.Create())
            {
                AddEntry(BitConverter.ToUInt64(xxHash.ComputeHash(Encoding.ASCII.GetBytes(path)), 0), data, type);
            }
        }

        /// <summary>
        /// Adds a new <see cref="WADEntry"/> to this <see cref="WADFile"/>
        /// </summary>
        /// <param name="xxHash">The hash of the virtual path being added</param>
        /// <param name="data">Data of file being added</param>
        /// <param name="type">Type of file being added</param>
        public void AddEntry(ulong xxHash, byte[] data, EntryType type)
        {
            if(!this.Entries.Exists(x => x.XXHash == xxHash))
            {
                if (type == EntryType.FileRedirection)
                {
                    this.Entries.Add(new WADEntry(this, xxHash, Encoding.ASCII.GetString(data)));
                }
                else
                {
                    this.Entries.Add(new WADEntry(this, xxHash, data, type));
                }
            }
            else
            {
                throw new Exception("An Entry with this path already exists");
            }
        }

        /// <summary>
        /// Removes a <see cref="WADEntry"/> Entry with the specified path
        /// </summary>
        /// <param name="path">The path of the <see cref="WADEntry"/> to remove</param>
        public void RemoveEntry(string path)
        {
            using (XXHash64 xxHash = XXHash64.Create())
            {
                RemoveEntry(BitConverter.ToUInt64(xxHash.ComputeHash(Encoding.ASCII.GetBytes(path)), 0));
            }
        }

        /// <summary>
        /// Removes a <see cref="WADEntry"/> with the specified hash
        /// </summary>
        /// <param name="xxHash">The hash of the <see cref="WADEntry"/> to remove</param>
        public void RemoveEntry(ulong xxHash)
        {
            if(this.Entries.Exists(x => x.XXHash == xxHash))
            {
                this.Entries.RemoveAll(x => x.XXHash == xxHash);
            }
        }

        /// <summary>
        /// Removes a <see cref="WADEntry"/>
        /// </summary>
        /// <param name="entry">The <see cref="WADEntry"/> to remove</param>
        public void RemoveEntry(WADEntry entry)
        {
            this.Entries.Remove(entry);
        }

        /// <summary>
        /// Writes this <see cref="WADFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">The location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="WADFile"/> into the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("RW"));
                bw.Write((byte)2);
                bw.Write((byte)0);
                bw.Write(new byte[92]);
                bw.Write((ushort)104);
                bw.Write((ushort)32);
                bw.Write(this.Entries.Count);

                bw.Seek(this.Entries.Count * 32, SeekOrigin.Current);
                foreach (WADEntry entry in this.Entries)
                {
                    entry._dataOffset = (uint)bw.BaseStream.Position;

                    if (entry.Type == EntryType.FileRedirection)
                    {
                        byte[] data = new byte[4 + entry.FileRedirection.Length];

                        Buffer.BlockCopy(BitConverter.GetBytes(entry.FileRedirection.Length), 0, data, 0, 4);
                        Buffer.BlockCopy(Encoding.ASCII.GetBytes(entry.FileRedirection), 0, data, 4, entry.FileRedirection.Length);

                        entry.CompressedSize = (uint)entry.FileRedirection.Length;
                        entry.UncompressedSize = (uint)entry.FileRedirection.Length;
                        entry.SHA256 = GetSha256(data.ToArray());

                        bw.Write(data.ToArray());
                    }
                    else
                    {
                        byte[] data = entry.GetContent(false);

                        entry.CompressedSize = (uint)data.Length;
                        entry.UncompressedSize = entry.Type == EntryType.Compressed ? BitConverter.ToUInt32(data, data.Length - 4) : (uint)data.Length;

                        bw.Write(data);
                    }
                }

                bw.Seek(104, SeekOrigin.Begin);
                foreach (WADEntry entry in this.Entries)
                {
                    entry.Write(bw);
                }
            }
        }

        private byte[] GetSha256(byte[] data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(data).Take(8).ToArray();
            }
        }


        /// <summary>
        /// Closes the opened <see cref="Stream"/> of this <see cref="WADFile"/> instance.
        /// </summary>
        public void Dispose()
        {
            _stream?.Close();
            _stream = null;
        }
    }
}
