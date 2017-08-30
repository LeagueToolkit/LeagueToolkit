using Fantome.Libraries.League.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.WAD
{
    /// <summary>
    /// Represents a WAD File
    /// </summary>
    /// <remarks>WAD Files can only be read</remarks>
    public class WADFile
    {
        /// <summary>
        /// ECDSA Signature contained in the header of the file
        /// </summary>
        public byte[] ECDSA { get; private set; }
        /// <summary>
        /// A collection of <see cref="WADEntry"/>
        /// </summary>
        public List<WADEntry> Entries { get; private set; } = new List<WADEntry>();

        internal readonly MemoryStream _dataBuffer;

        /// <summary>
        /// Reads a <see cref="WADFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">The location to read from</param>
        public WADFile(string fileLocation, bool readData) : this(File.OpenRead(fileLocation), readData) { }

        /// <summary>
        /// Reads a <see cref="WADFile"/> from the specified stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        public WADFile(Stream stream, bool readData)
        {
            //Creates a copy of stream which is independent
            byte[] dataBuffer = new byte[stream.Length];
            stream.Read(dataBuffer, 0, (int)stream.Length);
            stream.Seek(0, SeekOrigin.Begin);
            this._dataBuffer = new MemoryStream(dataBuffer);

            using (BinaryReader br = new BinaryReader(stream))
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

                Parallel.ForEach(this.Entries.Where(x => x.Type == EntryType.String), entry =>
                {
                    entry.ReadData();
                });

                if (readData)
                {
                    Parallel.ForEach(this.Entries, entry =>
                    {
                        if (entry.Data == null)
                        {
                            entry.ReadData();
                        }
                    });
                }
            }
        }

        public void Extract(string directoryLocation, bool identifyFiles)
        {
            if (!Directory.Exists(directoryLocation))
            {
                Directory.CreateDirectory(directoryLocation);
            }
            //Using Parallel Foreach loop to make extraction super duper fast
            Parallel.ForEach(this.Entries, entry =>
            {
                if (identifyFiles)
                {
                    File.WriteAllBytes(string.Format("{0}//{1}.{2}", directoryLocation, Utilities.ByteArrayToHex(entry.XXHash), entry.GetEntryExtension()), entry.Data);
                }
                else
                {
                    File.WriteAllBytes(string.Format("{0}//{1}", directoryLocation, Utilities.ByteArrayToHex(entry.XXHash)), entry.Data);
                }
            });
        }
    }
}
