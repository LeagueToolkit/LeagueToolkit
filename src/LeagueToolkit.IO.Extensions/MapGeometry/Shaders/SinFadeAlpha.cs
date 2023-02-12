using LeagueToolkit.Core.Environment;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.Meta.Classes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GltfImage = SharpGLTF.Schema2.Image;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;

namespace LeagueToolkit.IO.Extensions.MapGeometry.Shaders;

internal sealed class SinFadeAlpha : IMaterialAdapter
{
    private const string DEFAULT_MASK_TEXTURES = "ASSETS/Shared/Materials/white.dds";

    public void InitializeMaterial(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        EnvironmentAssetMesh mesh,
        TextureRegistry textureRegistry,
        ModelRoot root,
        MapGeometryGltfConversionContext context
    )
    {
        gltfMaterial.InitializePBRMetallicRoughness();
        gltfMaterial.WithChannelFactor("MetallicRoughness", "MetallicFactor", 0f);
        gltfMaterial.WithChannelFactor("MetallicRoughness", "RoughnessFactor", 0f);

        InitializeMaterialRenderTechnique(gltfMaterial);
        InitializeMaterialBaseColorChannel(gltfMaterial, materialDef, root, textureRegistry, context);
        InitializeMaterialEmissiveChannel(gltfMaterial, materialDef, root, textureRegistry, context);
    }

    private static void InitializeMaterialRenderTechnique(Material gltfMaterial)
    {
        gltfMaterial.Alpha = AlphaMode.MASK;
        gltfMaterial.AlphaCutoff = 0.5f;
    }

    private static void InitializeMaterialBaseColorChannel(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        ModelRoot root,
        TextureRegistry textureRegistry,
        MapGeometryGltfConversionContext context
    )
    {
        StaticMaterialShaderParamDef baseColorDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "BaseColor"
        );

        gltfMaterial.WithChannelColor("BaseColor", (baseColorDef?.Value ?? new(0f, 0.95f, 1f, 1f)) with { W = 1f });
    }

    private static void InitializeMaterialEmissiveChannel(
        Material gltfMaterial,
        StaticMaterialDef materialDef,
        ModelRoot root,
        TextureRegistry textureRegistry,
        MapGeometryGltfConversionContext context
    )
    {
        StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(
            x => x.Value.SamplerName is "Mask_Textures"
        );
        samplerDef ??= new() { TextureName = DEFAULT_MASK_TEXTURES };

        StaticMaterialShaderParamDef emissiveIntensityDef = materialDef.ParamValues.FirstOrDefault(
            x => x.Value.Name is "Emissive_Intensity"
        );

        gltfMaterial.WithChannelTexture(
            "Emissive",
            0,
            TextureUtils.CreateGltfImage(samplerDef.TextureName, root, textureRegistry, context)
        );
        gltfMaterial.WithChannelColor("Emissive", Vector4.One);
        gltfMaterial.WithChannelFactor("Emissive", "EmissiveStrength", emissiveIntensityDef?.Value.X ?? 1f);
    }
}
