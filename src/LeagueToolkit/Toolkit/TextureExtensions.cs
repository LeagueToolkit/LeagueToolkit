using CommunityToolkit.HighPerformance;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LeagueToolkit.Toolkit;

public static class TextureExtensions
{
    public static Image<Rgba32> ToImage(this Memory2D<Rgba32> colorData) =>
        ToImage((ReadOnlyMemory2D<Rgba32>)colorData);

    public static Image<Rgba32> ToImage(this ReadOnlyMemory2D<Rgba32> colorData)
    {
        Image<Rgba32> image = new(colorData.Width, colorData.Height);
        for (int i = 0; i < colorData.Height; i++)
        {
            ReadOnlySpan<Rgba32> colorDataRow = colorData.Span.GetRowSpan(i);
            Span<Rgba32> imageRow = image.Frames.RootFrame.PixelBuffer.DangerousGetRowSpan(i);

            colorDataRow.CopyTo(imageRow);
        }

        return image;
    }
}
