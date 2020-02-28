using System;
using System.IO;
using System.Text;
using FlatSharp;
using Fantome.Libraries.League.Helpers.Compression;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANFile
    {
        public ulong Id { get; set; }
        public ushort Flags { get; set; }
        public RMANBody Body { get; set; }

        public RMANFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "RMAN")
                {
                    throw new Exception("This is not a valid RMAN file");
                }
                byte major = br.ReadByte();
                byte minor = br.ReadByte();
                // NOTE: only check major because minor version are compatabile forwards-backwards
                if (major != 2)
                {
                    throw new Exception($"This RMAN file is of an unsupported version: ${major}");
                }
                Flags = br.ReadUInt16();
                uint contentOffset = br.ReadUInt32();
                uint compressedContentSize = br.ReadUInt32();
                this.Id = br.ReadUInt64();
                uint uncompressedContentSize = br.ReadUInt32();
                br.BaseStream.Seek(contentOffset, SeekOrigin.Begin);
                byte[] compressedFile = br.ReadBytes((int)compressedContentSize);
                byte[] uncompressedFile = Compression.DecompressZStandard(compressedFile);
                this.Body = FlatBufferSerializer.Default.Parse<RMANBody>(uncompressedFile);
            }
        }
    }
}
