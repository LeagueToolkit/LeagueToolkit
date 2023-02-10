namespace LeagueToolkit.Core.Environment;

/// <summary>
/// Contains information about which samplers should be used for sampling "BAKED_PAINT" textures
/// </summary>
public struct EnvironmentAssetBakedTerrainSamplers
{
    /// <summary>
    /// The name of the primary sampler
    /// </summary>
    /// <remarks>
    /// Known values: <c>BAKED_DIFFUSE_TEXTURE</c>
    /// </remarks>
    public string Primary;

    /// <summary>
    /// The name of the secondary sampler
    /// </summary>
    /// <remarks>
    /// This sampler uses the same description and texture handle as <see cref="Primary"/>
    /// <br>
    /// Known values: <c>BAKED_DIFFUSE_TEXTURE_ALPHA</c>
    /// </br>
    /// </remarks>
    public string Secondary;

    public EnvironmentAssetBakedTerrainSamplers()
    {
        this.Primary = string.Empty;
        this.Secondary = string.Empty;
    }

    public EnvironmentAssetBakedTerrainSamplers(string primary, string secondary)
    {
        this.Primary = primary;
        this.Secondary = secondary;
    }
}
