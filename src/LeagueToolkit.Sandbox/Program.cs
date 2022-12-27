using LeagueToolkit.Converters;
using LeagueToolkit.Helpers.Structures.BucketGrid;
using LeagueToolkit.IO.AnimationFile;
using LeagueToolkit.IO.PropertyBin;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.IO.NavigationGridOverlay;
using LeagueToolkit.IO.NVR;
using LeagueToolkit.IO.OBJ;
using LeagueToolkit.IO.ReleaseManifestFile;
using LeagueToolkit.IO.SimpleSkinFile;
using LeagueToolkit.IO.SkeletonFile;
using LeagueToolkit.IO.StaticObjectFile;
using LeagueToolkit.IO.WadFile;
using LeagueToolkit.IO.WGT;
using LeagueToolkit.IO.WorldGeometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeagueAnimation = LeagueToolkit.IO.AnimationFile.Animation;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Attributes;
using System.Numerics;
using LeagueToolkit.Meta.Dump;
using System.Reflection;
using LeagueToolkit.Helpers;
using LeagueToolkit.IO.MapGeometryFile.Builder;
using CommunityToolkit.HighPerformance.Buffers;
using System.Buffers;
using LeagueToolkit.Core.Memory;
using CommunityToolkit.Diagnostics;

namespace LeagueToolkit.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            ProfileMapgeo("ioniabase.mapgeo", "worlds_trophyonly_rewritten.mapgeo");
        }

        static void TestMetaRoslynCodegen(string outputFile)
        {
            IEnumerable<string> classes = File.ReadLines("hashes.bintypes.txt").Select(line => line.Split(' ')[1]);
            IEnumerable<string> properties = File.ReadLines("hashes.binfields.txt").Select(line => line.Split(' ')[1]);

            MetaDump
                .Deserialize(File.ReadAllText("latest_meta.json"))
                .WriteMetaClasses(outputFile, classes, properties);
        }

        static void ProfileMapgeo(string toRead, string rewriteTo)
        {
            using MapGeometry mgeo = new(toRead);
            MapGeometryBuilder mapBuilder = new();

            mapBuilder.UseBucketGrid(mgeo.BucketGrid).UseBakedTerrainSamplers(mgeo.BakedTerrainSamplers);

            foreach (MapGeometryModel mesh in mgeo.Meshes)
            {
                MapGeometryModelBuilder meshBuilder = new();

                for (int i = 0; i < mesh.Submeshes.Count; i++)
                {
                    MapGeometrySubmesh submesh = mesh.Submeshes[i];

                    meshBuilder.UseSubmesh(new(submesh.Material, submesh.StartIndex, submesh.IndexCount));
                }

                meshBuilder
                    .UseTransform(mesh.Transform)
                    .UseFlipNormalsToggle(mesh.FlipNormals)
                    .UseEnvironmentQualityFilter(mesh.EnvironmentQualityFilter)
                    .UseVisibilityFlags(mesh.VisibilityFlags)
                    .UseRenderFlags(mesh.RenderFlags)
                    .UseStationaryLightSampler(mesh.StationaryLight)
                    .UseBakedLightSampler(mesh.BakedLight)
                    .UseBakedPaintSampler(mesh.BakedPaint)
                    .UseGeometry(
                        mesh.Indices.Length,
                        mesh.VertexData.VertexCount,
                        mesh.VertexData.Description.Elements,
                        (indexBufferWriter, vertexBufferWriter) =>
                        {
                            indexBufferWriter.Write(mesh.Indices);

                            RewriteVertexBuffer(mesh, vertexBufferWriter);
                        }
                    );

                mapBuilder.UseMesh(meshBuilder);
            }

            using MapGeometry builtMap = mapBuilder.Build();
            builtMap.Write(rewriteTo, 13);

            static void RewriteVertexBuffer(MapGeometryModel mesh, VertexBufferWriter writer)
            {
                bool hasPositions = mesh.VertexData.TryGetAccessor(ElementName.Position, out var positionAccessor);
                bool hasNormals = mesh.VertexData.TryGetAccessor(ElementName.Normal, out var normalAccessor);
                bool hasBaseColor = mesh.VertexData.TryGetAccessor(ElementName.BaseColor, out var baseColorAccessor);
                bool hasDiffuseUvs = mesh.VertexData.TryGetAccessor(ElementName.DiffuseUV, out var diffuseUvAccessor);
                bool hasLightmapUvs = mesh.VertexData.TryGetAccessor(
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

                for (int i = 0; i < mesh.VertexData.VertexCount; i++)
                {
                    writer.WriteVector3(i, ElementName.Position, positionsArray[i]);
                    if (hasNormals)
                        writer.WriteVector3(i, ElementName.Normal, normalsArray[i]);
                    if (hasBaseColor)
                    {
                        var (b, g, r, a) = baseColorArray[i];
                        writer.WriteColorBgraU8(i, ElementName.BaseColor, new(r, g, b, a));
                    }
                    if (hasDiffuseUvs)
                        writer.WriteVector2(i, ElementName.DiffuseUV, diffuseUvsArray[i]);
                    if (hasLightmapUvs)
                        writer.WriteVector2(i, ElementName.LightmapUV, lightmapUvsArray[i]);
                }
            }
        }

        static void TestWGEO()
        {
            WorldGeometry wgeo = new WorldGeometry("room.wgeo");
            Directory.CreateDirectory("kek");

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    BucketGridBucket bucket = wgeo.BucketGrid.Buckets[i, j];

                    List<uint> indices = wgeo.BucketGrid.Indices
                        .GetRange((int)bucket.StartIndex, (bucket.InsideFaceCount + bucket.StickingOutFaceCount) * 3)
                        .Select(x => (uint)x)
                        .ToList();

                    if (indices.Count != 0)
                    {
                        int startVertex = (int)indices.Min();
                        int vertexCount = (int)indices.Max() - startVertex;
                        List<Vector3> vertices = wgeo.BucketGrid.Vertices.GetRange(
                            startVertex + (int)bucket.BaseVertex,
                            vertexCount
                        );

                        new OBJFile(vertices, indices).Write(string.Format("kek/bucket{0}_{1}.obj", i, j));
                    }
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
