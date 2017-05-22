using Fantome.League.Helpers.Structures;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace Fantome.League.IO.OBJ
{
    public class OBJFile
    {
        public List<string> Comments { get; private set; } = new List<string>();
        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public List<Vector2> UVs { get; private set; } = new List<Vector2>();
        public List<Vector3> Normals { get; private set; } = new List<Vector3>();
        public List<Face> Faces { get; private set; } = new List<Face>();

        public OBJFile(List<Vector3> Vertices, List<UInt16> Indices)
        {
            this.Vertices = Vertices;
            for (int i = 0; i < Indices.Count; i += 3)
            {
                UInt16[] FaceIndices = new UInt16[] { Indices[i], Indices[i + 1], Indices[i + 2] };
                this.Faces.Add(new Face(FaceIndices));
            }
        }

        public OBJFile(List<Vector3> Vertices, List<Vector2> UVs, List<UInt16> Indices)
        {
            this.Vertices = Vertices;
            this.UVs = UVs;
            for (int i = 0; i < Indices.Count; i += 3)
            {
                UInt16[] FaceIndices = new UInt16[] { Indices[i], Indices[i + 1], Indices[i + 2] };
                this.Faces.Add(new Face(FaceIndices, FaceIndices));
            }
        }

        public OBJFile(List<Vector3> Vertices, List<Vector2> UVs, List<Vector3> Normals, List<UInt16> Indices)
        {
            this.Vertices = Vertices;
            this.UVs = UVs;
            this.Normals = Normals;
            for (int i = 0; i < Indices.Count; i += 3)
            {
                UInt16[] FaceIndices = new UInt16[] { Indices[i], Indices[i + 1], Indices[i + 2] };
                this.Faces.Add(new Face(FaceIndices, FaceIndices, FaceIndices));
            }
        }

        public OBJFile(string Location)
        {
            using (StreamReader sr = new StreamReader(Location))
            {
                while (sr.BaseStream.Position != sr.BaseStream.Length)
                {
                    this.ReadLine(sr);
                }
            }
        }

        public void Write(string Location)
        {
            using (StreamWriter sw = new StreamWriter(Location))
            {
                foreach (string Comment in this.Comments)
                {
                    sw.WriteLine("#" + Comment);
                }
                foreach (Vector3 Vertex in this.Vertices)
                {
                    sw.WriteLine(string.Format("v {0} {1} {2}", Vertex.X, Vertex.Y, Vertex.Z));
                }
                foreach (Vector2 UV in this.UVs)
                {
                    sw.WriteLine(string.Format("vt {0} {1}", UV.X, 1 - UV.Y));
                }
                foreach (Vector3 Normal in this.Normals)
                {
                    sw.WriteLine(string.Format("vn {0} {1} {2}", Normal.X, Normal.Y, Normal.Z));
                }
                foreach (Face Face in this.Faces)
                {
                    Face.Write(sw);
                }
            }
        }

        private void ReadLine(StreamReader sr)
        {
            string[] Input = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (Input.Length == 0)
            {
                return;
            }
            if (Input[0] == "#")
            {
                this.Comments.Add(String.Join(" ", Input).Remove(0, 1));
            }
            else if (Input[0] == "v")
            {
                this.Vertices.Add(new Vector3
                    (
                        float.Parse(Input[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(Input[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(Input[3], CultureInfo.InvariantCulture.NumberFormat)
                    )
                    );
            }
            else if (Input[0] == "vt")
            {
                this.UVs.Add(new Vector2
                    (
                        float.Parse(Input[1], CultureInfo.InvariantCulture.NumberFormat),
                        1 - float.Parse(Input[2], CultureInfo.InvariantCulture.NumberFormat)
                    )
                    );
            }
            else if (Input[0] == "vn")
            {
                this.Normals.Add(new Vector3
                    (
                        float.Parse(Input[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(Input[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(Input[3], CultureInfo.InvariantCulture.NumberFormat)
                    )
                    );
            }
            else if (Input[0] == "f")
            {
                string[] Vertex1 = Input[1].Split('/');
                string[] Vertex2 = Input[2].Split('/');
                string[] Vertex3 = Input[3].Split('/');

                if (Vertex1.Length == 1)
                {
                    this.Faces.Add(new Face
                        (
                            new UInt16[]
                            {
                                (ushort)(UInt16.Parse(Vertex1[0])-1),
                                (ushort)(UInt16.Parse(Vertex2[0])-1),
                                (ushort)(UInt16.Parse(Vertex3[0])-1)
                            }
                        )
                        );
                }
                else if (Vertex1.Length == 2)
                {
                    this.Faces.Add(new Face
                        (
                            new UInt16[]
                            {
                                (ushort)(UInt16.Parse(Vertex1[0])-1),
                                (ushort)(UInt16.Parse(Vertex2[0])-1),
                                (ushort)(UInt16.Parse(Vertex3[0])-1)
                            },
                            new UInt16[]
                            {
                                (ushort)(UInt16.Parse(Vertex1[1])-1),
                                (ushort)(UInt16.Parse(Vertex2[1])-1),
                                (ushort)(UInt16.Parse(Vertex3[1])-1)
                            }
                        )
                        );
                }
                else if (Vertex1.Length == 3)
                {
                    this.Faces.Add(new Face
                        (
                            new UInt16[]
                            {
                                (ushort)(UInt16.Parse(Vertex1[0])-1),
                                (ushort)(UInt16.Parse(Vertex2[0])-1),
                                (ushort)(UInt16.Parse(Vertex3[0])-1)
                            },
                            new UInt16[]
                            {
                                (ushort)(UInt16.Parse(Vertex1[1])-1),
                                (ushort)(UInt16.Parse(Vertex2[1])-1),
                                (ushort)(UInt16.Parse(Vertex3[1])-1)
                            },
                            new UInt16[]
                            {
                                (ushort)(UInt16.Parse(Vertex1[2])-1),
                                (ushort)(UInt16.Parse(Vertex2[2])-1),
                                (ushort)(UInt16.Parse(Vertex3[2])-1)
                            }
                        )
                        );
                }
            }
        }
    }

    public class Face
    {
        private bool IsUVSet { get; set; }
        private bool IsNormalSet { get; set; }
        public UInt16[] VertexIndices { get; private set; }
        public UInt16[] UVIndices { get; private set; }
        public UInt16[] NormalIndices { get; private set; }

        public Face(UInt16[] VertexIndices)
        {
            this.VertexIndices = VertexIndices;
            this.IsUVSet = false;
            this.IsNormalSet = false;
            this.UVIndices = new UInt16[3];
            this.NormalIndices = new UInt16[3];
        }

        public Face(UInt16[] VertexIndices, UInt16[] UVIndices)
        {
            this.VertexIndices = VertexIndices;
            this.UVIndices = UVIndices;
            this.IsUVSet = true;
            this.IsNormalSet = false;
            this.NormalIndices = new UInt16[3];
        }

        public Face(UInt16[] VertexIndices, UInt16[] UVIndices, UInt16[] NormalIndices)
        {
            this.VertexIndices = VertexIndices;
            this.UVIndices = UVIndices;
            this.NormalIndices = NormalIndices;
            this.IsUVSet = true;
            this.IsNormalSet = true;
        }

        public void Write(StreamWriter sw)
        {
            if (this.IsUVSet && !this.IsNormalSet)
            {
                sw.WriteLine(string.Format(
                    "f {0}/{1} {2}/{3} {4}/{5}",
                    this.VertexIndices[0] + 1,
                    this.UVIndices[0] + 1,
                    this.VertexIndices[1] + 1,
                    this.UVIndices[1] + 1,
                    this.VertexIndices[2] + 1,
                    this.UVIndices[2] + 1
                    ));
            }
            else if (this.IsUVSet && this.IsNormalSet)
            {
                sw.WriteLine(string.Format(
                    "f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8}",
                    this.VertexIndices[0] + 1,
                    this.UVIndices[0] + 1,
                    this.NormalIndices[0] + 1,
                    this.VertexIndices[1] + 1,
                    this.UVIndices[1] + 1,
                    this.NormalIndices[1] + 1,
                    this.VertexIndices[2] + 1,
                    this.UVIndices[2] + 1,
                    this.NormalIndices[2] + 1
                    ));
            }
            else
            {
                sw.WriteLine(string.Format("f {0} {1} {2}", this.VertexIndices[0] + 1, this.VertexIndices[1] + 1, this.VertexIndices[2] + 1));
            }
        }
    }
}
