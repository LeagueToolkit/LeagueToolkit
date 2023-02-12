using BCnEncoder.Shared;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using LeagueToolkit.Toolkit;
using SharpGLTF.IO;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GltfImage = SharpGLTF.Schema2.Image;
using LeagueTexture = LeagueToolkit.Core.Renderer.Texture;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;
using VisibilityNodeRegistry = System.Collections.Generic.Dictionary<
    LeagueToolkit.Core.Environment.EnvironmentVisibility,
    SharpGLTF.Schema2.Node
>;
using LeagueToolkit.IO.Extensions.Utils;
using LeagueToolkit.Core.Meta;
using SixLabors.ImageSharp.Advanced;
using LeagueToolkit.IO.Extensions.MapGeometry;
using LeagueToolkit.IO.Extensions.MapGeometry.Shaders;

namespace LeagueToolkit.IO.MapGeometryFile
{
    public static class MapGeometryGltfExtensions
    {
        private const string DEFAULT_MAP_NAME = "map";

        private static readonly string[] DIFFUSE_SAMPLER_NAMES = new[]
        {
            "DiffuseTexture",
            "Diffuse_Texture",
            "GlowTexture",
            "Mask_Textures"
        };

        private static readonly string[] ALPHA_CLIP_PARAM_NAMES = new[] { "AlphaTestValue", "Opacity_Clip" };
        private static readonly string[] TINT_COLOR_PARAM_NAMES = new[] { "TintColor", "Tint_Color" };

        private const float DEFAULT_ALPHA_TEST = 0.3f;

        private static readonly Dictionary<uint, IMaterialAdapter> MATERIAL_ADAPTERS =
            new()
            {
                { Fnv1a.HashLower("Shaders/Environment/DefaultEnv"), new DefaultEnv() },
                { Fnv1a.HashLower("Shaders/Environment/DefaultEnv_Flat"), new DefaultEnvFlat() },
                { Fnv1a.HashLower("Shaders/Environment/DefaultEnv_Flat_AlphaTest"), new DefaultEnvFlatAlphaTest() },
                {
                    Fnv1a.HashLower("Shaders/Environment/DefaultEnv_Flat_AlphaTest_DoubleSided"),
                    new DefaultEnvFlatAlphaTestDoubleSided()
                },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Chemtech_Decal"), new SrxBlendChemtechDecal() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Chemtech_Ground"), new SrxBlendChemtechGround() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Cloud_Ground"), new SrxBlendCloudGround() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Cloud_WindZone"), new SrxBlendCloudWindZone() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Earth_Ground"), new SrxBlendEarthGround() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Earth_Island"), new SrxBlendEarthIsland() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Earth_Rocks"), new SrxBlendEarthRocks() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Generic_Island"), new SrxBlendGenericIsland() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Hextech_Dragon"), new SrxBlendHextechDragon() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Hextech_Ground"), new SrxBlendHextechGround() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Hextech_Island"), new SrxBlendHextechIsland() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Infernal_Dragon"), new SrxBlendInfernalDragon() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Infernal_Ground"), new SrxBlendInfernalGround() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Infernal_Island"), new SrxBlendInfernalIsland() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Master"), new SrxBlendMaster() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Blend_Ocean"), new SrxBlendOcean() },
                { Fnv1a.HashLower("Shaders/Environment/SRX_Brush"), new SrxBrush() },
                { Fnv1a.HashLower("Shaders/Environment/OD_FlowMap"), new OdFlowMap() },
                { Fnv1a.HashLower("Shaders/StaticMesh/Hologram"), new StaticMeshHologram() },
                { Fnv1a.HashLower("Shaders/StaticMesh/Env_Glow"), new EnvGlow() },
                { Fnv1a.HashLower("Shaders/StaticMesh/ENV_TileableDiffuse"), new EnvTileableDiffuse() },
                { Fnv1a.HashLower("Shaders/StaticMesh/Emissive_Basic"), new EmissiveBasic() },
                { Fnv1a.HashLower("Shaders/StaticMesh/AlphaTest_ENV"), new AlphaTestEnv() },
                { Fnv1a.HashLower("Shaders/StaticMesh/SinFade_Alpha"), new SinFadeAlpha() }
            };

        /// <summary>
        /// Converts the <see cref="EnvironmentAsset"/> object into a glTF asset
        /// </summary>
        /// <param name="mapGeometry">The <see cref="EnvironmentAsset"/> object to convert</param>
        /// <param name="materialsBin">The "materials.bin" <see cref="BinTree"/> to use for conversion</param>
        /// <param name="context">The conversion context</param>
        /// <returns>The created glTF asset</returns>
        public static ModelRoot ToGltf(
            this EnvironmentAsset mapGeometry,
            BinTree materialsBin,
            MapGeometryGltfConversionContext context
        )
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene(root.LogicalScenes.Count);

            MapContainer mapContainer = GetMapContainer(materialsBin, context);

            Node mapNode = CreateMapNode(scene, mapContainer, context);

            if (context.Settings.FlipAcrossX)
                mapNode = mapNode.WithLocalScale(new(-1f, 1f, 1f));

            // Create sun
            CreateSun(mapContainer, root);

            // Create visibility nodes
            VisibilityNodeRegistry visibilityNodeRegistry = CreateIndividualVisibilityFlagsNodeRegistry(mapNode);

            // Create meshes
            TextureRegistry textureRegistry = new();
            foreach (EnvironmentAssetMesh mesh in mapGeometry.Meshes)
            {
                // Create materials
                Dictionary<string, Material> materials = CreateGltfMeshMaterials(
                    mesh,
                    mapGeometry.BakedTerrainSamplers,
                    root,
                    materialsBin,
                    textureRegistry,
                    context
                );

                // Create mesh
                Mesh gltfMesh = CreateGltfMesh(root, mesh, materials);

                PlaceGltfMeshIntoScene(gltfMesh, mesh, mapNode, visibilityNodeRegistry, context);
            }

            root.MergeImages();
            root.MergeBuffers();

            root.DefaultScene = scene;

            return root;
        }

        private static Node CreateMapNode(
            Scene scene,
            MapContainer mapContainer,
            MapGeometryGltfConversionContext context
        )
        {
            string mapNodeName = string.IsNullOrEmpty(mapContainer.MapPath) ? DEFAULT_MAP_NAME : mapContainer.MapPath;
            return scene.CreateNode(mapNodeName);
        }

        private static VisibilityNodeRegistry CreateIndividualVisibilityFlagsNodeRegistry(Node mapNode) =>
            new(
                Enum.GetValues<EnvironmentVisibility>()
                    .Where(x => x is not (EnvironmentVisibility.NoLayer or EnvironmentVisibility.AllLayers))
                    .Select(x => new KeyValuePair<EnvironmentVisibility, Node>(x, mapNode.CreateNode(x.ToString())))
            );

        private static Mesh CreateGltfMesh(
            ModelRoot root,
            EnvironmentAssetMesh mesh,
            IReadOnlyDictionary<string, Material> materials
        )
        {
            Mesh gltfMesh = root.CreateMesh(mesh.Name);

            gltfMesh.Extras = JsonContent.Serialize(CreateGltfMeshExtras(mesh));

            MemoryAccessor[] meshVertexMemoryAccessors = mesh.VerticesView.Buffers
                .SelectMany(GltfUtils.CreateVertexMemoryAccessors)
                .ToArray();
            MemoryAccessor.SanitizeVertexAttributes(meshVertexMemoryAccessors);
            GltfUtils.SanitizeVertexMemoryAccessors(meshVertexMemoryAccessors);

            foreach (EnvironmentAssetMeshPrimitive range in mesh.Submeshes)
            {
                MemoryAccessor indicesMemoryAccessor = GltfUtils.CreateIndicesMemoryAccessor(
                    mesh.Indices.Slice(range.StartIndex, range.IndexCount),
                    range.MinVertex
                );
                MemoryAccessor[] vertexMemoryAccessors = GltfUtils.SliceVertexMemoryAccessors(
                    range.MinVertex,
                    range.VertexCount,
                    meshVertexMemoryAccessors
                );

                MemoryAccessor.VerifyVertexIndices(indicesMemoryAccessor, (uint)range.VertexCount);

                gltfMesh
                    .CreatePrimitive()
                    .WithMaterial(materials[range.Material])
                    .WithIndicesAccessor(PrimitiveType.TRIANGLES, indicesMemoryAccessor)
                    .WithVertexAccessors(vertexMemoryAccessors);
            }

            return gltfMesh;
        }

        private static GltfMeshExtras CreateGltfMeshExtras(EnvironmentAssetMesh mesh) =>
            new()
            {
                Name = mesh.Name,
                VisibilityFlags = (int)mesh.VisibilityFlags,
                RenderFlags = (int)mesh.RenderFlags,
                QualityFlags = (int)mesh.EnvironmentQualityFilter,
                StationaryLight = mesh.StationaryLight,
                BakedLight = mesh.BakedLight,
                BakedPaint = mesh.BakedPaint,
            };

        private static void PlaceGltfMeshIntoScene(
            Mesh gltfMesh,
            EnvironmentAssetMesh mesh,
            Node mapNode,
            VisibilityNodeRegistry visibilityNodeRegistry,
            MapGeometryGltfConversionContext context
        )
        {
            if (context.Settings.LayerGroupingPolicy is MapGeometryGltfLayerGroupingPolicy.Default)
            {
                PlaceGltfMeshIntoVisibilityNodes(gltfMesh, mesh, mapNode, visibilityNodeRegistry);
            }
            else if (context.Settings.LayerGroupingPolicy is MapGeometryGltfLayerGroupingPolicy.Ignore)
            {
                PlaceGltfMeshIntoNode(gltfMesh, mesh, mapNode, mesh.Name);
            }
        }

        private static void PlaceGltfMeshIntoVisibilityNodes(
            Mesh gltfMesh,
            EnvironmentAssetMesh mesh,
            Node mapNode,
            VisibilityNodeRegistry visibilityNodeRegistry
        )
        {
            if (mesh.VisibilityFlags == EnvironmentVisibility.NoLayer)
                PlaceGltfMeshIntoNode(gltfMesh, mesh, mapNode, mesh.Name);

            foreach (var (visibilityFlag, node) in visibilityNodeRegistry)
            {
                if (!mesh.VisibilityFlags.HasFlag(visibilityFlag))
                    continue;

                PlaceGltfMeshIntoNode(gltfMesh, mesh, node, $"{visibilityFlag}.{mesh.Name}");
            }
        }

        private static void PlaceGltfMeshIntoNode(
            Mesh gltfMesh,
            EnvironmentAssetMesh mesh,
            Node node,
            string meshNodeName
        )
        {
            Node meshNode = node.CreateNode(meshNodeName).WithMesh(gltfMesh);

            meshNode.WorldMatrix = mesh.Transform;
        }

        #region Material Creation
        private static Dictionary<string, Material> CreateGltfMeshMaterials(
            EnvironmentAssetMesh mesh,
            EnvironmentAssetBakedTerrainSamplers bakedTerrainSamplers,
            ModelRoot root,
            BinTree materialsBin,
            TextureRegistry textureRegistry,
            MapGeometryGltfConversionContext context
        )
        {
            Guard.IsNotNull(mesh, nameof(mesh));
            Guard.IsNotNull(root, nameof(root));
            Guard.IsNotNull(textureRegistry, nameof(textureRegistry));
            Guard.IsNotNull(context, nameof(context));

            Dictionary<string, Material> materials = new();
            foreach (EnvironmentAssetMeshPrimitive range in mesh.Submeshes)
            {
                Material material = root.CreateMaterial(range.Material)
                    .WithPBRMetallicRoughness()
                    .WithDoubleSide(mesh.DisableBackfaceCulling);

                InitializeMaterial(material, mesh, bakedTerrainSamplers, root, materialsBin, textureRegistry, context);

                materials.TryAdd(material.Name, material);
            }

            return materials;
        }

        private static void InitializeMaterial(
            Material gltfMaterial,
            EnvironmentAssetMesh mesh,
            EnvironmentAssetBakedTerrainSamplers bakedTerrainSamplers,
            ModelRoot root,
            BinTree materialsBin,
            TextureRegistry textureRegistry,
            MapGeometryGltfConversionContext context
        )
        {
            // If game path is not set, return
            if (string.IsNullOrEmpty(context.Settings.GameDataPath))
                return;

            // Get material meta definition
            StaticMaterialDef materialDef = materialsBin switch
            {
                null => new(),
                _ => ResolveMaterialDefiniton(gltfMaterial.Name, materialsBin, context)
            };

            // Include material metadata
            gltfMaterial.Extras = JsonContent.Serialize(new GltfMaterialExtras() { Name = gltfMaterial.Name });

            uint defaultTechniqueShader = GetDefaultTechniqueShaderLink(materialDef);
            if (MATERIAL_ADAPTERS.TryGetValue(defaultTechniqueShader, out IMaterialAdapter techniqueAdapter))
            {
                techniqueAdapter.InitializeMaterial(gltfMaterial, materialDef, mesh, textureRegistry, root, context);
            }
            else
            {
                // TODO
            }
        }

        private static StaticMaterialDef ResolveMaterialDefiniton(
            string materialName,
            BinTree materialsBin,
            MapGeometryGltfConversionContext context
        )
        {
            // Find material definition bin object
            if (!materialsBin.Objects.TryGetValue(Fnv1a.HashLower(materialName), out BinTreeObject materialDefObject))
                ThrowHelper.ThrowInvalidOperationException($"Failed to find {materialName} in {nameof(materialsBin)}");

            // Deserialize material definition
            return MetaSerializer.Deserialize<StaticMaterialDef>(context.MetaEnvironment, materialDefObject);
        }

        private static GltfImage CreateImage(
            string textureName,
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
            LeagueTexture texture = TextureUtils.Load(texturePath);

            // Re-encode to PNG
            ReadOnlyMemory2D<ColorRgba32> biggestMipMap = texture.Mips[0];
            using Image<Rgba32> image = biggestMipMap.ToImage();

            return TextureUtils.CreateGltfImage(texturePath, image, root, textureRegistry);
        }

        #endregion

        private static void CreateSun(MapContainer mapContainer, ModelRoot root)
        {
            MapSunProperties sunComponent = (MapSunProperties)
                mapContainer.Components.FirstOrDefault(x => x is MapSunProperties);

            Vector2 mapCenter = Vector2.Multiply(
                Vector2.Abs(mapContainer.BoundsMin) + Vector2.Abs(mapContainer.BoundsMax),
                0.5f
            );

            Node sunNode = root.CreateLogicalNode()
                .WithLocalTransform(
                    Matrix4x4.CreateLookAt(
                        Vector3.Zero,
                        Vector3.Multiply(sunComponent.SunDirection, new Vector3(1f, 1f, -1f)),
                        Vector3.UnitY
                    )
                );

            sunNode.PunctualLight = root.CreatePunctualLight("sun", PunctualLightType.Directional)
                .WithColor(
                    new(sunComponent.SunColor.X, sunComponent.SunColor.Y, sunComponent.SunColor.Z),
                    sunComponent.SunIntensityScale / 10f // convert to lux unit
                );
        }

        private static MapContainer GetMapContainer(BinTree materialsBin, MapGeometryGltfConversionContext context)
        {
            if (
                materialsBin.Objects.Values.FirstOrDefault(x => x.ClassHash == Fnv1a.HashLower(nameof(MapContainer)))
                is not BinTreeObject mapContainerObject
            )
                throw new InvalidOperationException(
                    $"Failed to find {nameof(MapContainer)} in the provided materials.bin"
                );

            return MetaSerializer.Deserialize<MapContainer>(context.MetaEnvironment, mapContainerObject);
        }

        private static MetaObjectLink GetDefaultTechniqueShaderLink(StaticMaterialDef material)
        {
            StaticMaterialTechniqueDef technique = material.Techniques.FirstOrDefault(
                x => x.Value.Name == material.DefaultTechnique
            );

            return technique?.Passes.FirstOrDefault()?.Value.Shader ?? default;
        }

        private readonly struct GltfMapRootExtras
        {
            public string MapPath { get; init; }
        }

        private readonly struct GltfMeshExtras
        {
            public string Name { get; init; }

            public int VisibilityFlags { get; init; }
            public int QualityFlags { get; init; }
            public int RenderFlags { get; init; }

            public EnvironmentAssetSampler StationaryLight { get; init; }
            public EnvironmentAssetSampler BakedLight { get; init; }
            public EnvironmentAssetSampler BakedPaint { get; init; }
        }

        private readonly struct GltfMaterialExtras
        {
            public string Name { get; init; }
        }
    }

    /// <summary>
    /// Contains the necessary information for conversion to glTF
    /// </summary>
    public readonly struct MapGeometryGltfConversionContext
    {
        /// <summary>
        /// Gets the <see cref="Meta.MetaEnvironment"/> used during the conversion
        /// </summary>
        public MetaEnvironment MetaEnvironment { get; init; }

        /// <summary>
        /// Gets the <see cref="MapGeometryGltfConversionSettings"/> used during the conversion
        /// </summary>
        public MapGeometryGltfConversionSettings Settings { get; init; }

        /// <summary>
        /// Creates a new <see cref="MapGeometryGltfConversionContext"/> object with the specified parameters
        /// </summary>
        /// <param name="metaEnvironment">The <see cref="Meta.MetaEnvironment"/> to use</param>
        /// <param name="settings">The <see cref="MapGeometryGltfConversionSettings"/> to use</param>
        public MapGeometryGltfConversionContext(
            MetaEnvironment metaEnvironment,
            MapGeometryGltfConversionSettings settings
        )
        {
            this.MetaEnvironment = metaEnvironment;
            this.Settings = settings;
        }
    }

    /// <summary>
    /// glTF Conversion settings
    /// </summary>
    public struct MapGeometryGltfConversionSettings
    {
        /// <summary>
        /// Gets or sets the game data path which is used for resolving assets.<br></br>
        /// Set this if you want to bundle textures and materials into the glTF asset.
        /// </summary>
        public string GameDataPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the main map node should be flipped across the X axis<br></br>
        /// Default: <see langword="true"/>
        /// </summary>
        public bool FlipAcrossX { get; set; }

        /// <summary>
        /// Gets or sets the visibility flags grouping policy for meshes<br></br>
        /// Default: <see cref="MapGeometryGltfLayerGroupingPolicy.Default"/>
        /// </summary>
        public MapGeometryGltfLayerGroupingPolicy LayerGroupingPolicy { get; set; }

        /// <summary>
        /// Gets or sets the quality of the resolved textures<br></br>
        /// Default: <see cref="MapGeometryGltfTextureQuality.Low"/>
        /// </summary>
        public MapGeometryGltfTextureQuality TextureQuality { get; set; }

        /// <summary>
        /// Creates a new <see cref="MapGeometryGltfConversionSettings"/> object
        /// </summary>
        public MapGeometryGltfConversionSettings()
        {
            this.GameDataPath = null;
            this.FlipAcrossX = true;
            this.LayerGroupingPolicy = MapGeometryGltfLayerGroupingPolicy.Default;
            this.TextureQuality = MapGeometryGltfTextureQuality.Low;
        }
    }

    /// <summary>
    /// Specifies the <see cref="EnvironmentVisibility"/> grouping policy for meshes
    /// </summary>
    /// <remarks>
    /// The <see cref="EnvironmentVisibility"/> will always be written into the
    /// <see href="https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-extras">extras field</see>
    /// </remarks>
    public enum MapGeometryGltfLayerGroupingPolicy
    {
        /// <summary>
        /// Each mesh will be placed under the node of each of its <see cref="EnvironmentVisibility"/>
        /// </summary>
        Default,

        /// <summary>
        /// The meshes will all be placed under the root node
        /// </summary>
        Ignore,
    }

    /// <summary>
    /// Specifies which texture quality to use
    /// </summary>
    public enum MapGeometryGltfTextureQuality
    {
        /// <summary>
        /// Use textures prefixed with "4x_"
        /// </summary>
        Low,

        /// <summary>
        /// Use textures prefixed with "2x_"
        /// </summary>
        Medium,

        /// <summary>
        /// Use textures without a prefix
        /// </summary>
        High
    }
}
