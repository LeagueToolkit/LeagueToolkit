using Fantome.Libraries.League.Converters;
using Fantome.Libraries.League.Helpers.Structures.BucketGrid;
using Fantome.Libraries.League.IO.AnimationFile;
using Fantome.Libraries.League.IO.PropertyBin;
using Fantome.Libraries.League.IO.MapGeometry;
using Fantome.Libraries.League.IO.NavigationGridOverlay;
using Fantome.Libraries.League.IO.NVR;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.ReleaseManifestFile;
using Fantome.Libraries.League.IO.SimpleSkinFile;
using Fantome.Libraries.League.IO.SkeletonFile;
using Fantome.Libraries.League.IO.StaticObjectFile;
using Fantome.Libraries.League.IO.WadFile;
using Fantome.Libraries.League.IO.WGT;
using Fantome.Libraries.League.IO.WorldGeometry;
using ImageMagick;
using Newtonsoft.Json;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeagueAnimation = Fantome.Libraries.League.IO.AnimationFile.Animation;
using Fantome.Libraries.League.Meta;
using Fantome.Libraries.League.Meta.Attributes;
using System.Numerics;

namespace Fantome.Libraries.League.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            BinTree binTree = new BinTree(@"C:\Users\Crauzer\Desktop\New folder\data\characters\aatrox\skins\skin0.bin");
            MetaEnvironment environment = MetaEnvironment.Create(new List<Type>() 
            {
                typeof(SkinCharacterDataProperties),
                typeof(CensoredImage),
                typeof(SkinAudioProperties),
                typeof(BankUnit),
                typeof(SkinMeshDataProperties)
            });

            var scdp = MetaSerializer.Deserialize<SkinCharacterDataProperties>(environment, binTree.Objects[0]);
        }

        static void TestMapgeo()
        {
            MapGeometry mgeo = new MapGeometry(@"C:/Users/Crauzer/Desktop/data/maps/mapgeometry/sr/base_srx.mapgeo");

            string randomMaterialName = mgeo.Models[180].Submeshes[0].Material;

            mgeo.Models.Clear();

            OBJFile object1 = new OBJFile("room155.obj");
            OBJFile object2 = new OBJFile("room156.obj");
            OBJFile object3 = new OBJFile("room157.obj");

            AddOBJ(object1, "MapGeo_Instance_0");
            AddOBJ(object2, "MapGeo_Instance_1");
            AddOBJ(object3, "MapGeo_Instance_2");

            mgeo.Write("base_srx.mapgeo.edited", 7);

            void AddOBJ(OBJFile obj, string name)
            {
                //We will add each object 2 times just for fun to see how transformation works

                (List<ushort> indices, List<MapGeometryVertex> vertices) = obj.GetMGEOData();

                Matrix4x4 transformation = Matrix4x4.CreateTranslation(new Vector3(0, 50, 100));

                MapGeometrySubmesh submesh = new MapGeometrySubmesh("", 0, (uint)indices.Count, 0, (uint)vertices.Count);
                MapGeometryModel model1 = new MapGeometryModel(name, vertices, indices, new List<MapGeometrySubmesh>() { submesh }, MapGeometryLayer.AllLayers);

                mgeo.AddModel(model1);
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
                        List<Vector3> vertices = wgeo.BucketGrid.Vertices.GetRange(startVertex + (int)bucket.BaseVertex, vertexCount);

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

    [MetaClass("SkinCharacterDataProperties")]
    public class SkinCharacterDataProperties : IMetaClass
    {
        [MetaProperty("skinClassification", BinPropertyType.UInt32)] public uint SkinClassification { get; set; }
        [MetaProperty("championSkinName", BinPropertyType.String)] public string ChampionSkinName { get; set; }
        [MetaProperty("loadscreen", BinPropertyType.Embedded)] public CensoredImage Loadscreen { get; set; }
        [MetaProperty("skinAudioProperties", BinPropertyType.Embedded)] public SkinAudioProperties SkinAudioProperties { get; set; }
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)] public SkinMeshDataProperties SkinMeshProperties { get; set; }

    }

    [MetaClass("CensoredImage")]
    public class CensoredImage : IMetaClass
    {
        [MetaProperty("image", BinPropertyType.String)] public string Image { get; set; }
    }
    [MetaClass("skinAudioProperties")]
    public class SkinAudioProperties : IMetaClass
    {
        [MetaProperty("tagEventList", BinPropertyType.Container)] public List<string> TagEventList { get; set; }
        [MetaProperty("bankUnits", BinPropertyType.Container)] public List<BankUnit> BankUnits { get; set; }
    }
    [MetaClass("BankUnit")]
    public class BankUnit
    {
        [MetaProperty("name", BinPropertyType.String)] public string Name { get; set; }
        [MetaProperty("bankPath", BinPropertyType.Container)] public List<string> BankPath { get; set; }
        [MetaProperty("events", BinPropertyType.Container)] public List<string> Events { get; set; }
    }
    [MetaClass("SkinMeshDataProperties")]
    public class SkinMeshDataProperties
    {
        [MetaProperty("overrideBoundingBox", BinPropertyType.Optional)] public Vector3? OverrideBoundingBox { get; set; }
    }
}
