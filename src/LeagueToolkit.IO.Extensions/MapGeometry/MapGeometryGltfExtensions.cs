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
using GltfTextureInterpolationFilter = SharpGLTF.Schema2.TextureInterpolationFilter;
using GltfTextureWrapMode = SharpGLTF.Schema2.TextureWrapMode;
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
            if (defaultTechniqueShader == Fnv1a.HashLower("Shaders/StaticMesh/Hologram"))
            {
                StaticMeshHologram.InitializeMaterial(gltfMaterial, materialDef, mesh, textureRegistry, root, context);
            }
            else
            {
                TryInitializeMetallicRoughness(gltfMaterial, materialDef, root, textureRegistry, context);

                // Initialize material properties
                InitializeMaterialRenderTechnique(gltfMaterial, materialDef);
                InitializeMaterialBaseColorChannel(
                    gltfMaterial,
                    materialDef,
                    bakedTerrainSamplers,
                    mesh,
                    root,
                    textureRegistry,
                    context
                );
                InitializeMaterialEmissiveColorChannel(gltfMaterial, materialDef, mesh, root, textureRegistry, context);
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

        private static void TryInitializeMetallicRoughness(
            Material gltfMaterial,
            StaticMaterialDef materialDef,
            ModelRoot root,
            TextureRegistry textureRegistry,
            MapGeometryGltfConversionContext context
        )
        {
            // Find specular glossiness sampler
            StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(
                x => x.Value.SamplerName is "Material"
            );
            MaterialChannel channel = gltfMaterial.FindChannel("MetallicRoughness").Value;

            // Material isn't PBR Metallic Roughness
            if (samplerDef is null)
            {
                channel.SetFactor("RoughnessFactor", 0.5f);
                return;
            }

            // Resolve gloss factor parameter
            StaticMaterialShaderParamDef glossParamDef = materialDef.ParamValues.FirstOrDefault(
                x => x.Value.Name is "Gloss"
            );

            // Resolve specular factor parameter
            StaticMaterialShaderParamDef specularFactorParamDef = materialDef.ParamValues.FirstOrDefault(
                x => x.Value.Name is "SpecularColor_Multiplier"
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

            channel.SetFactor("MetallicFactor", 0.75f);
            channel.SetFactor("RoughnessFactor", 1);

            channel.SetTexture(
                0,
                image,
                ws: GetGltfTextureWrapMode((TextureAddress)samplerDef.AddressU),
                wt: GetGltfTextureWrapMode((TextureAddress)samplerDef.AddressV)
            );
        }

        private static void InitializeMaterialRenderTechnique(Material gltfMaterial, StaticMaterialDef materialDef)
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
                gltfMaterial.Alpha = AlphaMode.MASK;
                gltfMaterial.AlphaCutoff = alphaCutoffParameter.Value.X;
            }
            else if (pass.BlendEnable)
            {
                gltfMaterial.Alpha = AlphaMode.BLEND;
            }
            else
            {
                gltfMaterial.Alpha = AlphaMode.MASK;
                gltfMaterial.AlphaCutoff = DEFAULT_ALPHA_TEST;
            }
        }

        private static void InitializeMaterialBaseColorChannel(
            Material gltfMaterial,
            StaticMaterialDef materialDef,
            EnvironmentAssetBakedTerrainSamplers bakedTerrainSamplers,
            EnvironmentAssetMesh mesh,
            ModelRoot root,
            TextureRegistry textureRegistry,
            MapGeometryGltfConversionContext context
        )
        {
            Guard.IsNotNull(materialDef, nameof(materialDef));

            // Resolve diffuse sampler definition, return if not found
            StaticMaterialShaderSamplerDef samplerDef = materialDef.SamplerValues.FirstOrDefault(
                x =>
                    DIFFUSE_SAMPLER_NAMES.Contains(x.Value.SamplerName)
                    || x.Value.SamplerName == bakedTerrainSamplers.Primary
            );
            if (samplerDef is null)
                return;

            // Figure out texcoord id and sampler transform
            int texcoordId = 0;
            EnvironmentAssetSampler sampler = new();
            if (!string.IsNullOrEmpty(mesh.BakedPaint.Texture))
            {
                texcoordId = 1;
                sampler = mesh.BakedPaint;
            }
            else if (!string.IsNullOrEmpty(samplerDef.TextureName))
            {
                sampler = new(samplerDef.TextureName, Vector2.One, Vector2.Zero);
            }
            else
            {
                sampler = mesh.StationaryLight;
            }

            StaticMaterialShaderParamDef colorParam = materialDef.ParamValues.FirstOrDefault(
                x => x.Value.Name is "Color"
            );
            gltfMaterial.WithChannelColor("BaseColor", (colorParam?.Value ?? Vector4.One) with { W = 1f });

            if (!string.IsNullOrEmpty(sampler.Texture))
            {
                MaterialChannel channel = gltfMaterial.FindChannel("BaseColor").Value;
                GltfImage image = CreateImage(sampler.Texture, textureRegistry, root, context);

                channel.SetTransform(sampler.Bias, sampler.Scale);
                channel.SetTexture(
                    texcoordId,
                    image,
                    ws: GetGltfTextureWrapMode((TextureAddress)samplerDef.AddressU),
                    wt: GetGltfTextureWrapMode((TextureAddress)samplerDef.AddressV)
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
            StaticMaterialShaderParamDef emissiveColorParamDef = materialDef.ParamValues.FirstOrDefault(
                x => x.Value.Name is "Emissive_Color"
            );
            StaticMaterialShaderParamDef emissiveIntensityParamDef = materialDef.ParamValues.FirstOrDefault(
                x => x.Value.Name is "Emissive_Intensity"
            );

            MaterialChannel emissiveChannel = gltfMaterial.FindChannel("Emissive").Value;

            gltfMaterial.WithChannelFactor("Emissive", "EmissiveStrength", emissiveIntensityParamDef?.Value.X ?? 0.1f);
            gltfMaterial.WithChannelColor("Emissive", Vector4.One);

            if (emissiveColorParamDef is not null)
            {
                gltfMaterial.WithChannelColor("Emissive", emissiveColorParamDef?.Value ?? Vector4.One);
            }
            else if (!string.IsNullOrEmpty(mesh.BakedLight.Texture))
            {
                // Create glTF Image
                GltfImage image = CreateImage(mesh.BakedLight.Texture, textureRegistry, root, context);

                emissiveChannel.SetTransform(mesh.BakedLight.Bias, mesh.BakedLight.Scale);
                emissiveChannel.SetTexture(1, image);
            }
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
            LeagueTexture texture = TextureUtils.Load(texturePath);

            // Re-encode to PNG
            ReadOnlyMemory2D<ColorRgba32> biggestMipMap = texture.Mips[0];
            using Image<Rgba32> image = biggestMipMap.ToImage();

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
                            B = color.R,
                            A = 255
                        };
                    }
                }
            });

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
