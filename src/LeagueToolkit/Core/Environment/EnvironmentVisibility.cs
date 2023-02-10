namespace LeagueToolkit.Core.Environment;

/// <summary>
/// Used for limiting the visibility of environment assets
/// </summary>
[Flags]
public enum EnvironmentVisibility : byte
{
    /// <summary>
    /// Toggles visibility on no layers
    /// </summary>
    NoLayer = 0,

    Layer1 = 1 << 0,
    Layer2 = 1 << 1,
    Layer3 = 1 << 2,
    Layer4 = 1 << 3,
    Layer5 = 1 << 4,
    Layer6 = 1 << 5,
    Layer7 = 1 << 6,
    Layer8 = 1 << 7,

    /// <summary>
    /// Toggles visibility on all layers
    /// </summary>
    AllLayers = Layer1 | Layer2 | Layer3 | Layer4 | Layer5 | Layer6 | Layer7 | Layer8
}
