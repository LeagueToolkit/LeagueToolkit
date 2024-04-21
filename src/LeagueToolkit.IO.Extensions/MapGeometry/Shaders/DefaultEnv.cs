using System;
using System.IO;
using System.Linq;
using System.Numerics;
using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.Meta.Classes;
using SharpGLTF.Schema2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using GltfImage = SharpGLTF.Schema2.Image;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;

namespace LeagueToolkit.IO.Extensions.MapGeometry.Shaders;

internal sealed class DefaultEnv : IMaterialAdapter
{
    public void InitializeMaterial(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        EnvironmentAssetMesh mesh,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        gltfMaterial.WithPBRMetallicRoughness();

        InitializeMaterialRenderTechnique(gltfMaterial, materialDef);
        InitializeMaterialBaseColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
        InitializeMaterialEmissiveColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
        InitializeMetalicRoughnessChannel(gltfMaterial, materialDef, root, textureRegistry, context);
    }

    private static void InitializeMaterialRenderTechnique(Material gltfMaterial, StaticMaterialDef materialDef)
    {
        StaticMaterialTechniqueDef techniqueDef = materialDef.Techniques.FirstOrDefault() ?? new(new());
        StaticMaterialPassDef passDef = techniqueDef.Passes.FirstOrDefault() ?? new(new());

        // Try to get alpha cutoff, if it doesn't exist then assign default one
        StaticMaterialShaderParamDef alphaCutoffParameter = materialDef.ParamValues.FirstOrDefault(x =>
            x.Value.Name is "Overlay_TEST"
        );

        if (alphaCutoffParameter is not null)
        {
            gltfMaterial.Alpha = AlphaMode.MASK;
            gltfMaterial.AlphaCutoff = alphaCutoffParameter.Value.X;
        }
        else if (passDef.BlendEnable)
        {
            gltfMaterial.Alpha = AlphaMode.BLEND;
        }
        else
        {
            gltfMaterial.Alpha = AlphaMode.MASK;
            gltfMaterial.AlphaCutoff = 0.5f;
        }
    }

    private static void InitializeMaterialBaseColorChannel(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        EnvironmentAssetMesh mesh,
        ModelRoot root,
        TextureRegistry textureRegistry,
        MapGeometryGltfConversionContext context
    )
    {
        Guard.IsNotNull(materialDef, nameof(materialDef));

        // Resolve diffuse sampler definition, return if not found
        StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(x =>
            x.Value.SamplerName is "DiffuseTexture"
        );
        if (samplerDef is null)
            return;

        // Figure out texcoord id and sampler transform
        int texcoordId = 0;
        EnvironmentAssetChannel sampler = new();
        var bakedPaintTexture = mesh
            .BakedPaintChannelDefs.FirstOrDefault(new EnvironmentAssetBakedPaintChannelDef(0, string.Empty))
            .Texture;
        if (!string.IsNullOrEmpty(bakedPaintTexture))
        {
            texcoordId = 1;
            sampler = new(bakedPaintTexture, mesh.BakedPaintScale, mesh.BakedPaintBias);
        }
        else if (!string.IsNullOrEmpty(samplerDef.TextureName))
        {
            sampler = new(samplerDef.TextureName, Vector2.One, Vector2.Zero);
        }
        else
        {
            sampler = mesh.StationaryLight;
        }

        StaticMaterialShaderParamDef colorParam = materialDef.ParamValues.FirstOrDefault(x => x.Value.Name is "Color");
        gltfMaterial.WithChannelColor("BaseColor", (colorParam?.Value ?? Vector4.One) with { W = 1f });

        if (!string.IsNullOrEmpty(sampler.Texture))
        {
            MaterialChannel channel = gltfMaterial.FindChannel("BaseColor").Value;

            channel.SetTransform(sampler.Bias, sampler.Scale);
            channel.SetTexture(
                texcoordId,
                TextureUtils.CreateGltfImage(sampler.Texture, root, textureRegistry, context),
                ws: TextureUtils.GetWrapMode((TextureAddress)samplerDef.AddressU),
                wt: TextureUtils.GetWrapMode((TextureAddress)samplerDef.AddressV)
            );
        }
    }

    private static void InitializeMaterialEmissiveColorChannel(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        EnvironmentAssetMesh mesh,
        ModelRoot root,
        TextureRegistry textureRegistry,
        MapGeometryGltfConversionContext context
    )
    {
        StaticMaterialShaderParamDef emissiveColorParamDef = materialDef.ParamValues.FirstOrDefault(x =>
            x.Value.Name is "Emissive_Color"
        );
        StaticMaterialShaderParamDef emissiveIntensityParamDef = materialDef.ParamValues.FirstOrDefault(x =>
            x.Value.Name is "Emissive_Intensity"
        );

        MaterialChannel emissiveChannel = gltfMaterial.FindChannel("Emissive").Value;

        gltfMaterial.WithChannelFactor("Emissive", "EmissiveStrength", emissiveIntensityParamDef?.Value.X ?? 0.1f);
        gltfMaterial.WithChannelColor("Emissive", emissiveColorParamDef?.Value ?? Vector4.One);

        if (!string.IsNullOrEmpty(mesh.BakedLight.Texture))
        {
            emissiveChannel.SetTransform(mesh.BakedLight.Bias, mesh.BakedLight.Scale);
            emissiveChannel.SetTexture(
                1,
                TextureUtils.CreateGltfImage(mesh.BakedLight.Texture, root, textureRegistry, context)
            );
        }
    }

    private static void InitializeMetalicRoughnessChannel(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        ModelRoot root,
        TextureRegistry textureRegistry,
        MapGeometryGltfConversionContext context
    )
    {
        // Find specular glossiness sampler
        StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(x =>
            x.Value.SamplerName is "Material"
        );
        MaterialChannel channel = gltfMaterial.FindChannel("MetallicRoughness").Value;

        // Material isn't PBR Metallic Roughness
        if (samplerDef is null)
        {
            channel.SetFactor("RoughnessFactor", 0.5f);
            return;
        }

        // Resolve gloss factor parameter
        StaticMaterialShaderParamDef glossParamDef = materialDef.ParamValues.FirstOrDefault(x =>
            x.Value.Name is "Gloss"
        );

        // Resolve specular factor parameter
        StaticMaterialShaderParamDef specularFactorParamDef = materialDef.ParamValues.FirstOrDefault(x =>
            x.Value.Name is "SpecularColor_Multiplier"
        );

        // Convert specular glossiness map to roughness
        float glossinessFactor = glossParamDef?.Value.X ?? 1f;
        GltfImage image = CreateRoughnessImage(
            samplerDef.TextureName,
            glossinessFactor,
            textureRegistry,
            root,
            context
        );

        channel.SetFactor("MetallicFactor", 0f);
        channel.SetFactor("RoughnessFactor", 1 - glossinessFactor);

        channel.SetTexture(
            0,
            image,
            ws: TextureUtils.GetWrapMode((TextureAddress)samplerDef.AddressU),
            wt: TextureUtils.GetWrapMode((TextureAddress)samplerDef.AddressV)
        );
    }

    private static GltfImage CreateRoughnessImage(
        string textureName,
        float glossinessFactor,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        string texturePath = TextureUtils.GetQualityPrefixedTexturePath(
            Path.Join(context.Settings.GameDataPath, textureName),
            context.Settings.TextureQuality
        );

        // If texture is already loaded, return it
        if (textureRegistry.TryGetValue(texturePath, out GltfImage existingImage))
            return existingImage;

        // Load texture
        using Image<Rgba32> image = TextureUtils.GetImage(TextureUtils.Load(texturePath));

        // Convert specular glossiness to roughness
        image.ProcessPixelRows(x =>
        {
            for (int rowId = 0; rowId < x.Height; rowId++)
            {
                Span<Rgba32> row = x.GetRowSpan(rowId);

                for (int i = 0; i < row.Length; i++)
                {
                    Rgba32 color = row[i];

                    row[i] = new()
                    {
                        R = 0,
                        G = (byte)(255 - Math.Round(color.A * glossinessFactor)),
                        B = 0,
                        A = 255
                    };
                }
            }
        });

        return TextureUtils.CreateGltfImage(texturePath, image, root, textureRegistry);
    }
}
