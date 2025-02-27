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

internal sealed class DefaultEnvFlat : IMaterialAdapter
{
    private const string DEFAULT_DIFFUSE_TEXTURE = "ASSETS/Shared/Materials/rock_texture.dds";

    public void InitializeMaterial(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        EnvironmentAssetMesh mesh,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        gltfMaterial.WithUnlit();

        InitializeMaterialRenderTechnique(gltfMaterial);
        InitializeMaterialBaseColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
        InitializeMaterialEmissiveColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
    }

    private static void InitializeMaterialRenderTechnique(Material gltfMaterial)
    {
        gltfMaterial.Alpha = AlphaMode.MASK;
        gltfMaterial.AlphaCutoff = 0.3f;
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
        // Resolve diffuse sampler definition, return if not found
        StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(x =>
            x.Value.TextureName is "DiffuseTexture"
        );
        samplerDef ??= new() { TexturePath = DEFAULT_DIFFUSE_TEXTURE };

        gltfMaterial.WithChannelTexture(
            "BaseColor",
            0,
            TextureUtils.CreateGltfImage(samplerDef.TexturePath, root, textureRegistry, context)
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
        if (string.IsNullOrEmpty(mesh.BakedLight.Texture))
            return;

        MaterialChannel emissiveChannel = gltfMaterial.FindChannel("Emissive").Value;

        gltfMaterial.WithChannelFactor("Emissive", "EmissiveStrength", 0.1f);
        gltfMaterial.WithChannelColor("Emissive", Vector4.One);

        emissiveChannel.SetTransform(mesh.BakedLight.Bias, mesh.BakedLight.Scale);
        emissiveChannel.SetTexture(
            1,
            TextureUtils.CreateGltfImage(mesh.BakedLight.Texture, root, textureRegistry, context)
        );
    }
}
