using BCnEncoder.Shared;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.Hashing;
using LeagueToolkit.IO.PropertyBin;
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
using GltfTextureInterpolationFilter = SharpGLTF.Schema2.TextureInterpolationFilter;
using GltfTextureWrapMode = SharpGLTF.Schema2.TextureWrapMode;
using LeagueTexture = LeagueToolkit.Core.Renderer.Texture;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;
using VisibilityNodeRegistry = System.Collections.Generic.Dictionary<
    LeagueToolkit.Core.Environment.EnvironmentVisibilityFlags,
    SharpGLTF.Schema2.Node
>;
using LeagueToolkit.IO.Extensions.Utils;

namespace LeagueToolkit.IO.MapGeometryFile
{
    public static class MapGeometryGltfExtensions
    {
        private const string DEFAULT_MAP_NAME = "map";

        private static readonly string[] DIFFUSE_SAMPLER_NAMES = new[] { "DiffuseTexture", "Diffuse_Texture" };

        private static readonly string[] ALPHA_CLIP_PARAM_NAMES = new[] { "AlphaTestValue", "Opacity_Clip" };
        private static readonly string[] TINT_COLOR_PARAM_NAMES = new[] { "TintColor", "Tint_Color" };

        private const float DEFAULT_ALPHA_TEST = 0.3f;

        private const string TEXTURE_QUALITY_PREFIX_LOW = "4x";
        private const string TEXTURE_QUALITY_PREFIX_MEDIUM = "2x";

        /// <summary>
        /// Converts the <see cref="MapGeometry"/> object into a glTF asset
        /// </summary>
        /// <param name="mapGeometry">The <see cref="MapGeometry"/> object to convert</param>
        /// <param name="materialsBin">The "materials.bin" <see cref="BinTree"/> to use for conversion</param>
        /// <param name="context">The conversion context</param>
        /// <returns>The created glTF asset</returns>
        public static ModelRoot ToGltf(
            this MapGeometry mapGeometry,
            BinTree materialsBin,
            MapGeometryGltfConversionContext context
        )
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene(root.LogicalScenes.Count);
            Node mapNode = CreateMapNode(scene, materialsBin, context);

            if (context.Settings.FlipAcrossX)
                mapNode = mapNode.WithLocalScale(new(-1f, 1f, 1f));

            // Create visibility nodes
            VisibilityNodeRegistry visibilityNodeRegistry = CreateIndividualVisibilityFlagsNodeRegistry(mapNode);

            // Create meshes
            TextureRegistry textureRegistry = new();
            foreach (MapGeometryModel mesh in mapGeometry.Meshes)
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

        private static Node CreateMapNode(Scene scene, BinTree materialsBin, MapGeometryGltfConversionContext context)
        {
            string mapNodeName = GetMapName(materialsBin, context);
            return scene.CreateNode(mapNodeName);
        }

        private static VisibilityNodeRegistry CreateIndividualVisibilityFlagsNodeRegistry(Node mapNode) =>
            new(
                Enum.GetValues<EnvironmentVisibilityFlags>()
                    .Where(x => x is not (EnvironmentVisibilityFlags.NoLayer or EnvironmentVisibilityFlags.AllLayers))
                    .Select(
                        x => new KeyValuePair<EnvironmentVisibilityFlags, Node>(x, mapNode.CreateNode(x.ToString()))
                    )
            );

        private static Mesh CreateGltfMesh(
            ModelRoot root,
            MapGeometryModel mesh,
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

            foreach (MapGeometrySubmesh range in mesh.Submeshes)
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

        private static GltfMeshExtras CreateGltfMeshExtras(MapGeometryModel mesh) =>
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
            MapGeometryModel mesh,
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
            MapGeometryModel mesh,
            Node mapNode,
            VisibilityNodeRegistry visibilityNodeRegistry
        )
        {
            if (mesh.VisibilityFlags == EnvironmentVisibilityFlags.NoLayer)
                PlaceGltfMeshIntoNode(gltfMesh, mesh, mapNode, mesh.Name);

            foreach (var (visibilityFlag, node) in visibilityNodeRegistry)
            {
                if (!mesh.VisibilityFlags.HasFlag(visibilityFlag))
                    continue;

                PlaceGltfMeshIntoNode(gltfMesh, mesh, node, $"{visibilityFlag}.{mesh.Name}");
            }
        }

        private static void PlaceGltfMeshIntoNode(Mesh gltfMesh, MapGeometryModel mesh, Node node, string meshNodeName)
        {
            Node meshNode = node.CreateNode(meshNodeName).WithMesh(gltfMesh);

            meshNode.WorldMatrix = mesh.Transform;
        }

        #region Material Creation
        private static Dictionary<string, Material> CreateGltfMeshMaterials(
            MapGeometryModel mesh,
            MapGeometryBakedTerrainSamplers bakedTerrainSamplers,
            ModelRoot root,
            BinTree materialsBin,
            TextureRegistry textureRegistry,
            MapGeometryGltfConversionContext context
        )
        {
            Guard.IsNotNull(mesh, nameof(mesh));
            Guard.IsNotNull(root, nameof(root));
            Guard.IsNotNull(materialsBin, nameof(materialsBin));
            Guard.IsNotNull(textureRegistry, nameof(textureRegistry));
            Guard.IsNotNull(context, nameof(context));

            Dictionary<string, Material> materials = new();
            foreach (MapGeometrySubmesh range in mesh.Submeshes)
            {
                Material material = root.CreateMaterial(range.Material)
                    .WithUnlit()
                    .WithDoubleSide(mesh.DisableBackfaceCulling);

                InitializeMaterial(material, mesh, bakedTerrainSamplers, root, materialsBin, textureRegistry, context);

                materials.TryAdd(material.Name, material);
            }

            return materials;
        }

        private static void InitializeMaterial(
            Material material,
            MapGeometryModel mesh,
            MapGeometryBakedTerrainSamplers bakedTerrainSamplers,
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
            StaticMaterialDef materialDef = ResolveMaterialDefiniton(material.Name, materialsBin, context);

            // Include material metadata
            material.Extras = JsonContent.Serialize(new GltfMaterialExtras() { Name = material.Name });

            // Initialize material properties
            InitializeMaterialRenderTechnique(material, materialDef);
            InitializeMaterialBaseColorChannel(
                material,
                materialDef,
                bakedTerrainSamplers,
                mesh,
                root,
                textureRegistry,
                context
            );
        }

        private static StaticMaterialDef ResolveMaterialDefiniton(
            string materialName,
            BinTree materialsBin,
            MapGeometryGltfConversionContext context
        )
        {
            // Find material definition bin object
            BinTreeObject materialBinObject = materialsBin.Objects.FirstOrDefault(
                x => x.PathHash == Fnv1a.HashLower(materialName)
            );
            if (materialBinObject is null)
                ThrowHelper.ThrowInvalidOperationException($"Failed to find {materialName} in {nameof(materialsBin)}");

            // Deserialize material definition
            return MetaSerializer.Deserialize<StaticMaterialDef>(context.MetaEnvironment, materialBinObject);
        }

        private static void InitializeMaterialRenderTechnique(Material material, StaticMaterialDef materialDef)
        {
            // Resolve default technique
            StaticMaterialTechniqueDef defaultTechnique = materialDef.Techniques.FirstOrDefault(
                x => x.Value.Name == materialDef.DefaultTechnique
            );
            if (defaultTechnique is null)
                return;

            // Get first render pass definition
            StaticMaterialPassDef pass = defaultTechnique.Passes.FirstOrDefault();
            if (pass is null)
                return;

            // Try to get alpha cutoff, if it doesn't exist then assign default one
            StaticMaterialShaderParamDef alphaCutoffParameter = materialDef.ParamValues.FirstOrDefault(
                x => ALPHA_CLIP_PARAM_NAMES.Contains(x.Value.Name)
            );
            if (alphaCutoffParameter is not null)
            {
                material.Alpha = AlphaMode.MASK;
                material.AlphaCutoff = alphaCutoffParameter.Value.X;
            }
            else if (pass.BlendEnable)
            {
                material.Alpha = AlphaMode.MASK;
                material.AlphaCutoff = DEFAULT_ALPHA_TEST;
            }
        }

        private static void InitializeMaterialBaseColorChannel(
            Material material,
            StaticMaterialDef materialDef,
            MapGeometryBakedTerrainSamplers bakedTerrainSamplers,
            MapGeometryModel mesh,
            ModelRoot root,
            TextureRegistry textureRegistry,
            MapGeometryGltfConversionContext context
        )
        {
            Guard.IsNotNull(materialDef, nameof(materialDef));

            // Resolve diffuse sampler definition, return if not found
            StaticMaterialShaderSamplerDef diffuseSampler = materialDef.SamplerValues.FirstOrDefault(
                x =>
                    DIFFUSE_SAMPLER_NAMES.Contains(x.Value.SamplerName)
                    || x.Value.SamplerName == bakedTerrainSamplers.Primary
            );
            if (diffuseSampler is null)
                return;

            int texcoordId = 0;
            MapGeometrySamplerData sampler = new();
            if (!string.IsNullOrEmpty(mesh.BakedPaint.Texture))
            {
                texcoordId = 1;
                sampler = mesh.BakedPaint;
            }
            else if (!string.IsNullOrEmpty(diffuseSampler.TextureName))
            {
                sampler = new(diffuseSampler.TextureName, Vector2.One, Vector2.Zero);
            }
            else
            {
                sampler = mesh.StationaryLight;
            }

            // Return if we couldn't figure out the sampler
            if (string.IsNullOrEmpty(sampler.Texture))
                return;

            // Create glTF Image
            GltfImage image = CreateImage(sampler.Texture, textureRegistry, root, context);

            // Set channel properties
            MaterialChannel baseColorChannel = material.FindChannel("BaseColor").Value;

            baseColorChannel.SetTransform(sampler.Bias, sampler.Scale);
            baseColorChannel.SetTexture(
                texcoordId,
                image,
                ws: GetGltfTextureWrapMode((TextureAddress)diffuseSampler.AddressU),
                wt: GetGltfTextureWrapMode((TextureAddress)diffuseSampler.AddressV)
            );
        }

        private static GltfImage CreateImage(
            string textureName,
            TextureRegistry textureRegistry,
            ModelRoot root,
            MapGeometryGltfConversionContext context
        )
        {
            string texturePath = GetQualityPrefixedTexturePath(
                Path.Join(context.Settings.GameDataPath, textureName),
                context.Settings.TextureQuality
            );

            // If texture is already loaded, return it
            if (textureRegistry.TryGetValue(texturePath, out GltfImage existingImage))
                return existingImage;

            // Load texture
            LeagueTexture texture = LoadTexture(texturePath);

            // Re-encode to PNG
            ReadOnlyMemory2D<ColorRgba32> biggestMipMap = texture.Mips[0];
            using MemoryStream imageStream = new();
            using Image<Rgba32> image = biggestMipMap.ToImage();

            image.SaveAsPng(imageStream);

            // Create glTF image
            GltfImage gltfImage = root.UseImage(new(imageStream.ToArray()));
            gltfImage.Name = Path.GetFileNameWithoutExtension(texturePath);

            textureRegistry.Add(texturePath, gltfImage);

            return gltfImage;
        }

        private static LeagueTexture LoadTexture(string texturePath)
        {
            // Get texture file format and return if it's unknown
            using FileStream textureStream = File.OpenRead(texturePath);
            TextureFileFormat format = LeagueTexture.IdentifyFileFormat(textureStream);
            if (format is TextureFileFormat.Unknown)
                return null;

            // Load and register texture
            return LeagueTexture.Load(textureStream);
        }
        #endregion

        private static string GetMapName(BinTree materialsBin, MapGeometryGltfConversionContext context)
        {
            if (materialsBin is null)
                return DEFAULT_MAP_NAME;

            BinTreeObject mapContainerObject = materialsBin.Objects.FirstOrDefault(
                x => x.MetaClassHash == Fnv1a.HashLower(nameof(MapContainer))
            );
            if (mapContainerObject is null)
                throw new InvalidOperationException(
                    $"Failed to find {nameof(MapContainer)} in the provided materials.bin"
                );

            MapContainer mapContainer = MetaSerializer.Deserialize<MapContainer>(
                context.MetaEnvironment,
                mapContainerObject
            );

            return string.IsNullOrEmpty(mapContainer.MapPath) ? DEFAULT_MAP_NAME : mapContainer.MapPath;
        }

        private static string GetQualityPrefixedTexturePath(string texturePath, MapGeometryGltfTextureQuality quality)
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

        private static GltfTextureWrapMode GetGltfTextureWrapMode(TextureAddress textureAddress) =>
            textureAddress switch
            {
                TextureAddress.Wrap => GltfTextureWrapMode.REPEAT,
                TextureAddress.Clamp => GltfTextureWrapMode.CLAMP_TO_EDGE,
                _ => throw new NotImplementedException($"Invalid {nameof(TextureAddress)}: {textureAddress}")
            };

        private static GltfTextureInterpolationFilter GetGltfTextureInterpolationFilter(TextureFilter textureFilter) =>
            textureFilter switch
            {
                TextureFilter.None => GltfTextureInterpolationFilter.DEFAULT,
                TextureFilter.Nearest => GltfTextureInterpolationFilter.NEAREST,
                TextureFilter.Linear => GltfTextureInterpolationFilter.LINEAR,
                _ => throw new NotImplementedException($"Invalid {nameof(TextureFilter)}: {textureFilter}")
            };

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

            public MapGeometrySamplerData StationaryLight { get; init; }
            public MapGeometrySamplerData BakedLight { get; init; }
            public MapGeometrySamplerData BakedPaint { get; init; }
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
    /// Specifies the <see cref="EnvironmentVisibilityFlags"/> grouping policy for meshes
    /// </summary>
    /// <remarks>
    /// The <see cref="EnvironmentVisibilityFlags"/> will always be written into the
    /// <see href="https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#reference-extras">extras field</see>
    /// </remarks>
    public enum MapGeometryGltfLayerGroupingPolicy
    {
        /// <summary>
        /// Each mesh will be placed under the node of each of its <see cref="EnvironmentVisibilityFlags"/>
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
