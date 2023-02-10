using LeagueToolkit.Helpers.Extensions;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Environment;

/// <summary>
/// Represents a texture sampler
/// </summary>
public struct EnvironmentAssetSampler
{
    /// <summary>
    /// Gets the texture
    /// </summary>
    public string Texture;

    /// <summary>
    /// Gets the UV scale being used to sample from <see cref="Texture"/>
    /// </summary>
    public Vector2 Scale;

    /// <summary>
    /// Gets the UV offset being used to sample from <see cref="Texture"/>
    /// </summary>
    public Vector2 Bias;

    /// <summary>
    /// Creates a new <see cref="EnvironmentAssetSampler"/> object
    /// </summary>
    public EnvironmentAssetSampler()
    {
        this.Texture = string.Empty;
        this.Scale = Vector2.Zero;
        this.Bias = Vector2.Zero;
    }

    /// <summary>
    /// Creates a new <see cref="EnvironmentAssetSampler"/> object with the specified parameters
    /// </summary>
    public EnvironmentAssetSampler(string texture, Vector2 scale, Vector2 bias)
    {
        this.Texture = texture;
        this.Scale = scale;
        this.Bias = bias;
    }

    internal static EnvironmentAssetSampler Read(BinaryReader br)
    {
        return new()
        {
            Texture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
            Scale = br.ReadVector2(),
            Bias = br.ReadVector2()
        };
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(this.Texture.Length);
        bw.Write(Encoding.ASCII.GetBytes(this.Texture ?? string.Empty));
        bw.WriteVector2(this.Scale);
        bw.WriteVector2(this.Bias);
    }
}
