using FlatSharp;
using LeagueToolkit.Utils.Exceptions;
using System.Text;

namespace LeagueToolkit.IO.ReleaseManifestFile;

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
        using BinaryReader br = new(stream);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic != "RMAN")
            throw new InvalidFileSignatureException();

        byte major = br.ReadByte();
        byte minor = br.ReadByte();
        // NOTE: only check major because minor version are compatabile forwards-backwards
        if (major != 2)
            throw new InvalidFileVersionException();

        //Could possibly be Compression Type
        byte unknown = br.ReadByte();
        if (unknown != 0)
            throw new Exception("Unknown: " + unknown);

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

        using var decompressor = new ZstdSharp.Decompressor();
        byte[] decompressedBuffer = new byte[uncompressedContentSize];
        decompressor.Unwrap(compressedFile, decompressedBuffer);

        this._body = ReleaseManifestBody.Serializer.Parse(decompressedBuffer);
    }

    public void Write(string fileLocation) => Write(File.Create(fileLocation));

    public void Write(Stream stream, bool leaveOpen = false)
    {
        ReadOnlySpan<byte> magic = "RMAN"u8;
        byte major = 2;
        byte minor = 0;
        byte unknown = 0;
        byte signatureType = 0;
        int contentOffset = 4 + 4 + 4 + 4 + 8 + 4;

        byte[] uncompressedFile = new byte[ReleaseManifestBody.Serializer.GetMaxSize(this._body)];
        int uncompressedContentSize = ReleaseManifestBody.Serializer.Write(uncompressedFile, this._body);

        using var compressor = new ZstdSharp.Compressor();
        var compressedFile = compressor.Wrap(new Span<byte>(uncompressedFile, 0, uncompressedContentSize));

        int compressedContentSize = compressedFile.Length;

        using BinaryWriter bw = new(stream, Encoding.UTF8, leaveOpen);

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
