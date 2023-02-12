using BCnEncoder.Shared;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.Toolkit;
using SharpGLTF.Schema2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using LeagueTexture = LeagueToolkit.Core.Renderer.Texture;
using GltfImage = SharpGLTF.Schema2.Image;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;

namespace LeagueToolkit.IO.Extensions.MapGeometry;

internal static class TextureUtils
{
    private const string TEXTURE_QUALITY_PREFIX_LOW = "4x";
    private const string TEXTURE_QUALITY_PREFIX_MEDIUM = "2x";

    public static LeagueTexture Load(string texturePath)
    {
        // Get texture file format and return if it's unknown
        using FileStream textureStream = File.OpenRead(texturePath);
        TextureFileFormat format = LeagueTexture.IdentifyFileFormat(textureStream);
        if (format is TextureFileFormat.Unknown)
            return null;

        // Load and register texture
        return LeagueTexture.Load(textureStream);
    }

    public static GltfImage CreateGltfImage(
        string name,
        Image<Rgba32> image,
        ModelRoot root,
        TextureRegistry textureRegistry
    )
    {
        if (textureRegistry.TryGetValue(name, out GltfImage existingImage))
            return existingImage;

        using MemoryStream imageStream = new();
        image.SaveAsPng(imageStream);

        // Create glTF image
        GltfImage gltfImage = root.UseImage(new(imageStream.ToArray()));
        gltfImage.Name = Path.GetFileNameWithoutExtension(name);

        textureRegistry.Add(name, gltfImage);
        return gltfImage;
    }

    public static Image<Rgba32> GetImage(LeagueTexture texture)
    {
        ReadOnlyMemory2D<ColorRgba32> biggestMipMap = texture.Mips[0];
        return biggestMipMap.ToImage();
    }

    public static string GetQualityPrefixedTexturePath(string texturePath, MapGeometryGltfTextureQuality quality)
    {
        string prefixedPath = quality switch
        {
            MapGeometryGltfTextureQuality.Low
                => Path.Combine(
                    Path.GetDirectoryName(texturePath),
                    $"{TEXTURE_QUALITY_PREFIX_LOW}_{Path.GetFileName(texturePath)}"
                ),
            MapGeometryGltfTextureQuality.Medium
                => Path.Combine(
                    Path.GetDirectoryName(texturePath),
                    $"{TEXTURE_QUALITY_PREFIX_MEDIUM}_{Path.GetFileName(texturePath)}"
                ),
            MapGeometryGltfTextureQuality.High => texturePath,
            _ => throw new NotImplementedException($"Invalid {nameof(MapGeometryGltfTextureQuality)}: {quality}"),
        };

        // Check if file exists, otherwise return non-prefixed
        return File.Exists(prefixedPath) switch
        {
            true => prefixedPath,
            false => texturePath
        };
    }
}
