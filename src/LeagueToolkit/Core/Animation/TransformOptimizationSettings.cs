using System.IO;

namespace LeagueToolkit.Core.Animation;

/// <summary>
/// Represents the optimization settings used for a specific joint transform property
/// </summary>
public readonly struct TransformOptimizationSettings
{
    /// <summary>
    /// The maximum allowed error
    /// </summary>
    public float ErrorMargin { get; }

    /// <summary>
    /// The distance at which the error is measured
    /// </summary>
    public float DiscontinuityThreshold { get; }

    public TransformOptimizationSettings()
    {
        this.ErrorMargin = 2f;
        this.DiscontinuityThreshold = 10f;
    }

    public TransformOptimizationSettings(float errorMargin, float discontinuityThreshold)
    {
        this.ErrorMargin = errorMargin;
        this.DiscontinuityThreshold = discontinuityThreshold;
    }

    internal static TransformOptimizationSettings Read(BinaryReader br)
    {
        float errorMargin = br.ReadSingle();
        float discontinuityThreshold = br.ReadSingle();

        return new(errorMargin, discontinuityThreshold);
    }
}
