using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Renderer;
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

internal sealed class OdFlowMap : IMaterialAdapter
{
    private const string DEFAULT_DIFFUSE_TEXTURE = "ASSETS/Shared/Materials/Water_Test.dds";

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
        gltfMaterial.WithChannelFactor("MetallicRoughness", "MetallicFactor", 0f);
        gltfMaterial.WithChannelFactor("MetallicRoughness", "RoughnessFactor", 1f);

        InitializeMaterialBaseColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
        InitializeMaterialEmissiveColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
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
        StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(
            x => x.Value.SamplerName is "Diffuse_Texture"
        );
        samplerDef ??= new() { TextureName = DEFAULT_DIFFUSE_TEXTURE };

        gltfMaterial.WithChannelTexture(
            "BaseColor",
            0,
            TextureUtils.CreateGltfImage(samplerDef.TextureName, root, textureRegistry, context)
        );
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
        StaticMaterialShaderParamDef emissiveColorParamDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "Emissive_Color"
        );
        StaticMaterialShaderParamDef emissiveIntensityParamDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "Emissive_Intensity"
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
}
