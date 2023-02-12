using LeagueToolkit.Core.Environment;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.Meta.Classes;
using SharpGLTF.Schema2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using GltfImage = SharpGLTF.Schema2.Image;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;

namespace LeagueToolkit.IO.Extensions.MapGeometry.Shaders;

internal static class StaticMeshHologram
{
    private static readonly string DEFAULT_DIFFUSE_TEXTURE = "ASSETS/Shared/Materials/black.dds";
    private static readonly string DEFAULT_MASK_TEXTURE = "ASSETS/Shared/Materials/black.dds";

    public static void InitializeMaterial(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        EnvironmentAssetMesh mesh,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        gltfMaterial.WithPBRMetallicRoughness();
        gltfMaterial.WithChannelFactor("MetallicRoughness", "MetallicFactor", 0f);
        gltfMaterial.WithChannelFactor("MetallicRoughness", "RoughnessFactor", 0f);

        gltfMaterial.Alpha = AlphaMode.MASK;
        gltfMaterial.AlphaCutoff = 0.3f;

        InitializeChannels(gltfMaterial, materialDef, mesh, textureRegistry, root, context);
    }

    private static void InitializeChannels(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        EnvironmentAssetMesh mesh,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        StaticMaterialShaderParamDef emissiveIntensityDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "Emissive_Intensity"
        );

        var (baseColorTexture, emissiveTexture) = CreateBaseColorAndEmissiveTexture(
            materialDef,
            textureRegistry,
            root,
            context
        );

        gltfMaterial.WithChannelFactor("Emissive", "EmissiveStrength", emissiveIntensityDef?.Value.X ?? 0.3f);
        gltfMaterial.WithChannelColor("Emissive", Vector4.One);
        gltfMaterial.WithChannelTexture("Emissive", 0, emissiveTexture);
        gltfMaterial.WithChannelTexture("BaseColor", 0, baseColorTexture);
    }

    private static (GltfImage BaseColorTexture, GltfImage EmissiveTexture) CreateBaseColorAndEmissiveTexture(
        StaticMaterialDef materialDef,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        // If textures are already loaded, return them
        string diffuseTextureName = GetDiffuseTextureName(materialDef);
        string maskTextureName = GetMaskTextureName(materialDef);
        if (textureRegistry.TryGetValue(diffuseTextureName, out GltfImage existingDiffuse))
            return (existingDiffuse, textureRegistry[maskTextureName]);

        string diffuseTexturePath = TextureUtils.GetQualityPrefixedTexturePath(
            Path.Join(context.Settings.GameDataPath, diffuseTextureName),
            context.Settings.TextureQuality
        );
        string maskTexturePath = TextureUtils.GetQualityPrefixedTexturePath(
            Path.Join(context.Settings.GameDataPath, maskTextureName),
            context.Settings.TextureQuality
        );

        StaticMaterialShaderParamDef bloomColorDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "Bloom_Color"
        );

        Image<Rgba32> diffuseTexture = TextureUtils.GetImage(TextureUtils.Load(diffuseTexturePath));
        Image<Rgba32> maskTexture = TextureUtils.GetImage(TextureUtils.Load(maskTexturePath));

        return (
            TextureUtils.CreateGltfImage(
                diffuseTextureName,
                CreateBaseColorFromDiffuseAndMask(diffuseTexture, maskTexture),
                root,
                textureRegistry
            ),
            TextureUtils.CreateGltfImage(
                maskTextureName,
                CreateEmissiveFromDiffuseAndMask(diffuseTexture, maskTexture, bloomColorDef?.Value ?? Vector4.One),
                root,
                textureRegistry
            )
        );
    }

    private static Image<Rgba32> CreateBaseColorFromDiffuseAndMask(Image<Rgba32> diffuse, Image<Rgba32> mask)
    {
        diffuse.ProcessPixelRows(x =>
        {
            for (int rowId = 0; rowId < x.Height; rowId++)
            {
                Span<Rgba32> row = x.GetRowSpan(rowId);

                for (int i = 0; i < row.Length; i++)
                {
                    row[i].A = mask[i, rowId].B;
                }
            }
        });

        return diffuse;
    }

    private static Image<Rgba32> CreateEmissiveFromDiffuseAndMask(
        Image<Rgba32> diffuse,
        Image<Rgba32> mask,
        Vector4 emissiveColor
    )
    {
        mask.ProcessPixelRows(x =>
        {
            for (int rowId = 0; rowId < x.Height; rowId++)
            {
                Span<Rgba32> row = x.GetRowSpan(rowId);

                for (int i = 0; i < row.Length; i++)
                {
                    Vector4 maskColor = row[i].ToVector4();
                    row[i].FromVector4(diffuse[i, rowId].ToVector4() * ((maskColor.Z * emissiveColor) with { W = 1f }));
                }
            }
        });

        return mask;
    }

    private static string GetDiffuseTextureName(StaticMaterialDef materialDef)
    {
        StaticMaterialShaderSamplerDef sampler = materialDef.SamplerValues.FirstOrDefault(
            x => x.Value.SamplerName is "Diffuse_Texture"
        );

        return string.IsNullOrEmpty(sampler?.TextureName) switch
        {
            true => DEFAULT_DIFFUSE_TEXTURE,
            false => sampler.TextureName
        };
    }

    private static string GetMaskTextureName(StaticMaterialDef materialDef)
    {
        StaticMaterialShaderSamplerDef sampler = materialDef.SamplerValues.FirstOrDefault(
            x => x.Value.SamplerName is "Mask_Texture"
        );

        return string.IsNullOrEmpty(sampler?.TextureName) switch
        {
            true => DEFAULT_MASK_TEXTURE,
            false => sampler.TextureName
        };
    }
}
