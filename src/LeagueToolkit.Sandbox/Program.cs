using BCnEncoder.Shared;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.IO.SimpleSkinFile;
using LeagueToolkit.IO.StaticObjectFile;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using LeagueToolkit.Meta.Dump;
using LeagueToolkit.Toolkit;
using SharpGLTF.Schema2;
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
using LeagueTexture = LeagueToolkit.Core.Renderer.Texture;
using LeagueAnimation = LeagueToolkit.Core.Animation.Animation;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Wad;
using System.Text;
using System.Drawing;
using LeagueToolkit.Hashing;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Environment.Builder;

namespace LeagueToolkit.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        foreach (
            string file in Directory.EnumerateFiles(
                @"X:\lol\game\data\maps\mapgeometry\map22",
                "*.mapgeo",
                SearchOption.AllDirectories
            )
        )
        {
            using FileStream fileStream = File.OpenRead(file);
            using EnvironmentAsset environmentAsset = new(fileStream);

            if (
                !string.IsNullOrEmpty(environmentAsset.BakedTerrainSamplers.Primary)
                || !string.IsNullOrEmpty(environmentAsset.BakedTerrainSamplers.Secondary)
            )
            {
                ;
            }
        }
    }

    static void ExtractWad(string wadPath, string extractTo)
    {
        Dictionary<ulong, string> hashtable = new();
        foreach (string line in File.ReadLines(@"X:\lol\tools\Obsidian\GAME_HASHTABLE.txt"))
        {
            string[] split = line.Split(' ', StringSplitOptions.TrimEntries);

            hashtable.Add(Convert.ToUInt64(split[0], 16), split[1]);
        }

        using WadFile wad = new(wadPath);

        foreach (var (chunkHash, chunk) in wad.Chunks)
        {
            if (hashtable.TryGetValue(chunkHash, out string chunkPath))
            {
                using FileStream chunkFileStream = File.Create(Path.Join(extractTo, chunkPath));
                using Stream chunkStream = wad.OpenChunk(chunk);

                chunkStream.CopyTo(chunkFileStream);
            }
        }
    }

    static void ProfileBin(string path)
    {
        BinTree bin = new(path);
    }

    static void ProfileWadFile(string path)
    {
        using WadFile wad = new(path);

        using MemoryOwner<byte> chunkData = wad.LoadChunkDecompressed(
            "assets/characters/belveth/skins/base/belveth_base_main_tx.belveth.dds"
        );

        using FileStream chunkFile = File.Create("X:/sandbox/lol/belveth_base_main_tx.belveth.dds");

        chunkFile.Write(chunkData.Span);
    }

    static void ProfileWadBuilder()
    {
        IEnumerable<string> files = Directory.EnumerateFiles(
            @"X:\sandbox\lol\wadbaketest",
            "*.*",
            SearchOption.AllDirectories
        );
        WadBuilder.BakeFiles(
            files,
            "X:\\sandbox\\lol\\wadbaketest",
            "X:\\sandbox\\lol\\testwad.wad.client",
            new() { DetectDuplicateChunkData = true }
        );
    }

    static void ProfileGltfToRiggedMesh(string gltfPath, string sknPath, string sklPath)
    {
        ModelRoot gltf = ModelRoot.Load(gltfPath);

        var (convertedMesh, convertedRig) = gltf.ToRiggedMesh();

        convertedMesh.WriteSimpleSkin(sknPath);
        using FileStream rigStream = File.Create(sklPath);
        convertedRig.Write(rigStream);
    }

    static void ProfileRiggedMeshToGltf(
        string sknPath,
        string sklPath,
        string gltfPath,
        IEnumerable<(string, IAnimationAsset)> animations
    )
    {
        SkinnedMesh skn = SkinnedMesh.ReadFromSimpleSkin(sknPath);

        using FileStream rigStream = File.OpenRead(sklPath);
        RigResource rig = new(rigStream);

        skn.ToGltf(rig, new List<(string, Stream)>(), animations).Save(gltfPath);
    }

    static void ProfileRigResource()
    {
        RigResource skeleton = new(File.OpenRead("Brand.skl"));
    }

    static void ProfileMapgeoToGltf()
    {
        BinTree materialsBin = new(@"X:\lol\game_old\data\maps\mapgeometry\map19\base.materials.bin");

        using EnvironmentAsset mgeo = new(@"X:\lol\game_old\data\maps\mapgeometry\map19\base.mapgeo");

        MetaEnvironment metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").GetExportedTypes().Where(x => x.IsClass)
        );

        mgeo.ToGltf(
                materialsBin,
                new(
                    metaEnvironment,
                    new()
                    {
                        GameDataPath = @"X:\lol\game_old",
                        FlipAcrossX = false,
                        LayerGroupingPolicy = MapGeometryGltfLayerGroupingPolicy.Ignore,
                        TextureQuality = MapGeometryGltfTextureQuality.High
                    }
                )
            )
            .SaveGLB("map19.glb");
    }

    static void ProfileTexture()
    {
        LeagueTexture texture = LeagueTexture.Load(File.OpenRead("grasstint_srx_infernal.dds"));

        ReadOnlyMemory2D<ColorRgba32> mipmap = texture.Mips[0];

        Image<Rgba32> image = mipmap.ToImage();

        image.SaveAsPng("grasstint_srx_infernal.dds.png");
    }

    static void ProfileSkinnedMesh()
    {
        using SkinnedMesh skinnedMesh = SkinnedMesh.ReadFromSimpleSkin("akali.skn");
        RigResource skeleton = new(File.OpenRead("akali.skl"));

        List<(string name, IAnimationAsset animation)> animations = new();
        foreach (string animationFile in Directory.EnumerateFiles("animations"))
        {
            using FileStream stream = File.OpenRead(animationFile);
            IAnimationAsset animation = AnimationAsset.Load(stream);

            animations.Add((Path.GetFileNameWithoutExtension(animationFile), animation));
        }

        skinnedMesh.ToGltf(new List<(string, Stream)>()).WriteGLB(File.OpenWrite("akali.glb"));
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
    }

    static void ProfileMapgeo(string toRead, string rewriteTo)
    {
        using EnvironmentAsset mgeo = new(toRead);
        //mgeo.ToGLTF().WriteGLB(File.OpenWrite("instanced.glb"));
        //mgeo.Write(Path.ChangeExtension(rewriteTo, "instanced.mapgeo"), 13);

        MapGeometryBuilder mapBuilder = new MapGeometryBuilder()
            .WithSceneGraph(mgeo.SceneGraph)
            .WithBakedTerrainSamplers(mgeo.BakedTerrainSamplers);

        Dictionary<ElementName, int> elementOrder =
            new() { { ElementName.DiffuseUV, 0 }, { ElementName.Normal, 1 }, { ElementName.Position, 2 } };

        foreach (EnvironmentAssetMesh mesh in mgeo.Meshes)
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
                            new MeshPrimitiveBuilder(
                                EnvironmentAssetMeshPrimitive.MISSING_MATERIAL,
                                submesh.StartIndex,
                                submesh.IndexCount
                            )
                    ),
                    vertexBuffer,
                    indexBuffer
                );

            mapBuilder.WithMesh(meshBuilder);
        }

        using EnvironmentAsset builtMap = mapBuilder.Build();
        builtMap.Write(rewriteTo, 13);

        static void RewriteVertexBuffer(EnvironmentAssetMesh mesh, VertexBufferWriter writer)
        {
            bool hasPositions = mesh.VerticesView.TryGetAccessor(ElementName.Position, out var positionAccessor);
            bool hasNormals = mesh.VerticesView.TryGetAccessor(ElementName.Normal, out var normalAccessor);
            bool hasBaseColor = mesh.VerticesView.TryGetAccessor(ElementName.PrimaryColor, out var baseColorAccessor);
            bool hasDiffuseUvs = mesh.VerticesView.TryGetAccessor(ElementName.DiffuseUV, out var diffuseUvAccessor);
            bool hasLightmapUvs = mesh.VerticesView.TryGetAccessor(ElementName.LightmapUV, out var lightmapUvAccessor);

            if (hasPositions is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh: {mesh.Name} does not have vertex positions");

            VertexElementArray<Vector3> positionsArray = positionAccessor.AsVector3Array();
            VertexElementArray<Vector3> normalsArray = hasNormals ? normalAccessor.AsVector3Array() : new();
            var baseColorArray = hasBaseColor ? baseColorAccessor.AsBgraU8Array() : new();
            VertexElementArray<Vector2> diffuseUvsArray = hasDiffuseUvs ? diffuseUvAccessor.AsVector2Array() : new();
            VertexElementArray<Vector2> lightmapUvsArray = hasLightmapUvs ? lightmapUvAccessor.AsVector2Array() : new();

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

    static IEnumerable<(string, IAnimationAsset)> LoadAnimations(string path)
    {
        foreach (string animationFile in Directory.EnumerateFiles(path))
        {
            using FileStream stream = File.OpenRead(animationFile);
            IAnimationAsset animation = AnimationAsset.Load(stream);

            yield return (Path.GetFileNameWithoutExtension(animationFile), animation);
        }
    }
}
