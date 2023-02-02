namespace LeagueToolkit.Core.Animation;

/// <summary>
/// Represents the optimization settings of a transform component
/// </summary>
public readonly struct ErrorMetric
{
    /// <summary>
    /// The maximum allowed error
    /// </summary>
    public float Margin { get; }

    /// <summary>
    /// The distance at which the error is measured
    /// </summary>
    public float DiscontinuityThreshold { get; }

    public ErrorMetric()
    {
        this.Margin = 2f;
        this.DiscontinuityThreshold = 10f;
    }

    public ErrorMetric(float margin, float discontinuityThreshold)
    {
        this.Margin = margin;
        this.DiscontinuityThreshold = discontinuityThreshold;
    }

    internal static ErrorMetric Read(BinaryReader br)
    {
        float errorMargin = br.ReadSingle();
        float discontinuityThreshold = br.ReadSingle();

        return new(errorMargin, discontinuityThreshold);
    }
}
