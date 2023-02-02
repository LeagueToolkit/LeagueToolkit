using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Extensions;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Animation;

/// <summary>
/// Represents an uncompressed animation asset
/// </summary>
public sealed class UncompressedAnimationAsset : IAnimationAsset
{
    /// <inheritdoc/>
    public float Duration { get; private set; }

    /// <inheritdoc/>
    public float Fps { get; private set; }

    private int _frameCount;
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
        this._frameCount = br.ReadInt32();
        float frameDuration = br.ReadSingle();

        this.Fps = 1 / frameDuration;
        this.Duration = this._frameCount * frameDuration;

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
        for (int frameId = 0; frameId < this._frameCount; frameId++)
            for (int trackId = 0; trackId < trackCount; trackId++)
            {
                // Try to get the frame buffer for the given joint, create it if it doesn't exist
                uint jointHash = br.ReadUInt32();
                if (!this._jointFrames.TryGetValue(jointHash, out UncompressedFrame[] jointFrames))
                {
                    jointFrames = new UncompressedFrame[this._frameCount];
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
        this._frameCount = br.ReadInt32();
        float frameDuration = br.ReadSingle();

        this.Fps = 1 / frameDuration;
        this.Duration = this._frameCount * frameDuration;

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
        for (int frameId = 0; frameId < this._frameCount; frameId++)
            for (int trackId = 0; trackId < trackCount; trackId++)
            {
                // Try to get the frame buffer for the given joint, create it if it doesn't exist
                uint jointHash = jointHashes[trackId];
                if (!this._jointFrames.TryGetValue(jointHash, out UncompressedFrame[] jointFrames))
                {
                    jointFrames = new UncompressedFrame[this._frameCount];
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

        ThrowHelper.ThrowNotSupportedException("Reading legacy animations is not supported");
        // TODO
        //for (int i = 0; i < trackCount; i++)
        //{
        //    string trackName = br.ReadPaddedString(32);
        //    uint flags = br.ReadUInt32();
        //
        //    AnimationTrack track = new(Elf.HashLower(trackName));
        //
        //    for (int j = 0; j < frameCount; j++)
        //    {
        //        float frameTime = frameDuration * j;
        //
        //        track.Rotations.Add(frameTime, br.ReadQuaternion());
        //        track.Translations.Add(frameTime, br.ReadVector3());
        //        track.Scales.Add(frameTime, new Vector3(1, 1, 1));
        //    }
        //}
    }

    /// <inheritdoc/>
    public void Evaluate(float time, IDictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose)
    {
        ThrowIfDisposed();

        float frameDuration = 1 / this.Fps;
        int maxFrame = this._frameCount - 1;
        int evaluationFrame = Math.Min(maxFrame, (int)(time / frameDuration));
        int nextFrame = Math.Min(maxFrame, evaluationFrame + 1);

        float evaluationFrameTime = evaluationFrame * frameDuration;
        float interpolationDuration = (nextFrame * frameDuration) - evaluationFrameTime;

        // Prevent divison by zero, we don't need to interpolate
        if (interpolationDuration is 0.0f)
        {
            Evaluate(evaluationFrame, pose);
            return;
        }

        float amount = (time - evaluationFrameTime) / interpolationDuration;
        if (amount is not 0.0f)
            EvaluateWithInterpolation(evaluationFrame, nextFrame, amount, pose);
        else
            Evaluate(evaluationFrame, pose);
    }

    private void EvaluateWithInterpolation(
        int evaluationFrameId,
        int nextFrameId,
        float amount,
        IDictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose
    )
    {
        foreach (var (jointHash, frames) in this._jointFrames)
        {
            UncompressedFrame evaluationFrame = frames[evaluationFrameId];
            UncompressedFrame nextFrame = frames[nextFrameId];

            Quaternion rotation = Quaternion.Lerp(
                this._quatPalette[evaluationFrame.RotationId],
                this._quatPalette[nextFrame.RotationId],
                amount
            );

            Vector3 translation = Vector3.Lerp(
                this._vectorPalette[evaluationFrame.TranslationId],
                this._vectorPalette[nextFrame.TranslationId],
                amount
            );

            Vector3 scale = Vector3.Lerp(
                this._vectorPalette[evaluationFrame.ScaleId],
                this._vectorPalette[nextFrame.ScaleId],
                amount
            );

            if (!pose.TryAdd(jointHash, (rotation, translation, scale)))
                pose[jointHash] = (rotation, translation, scale);
        }
    }

    private void Evaluate(
        int frameId,
        IDictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose
    )
    {
        foreach (var (jointHash, frames) in this._jointFrames)
        {
            UncompressedFrame frame = frames[frameId];

            Quaternion rotation = this._quatPalette[frame.RotationId];
            Vector3 translation = this._vectorPalette[frame.TranslationId];
            Vector3 scale = this._vectorPalette[frame.ScaleId];

            if (!pose.TryAdd(jointHash, (rotation, translation, scale)))
                pose[jointHash] = (rotation, translation, scale);
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

    private void ThrowIfDisposed()
    {
        if (this.IsDisposed)
            ThrowHelper.ThrowObjectDisposedException(
                nameof(UncompressedAnimationAsset),
                "Cannot use a disposed animation asset"
            );
    }
}
