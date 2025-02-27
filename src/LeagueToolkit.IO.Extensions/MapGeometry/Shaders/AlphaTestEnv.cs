using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.Meta.Classes;
using SharpGLTF.Schema2;
using GltfImage = SharpGLTF.Schema2.Image;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;

namespace LeagueToolkit.IO.Extensions.MapGeometry.Shaders;

internal sealed class AlphaTestEnv : IMaterialAdapter
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
        gltfMaterial.InitializeUnlit();

        InitializeMaterialRenderTechnique(gltfMaterial);
        InitializeMaterialBaseColorChannel(gltfMaterial, materialDef, root, textureRegistry, context);
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
        StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(x =>
            x.Value.TextureName is "DiffuseTexture"
        );

        gltfMaterial.WithChannelTexture(
            "BaseColor",
            0,
            TextureUtils.CreateGltfImage(samplerDef.TexturePath, root, textureRegistry, context)
        );
    }
}
