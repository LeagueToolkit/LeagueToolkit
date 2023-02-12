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
        gltfMaterial.InitializeUnlit();

        gltfMaterial.Alpha = AlphaMode.MASK;
        gltfMaterial.AlphaCutoff = 0.3f;

        gltfMaterial.WithChannelTexture(
            "BaseColor",
            0,
            CreateBaseColorChannelTexture(materialDef, textureRegistry, root, context)
        );
        InitializeEmissiveChannel(gltfMaterial, materialDef, mesh, textureRegistry, root, context);
    }

    private static GltfImage CreateBaseColorChannelTexture(
        StaticMaterialDef materialDef,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        // If texture is already loaded, return it
        string diffuseTextureName = GetDiffuseTextureName(materialDef);
        if (textureRegistry.TryGetValue(diffuseTextureName, out GltfImage existingImage))
            return existingImage;

        string maskTextureName = GetMaskTextureName(materialDef);

        string diffuseTexturePath = TextureUtils.GetQualityPrefixedTexturePath(
            Path.Join(context.Settings.GameDataPath, diffuseTextureName),
            context.Settings.TextureQuality
        );
        string maskTexturePath = TextureUtils.GetQualityPrefixedTexturePath(
            Path.Join(context.Settings.GameDataPath, maskTextureName),
            context.Settings.TextureQuality
        );

        Image<Rgba32> diffuseTexture = TextureUtils.GetImage(TextureUtils.Load(diffuseTexturePath));
        Image<Rgba32> maskTexture = TextureUtils.GetImage(TextureUtils.Load(maskTexturePath));

        MergeMaskIntoDiffuse(diffuseTexture, maskTexture);

        return TextureUtils.CreateGltfImage(diffuseTextureName, diffuseTexture, root, textureRegistry);
    }

    private static void MergeMaskIntoDiffuse(Image<Rgba32> diffuse, Image<Rgba32> mask)
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
    }

    private static void InitializeEmissiveChannel(
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
        StaticMaterialShaderParamDef bloomColorDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "Bloom_Color"
        );

        gltfMaterial.WithChannelFactor("Emissive", "EmissiveStrength", emissiveIntensityDef?.Value.X ?? 0.3f);
        gltfMaterial.WithChannelColor("Emissive", bloomColorDef?.Value ?? Vector4.One);

        if (!string.IsNullOrEmpty(mesh.BakedLight.Texture))
        {
            string texturePath = TextureUtils.GetQualityPrefixedTexturePath(
                Path.Join(context.Settings.GameDataPath, mesh.BakedLight.Texture),
                context.Settings.TextureQuality
            );

            GltfImage image = textureRegistry.TryGetValue(texturePath, out GltfImage existingImage) switch
            {
                true => existingImage,
                false
                    => TextureUtils.CreateGltfImage(
                        texturePath,
                        TextureUtils.GetImage(TextureUtils.Load(texturePath)),
                        root,
                        textureRegistry
                    )
            };
            MaterialChannel channel = gltfMaterial.FindChannel("Emissive").Value;

            channel.SetTransform(mesh.BakedLight.Bias, mesh.BakedLight.Scale);
            channel.SetTexture(1, image);
        }
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
