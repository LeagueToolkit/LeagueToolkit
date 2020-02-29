using System;
using System.IO;
using System.Text;
using FlatSharp;
using Fantome.Libraries.League.Helpers.Compression;
using System.Linq;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANFile
    {
        public ulong Id { get; set; }
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
                //Could possibly be Compression Type
                byte unknown = br.ReadByte();
                if (unknown != 0) throw new Exception("Unknown: " + unknown);
                byte signatureType = br.ReadByte();
                uint contentOffset = br.ReadUInt32();
                uint compressedContentSize = br.ReadUInt32();
                this.Id = br.ReadUInt64();
                uint uncompressedContentSize = br.ReadUInt32();
                br.BaseStream.Seek(contentOffset, SeekOrigin.Begin);
                byte[] compressedFile = br.ReadBytes((int)compressedContentSize);
                if (signatureType != 0)
                {
                    byte[] signature = br.ReadBytes(256);
                    // NOTE: verify signature here
                }
                byte[] uncompressedFile = Compression.DecompressZStandard(compressedFile);
                this.Body = FlatBufferSerializer.Default.Parse<RMANBody>(uncompressedFile);
            }
        }

        public void Write(Stream stream) 
        {
            byte[] magic = Encoding.ASCII.GetBytes("RMAN");
            byte major = 2;
            byte minor = 0;
            byte unknown = 0;
            byte signatureType = 0;
            int contentOffset = 4 + 4 + 4 + 4 + 8 + 4;
            byte[] uncompressedFile = new byte[FlatBufferSerializer.Default.GetMaxSize(Body)];
            int uncompressedContentSize = FlatBufferSerializer.Default.Serialize(Body, uncompressedFile);
            Array.Resize(ref uncompressedFile, uncompressedContentSize);
            byte[] compressedFile = Compression.CompressZStandard(uncompressedFile);
            int compressedContentSize = compressedFile.Length;
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(magic);
                bw.Write(major);
                bw.Write(minor);
                bw.Write(unknown);
                bw.Write(signatureType);
                bw.Write(contentOffset);
                bw.Write(compressedContentSize);
                bw.Write(Id);
                bw.Write(uncompressedContentSize);
                bw.Write(compressedFile);
            }
        }
    }
}
