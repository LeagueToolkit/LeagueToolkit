﻿using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.Hashing;
using LeagueToolkit.IO.PropertyBin;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using SharpGLTF.IO;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Textures.Formats;
using SixLabors.ImageSharp.Textures.Formats.Dds;
using SixLabors.ImageSharp.Textures.TextureFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GltfImage = SharpGLTF.Schema2.Image;
using GltfTextureInterpolationFilter = SharpGLTF.Schema2.TextureInterpolationFilter;
using GltfTextureWrapMode = SharpGLTF.Schema2.TextureWrapMode;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using ImageSharpTexture = SixLabors.ImageSharp.Textures.Texture;
using TextureRegistry = System.Collections.Generic.Dictionary<string, SharpGLTF.Schema2.Image>;

namespace LeagueToolkit.IO.MapGeometryFile
{
    public static class MapGeometryGltfExtensions
    {
        private static readonly string[] DIFFUSE_SAMPLER_NAMES = new[] { "DiffuseTexture", "Diffuse_Texture" };
        private static readonly string[] TINT_COLOR_PARAM_NAMES = new[] { "TintColor", "Tint_Color" };

        private const float DEFAULT_ALPHA_TEST = 0.3f;

        private const string TEXTURE_QUALITY_PREFIX_LOW = "4x";
        private const string TEXTURE_QUALITY_PREFIX_MEDIUM = "2x";

        public static ModelRoot ToGltf(
            this MapGeometry mapGeometry,
            BinTree materialsBin,
            MapGeometryGltfConversionContext context
        )
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene(root.LogicalScenes.Count);
            Node mapNode = scene.CreateNode("map").WithLocalScale(new(-1f, 1f, 1f));

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

                // Create mesh node
                mapNode.CreateNode($"__{mesh.Name}__").WithMesh(gltfMesh).WithLocalTransform(mesh.Transform);
            }

            root.MergeImages();
            root.MergeBuffers();

            root.DefaultScene = scene;

            return root;
        }

        private static Mesh CreateGltfMesh(
            ModelRoot root,
            MapGeometryModel mesh,
            IReadOnlyDictionary<string, Material> materials
        )
        {
            Mesh gltfMesh = root.CreateMesh(mesh.Name);

            MemoryAccessor[] meshVertexMemoryAccessors = CreateGltfMeshVertexMemoryAccessors(mesh.VerticesView);

            MemoryAccessor.SanitizeVertexAttributes(meshVertexMemoryAccessors);
            SanitizeVertexMemoryAccessors(meshVertexMemoryAccessors);

            foreach (MapGeometrySubmesh range in mesh.Submeshes)
            {
                MemoryAccessor indicesMemoryAccessor = CreateGltfMeshPrimitiveIndicesMemoryAccessor(
                    range,
                    mesh.Indices
                );
                MemoryAccessor[] vertexMemoryAccessors = CreateGltfMeshPrimitiveVertexMemoryAccessors(
                    range,
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
            Guard.IsNotNull(material, nameof(material));

            if (string.IsNullOrEmpty(context.Settings.GameDataPath))
                return;

            StaticMaterialDef materialDef = ResolveMaterialDefiniton(material.Name, materialsBin, context);

            material.Extras = JsonContent.Serialize(new GltfMaterialExtras() { Name = material.Name });

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
            if (materialBinObject is null && context.Settings.ThrowIfMaterialNotFound is true)
                ThrowHelper.ThrowInvalidOperationException($"Failed to find {materialName} in {nameof(materialsBin)}");

            // Deserialize material definition
            StaticMaterialDef materialDef = null;
            try
            {
                materialDef = MetaSerializer.Deserialize<StaticMaterialDef>(context.MetaEnvironment, materialBinObject);
            }
            catch (Exception)
            {
                if (context.Settings.ThrowOnMaterialDeserializationFailure)
                    throw;
            }

            return materialDef;
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
            StaticMaterialShaderParamDef alphaTestParameter = materialDef.ParamValues.FirstOrDefault(
                x => x.Value.Name == "AlphaTestValue"
            );
            if (alphaTestParameter is not null)
            {
                material.Alpha = AlphaMode.MASK;
                material.AlphaCutoff = alphaTestParameter.Value.X;
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

            // Resolve diffuse sampler definition
            StaticMaterialShaderSamplerDef diffuseSampler = materialDef.SamplerValues.FirstOrDefault(
                x =>
                    DIFFUSE_SAMPLER_NAMES.Contains(x.Value.SamplerName)
                    || x.Value.SamplerName == bakedTerrainSamplers.Primary
            );

            // If diffuse sampler wasn't found and STATIONARY_LIGHT and BAKED_PAINT
            // are also null then we cannot create a valid diffuse channel, return
            string textureName = diffuseSampler?.TextureName ?? mesh.StationaryLight.Texture ?? mesh.BakedPaint.Texture;
            if (string.IsNullOrEmpty(textureName))
                return;

            // Create glTF Image
            string texturePath = GetQualityPrefixedTexturePath(
                Path.Join(context.Settings.GameDataPath, textureName),
                context.Settings.TextureQuality
            );
            GltfImage image = CreateImage(texturePath, textureRegistry, root);

            // Set channel properties
            MaterialChannel baseColorChannel = material.FindChannel("BaseColor").Value;
            baseColorChannel.SetTexture(
                0,
                image,
                ws: GetGltfTextureWrapMode((TextureAddress)diffuseSampler.AddressU),
                wt: GetGltfTextureWrapMode((TextureAddress)diffuseSampler.AddressV)
            );
        }

        private static GltfImage CreateImage(string texturePath, TextureRegistry textureRegistry, ModelRoot root)
        {
            // If texture is already loaded, return it
            if (textureRegistry.TryGetValue(texturePath, out GltfImage existingImage))
                return existingImage;

            // Load texture
            using ImageSharpTexture texture = LoadTexture(texturePath);
            if (texture is not FlatTexture flatTexture)
                return null;

            // Re-encode to PNG
            using MemoryStream imageStream = new();
            using ImageSharpImage image = flatTexture.MipMaps[0].GetImage();
            image.SaveAsPng(imageStream);

            // Create glTF image
            GltfImage gltfImage = root.UseImage(new(imageStream.ToArray()));
            gltfImage.Name = Path.GetFileNameWithoutExtension(texturePath);

            textureRegistry.Add(texturePath, gltfImage);

            return gltfImage;
        }

        private static ImageSharpTexture LoadTexture(string texturePath)
        {
            // TODO: Support TEX files
            // Get texture format and if it's not DDS, return
            ITextureFormat textureFormat = ImageSharpTexture.DetectFormat(texturePath);
            if (textureFormat is not DdsFormat)
                return null;

            // Load and register texture
            return ImageSharpTexture.Load(texturePath);
        }

        private static MemoryAccessor[] CreateGltfMeshVertexMemoryAccessors(InstancedVertexBufferView vertexBuffer)
        {
            List<MemoryAccessor> memoryAccessors = new();
            foreach (VertexElement element in vertexBuffer.Buffers.SelectMany(x => x.Description.Elements))
            {
                // Create access info
                MemoryAccessInfo accessInfo = GetElementMemoryAccessInfo(element, vertexBuffer.VertexCount);

                // Create vertex memory accessor
                ArraySegment<byte> gltfAccessorBuffer =
                    new(new byte[accessInfo.StepByteLength * vertexBuffer.VertexCount]);
                MemoryAccessor gltfMemoryAccessor = new(gltfAccessorBuffer, accessInfo);

                // Write data
                int elementSize = element.GetSize();
                VertexElementAccessor elementAccessor = vertexBuffer.GetAccessor(element.Name);
                Span<byte> gltfMemoryAccessorData = gltfMemoryAccessor.Data.AsSpan();
                for (int i = 0; i < vertexBuffer.VertexCount; i++)
                {
                    // First slice into start vertex and then slice the element
                    ReadOnlySpan<byte> elementData = elementAccessor.BufferView.Span.Slice(
                        i * elementAccessor.VertexStride + elementAccessor.ElementOffset,
                        elementSize
                    );

                    WriteAttributeData(
                        gltfMemoryAccessorData,
                        i * accessInfo.StepByteLength,
                        element.Format,
                        elementData
                    );
                }

                memoryAccessors.Add(gltfMemoryAccessor);
            }

            return memoryAccessors.ToArray();
        }

        private static MemoryAccessor[] CreateGltfMeshPrimitiveVertexMemoryAccessors(
            MapGeometrySubmesh range,
            MemoryAccessor[] meshMemoryAccessors
        ) =>
            meshMemoryAccessors
                .Select(x => new MemoryAccessor(x.Data, x.Attribute.Slice(range.MinVertex, range.VertexCount)))
                .ToArray();

        private static MemoryAccessor CreateGltfMeshPrimitiveIndicesMemoryAccessor(
            MapGeometrySubmesh range,
            ReadOnlyMemory<ushort> indexBuffer
        )
        {
            MemoryAccessor memoryAccessor =
                new(
                    new(new byte[2 * range.IndexCount]),
                    new("INDEX", 0, range.IndexCount, 0, DimensionType.SCALAR, EncodingType.UNSIGNED_SHORT)
                );

            Span<ushort> accessorBuffer = memoryAccessor.Data.AsSpan().Cast<byte, ushort>();
            for (int i = 0; i < range.IndexCount; i++)
                accessorBuffer[i] = (ushort)(indexBuffer.Span[i + range.StartIndex] - range.MinVertex);

            return memoryAccessor;
        }

        private static void WriteAttributeData(
            Span<byte> buffer,
            int offset,
            ElementFormat format,
            ReadOnlySpan<byte> data
        )
        {
            if (format is ElementFormat.BGRA_Packed8888)
                WriteAttributeBgraU8(buffer, offset, data);
            else if (format is ElementFormat.ZYXW_Packed8888)
                WriteAttributeZyxwU8(buffer, offset, data);
            else
                data.CopyTo(buffer[offset..]);
        }

        private static void WriteAttributeBgraU8(Span<byte> buffer, int offset, ReadOnlySpan<byte> data)
        {
            buffer[offset + 0] = data[2];
            buffer[offset + 1] = data[1];
            buffer[offset + 2] = data[0];
            buffer[offset + 3] = data[3];
        }

        private static void WriteAttributeZyxwU8(Span<byte> buffer, int offset, ReadOnlySpan<byte> data)
        {
            buffer[offset + 0] = data[2];
            buffer[offset + 1] = data[1];
            buffer[offset + 2] = data[0];
            buffer[offset + 3] = data[3];
        }

        private static void SanitizeVertexMemoryAccessors(MemoryAccessor[] memoryAccessors)
        {
            SanitizeVertexNormalMemoryAccessor(memoryAccessors);
        }

        private static void SanitizeVertexNormalMemoryAccessor(MemoryAccessor[] memoryAccessors)
        {
            MemoryAccessor normalAccessor = memoryAccessors.FirstOrDefault(x => x.Attribute.Name == "NORMAL");
            if (normalAccessor is not null)
            {
                MemoryAccessor positionAccessor = memoryAccessors.First(x => x.Attribute.Name == "POSITION");

                Vector3Array positionsArray = positionAccessor.AsVector3Array();
                Vector3Array normalsArray = normalAccessor.AsVector3Array();

                for (int i = 0; i < positionsArray.Count; i++)
                {
                    Vector3 normal = normalsArray[i];

                    if (normal == Vector3.Zero)
                        normal = positionsArray[i];

                    float length = normal.Length();
                    if (length < 0.99f || length > 0.01f)
                        normal = Vector3.Normalize(normal);

                    normalsArray[i] = normal;
                }
            }
        }

        private static MemoryAccessInfo GetElementMemoryAccessInfo(VertexElement element, int vertexCount)
        {
            string attribute = GetGltfVertexAttributeFromElementName(element.Name);

            return element.Format switch
            {
                ElementFormat.X_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.SCALAR, EncodingType.FLOAT),
                ElementFormat.XY_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.VEC2, EncodingType.FLOAT),
                ElementFormat.XYZ_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.VEC3, EncodingType.FLOAT),
                ElementFormat.XYZW_Float32 => new(attribute, 0, vertexCount, 0, DimensionType.VEC4, EncodingType.FLOAT),

                ElementFormat.BGRA_Packed8888
                or ElementFormat.ZYXW_Packed8888
                or ElementFormat.RGBA_Packed8888
                or ElementFormat.XYZW_Packed8888
                    => new(attribute, 0, vertexCount, 0, DimensionType.VEC4, EncodingType.UNSIGNED_BYTE, true),
                _ => throw new NotImplementedException("Invalid element format"),
            };
        }

        private static string GetGltfVertexAttributeFromElementName(ElementName element)
        {
            return element switch
            {
                ElementName.Position => "POSITION",
                ElementName.Normal => "NORMAL",
                ElementName.PrimaryColor => "COLOR_0",
                ElementName.SecondaryColor => "COLOR_1",
                ElementName.DiffuseUV => "TEXCOORD_0",
                ElementName.LightmapUV => "TEXCOORD_1",
                ElementName.Tangent => "TANGENT",
                _ => throw new NotImplementedException($"Cannot map element: {element} to a glTF vertex attribute")
            };
        }

        private static string GetQualityPrefixedTexturePath(string texturePath, MapGeometryGltfTextureQuality quality)
        {
            return quality switch
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
        /// Gets or sets the quality of the resolved textures
        /// </summary>
        public MapGeometryGltfTextureQuality TextureQuality { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the conversion routine
        /// should throw if it cannot find a material in the provided <see cref="BinTree"/>
        /// </summary>
        public bool ThrowIfMaterialNotFound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the conversion routine
        /// should throw if it fails to deserialize a <see cref="Meta.Classes.StaticMaterialDef"/>
        /// from the provided <see cref="BinTree"/>
        /// </summary>
        public bool ThrowOnMaterialDeserializationFailure { get; set; }
    }

    public enum MapGeometryGltfTextureQuality
    {
        Low,
        Medium,
        High
    }
}
