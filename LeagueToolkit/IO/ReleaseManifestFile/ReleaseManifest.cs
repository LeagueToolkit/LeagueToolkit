using LeagueToolkit.Helpers.Compression;
using LeagueToolkit.Helpers.Exceptions;
using FlatSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZstdSharp;

namespace LeagueToolkit.IO.ReleaseManifestFile
{
    public class ReleaseManifest
    {
        public ulong ID { get; private set; }
        public IList<ReleaseManifestBundle> Bundles => this._body.Bundles;
        public IList<ReleaseManifestLanguage> Languages => this._body.Languages;
        public IList<ReleaseManifestFile> Files => this._body.Files;
        public IList<ReleaseManifestDirectory> Directories => this._body.Directories;
        public IList<ReleaseManifestEncryptionKey> EncryptionKeys => this._body.EncryptionKeys;
        public IList<ReleaseManifestChunkingParameter> ChunkingParameters => this._body.ChunkingParameters;

        private readonly ReleaseManifestBody _body;

        public ReleaseManifest(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public ReleaseManifest(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "RMAN")
                {
                    throw new InvalidFileSignatureException();
                }

                byte major = br.ReadByte();
                byte minor = br.ReadByte();
                // NOTE: only check major because minor version are compatabile forwards-backwards
                if (major != 2)
                {
                    throw new UnsupportedFileVersionException();
                }

                //Could possibly be Compression Type
                byte unknown = br.ReadByte();
                if (unknown != 0)
                {
                    throw new Exception("Unknown: " + unknown);
                }

                byte signatureType = br.ReadByte();
                uint contentOffset = br.ReadUInt32();
                uint compressedContentSize = br.ReadUInt32();
                this.ID = br.ReadUInt64();
                uint uncompressedContentSize = br.ReadUInt32();

                br.BaseStream.Seek(contentOffset, SeekOrigin.Begin);
                byte[] compressedFile = br.ReadBytes((int)compressedContentSize);

                if (signatureType != 0)
                {
                    byte[] signature = br.ReadBytes(256);
                    // NOTE: verify signature here
                }
                byte[] uncompressedFile = Zstd.Decompress(compressedFile, (int)uncompressedContentSize);
                this._body = FlatBufferSerializer.Default.Parse<ReleaseManifestBody>(uncompressedFile);
            }
        }

        public void Write(string fileLocation) => Write(File.Create(fileLocation));
        public void Write(Stream stream, bool leaveOpen = false)
        {
            byte[] magic = Encoding.ASCII.GetBytes("RMAN");
            byte major = 2;
            byte minor = 0;
            byte unknown = 0;
            byte signatureType = 0;
            int contentOffset = 4 + 4 + 4 + 4 + 8 + 4;

            byte[] uncompressedFile = new byte[FlatBufferSerializer.Default.GetMaxSize(this._body)];
            int uncompressedContentSize = FlatBufferSerializer.Default.Serialize(this._body, uncompressedFile);
            Array.Resize(ref uncompressedFile, uncompressedContentSize);

            byte[] compressedFile = Zstd.Compress(uncompressedFile);
            int compressedContentSize = compressedFile.Length;

            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen))
            {
                bw.Write(magic);
                bw.Write(major);
                bw.Write(minor);
                bw.Write(unknown);
                bw.Write(signatureType);
                bw.Write(contentOffset);
                bw.Write(compressedContentSize);
                bw.Write(this.ID);
                bw.Write(uncompressedContentSize);
                bw.Write(compressedFile);
            }
        }
    }
}
