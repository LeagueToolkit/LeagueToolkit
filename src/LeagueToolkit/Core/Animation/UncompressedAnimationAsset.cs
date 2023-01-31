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

    public List<AnimationTrack> Tracks { get; private set; } = new();

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

        int tracksOffset = br.ReadInt32();
        int assetNameOffset = br.ReadInt32();
        int timeOffset = br.ReadInt32();
        int vectorsOffset = br.ReadInt32();
        int rotationsOffset = br.ReadInt32();
        int framesOffset = br.ReadInt32();

        if (vectorsOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a vector palette");
        if (rotationsOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a quaternion palette");
        if (framesOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain any frame data");

        int vectorsCount = (rotationsOffset - vectorsOffset) / 12;
        int rotationsCount = (framesOffset - rotationsOffset) / 16;

        br.BaseStream.Seek(vectorsOffset + 12, SeekOrigin.Begin);
        List<Vector3> vectors = new();
        for (int i = 0; i < vectorsCount; i++)
        {
            vectors.Add(br.ReadVector3());
        }

        br.BaseStream.Seek(rotationsOffset + 12, SeekOrigin.Begin);
        List<Quaternion> rotations = new();
        for (int i = 0; i < rotationsCount; i++)
        {
            rotations.Add(Quaternion.Normalize(br.ReadQuaternion()));
        }

        br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
        List<(uint, ushort, ushort, ushort)> frames = new(frameCount * trackCount);
        for (int i = 0; i < frameCount * trackCount; i++)
        {
            frames.Add((br.ReadUInt32(), br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
            br.ReadUInt16(); // padding
        }

        foreach ((uint jointHash, ushort translationIndex, ushort scaleIndex, ushort rotationIndex) in frames)
        {
            if (!this.Tracks.Any(x => x.JointHash == jointHash))
            {
                this.Tracks.Add(new AnimationTrack(jointHash));
            }

            AnimationTrack track = this.Tracks.First(x => x.JointHash == jointHash);

            int trackFrameTranslationIndex = track.Translations.Count;
            int trackFrameScaleIndex = track.Scales.Count;
            int trackFrameRotationIndex = track.Rotations.Count;

            Vector3 translation = vectors[translationIndex];
            Vector3 scale = vectors[scaleIndex];
            Quaternion rotation = rotations[rotationIndex];

            track.Translations.Add(frameDuration * trackFrameTranslationIndex, translation);
            track.Scales.Add(frameDuration * trackFrameScaleIndex, scale);
            track.Rotations.Add(frameDuration * trackFrameRotationIndex, rotation);
        }
    }

    private void ReadV5(BinaryReader br)
    {
        uint resourceSize = br.ReadUInt32();
        uint formatToken = br.ReadUInt32();
        uint version = br.ReadUInt32();
        uint flags = br.ReadUInt32();

        int trackCount = br.ReadInt32();
        int framesPerTrack = br.ReadInt32();
        float frameDuration = br.ReadSingle();

        int jointHashesOffset = br.ReadInt32();
        int assetNameOffset = br.ReadInt32();
        int timeOffset = br.ReadInt32();
        int vectorsOffset = br.ReadInt32();
        int rotationsOffset = br.ReadInt32();
        int framesOffset = br.ReadInt32();

        if (jointHashesOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain any joint data");
        if (vectorsOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a vector palette");
        if (rotationsOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain a quaternion palette");
        if (framesOffset <= 0)
            ThrowHelper.ThrowInvalidDataException("Animation does not contain any frame data");

        int jointHashesCount = (framesOffset - jointHashesOffset) / sizeof(uint);
        int vectorsCount = (rotationsOffset - vectorsOffset) / 12;
        int rotationsCount = (jointHashesOffset - rotationsOffset) / 6;

        List<uint> jointHashes = new(jointHashesCount);
        List<Vector3> vectors = new(vectorsCount);
        List<Quaternion> rotations = new(rotationsCount);
        var frames = new List<(ushort, ushort, ushort)>(framesPerTrack * trackCount);

        // Read Joint Hashes
        br.BaseStream.Seek(jointHashesOffset + 12, SeekOrigin.Begin);
        for (int i = 0; i < jointHashesCount; i++)
        {
            jointHashes.Add(br.ReadUInt32());
        }

        // Read Vectors
        br.BaseStream.Seek(vectorsOffset + 12, SeekOrigin.Begin);
        for (int i = 0; i < vectorsCount; i++)
        {
            vectors.Add(br.ReadVector3());
        }

        // Read Rotations
        Span<byte> quantizedRotation = stackalloc byte[6];
        br.BaseStream.Seek(rotationsOffset + 12, SeekOrigin.Begin);
        for (int i = 0; i < rotationsCount; i++)
        {
            br.Read(quantizedRotation);
            rotations.Add(Quaternion.Normalize(QuantizedQuaternion.Decompress(quantizedRotation)));
        }

        // Read Frames
        br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
        for (int i = 0; i < framesPerTrack * trackCount; i++)
        {
            frames.Add((br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
        }

        // Create tracks
        for (int i = 0; i < trackCount; i++)
        {
            this.Tracks.Add(new AnimationTrack(jointHashes[i]));
        }

        for (int t = 0; t < trackCount; t++)
        {
            AnimationTrack track = this.Tracks[t];
            for (int f = 0; f < framesPerTrack; f++)
            {
                (int translationIndex, int scaleIndex, int rotationIndex) = frames[f * trackCount + t];
                float currentTime = frameDuration * f;

                track.Translations.Add(currentTime, vectors[translationIndex]);
                track.Scales.Add(currentTime, vectors[scaleIndex]);
                track.Rotations.Add(currentTime, rotations[rotationIndex]);
            }
        }
    }

    private void ReadLegacy(BinaryReader br)
    {
        uint skeletonId = br.ReadUInt32();

        int trackCount = br.ReadInt32();
        int frameCount = br.ReadInt32();
        this.Fps = br.ReadInt32();

        float frameDuration = 1.0f / this.Fps;

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

            this.Tracks.Add(track);
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
}
