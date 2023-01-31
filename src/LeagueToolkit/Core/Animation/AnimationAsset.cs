using System.Text;

namespace LeagueToolkit.Core.Animation;

public interface IAnimationAsset : IDisposable
{
    float Duration { get; }
    float Fps { get; }

    /// <summary>
    /// Gets a value indicating whether the asset has been disposed of
    /// </summary>
    bool IsDisposed { get; }
}

public static class AnimationAsset
{
    public static IAnimationAsset Load(Stream stream)
    {
        return Identify(stream) switch
        {
            AnimationAssetType.Uncompressed => new UncompressedAnimationAsset(stream),
            AnimationAssetType.Compressed => new CompressedAnimationAsset(stream),
            _ => throw new InvalidOperationException("Cannot load unknown animation asset"),
        };
    }

    public static AnimationAssetType Identify(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
        br.BaseStream.Seek(0, SeekOrigin.Begin);

        return magic switch
        {
            "r3d2anmd" => AnimationAssetType.Uncompressed,
            "r3d2canm" => AnimationAssetType.Compressed,
            _ => AnimationAssetType.Unknown,
        };
    }
}

public enum AnimationAssetType
{
    Unknown,
    Uncompressed,
    Compressed,
}
