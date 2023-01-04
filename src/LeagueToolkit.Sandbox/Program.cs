﻿using LeagueToolkit.Converters;
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
using System.Threading;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Mesh;

namespace LeagueToolkit.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            //using MapGeometry mgeo = new("worlds_trophyonly_rewritten_reordered.mapgeo");
            //ProfileMapgeo("ioniabase.mapgeo", "ioniabase_rewritten.mapgeo");
            ProfileSkinnedMesh();
        }

        static void ProfileSkinnedMesh()
        {
            using SkinnedMesh skinnedMesh = SkinnedMesh.ReadFromSimpleSkin("akali.skn");
            Skeleton skeleton = new("akali.skl");

            skinnedMesh.ToGltf(skeleton).WriteGLB(File.OpenWrite("akali.glb"));
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
            //mgeo.ToGLTF().WriteGLB(File.OpenWrite("instanced.glb"));
            //mgeo.Write(Path.ChangeExtension(rewriteTo, "instanced.mapgeo"), 13);

            MapGeometryBuilder mapBuilder = new MapGeometryBuilder()
                .WithBucketGrid(mgeo.BucketGrid)
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
