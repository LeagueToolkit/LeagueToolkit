using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace Fantome.Libraries.League.IO.OBJ
{
    public class OBJFile
    {
        public List<string> Comments { get; private set; } = new List<string>();
        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public List<Vector2> UVs { get; private set; } = new List<Vector2>();
        public List<Vector3> Normals { get; private set; } = new List<Vector3>();
        public List<OBJFace> Faces { get; private set; } = new List<OBJFace>();

        public OBJFile(List<Vector3> vertices, List<uint> indices)
        {
            this.Vertices = vertices;
            for (int i = 0; i < indices.Count; i += 3)
            {
                uint[] faceIndices = new uint[] { indices[i], indices[i + 1], indices[i + 2] };
                this.Faces.Add(new OBJFace(faceIndices));
            }
        }

        public OBJFile(List<Vector3> vertices, List<uint> indices, List<Vector2> uvs)
        {
            this.Vertices = vertices;
            this.UVs = uvs;
            for (int i = 0; i < indices.Count; i += 3)
            {
                uint[] faceIndices = new uint[] { indices[i], indices[i + 1], indices[i + 2] };
                this.Faces.Add(new OBJFace(faceIndices, faceIndices));
            }
        }

        public OBJFile(List<Vector3> vertices, List<uint> indices, List<Vector2> uvs, List<Vector3> normals)
        {
            this.Vertices = vertices;
            this.UVs = uvs;
            this.Normals = normals;
            for (int i = 0; i < indices.Count; i += 3)
            {
                uint[] faceIndices = new uint[] { indices[i], indices[i + 1], indices[i + 2] };
                this.Faces.Add(new OBJFace(faceIndices, faceIndices, faceIndices));
            }
        }


        public OBJFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public OBJFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    this.ReadLine(sr);
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        public void Write(Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (string comment in this.Comments)
                {
                    sw.WriteLine("#" + comment);
                }
                foreach (Vector3 vertex in this.Vertices)
                {
                    sw.WriteLine(string.Format("v {0} {1} {2}", vertex.X, vertex.Y, vertex.Z));
                }
                foreach (Vector2 uv in this.UVs)
                {
                    sw.WriteLine(string.Format("vt {0} {1}", uv.X, 1 - uv.Y));
                }
                foreach (Vector3 normal in this.Normals)
                {
                    sw.WriteLine(string.Format("vn {0} {1} {2}", normal.X, normal.Y, normal.Z));
                }
                foreach (OBJFace face in this.Faces)
                {
                    face.Write(sw);
                }
            }
        }

        private void ReadLine(StreamReader sr)
        {
            string[] input = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (input.Length == 0)
            {
                return;
            }
            if (input[0] == "#")
            {
                this.Comments.Add(String.Join(" ", input).Remove(0, 1));
            }
            else if (input[0] == "v")
            {
                this.Vertices.Add(new Vector3(float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[3], CultureInfo.InvariantCulture.NumberFormat)));
            }
            else if (input[0] == "vt")
            {
                this.UVs.Add(new Vector2(float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat),
                    1 - float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat)));
            }
            else if (input[0] == "vn")
            {
                this.Normals.Add(new Vector3(float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[3], CultureInfo.InvariantCulture.NumberFormat)));
            }
            else if (input[0] == "f")
            {
                string[] vertex1 = input[1].Split('/');
                string[] vertex2 = input[2].Split('/');
                string[] vertex3 = input[3].Split('/');

                if (vertex1.Length == 1)
                {
                    this.Faces.Add(new OBJFace(
                        new uint[]
                        {
                            (uint.Parse(vertex1[0]) - 1),
                            (uint.Parse(vertex2[0]) - 1),
                            (uint.Parse(vertex3[0]) - 1)
                        }));
                }
                else if (vertex1.Length == 2)
                {
                    this.Faces.Add(new OBJFace(
                        new uint[]
                        {
                            (uint.Parse(vertex1[0]) - 1),
                            (uint.Parse(vertex2[0]) - 1),
                            (uint.Parse(vertex3[0]) - 1)
                        },
                        new uint[]
                        {
                            (uint.Parse(vertex1[1]) - 1),
                            (uint.Parse(vertex2[1]) - 1),
                            (uint.Parse(vertex3[1]) - 1)
                        }));
                }
                else if (vertex1.Length == 3)
                {
                    this.Faces.Add(new OBJFace(
                        new uint[]
                        {
                             (uint.Parse(vertex1[0]) - 1),
                             (uint.Parse(vertex2[0]) - 1),
                             (uint.Parse(vertex3[0]) - 1)
                        },
                        new uint[]
                        {
                            (uint.Parse(vertex1[1]) - 1),
                            (uint.Parse(vertex2[1]) - 1),
                            (uint.Parse(vertex3[1]) - 1)
                        },
                        new uint[]
                        {
                            (uint.Parse(vertex1[2]) - 1),
                            (uint.Parse(vertex2[2]) - 1),
                            (uint.Parse(vertex3[2]) - 1)
                        }));
                }
            }
        }
    }
}
