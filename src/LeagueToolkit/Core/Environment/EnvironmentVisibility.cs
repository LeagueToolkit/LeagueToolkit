namespace LeagueToolkit.Core.Environment;

/// <summary>
/// Used for specifying the visibility of environment objects
/// </summary>
[Flags]
public enum EnvironmentVisibility : ushort
{
    /// <summary>
    /// Not visible on any layer
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
    /// Visibility on all layers
    /// </summary>
    AllLayers = Layer1 | Layer2 | Layer3 | Layer4 | Layer5 | Layer6 | Layer7 | Layer8
}
