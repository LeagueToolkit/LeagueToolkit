using Fantome.Libraries.League.Helpers.Cryptography;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
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

        internal byte _major;
        internal byte _minor;

        public WADFile()
        {
            this.Entries = this._entries.AsReadOnly();
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
            this._stream = stream;
            using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(2));
                if (magic != "RW")
                {
                    throw new Exception("This is not a valid WAD file");
                }

                this._major = br.ReadByte();
                this._minor = br.ReadByte();
                if (this._major > 3)
                {
                    throw new Exception("This version is not supported");
                }

                uint fileCount = 0;
                //XXHash Checksum of WAD Data (everything after TOC).
                ulong dataChecksum = 0;

                if (this._major == 2)
                {
                    byte ecdsaLength = br.ReadByte();
                    this.ECDSA = br.ReadBytes(ecdsaLength);
                    br.ReadBytes(83 - ecdsaLength);

                    dataChecksum = br.ReadUInt64();
                }
                else if (this._major == 3)
                {
                    this.ECDSA = br.ReadBytes(256);
                    dataChecksum = br.ReadUInt64();
                }

                if (this._major == 1 || this._major == 2)
                {
                    ushort tocStartOffset = br.ReadUInt16();
                    ushort tocFileEntrySize = br.ReadUInt16();
                }

                fileCount = br.ReadUInt32();
                for (int i = 0; i < fileCount; i++)
                {
                    this._entries.Add(new WADEntry(this, br, this._major));
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
                AddEntry(BitConverter.ToUInt64(xxHash.ComputeHash(Encoding.ASCII.GetBytes(path.ToLower(new CultureInfo("en-US")))), 0), fileRedirection);
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
                AddEntry(BitConverter.ToUInt64(xxHash.ComputeHash(Encoding.ASCII.GetBytes(path.ToLower(new CultureInfo("en-US")))), 0), data, compressedEntry);
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

        /// <summary>
        /// Adds an existing <see cref="WADEntry"/> to this <see cref="WADFile"/>
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(WADEntry entry)
        {
            if (!this._entries.Exists(x => x.XXHash == entry.XXHash) && entry._wad == this)
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
        /// <remarks>It is not possible to overwrite the opened file</remarks>
        /// <param name="fileLocation">The location to write to</param>
        public void Write(string fileLocation)
        {
            Write(fileLocation, this._major, this._minor);
        }

        /// <summary>
        /// Writes this <see cref="WADFile"/> to the specified location
        /// </summary>
        /// <remarks>It is not possible to overwrite the opened file</remarks>
        /// <param name="fileLocation">The location to write to</param>
        /// <param name="major">Which major version this <see cref="WADFile"/> should be saved as</param>
        /// <param name="minor">Which minor version this <see cref="WADFile"/> should be saved as</param>
        public void Write(string fileLocation, byte major, byte minor)
        {
            Write(File.Create(fileLocation), major, minor);
        }

        /// <summary>
        /// Writes this <see cref="WADFile"/> into the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            Write(stream, this._major, this._minor);
        }

        /// <summary>
        /// Writes this <see cref="WADFile"/> into the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        /// <param name="major">Which major version this <see cref="WADFile"/> should be saved as</param>
        /// <param name="minor">Which minor version this <see cref="WADFile"/> should be saved as</param>
        public void Write(Stream stream, byte major, byte minor)
        {
            if (major > 3)
            {
                throw new Exception("WAD File version: " + major + " either does not support writing or doesn't exist");
            }

            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                bw.Write(Encoding.ASCII.GetBytes("RW"));
                bw.Write(major);
                bw.Write(minor);

                // Writing signatures 
                if (major == 2)
                {
                    bw.Write(new byte[84]);
                }
                else if (major == 3)
                {
                    bw.Write(new byte[256]);
                }

                // Writing file checksums
                if (major == 2 || major == 3)
                {
                    bw.Write((long)0);
                }

                int tocSize = (major == 1) ? 24 : 32;
                long tocOffset = stream.Position + 4;

                // Writing TOC info
                if (major < 3)
                {
                    tocOffset += 4;
                    bw.Write((ushort)tocOffset);
                    bw.Write((ushort)tocSize);
                }

                bw.Write(this.Entries.Count);

                stream.Seek(tocOffset + (tocSize * this.Entries.Count), SeekOrigin.Begin);

                for (int i = 0; i < this.Entries.Count; i++)
                {
                    WADEntry currentEntry = this.Entries[i];
                    currentEntry._isDuplicated = false;

                    // Finding potential duplicated entry
                    WADEntry duplicatedEntry = null;
                    if (major != 1)
                    {
                        if (currentEntry.Type != EntryType.FileRedirection)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (this.Entries[j].SHA.SequenceEqual(currentEntry.SHA))
                                {
                                    currentEntry._isDuplicated = true;
                                    duplicatedEntry = this.Entries[j];
                                    break;
                                }
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
                foreach (WADEntry wadEntry in this.Entries)
                {
                    wadEntry.Write(bw, major);
                }
            }

            this.Dispose();
            this._stream = stream;
            this._major = major;
            this._minor = minor;
        }

        /// <summary>
        /// Closes the opened <see cref="Stream"/> of this <see cref="WADFile"/> instance.
        /// </summary>
        public void Dispose()
        {
            this._stream?.Close();
            this._stream = null;
        }
    }
}
