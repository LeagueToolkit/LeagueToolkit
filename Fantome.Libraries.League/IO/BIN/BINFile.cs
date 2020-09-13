using Fantome.Libraries.League.Helpers.Cryptography;
using Fantome.Libraries.League.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fantome.Libraries.League.IO.BIN
{
    /// <summary>
    /// Represents a <see cref="BINFile"/>
    /// </summary>
    public class BINFile
    {
        /// <summary>
        /// <see cref="BINFile"/> that should get loaded together with this one
        /// </summary>
        public List<string> Dependencies { get; private set; } = new List<string>();
        /// <summary>
        /// A Collection of <see cref="BINEntry"/>
        /// </summary>
        public List<BINEntry> Entries { get; private set; } = new List<BINEntry>();
        public BINValue this[string entryPath, string valuePath]
        {
            get
            {
                uint entryHash;
                if(entryPath.Contains('/'))
                {
                    entryHash = Cryptography.FNV32Hash(entryPath.ToLower());
                }
                else
                {
                    entryHash = uint.Parse(entryPath);
                }

                return this.Entries.Find(x => x.Property == entryHash)[valuePath];
            }
        }

        /// <summary>
        /// Initializes a new <see cref="BINFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">The location to read from</param>
        public BINFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initializes a new <see cref="BINFile"/> from the specified stream
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        public BINFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "PROP")
                {
                    throw new InvalidFileSignatureException();
                }

                uint version = br.ReadUInt32();
                if(version != 1 && version != 2)
                {
                    throw new UnsupportedFileVersionException();
                }

                if (version >= 2)
                {
                    uint dependencyCount = br.ReadUInt32();
                    for (int i = 0; i < dependencyCount; i++)
                    {
                        this.Dependencies.Add(Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt16())));
                    }
                }

                uint entryCount = br.ReadUInt32();
                for (int i = 0; i < entryCount; i++)
                {
                    this.Entries.Add(new BINEntry(br));
                }

                foreach (BINEntry entry in this.Entries)
                {
                    entry.ReadData(br);
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="BINFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">The location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="BINFile"/> to the specified stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("PROP"));
                bw.Write((uint)2);
                bw.Write((uint)this.Dependencies.Count);
                foreach (string linkedFile in this.Dependencies)
                {
                    bw.Write((ushort)linkedFile.Length);
                    bw.Write(Encoding.ASCII.GetBytes(linkedFile));
                }

                bw.Write((uint)this.Entries.Count);
                foreach (BINEntry entry in this.Entries)
                {
                    bw.Write(entry.Class);
                }
                foreach (BINEntry entry in this.Entries)
                {
                    entry.Write(bw);
                }
            }
        }
    }
}
