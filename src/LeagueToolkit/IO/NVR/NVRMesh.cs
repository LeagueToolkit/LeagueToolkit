using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.OBJ;
using System.Numerics;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Core.Primitives;

namespace LeagueToolkit.IO.NVR
{
    public class NVRMesh
    {
        public NVRMeshQuality QualityLevel { get; set; }
        public int Flag { get; private set; }
        public Sphere BoundingSphere { get; private set; }
        public Box BoundingBox { get; private set; }
        public NVRMaterial Material { get; private set; }
        public NVRDrawIndexedPrimitive[] IndexedPrimitives { get; private set; } = new NVRDrawIndexedPrimitive[2];

        //Used for writing
        public int MaterialIndex;

        public NVRMesh(BinaryReader br, NVRBuffers buffers, bool readOld)
        {
            this.QualityLevel = (NVRMeshQuality)br.ReadInt32();
            if (!readOld)
            {
                this.Flag = br.ReadInt32();
            }
            this.BoundingSphere = br.ReadSphere();
            this.BoundingBox = br.ReadBox();
            this.Material = buffers.Materials[br.ReadInt32()];
            this.IndexedPrimitives[0] = new NVRDrawIndexedPrimitive(br, buffers, this, true);
            this.IndexedPrimitives[1] = new NVRDrawIndexedPrimitive(br, buffers, this, false);
        }

        public NVRMesh(
            NVRMeshQuality meshQualityLevel,
            int flag,
            NVRMaterial material,
            List<NVRVertex> vertices,
            List<int> indices
        )
        {
            this.QualityLevel = meshQualityLevel;
            this.Flag = flag;
            this.Material = material;
            this.IndexedPrimitives[0] = new NVRDrawIndexedPrimitive(this, vertices, indices, true);
            this.IndexedPrimitives[1] = new NVRDrawIndexedPrimitive(this, vertices, indices, false);

            float[] min = new float[3] { vertices[0].Position.X, vertices[0].Position.Y, vertices[0].Position.Z };
            float[] max = new float[3] { vertices[0].Position.X, vertices[0].Position.Y, vertices[0].Position.Z };
            for (int i = 1; i < vertices.Count; i++)
            {
                Vector3 position = vertices[i].Position;
                if (position.X < min[0])
                {
                    min[0] = position.X;
                }
                if (position.Y < min[1])
                {
                    min[1] = position.Y;
                }
                if (position.Z < min[2])
                {
                    min[2] = position.Z;
                }
                if (position.X > max[0])
                {
                    max[0] = position.X;
                }
                if (position.Y > max[1])
                {
                    max[1] = position.Y;
                }
                if (position.Z > max[2])
                {
                    max[2] = position.Z;
                }
            }
            this.BoundingBox = new Box(new Vector3(min[0], min[1], min[2]), new Vector3(max[0], max[1], max[2]));

            float radius = max[0] - min[0];
            if (max[1] - min[1] > radius)
            {
                radius = max[1] - min[1];
            }
            if (max[2] - min[2] > radius)
            {
                radius = max[2] - min[2];
            }
            this.BoundingSphere = new(
                new Vector3((min[0] + max[0]) / 2, (min[1] + max[1]) / 2, (min[2] + max[2]) / 2),
                radius / 2
            );
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((int)this.QualityLevel);
            bw.Write(this.Flag);
            bw.WriteSphere(this.BoundingSphere);
            bw.WriteBox(this.BoundingBox);
            bw.Write(this.MaterialIndex);
            this.IndexedPrimitives[0].Write(bw);
            this.IndexedPrimitives[1].Write(bw);
        }

        public static Tuple<List<NVRVertex>, List<int>> GetGeometryFromOBJ(OBJFile objFile)
        {
            List<NVRVertex> vertices = new List<NVRVertex>();
            List<int> indices = new List<int>();

            Vector2 UV = new Vector2(0, 0);
            Vector3 normal = new Vector3(0, 0, 0);
            Color diffuseColor = new Color(0, 0, 0, 255);
            Color emissiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);

            foreach (Vector3 vert in objFile.Vertices)
            {
                vertices.Add(new NVRVertex8(vert, normal, UV, diffuseColor, emissiveColor));
            }

            foreach (OBJGroup group in objFile.Groups)
            {
                for (int faceIndex = 0; faceIndex < group.Faces.Count; faceIndex++)
                {
                    OBJFace face = group.Faces[faceIndex];
                    for (int j = 0; j < 3; j++)
                    {
                        int vertexIndex = (int)face.VertexIndices[j];
                        if (vertexIndex > vertices.Count)
                        {
                            vertexIndex = vertices.Count;
                        }
                        indices.Add(vertexIndex);
                    }
                }
            }

            Console.WriteLine("Faces: " + indices.Count / 3);

            return new Tuple<List<NVRVertex>, List<int>>(vertices, indices);
        }

        private static Vector3 CalcNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            // Calculate two vectors from the three points
            Vector3 vector1 = new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
            Vector3 vector2 = new Vector3(v2.X - v3.X, v2.Y - v3.Y, v2.Z - v3.Z);

            // Take the cross product of the two vectors to get
            // the normal vector which will be stored in out
            Vector3 norm = new Vector3(
                (v1.Y * v2.Z) - (v1.Z * v2.Y),
                (v1.Z * v2.X) - (v1.X * v2.Z),
                (v1.X * v2.Y) - (v1.Y * v2.X)
            );
            return norm;
        }
    }

    public enum NVRMeshQuality : int
    {
        VERY_LOW = -100,

        // -1 should mean something
        LOW = 0,
        MEDIUM = 1,
        HIGH = 2,
        VERY_HIGH = 3
    }
}
