using BCnEncoder.Shared;
using CommunityToolkit.HighPerformance;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Toolkit
{
    public static class TextureExtensions
    {
        public static Image<Rgba32> ToImage(this ReadOnlyMemory2D<ColorRgba32> colorData)
        {
            Image<Rgba32> image = new(colorData.Width, colorData.Height);
            for (int i = 0; i < colorData.Height; i++)
            {
                ReadOnlySpan<ColorRgba32> colorDataRow = colorData.Span.GetRowSpan(i);
                Span<Rgba32> imageRow = image.Frames.RootFrame.PixelBuffer.DangerousGetRowSpan(i);

                MemoryMarshal.Cast<ColorRgba32, Rgba32>(colorDataRow).CopyTo(imageRow);
            }

            return image;
        }
    }
}
