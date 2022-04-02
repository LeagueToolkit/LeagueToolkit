using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.OBJ
{
    public class OBJFile
    {
        public List<string> Comments { get; set; } = new List<string>();
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<Vector2> UVs { get; set; } = new List<Vector2>();
        public List<Vector3> Normals { get; set; } = new List<Vector3>();
        public List<OBJGroup> Groups { get; set; } = new List<OBJGroup>();
        public string MaterialsFile { get; set; } = "";
        public bool IsSmooth { get; set; } = false;

        public OBJFile(List<Vector3> vertices, List<uint> indices)
        {
            this.Vertices = vertices;
            this.Groups.Add(new OBJGroup("default", "defaultMat", indices));
        }

        public OBJFile(List<Vector3> vertices, List<uint> indices, List<Vector2> uvs)
        {
            this.Vertices = vertices;
            this.UVs = uvs;
            this.Groups.Add(new OBJGroup("default", "defaultMat", indices, indices));
        }

        public OBJFile(List<Vector3> vertices, List<uint> indices, List<Vector2> uvs, List<Vector3> normals)
        {
            this.Vertices = vertices;
            this.UVs = uvs;
            this.Normals = normals;
            this.Groups.Add(new OBJGroup("default", "defaultMat", indices, indices, indices));
        }

        public OBJFile(List<Vector3> vertices, List<OBJGroup> groups, List<Vector2> uvs, List<Vector3> normals, string mtlFile = "")
        {
            MaterialsFile = mtlFile;
            this.Vertices = vertices;
            this.UVs = uvs;
            this.Normals = normals;
            this.Groups.AddRange(groups);
        }

        public OBJFile(List<OBJFile> objs)
        {
            uint indicesOffset = 0;
            foreach (OBJFile obj in objs)
            {
                this.Comments.AddRange(obj.Comments);
                this.Vertices.AddRange(obj.Vertices);
                this.UVs.AddRange(obj.UVs);
                this.Normals.AddRange(obj.Normals);

                for (int i = 0; i < obj.Groups.Count; i++)
                {
                    OBJGroup group = obj.Groups[i];

                    if (group.Faces.Count <= 0)
                    {
                        continue;
                    }

                    int mainGroupIndex = this.Groups.FindIndex(g => g.Name == group.Name);
                    List<uint> tempIndices = new();

                    // Append vertex indices
                    foreach (OBJFace face in group.Faces)
                    {
                        foreach (uint indice in face.VertexIndices)
                        {
                            tempIndices.Add(indice + indicesOffset);
                        }
                    }

                    // No groups found
                    if (mainGroupIndex == -1)
                    {
                        this.Groups.Add(new OBJGroup(group.Name, group.Material, tempIndices, tempIndices, tempIndices));
                    }
                    else
                    {
                        this.Groups[mainGroupIndex].Faces.AddRange(new OBJGroup(group.Name, group.Material, tempIndices, tempIndices, tempIndices).Faces);
                    }
                }

                indicesOffset += (uint)obj.Vertices.Count;

                this.MaterialsFile = obj.MaterialsFile;
            }
        }

        public OBJFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Splits this OBJFile's groups into individual OBJFile(s).
        /// </summary>
        /// <returns></returns>
        public List<OBJFile> SplitGroups()
        {
            var returnList = new List<OBJFile>();

            foreach (OBJGroup group in Groups)
            {
                var vertices = new List<Vector3>();
                var indices = new List<uint>();
                var uvs = new List<Vector2>();
                var normals = new List<Vector3>();

                foreach (OBJFace face in group.Faces)
                {
                    var indexOffset = face.VertexIndices.Min();

                    // Pray that you don't end up with indices higher than int
                    for (int i = 0; i < face.VertexIndices.Length; i++)
                    {
                        vertices.Add(Vertices[(int)face.VertexIndices[i]]);
                        indices.Add(face.VertexIndices[i] - indexOffset);
                    }
                    for (int i = 0; i < face.UVIndices.Length; i++)
                    {
                        uvs.Add(UVs[(int)face.UVIndices[i]]);
                    }
                    for (int i = 0; i < face.NormalIndices.Length; i++)
                    {
                        normals.Add(Normals[(int)face.NormalIndices[i]]);
                    }
                }

                List<OBJGroup> groups = new();
                groups.Add(new OBJGroup(group.Name, group.Material, new List<uint>(indices)));

                returnList.Add(new OBJFile(vertices, groups, uvs, normals));
            }

            return returnList;
        }

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

        public void Write(Stream stream, bool leaveOpen = false)
        {
            using (StreamWriter sw = new StreamWriter(stream, Encoding.ASCII, 1024, leaveOpen))
            {
                foreach (string comment in this.Comments)
                {
                    sw.WriteLine("#" + comment);
                }
                if (!string.IsNullOrEmpty(this.MaterialsFile))
                {
                    sw.WriteLine("mtllib " + this.MaterialsFile);
                }
                foreach (Vector3 vertex in this.Vertices)
                {
                    sw.WriteLine(string.Format("v {0} {1} {2}", vertex.X, vertex.Y, vertex.Z));
                }
                foreach (Vector2 uv in this.UVs)
                {
                    string format = string.Format("vt {0} {1}", uv.X, 1 - uv.Y);

                    // Fixes issues with programs which cannot read the infinity symbol.
                    if (format.Contains("∞"))
                    {
                        format = "vt -Infinity NaN";
                    }

                    sw.WriteLine(format);
                }
                foreach (Vector3 normal in this.Normals)
                {
                    sw.WriteLine(string.Format("vn {0} {1} {2}", normal.X, normal.Y, normal.Z));
                }
                sw.WriteLine("s " + (this.IsSmooth ? "on" : "off"));
                foreach (OBJGroup group in this.Groups)
                {
                    group.Write(sw);
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
                this.Comments.Add(string.Join(" ", input).Remove(0, 1));
            }
            else if (input[0] == "mtllib")
            {
                this.MaterialsFile = input[1];
            }
            else if (input[0] == "g")
            {
                this.Groups.Add(new OBJGroup(input[1], "defaultMat", new List<OBJFace>()));
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
            else if (input[0] == "usemtl")
            {
                if (this.Groups.Count > 0)
                {
                    this.Groups[this.Groups.Count - 1].Material = input[1];
                }
                else
                {
                    this.Groups.Add(new OBJGroup("default", input[1], new List<OBJFace>()));
                }
            }
            else if (input[0] == "f")
            {
                string[] vertex1 = input[1].Split('/');
                string[] vertex2 = input[2].Split('/');
                string[] vertex3 = input[3].Split('/');

                OBJFace face = null;

                if (vertex1.Length == 1)
                {
                    face = new OBJFace(
                        new uint[]
                        {
                            (uint.Parse(vertex1[0]) - 1),
                            (uint.Parse(vertex2[0]) - 1),
                            (uint.Parse(vertex3[0]) - 1)
                        });
                }
                else if (vertex1.Length == 2)
                {
                    face = new OBJFace(
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
                        });
                }
                else if (vertex1.Length == 3)
                {
                    face = new OBJFace(
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
                        });
                }

                if (face != null)
                {
                    if (this.Groups.Count > 0)
                    {
                        this.Groups[this.Groups.Count - 1].Faces.Add(face);
                    }
                    else
                    {
                        this.Groups.Add(new OBJGroup("default", "defaultMat", new List<OBJFace> { face }));
                    }
                }
            }
        }
    }
}
