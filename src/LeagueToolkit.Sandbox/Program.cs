using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using BCnEncoder.Shared;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Environment.Builder;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.IO.MapObjects;
using LeagueToolkit.IO.SimpleSkinFile;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using LeagueToolkit.Meta.Dump;
using LeagueToolkit.Toolkit;
using LeagueToolkit.Toolkit.Gltf;
using LeagueToolkit.Toolkit.Ritobin;
using LeagueToolkit.Utils;
using SharpGLTF.Schema2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LeagueTexture = LeagueToolkit.Core.Renderer.Texture;
using MetaClass = LeagueToolkit.Meta.Classes;
using RigResource = LeagueToolkit.Core.Animation.RigResource;

namespace LeagueToolkit.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        var texture = LeagueTexture.Load(
            File.OpenRead(
                @"C:\lol\new-map11\assets\maps\kitpieces\srs\boba\textures\boba_chaos_bot_b_1bitalpha.boba_env.tex"
            )
        );

        texture.Mips[0].ToImage().SaveAsPng("boba_chaos_bot_b_1bitalpha.boba_env.png");

        ProfileMapgeoToGltf();
    }

    static void ProfileMetaSerializer()
    {
        using FileStream animationsBinStream = File.OpenRead(
            @"X:\lol\game\data\characters\akali\animations\skin0.bin"
        );
        BinTree bin = new(animationsBinStream);

        BinTreeObject animationGraphDataObject = bin
            .Objects.FirstOrDefault(x =>
                x.Value.ClassHash == Fnv1a.HashLower(nameof(AnimationGraphData))
            )
            .Value;

        MetaEnvironment metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").GetExportedTypes().Where(x => x.IsClass)
        );

        var kekek = MetaSerializer.Deserialize<AnimationGraphData>(
            metaEnvironment,
            animationGraphDataObject
        );
    }

    static void ProfileRitobinWriter()
    {
        using FileStream binFile = File.OpenRead(
            @"X:\lol\game\data\maps\mapgeometry\sr\worlds_trophyonly.materials.bin"
        );
        BinTree bin = new(binFile);

        using RitobinWriter writer =
            new(
                new Dictionary<uint, string>(),
                new Dictionary<uint, string>(),
                new Dictionary<uint, string>(),
                new Dictionary<uint, string>(),
                new Dictionary<ulong, string>()
            );

        File.WriteAllText("ritobintest.txt", writer.WritePropertyBin(bin));
    }

    static void ProfileNvrToEnvironmentAsset()
    {
        using FileStream stream = File.OpenRead(@"X:\lol\old_backup\Map10\scene\room.nvr");
        var nvr = SimpleEnvironment.Load(stream);

        using FileStream writeStream = File.Create("TT_room.nvr.mapgeo");
        nvr.Write(writeStream);

        MetaEnvironment metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").GetExportedTypes().Where(x => x.IsClass)
        );

        nvr.ToGltf(
                null,
                new(
                    metaEnvironment,
                    new()
                    {
                        GameDataPath = @"X:\lol\old_backup\Map10\scene\Textures",
                        FlipAcrossX = false,
                        LayerGroupingPolicy = MapGeometryGltfLayerGroupingPolicy.Ignore,
                        TextureQuality = MapGeometryGltfTextureQuality.High
                    }
                )
            )
            .SaveGLB("twistedtreeline.glb");
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

        List<(string, Stream)> textures = skn
            .Ranges.Select(x =>
            {
                using FileStream textureFileStream = File.OpenRead(
                    @"X:\lol\game\assets\characters\renekton\skins\skin26\renekton_skin26_tx_cm.dds"
                );
                LeagueTexture texture = LeagueTexture.Load(textureFileStream);

                MemoryStream pngStream = new();
                texture.Mips[0].ToImage().SaveAsPng(pngStream);
                pngStream.Position = 0;

                return (x.Material, (Stream)pngStream);
            })
            .ToList();

        skn.ToGltf(rig, textures, animations).Save(gltfPath);
    }

    static void ProfileRigResource()
    {
        RigResource skeleton = new(File.OpenRead("Brand.skl"));
    }

    static void ProfileMapgeoToGltf()
    {
        using FileStream materialsBinStream = File.OpenRead(
            @"C:\lol\new-map11\data\maps\mapgeometry\map11\boba_srs.materials.bin"
        );
        BinTree materialsBin = new(materialsBinStream);

        using FileStream mapgeoStream = File.OpenRead(
            @"C:\lol\new-map11\data\maps\mapgeometry\map11\boba_srs.mapgeo"
        );
        using EnvironmentAsset mgeo = new(mapgeoStream);

        MetaEnvironment metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").GetExportedTypes().Where(x => x.IsClass)
        );

        mgeo.ToGltf(
                materialsBin,
                new(
                    metaEnvironment,
                    new()
                    {
                        GameDataPath = @"C:\lol\new-map11",
                        FlipAcrossX = true,
                        LayerGroupingPolicy = MapGeometryGltfLayerGroupingPolicy.Default,
                        TextureQuality = MapGeometryGltfTextureQuality.High
                    }
                )
            )
            .SaveGLB("boba_srs.glb");
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

    static async Task TestMetaRoslynCodegen(string metaJsonFile, string outputFile)
    {
        using HttpClient client = new();

        byte[] binTypesBuffer = await client.GetByteArrayAsync(
            "https://github.com/LeagueToolkit/LeagueHashes/raw/master/hashes/hashes.bintypes.txt"
        );
        byte[] binFieldsBuffer = await client.GetByteArrayAsync(
            "https://github.com/LeagueToolkit/LeagueHashes/raw/master/hashes/hashes.binfields.txt"
        );

        File.WriteAllBytes("hashes.bintypes.txt", binTypesBuffer);
        File.WriteAllBytes("hashes.binfields.txt", binFieldsBuffer);

        IEnumerable<string> classes = File.ReadLines("hashes.bintypes.txt")
            .Select(line => line.Split(' ')[1]);
        IEnumerable<string> properties = File.ReadLines("hashes.binfields.txt")
            .Select(line => line.Split(' ')[1]);

        MetaDump
            .Deserialize(File.ReadAllText(metaJsonFile))
            .WriteMetaClasses(outputFile, classes, properties);
    }

    static void ProfileMetaSerialization()
    {
        using FileStream materialsBinStream = File.OpenRead("base_srx.materials.bin");
        BinTree binTree = new(materialsBinStream);

        MetaEnvironment metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").GetExportedTypes().Where(x => x.IsClass)
        );
    }

    static void ProfileMapgeo(string toRead, string rewriteTo)
    {
        using FileStream mapgeoStream = File.OpenRead(toRead);
        using EnvironmentAsset mgeo = new(mapgeoStream);
        //mgeo.ToGLTF().WriteGLB(File.OpenWrite("instanced.glb"));
        //mgeo.Write(Path.ChangeExtension(rewriteTo, "instanced.mapgeo"), 13);

        EnvironmentAssetBuilder mapBuilder = new EnvironmentAssetBuilder();

        foreach (var samplerDef in mgeo.ShaderTextureOverrides)
            mapBuilder.WithSamplerDef(samplerDef);

        foreach (var sceneGraph in mgeo.SceneGraphs)
            mapBuilder.WithSceneGraph(sceneGraph);

        Dictionary<ElementName, int> elementOrder =
            new()
            {
                { ElementName.Texcoord0, 0 },
                { ElementName.Normal, 1 },
                { ElementName.Position, 2 }
            };

        foreach (EnvironmentAssetMesh mesh in mgeo.Meshes)
        {
            var (vertexBuffer, vertexBufferWriter) = mapBuilder.UseVertexBuffer(
                VertexBufferUsage.Static,
                mesh.VerticesView.Buffers.SelectMany(vertexBuffer =>
                        vertexBuffer.Description.Elements
                    )
                    .OrderBy(element => element.Name),
                mesh.VerticesView.VertexCount
            );
            var (indexBuffer, indexBufferWriter) = mapBuilder.UseIndexBuffer(mesh.Indices.Count);

            indexBufferWriter.Write(mesh.Indices.Buffer.Span.Cast<byte, ushort>());
            RewriteVertexBuffer(mesh, vertexBufferWriter);

            EnvironmentAssetMeshBuilder meshBuilder = new EnvironmentAssetMeshBuilder()
                .WithTransform(mesh.Transform)
                .WithDisableBackfaceCulling(mesh.DisableBackfaceCulling)
                .WithEnvironmentQualityFilter(mesh.EnvironmentQualityFilter)
                .WithVisibilityFlags(mesh.VisibilityFlags)
                .WithRenderFlags(mesh.RenderFlags)
                .WithStationaryLightSampler(mesh.StationaryLight)
                .WithBakedLightSampler(mesh.BakedLight)
                .WithTextureOverrides(
                    mesh.TextureOverrides,
                    mesh.BakedPaintScale,
                    mesh.BakedPaintBias
                )
                .WithGeometry(
                    mesh.Submeshes.Select(submesh => new MeshPrimitiveBuilder(
                        EnvironmentAssetMeshPrimitive.MISSING_MATERIAL,
                        submesh.StartIndex,
                        submesh.IndexCount
                    )),
                    vertexBuffer,
                    indexBuffer
                );

            mapBuilder.WithMesh(meshBuilder);
        }

        using EnvironmentAsset builtMap = mapBuilder.Build();

        using FileStream rewriteToStream = File.Create(rewriteTo);
        builtMap.Write(rewriteToStream);

        static void RewriteVertexBuffer(EnvironmentAssetMesh mesh, VertexBufferWriter writer)
        {
            bool hasPositions = mesh.VerticesView.TryGetAccessor(
                ElementName.Position,
                out var positionAccessor
            );
            bool hasNormals = mesh.VerticesView.TryGetAccessor(
                ElementName.Normal,
                out var normalAccessor
            );
            bool hasBaseColor = mesh.VerticesView.TryGetAccessor(
                ElementName.PrimaryColor,
                out var baseColorAccessor
            );
            bool hasDiffuseUvs = mesh.VerticesView.TryGetAccessor(
                ElementName.Texcoord0,
                out var diffuseUvAccessor
            );
            bool hasLightmapUvs = mesh.VerticesView.TryGetAccessor(
                ElementName.Texcoord7,
                out var lightmapUvAccessor
            );

            if (hasPositions is false)
                ThrowHelper.ThrowInvalidOperationException(
                    $"Mesh: {mesh.Name} does not have vertex positions"
                );

            VertexElementArray<Vector3> positionsArray = positionAccessor.AsVector3Array();
            VertexElementArray<Vector3> normalsArray = hasNormals
                ? normalAccessor.AsVector3Array()
                : new();
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
                    writer.WriteVector2(i, ElementName.Texcoord0, diffuseUvsArray[i]);
                if (hasLightmapUvs)
                    writer.WriteVector2(i, ElementName.Texcoord7, lightmapUvsArray[i]);
            }
        }
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
