using BCnEncoder.Shared;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.IO.AnimationFile;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.IO.MapGeometryFile.Builder;
using LeagueToolkit.IO.PropertyBin;
using LeagueToolkit.IO.SimpleSkinFile;
using LeagueToolkit.IO.SkeletonFile;
using LeagueToolkit.IO.StaticObjectFile;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using LeagueToolkit.Meta.Dump;
using LeagueToolkit.Toolkit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using RigResource = LeagueToolkit.Core.Animation.RigResource;

namespace LeagueToolkit.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            ProfileRigResource();

            //ProfileMapgeo("ioniabase.mapgeo", "ioniabase_rewritten.mapgeo");
            //ProfileMetaSerialization();
        }

        static void ProfileRigResource()
        {
            RigResource skeleton = new(File.OpenRead("Brand.skl"));
        }

        static void ProfileMapgeoToGltf()
        {
            BinTree materialsBin = new("ioniabase.materials.bin");
            using MapGeometry mgeo = new("ioniabase.mapgeo");

            MetaEnvironment metaEnvironment = MetaEnvironment.Create(
                Assembly.Load("LeagueToolkit.Meta.Classes").GetExportedTypes().Where(x => x.IsClass)
            );

            mgeo.ToGltf(
                    materialsBin,
                    new(
                        metaEnvironment,
                        new()
                        {
                            GameDataPath = "X:/lol/game",
                            FlipAcrossX = false,
                            LayerGroupingPolicy = MapGeometryGltfLayerGroupingPolicy.Ignore,
                            TextureQuality = MapGeometryGltfTextureQuality.Low
                        }
                    )
                )
                .SaveGLB("ioniabase.glb");
        }

        static void ProfileTexture()
        {
            Texture texture = Texture.Load(File.OpenRead("3.dds"));

            ReadOnlyMemory2D<ColorRgba32> mipmap = texture.Mips[0];

            Image<Rgba32> image = mipmap.ToImage();

            image.SaveAsPng("3.dds.png");
        }

        static void ProfileSkinnedMesh()
        {
            using SkinnedMesh skinnedMesh = SkinnedMesh.ReadFromSimpleSkin("akali.skn");
            Skeleton skeleton = new("akali.skl");

            List<(string name, Animation animation)> animations = new();
            foreach (string animationFile in Directory.EnumerateFiles("animations"))
            {
                Animation animation = new(animationFile);

                animations.Add((Path.GetFileNameWithoutExtension(animationFile), animation));
            }

            skinnedMesh
                .ToGltf(
                    new Dictionary<string, ReadOnlyMemory<byte>>()
                    {
                        { "Akali_Base_Body_Mat", File.ReadAllBytes("akali_base_tx_cm.dds") }
                    }
                )
                .WriteGLB(File.OpenWrite("akali.glb"));
        }

#if DEBUG
        static async Task TestMetaRoslynCodegen(string metaJsonFile, string outputFile)
        {
            using HttpClient client = new();

            byte[] binTypesBuffer = await client.GetByteArrayAsync(
                "https://raw.githubusercontent.com/CommunityDragon/CDTB/master/cdragontoolbox/hashes.bintypes.txt"
            );
            byte[] binFieldsBuffer = await client.GetByteArrayAsync(
                "https://raw.githubusercontent.com/CommunityDragon/CDTB/master/cdragontoolbox/hashes.binfields.txt"
            );

            File.WriteAllBytes("hashes.bintypes.txt", binTypesBuffer);
            File.WriteAllBytes("hashes.binfields.txt", binFieldsBuffer);

            IEnumerable<string> classes = File.ReadLines("hashes.bintypes.txt").Select(line => line.Split(' ')[1]);
            IEnumerable<string> properties = File.ReadLines("hashes.binfields.txt").Select(line => line.Split(' ')[1]);

            MetaDump.Deserialize(File.ReadAllText(metaJsonFile)).WriteMetaClasses(outputFile, classes, properties);
        }
#endif

        static void ProfileMetaSerialization()
        {
            BinTree binTree = new("base_srx.materials.bin");

            MetaEnvironment metaEnvironment = MetaEnvironment.Create(
                Assembly.Load("LeagueToolkit.Meta.Classes").GetExportedTypes().Where(x => x.IsClass)
            );

            StaticMaterialDef staticMaterialDef = MetaSerializer.Deserialize<StaticMaterialDef>(
                metaEnvironment,
                binTree.Objects.First(x => x.PathHash == 0x75cccc52)
            );
        }

        static void ProfileMapgeo(string toRead, string rewriteTo)
        {
            using MapGeometry mgeo = new(toRead);
            //mgeo.ToGLTF().WriteGLB(File.OpenWrite("instanced.glb"));
            //mgeo.Write(Path.ChangeExtension(rewriteTo, "instanced.mapgeo"), 13);

            MapGeometryBuilder mapBuilder = new MapGeometryBuilder()
                .WithSceneGraph(mgeo.SceneGraph)
                .WithBakedTerrainSamplers(mgeo.BakedTerrainSamplers);

            Dictionary<ElementName, int> elementOrder =
                new() { { ElementName.DiffuseUV, 0 }, { ElementName.Normal, 1 }, { ElementName.Position, 2 } };

            foreach (MapGeometryModel mesh in mgeo.Meshes)
            {
                var (vertexBuffer, vertexBufferWriter) = mapBuilder.UseVertexBuffer(
                    VertexBufferUsage.Static,
                    mesh.VerticesView.Buffers
                        .SelectMany(vertexBuffer => vertexBuffer.Description.Elements)
                        .OrderBy(element => element.Name),
                    mesh.VerticesView.VertexCount
                );
                var (indexBuffer, indexBufferWriter) = mapBuilder.UseIndexBuffer(mesh.Indices.Length);

                indexBufferWriter.Write(mesh.Indices.Span);
                RewriteVertexBuffer(mesh, vertexBufferWriter);

                MapGeometryModelBuilder meshBuilder = new MapGeometryModelBuilder()
                    .WithTransform(mesh.Transform)
                    .WithDisableBackfaceCulling(mesh.DisableBackfaceCulling)
                    .WithEnvironmentQualityFilter(mesh.EnvironmentQualityFilter)
                    .WithVisibilityFlags(mesh.VisibilityFlags)
                    .WithRenderFlags(mesh.RenderFlags)
                    .WithStationaryLightSampler(mesh.StationaryLight)
                    .WithBakedLightSampler(mesh.BakedLight)
                    .WithBakedPaintSampler(mesh.BakedPaint)
                    .WithGeometry(
                        mesh.Submeshes.Select(
                            submesh =>
                                new MeshPrimitiveBuilder(submesh.Material, submesh.StartIndex, submesh.IndexCount)
                        ),
                        vertexBuffer,
                        indexBuffer
                    );

                mapBuilder.WithMesh(meshBuilder);
            }

            using MapGeometry builtMap = mapBuilder.Build();
            builtMap.Write(rewriteTo, 13);

            static void RewriteVertexBuffer(MapGeometryModel mesh, VertexBufferWriter writer)
            {
                bool hasPositions = mesh.VerticesView.TryGetAccessor(ElementName.Position, out var positionAccessor);
                bool hasNormals = mesh.VerticesView.TryGetAccessor(ElementName.Normal, out var normalAccessor);
                bool hasBaseColor = mesh.VerticesView.TryGetAccessor(
                    ElementName.PrimaryColor,
                    out var baseColorAccessor
                );
                bool hasDiffuseUvs = mesh.VerticesView.TryGetAccessor(ElementName.DiffuseUV, out var diffuseUvAccessor);
                bool hasLightmapUvs = mesh.VerticesView.TryGetAccessor(
                    ElementName.LightmapUV,
                    out var lightmapUvAccessor
                );

                if (hasPositions is false)
                    ThrowHelper.ThrowInvalidOperationException($"Mesh: {mesh.Name} does not have vertex positions");

                VertexElementArray<Vector3> positionsArray = positionAccessor.AsVector3Array();
                VertexElementArray<Vector3> normalsArray = hasNormals ? normalAccessor.AsVector3Array() : new();
                var baseColorArray = hasBaseColor ? baseColorAccessor.AsBgraU8Array() : new();
                VertexElementArray<Vector2> diffuseUvsArray = hasDiffuseUvs
                    ? diffuseUvAccessor.AsVector2Array()
                    : new();
                VertexElementArray<Vector2> lightmapUvsArray = hasLightmapUvs
                    ? lightmapUvAccessor.AsVector2Array()
                    : new();

                for (int i = 0; i < mesh.VerticesView.VertexCount; i++)
                {
                    writer.WriteVector3(i, ElementName.Position, positionsArray[i]);
                    if (hasNormals)
                        writer.WriteVector3(i, ElementName.Normal, normalsArray[i]);
                    if (hasBaseColor)
                    {
                        var (b, g, r, a) = baseColorArray[i];
                        writer.WriteColorBgraU8(i, ElementName.PrimaryColor, new(r, g, b, a));
                    }
                    if (hasDiffuseUvs)
                        writer.WriteVector2(i, ElementName.DiffuseUV, diffuseUvsArray[i]);
                    if (hasLightmapUvs)
                        writer.WriteVector2(i, ElementName.LightmapUV, lightmapUvsArray[i]);
                }
            }
        }

        static void TestStaticObject()
        {
            StaticObject sco = StaticObject.ReadSCB("aatrox_base_w_ground_ring.scb");
            sco.WriteSCO(@"C:\Users\Crauzer\Desktop\zzzz.sco");

            StaticObject x = StaticObject.ReadSCB(@"C:\Users\Crauzer\Desktop\zzzz.scb");
        }
    }
}
