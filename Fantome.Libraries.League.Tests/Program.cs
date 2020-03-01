using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.MapGeometry;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.ReleaseManifest;
using Fantome.Libraries.League.IO.SimpleSkin;
using Fantome.Libraries.League.IO.StaticObject;
using Fantome.Libraries.League.IO.WorldGeometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fantome.Libraries.League.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMapgeo();
        }

        static void TestMapgeo()
        {
            MGEOFile mgeo = new MGEOFile("base_srx.mapgeo");

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

                (List<ushort> indices, List<MGEOVertex> vertices) = obj.GetMGEOData();

                R3DMatrix44 transformation = R3DMatrix44.FromTranslation(new Vector3(0, 50, 100));

                MGEOSubmesh submesh = new MGEOSubmesh("", 0, (uint)indices.Count, 0, (uint)vertices.Count);
                MGEOModel model1 = new MGEOModel(name, vertices, indices, new List<MGEOSubmesh>() { submesh }, MGEOLayer.AllLayers);

                mgeo.AddModel(model1);
            }
        }

        static void TestWGEO()
        {
            WGEOFile wgeo = new WGEOFile("room.wgeo");
            Directory.CreateDirectory("kek");

            for(int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    WGEOBucket bucket = wgeo.BucketGeometry.Buckets[i, j];

                    List<uint> indices = wgeo.BucketGeometry.Indices
                        .GetRange((int)bucket.StartIndex,(bucket.InsideFaceCount + bucket.StickingOutFaceCount) * 3)
                        .Select(x => (uint)x)
                        .ToList();

                    if(indices.Count != 0)
                    {
                        int startVertex = (int)indices.Min();
                        int vertexCount = (int)indices.Max() - startVertex;
                        List<Vector3> vertices = wgeo.BucketGeometry.Vertices.GetRange(startVertex + (int)bucket.Vertex, vertexCount);

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
