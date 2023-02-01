using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Extensions;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Animation;

public sealed class UncompressedAnimationAsset : IAnimationAsset
{
    public float Duration { get; private set; }
    public float Fps { get; private set; }

    private Vector3[] _vectorPalette;
    private Quaternion[] _quatPalette;
    private Dictionary<uint, UncompressedFrame[]> _jointFrames;

    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }

    public UncompressedAnimationAsset(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
        uint version = br.ReadUInt32();

        if (magic is "r3d2canm")
            ThrowHelper.ThrowInvalidOperationException("Cannot read a compressed animation asset as uncompressed");
        else if (magic is not "r3d2anmd")
            ThrowHelper.ThrowInvalidOperationException($"Invalid file signature: {magic}");

        if (version is 5)
            ReadV5(br);
        else if (version is 4)
            ReadV4(br);
        else if (version is 3)
            ReadLegacy(br);
        else
            ThrowHelper.ThrowInvalidOperationException($"Invalid version: {version}");
    }

    private void ReadV4(BinaryReader br)
    {
        uint resourceSize = br.ReadUInt32();
        uint formatToken = br.ReadUInt32();
        uint version = br.ReadUInt32();
        uint flags = br.ReadUInt32();

        int trackCount = br.ReadInt32();
        int frameCount = br.ReadInt32();
        float frameDuration = br.ReadSingle();

        int jointNameHashesOffset = br.ReadInt32();
        int assetNameOffset = br.ReadInt32();
        int timeOffset = br.ReadInt32();
        int vectorPaletteOffset = br.ReadInt32();
        int quatPaletteOffset = br.ReadInt32();
        int framesOffset = br.ReadInt32();

        // V4 stores joint hashes in frames so we don't check that offset
        if (vectorPaletteOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a vector palette");
        if (quatPaletteOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a quaternion palette");
        if (framesOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain any frame data");

        // Read vector palette
        br.BaseStream.Seek(vectorPaletteOffset + 12, SeekOrigin.Begin);
        int vectorsCount = (quatPaletteOffset - vectorPaletteOffset) / 12;
        this._vectorPalette = new Vector3[vectorsCount];
        for (int i = 0; i < vectorsCount; i++)
            this._vectorPalette[i] = br.ReadVector3();

        // Read quat palette
        br.BaseStream.Seek(quatPaletteOffset + 12, SeekOrigin.Begin);
        int rotationsCount = (framesOffset - quatPaletteOffset) / 16;
        this._quatPalette = new Quaternion[rotationsCount];
        for (int i = 0; i < rotationsCount; i++)
            this._quatPalette[i] = Quaternion.Normalize(br.ReadQuaternion());

        // Read frames
        this._jointFrames = new(trackCount);

        br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
        for (int frameId = 0; frameId < frameCount; frameId++)
            for (int trackId = 0; trackId < trackCount; trackId++)
            {
                // Try to get the frame buffer for the given joint, create it if it doesn't exist
                uint jointHash = br.ReadUInt32();
                if (!this._jointFrames.TryGetValue(jointHash, out UncompressedFrame[] jointFrames))
                {
                    jointFrames = new UncompressedFrame[frameCount];
                    this._jointFrames[jointHash] = jointFrames;
                }

                jointFrames[frameId] = new()
                {
                    TranslationId = br.ReadUInt16(),
                    ScaleId = br.ReadUInt16(),
                    RotationId = br.ReadUInt16()
                };

                br.ReadUInt16(); // padding
            }
    }

    private void ReadV5(BinaryReader br)
    {
        uint resourceSize = br.ReadUInt32();
        uint formatToken = br.ReadUInt32();
        uint version = br.ReadUInt32();
        uint flags = br.ReadUInt32();

        int trackCount = br.ReadInt32();
        int frameCount = br.ReadInt32();
        float frameDuration = br.ReadSingle();

        int jointNameHashesOffset = br.ReadInt32();
        int assetNameOffset = br.ReadInt32();
        int timeOffset = br.ReadInt32();
        int vectorPaletteOffset = br.ReadInt32();
        int quatPaletteOffset = br.ReadInt32();
        int framesOffset = br.ReadInt32();

        if (jointNameHashesOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain any joint data");
        if (vectorPaletteOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a vector palette");
        if (quatPaletteOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a quaternion palette");
        if (framesOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain any frame data");

        // Read Joint Hashes
        int jointHashesCount = (framesOffset - jointNameHashesOffset) / sizeof(uint);
        uint[] jointHashes = new uint[jointHashesCount];

        br.BaseStream.Seek(jointNameHashesOffset + 12, SeekOrigin.Begin);
        for (int i = 0; i < jointHashesCount; i++)
            jointHashes[i] = br.ReadUInt32();

        // Read Vectors
        int vectorCount = (quatPaletteOffset - vectorPaletteOffset) / 12;
        this._vectorPalette = new Vector3[vectorCount];

        br.BaseStream.Seek(vectorPaletteOffset + 12, SeekOrigin.Begin);
        for (int i = 0; i < vectorCount; i++)
            this._vectorPalette[i] = br.ReadVector3();

        // Read Rotations
        Span<byte> quantizedRotation = stackalloc byte[6];
        int quatCount = (jointNameHashesOffset - quatPaletteOffset) / 6;
        this._quatPalette = new Quaternion[quatCount];

        br.BaseStream.Seek(quatPaletteOffset + 12, SeekOrigin.Begin);
        for (int i = 0; i < quatCount; i++)
        {
            br.Read(quantizedRotation);
            this._quatPalette[i] = Quaternion.Normalize(QuantizedQuaternion.Decompress(quantizedRotation));
        }

        // Read frames
        this._jointFrames = new(trackCount);

        br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
        for (int frameId = 0; frameId < frameCount; frameId++)
            for (int trackId = 0; trackId < trackCount; trackId++)
            {
                // Try to get the frame buffer for the given joint, create it if it doesn't exist
                uint jointHash = jointHashes[trackId];
                if (!this._jointFrames.TryGetValue(jointHash, out UncompressedFrame[] jointFrames))
                {
                    jointFrames = new UncompressedFrame[frameCount];
                    this._jointFrames[jointHash] = jointFrames;
                }

                jointFrames[frameId] = new()
                {
                    TranslationId = br.ReadUInt16(),
                    ScaleId = br.ReadUInt16(),
                    RotationId = br.ReadUInt16()
                };
            }
    }

    private void ReadLegacy(BinaryReader br)
    {
        uint skeletonId = br.ReadUInt32();

        int trackCount = br.ReadInt32();
        int frameCount = br.ReadInt32();
        this.Fps = br.ReadInt32();
        this.Duration = frameCount / this.Fps;

        float frameDuration = 1.0f / this.Fps;

        // TODO
        for (int i = 0; i < trackCount; i++)
        {
            string trackName = br.ReadPaddedString(32);
            uint flags = br.ReadUInt32();

            AnimationTrack track = new(Elf.HashLower(trackName));

            for (int j = 0; j < frameCount; j++)
            {
                float frameTime = frameDuration * j;

                track.Rotations.Add(frameTime, br.ReadQuaternion());
                track.Translations.Add(frameTime, br.ReadVector3());
                track.Scales.Add(frameTime, new Vector3(1, 1, 1));
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.IsDisposed)
            return;

        if (disposing) { }

        this.IsDisposed = true;
    }

    public Quaternion SampleTrackRotation(uint jointHash, float time) => throw new NotImplementedException();
}
