﻿using CommunityToolkit.Diagnostics;
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

internal sealed class DefaultEnvFlatAlphaTest : IMaterialAdapter
{
    private const string DEFAULT_DIFFUSE_TEXTURE = "ASSETS/Shared/Materials/Generic_Hex_2.dds";

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

        InitializeMaterialRenderTechnique(gltfMaterial, materialDef);
        InitializeMaterialBaseColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
        InitializeMaterialEmissiveColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
    }

    private static void InitializeMaterialRenderTechnique(Material gltfMaterial, StaticMaterialDef materialDef)
    {
        StaticMaterialShaderParamDef alphaTestDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "AlphaTestValue"
        );
        alphaTestDef ??= new() { Value = Vector4.Zero with { X = 0.3f } };

        gltfMaterial.Alpha = AlphaMode.MASK;
        gltfMaterial.AlphaCutoff = alphaTestDef.Value.X;
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
        StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(
            x => x.Value.SamplerName is "DiffuseTexture"
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
