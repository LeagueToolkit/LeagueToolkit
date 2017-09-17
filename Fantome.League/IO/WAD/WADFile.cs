using Fantome.Libraries.League.Helpers.Compression;
using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private List<WADEntry> _entries = new List<WADEntry>();

        /// <summary>
        /// A collection of <see cref="WADEntry"/>
        /// </summary>
        public ReadOnlyCollection<WADEntry> Entries { get; private set; }

        /// <summary>
        /// <see cref="Stream"/> of the currently opened <see cref="WADFile"/>.
        /// </summary>
        internal Stream _stream { get; private set; }

        private WADFile()
        {
            Entries = _entries.AsReadOnly();
        }

        /// <summary>
        /// Reads a <see cref="WADFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">The location to read from</param>
        public WADFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Reads a <see cref="WADFile"/> from the specified stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        public WADFile(Stream stream) : this()
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
                    _entries.Add(new WADEntry(this, br, major, minor));
                }
            }
        }

        /// <summary>
        /// Adds a new <see cref="EntryType.FileRedirection"/> <see cref="WADEntry"/> to this <see cref="WADFile"/>
        /// </summary>
        /// <param name="path">The virtual path of the file being added</param>
        /// <param name="fileRedirection">The file the game should load instead of this one</param>
        public void AddEntry(string path, string fileRedirection)
        {
            using (XXHash64 xxHash = XXHash64.Create())
            {
                AddEntry(BitConverter.ToUInt64(xxHash.ComputeHash(Encoding.ASCII.GetBytes(path)), 0), fileRedirection);
            }
        }

        /// <summary>
        /// Adds a new <see cref="WADEntry"/> to this <see cref="WADFile"/>
        /// </summary>
        /// <param name="path">The virtual path of the file being added</param>
        /// <param name="data">Data of file being added</param>
        /// <param name="compressedEntry">Whether the data needs to be GZip compressed inside WAD</param>
        public void AddEntry(string path, byte[] data, bool compressedEntry)
        {
            using (XXHash64 xxHash = XXHash64.Create())
            {
                AddEntry(BitConverter.ToUInt64(xxHash.ComputeHash(Encoding.ASCII.GetBytes(path)), 0), data, compressedEntry);
            }
        }

        /// <summary>
        /// Adds a new <see cref="EntryType.FileRedirection"/> <see cref="WADEntry"/> to this <see cref="WADFile"/>
        /// </summary>
        /// <param name="xxHash">The hash of the virtual path being added</param>
        /// <param name="fileRedirection">The file the game should load instead of this one</param>
        public void AddEntry(ulong xxHash, string fileRedirection)
        {
            AddEntry(new WADEntry(this, xxHash, fileRedirection));
        }

        /// <summary>
        /// Adds a new <see cref="WADEntry"/> to this <see cref="WADFile"/>
        /// </summary>
        /// <param name="xxHash">The hash of the virtual path being added</param>
        /// <param name="data">Data of file being added</param>
        /// <param name="compressedEntry">Whether the data needs to be GZip compressed inside WAD</param>
        public void AddEntry(ulong xxHash, byte[] data, bool compressedEntry)
        {
            AddEntry(new WADEntry(this, xxHash, data, compressedEntry));
        }

        private void AddEntry(WADEntry entry)
        {
            if (!this._entries.Exists(x => x.XXHash == entry.XXHash))
            {
                this._entries.Add(entry);
                this._entries.Sort();
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
            if (this._entries.Exists(x => x.XXHash == xxHash))
            {
                this._entries.RemoveAll(x => x.XXHash == xxHash);
            }
        }

        /// <summary>
        /// Removes a <see cref="WADEntry"/>
        /// </summary>
        /// <param name="entry">The <see cref="WADEntry"/> to remove</param>
        public void RemoveEntry(WADEntry entry)
        {
            this._entries.Remove(entry);
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
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                bw.Write(Encoding.ASCII.GetBytes("RW"));
                bw.Write((byte)2);
                bw.Write((byte)0);
                bw.Write(new byte[92]);
                bw.Write((ushort)104);
                bw.Write((ushort)32);
                bw.Write(Entries.Count);

                long tocOffset = stream.Position;
                stream.Seek(tocOffset + (32 * Entries.Count), SeekOrigin.Begin);

                for (int i = 0; i < Entries.Count; i++)
                {
                    WADEntry currentEntry = Entries[i];
                    currentEntry._isDuplicated = false;

                    // Finding potential duplicated entry
                    WADEntry duplicatedEntry = null;
                    if (currentEntry.Type != EntryType.FileRedirection)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (Entries[j].SHA256.SequenceEqual(currentEntry.SHA256))
                            {
                                currentEntry._isDuplicated = true;
                                duplicatedEntry = Entries[j];
                                break;
                            }
                        }
                    }

                    // Writing data
                    if (duplicatedEntry == null)
                    {
                        bw.Write(currentEntry.GetContent(false));
                        currentEntry._newData = null;
                        currentEntry._dataOffset = (uint)stream.Position - currentEntry.CompressedSize;
                    }
                    else
                    {
                        currentEntry._dataOffset = duplicatedEntry._dataOffset;
                    }
                }

                // Write TOC
                stream.Seek(tocOffset, SeekOrigin.Begin);
                foreach (WADEntry wadEntry in Entries)
                {
                    wadEntry.Write(bw);
                }
            }

            _stream = stream;
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
