namespace LeagueToolkit.Core.Animation;

internal readonly struct UncompressedFrame
{
    public ushort TranslationId { get; init; }
    public ushort ScaleId { get; init; }
    public ushort RotationId { get; init; }
}
