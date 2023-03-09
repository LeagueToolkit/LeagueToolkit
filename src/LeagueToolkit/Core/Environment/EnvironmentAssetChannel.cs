using LeagueToolkit.Utils.Extensions;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Environment;

/// <summary>
/// Represents a sampling channel
/// </summary>
public struct EnvironmentAssetChannel
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
    /// Creates a new <see cref="EnvironmentAssetChannel"/> object
    /// </summary>
    public EnvironmentAssetChannel()
    {
        this.Texture = string.Empty;
        this.Scale = Vector2.Zero;
        this.Bias = Vector2.Zero;
    }

    /// <summary>
    /// Creates a new <see cref="EnvironmentAssetChannel"/> object with the specified parameters
    /// </summary>
    public EnvironmentAssetChannel(string texture, Vector2 scale, Vector2 bias)
    {
        this.Texture = texture;
        this.Scale = scale;
        this.Bias = bias;
    }

    internal static EnvironmentAssetChannel Read(BinaryReader br)
    {
        return new()
        {
            Texture = Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32())),
            Scale = br.ReadVector2(),
            Bias = br.ReadVector2()
        };
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(this.Texture?.Length ?? 0);
        if (!string.IsNullOrEmpty(this.Texture))
            bw.Write(Encoding.UTF8.GetBytes(this.Texture));

        bw.WriteVector2(this.Scale);
        bw.WriteVector2(this.Bias);
    }
}
