using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Fantome.Libraries.League.IO.StaticObject
{
    public class StaticObject
    {
        public string Name { get; set; }
        public Vector3 PivotPoint { get; set; }
        public List<StaticObjectSubmesh> Submeshes { get; private set; }

        public StaticObject(List<StaticObjectSubmesh> submeshes) : this(string.Empty, submeshes) { }
        public StaticObject(string name, List<StaticObjectSubmesh> submeshes)
        {
            this.Name = name;
            this.Submeshes = submeshes;
        }
        public StaticObject(string name, List<StaticObjectSubmesh> submeshes, Vector3 pivotPoint) : this(name, submeshes)
        {
            this.PivotPoint = pivotPoint;
        }

        public static StaticObject ReadSCB(string fileLocation) => ReadSCB(File.OpenRead(fileLocation));
        public static StaticObject ReadSCB(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                if (magic != "r3d2Mesh")
                {
                    throw new Exception("This is not a valid SCB file");
                }

                ushort major = br.ReadUInt16();
                ushort minor = br.ReadUInt16();
                if (major != 3 && major != 2 && minor != 1) //There are versions [2][1] and [1][1] aswell
                {
                    throw new Exception(string.Format("The Version: {0}.{1} is not supported", major, minor));
                }

                string name = Encoding.ASCII.GetString(br.ReadBytes(128)).Replace("\0", "");
                uint vertexCount = br.ReadUInt32();
                uint faceCount = br.ReadUInt32();
                StaticObjectFlags flags = (StaticObjectFlags)br.ReadUInt32();
                R3DBox boundingBox = new R3DBox(br);

                bool hasVertexColors = false;
                if (major == 3 && minor == 2)
                {
                    hasVertexColors = br.ReadUInt32() == 1;
                }

                List<Vector3> vertices = new List<Vector3>((int)vertexCount);
                List<ColorRGBAVector4Byte> vertexColors = new List<ColorRGBAVector4Byte>((int)vertexCount);
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices.Add(new Vector3(br));
                }

                if (hasVertexColors)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        vertexColors.Add(new ColorRGBAVector4Byte(br));
                    }
                }

                Vector3 centralPoint = new Vector3(br);

                List<StaticObjectFace> faces = new List<StaticObjectFace>((int)faceCount);
                for (int i = 0; i < faceCount; i++)
                {
                    faces.Add(new StaticObjectFace(br));
                }

                return new StaticObject(name, CreateSubmeshes(vertices, vertexColors, faces), centralPoint);
            }
        }

        public static StaticObject ReadSCO(string fileLocation) => ReadSCO(File.OpenRead(fileLocation));
        public static StaticObject ReadSCO(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                char[] splittingArray = new char[] { ' ' };
                string[] input = null;

                if (sr.ReadLine() != "[ObjectBegin]")
                {
                    throw new Exception("Invalid signature");
                }

                input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
                string name = input.Length != 1 ? input[1] : string.Empty;

                input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
                Vector3 centralPoint = new Vector3(input[1], input[2], input[3]);
                Vector3 pivotPoint = centralPoint;

                bool hasVertexColors = false;

                input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
                if (input[0] == "PivotPoint=")
                {
                    pivotPoint = new Vector3(input[1], input[2], input[3]);
                }
                else if (input[0] == "VertexColors=")
                {
                    hasVertexColors = uint.Parse(input[1]) != 0;
                }

                int vertexCount = int.Parse(sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
                List<Vector3> vertices = new List<Vector3>(vertexCount);
                List<ColorRGBAVector4Byte> vertexColors = new List<ColorRGBAVector4Byte>(vertexCount);
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices.Add(new Vector3(sr));
                }

                if (hasVertexColors)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        vertexColors.Add(new ColorRGBAVector4Byte(sr));
                    }
                }

                int faceCount = int.Parse(sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
                List<StaticObjectFace> faces = new List<StaticObjectFace>(faceCount);
                for (int i = 0; i < faceCount; i++)
                {
                    faces.Add(new StaticObjectFace(sr));
                }

                return new StaticObject(name, CreateSubmeshes(vertices, vertexColors, faces), pivotPoint);
            }
        }

        private static List<StaticObjectSubmesh> CreateSubmeshes(List<Vector3> vertices, List<ColorRGBAVector4Byte> vertexColors, List<StaticObjectFace> faces)
        {
            bool hasVertexColors = vertexColors.Count != 0;
            Dictionary<string, List<StaticObjectFace>> submeshMap = CreateSubmeshMap(faces);
            List<StaticObjectSubmesh> submeshes = new List<StaticObjectSubmesh>();

            foreach (KeyValuePair<string, List<StaticObjectFace>> mappedSubmesh in submeshMap)
            {
                //Collect all indices and build UV map
                List<uint> indices = new List<uint>(mappedSubmesh.Value.Count * 3);
                Dictionary<uint, Vector2> uvMap = new Dictionary<uint, Vector2>(mappedSubmesh.Value.Count * 3);
                foreach (StaticObjectFace face in mappedSubmesh.Value)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        uint index = face.Indices[i];

                        indices.Add(index);

                        if (!uvMap.ContainsKey(index))
                        {
                            uvMap.Add(index, face.UVs[i]);
                        }
                    }
                }

                //Get Vertex range from indices
                uint minVertex = indices.Min();
                uint maxVertex = indices.Max();

                //Build vertex list
                uint vertexCount = maxVertex - minVertex;
                List<StaticObjectVertex> submeshVertices = new List<StaticObjectVertex>((int)vertexCount);
                for (uint i = minVertex; i < maxVertex + 1; i++)
                {
                    Vector2 uv = uvMap[i];
                    ColorRGBAVector4Byte color = hasVertexColors ? vertexColors[(int)i] : null;

                    submeshVertices.Add(new StaticObjectVertex(vertices[(int)i], uv, color));
                }

                //Normalize indices
                for (int i = 0; i < indices.Count; i++)
                {
                    indices[i] -= minVertex;
                }

                submeshes.Add(new StaticObjectSubmesh(mappedSubmesh.Key, submeshVertices, indices));
            }

            return submeshes;
        }
        private static Dictionary<string, List<StaticObjectFace>> CreateSubmeshMap(List<StaticObjectFace> faces)
        {
            Dictionary<string, List<StaticObjectFace>> submeshMap = new Dictionary<string, List<StaticObjectFace>>();

            foreach (StaticObjectFace face in faces)
            {
                if (!submeshMap.ContainsKey(face.Material))
                {
                    submeshMap.Add(face.Material, new List<StaticObjectFace>());
                }

                submeshMap[face.Material].Add(face);
            }

            return submeshMap;
        }

        public void WriteSCB(string fileLocation) => WriteSCB(File.Create(fileLocation));
        public void WriteSCB(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                List<StaticObjectVertex> vertices = GetVertices();
                List<StaticObjectFace> faces = new List<StaticObjectFace>();
                bool hasVertexColors = false;
                StaticObjectFlags flags = 0;

                foreach (StaticObjectSubmesh submesh in this.Submeshes)
                {
                    faces.AddRange(submesh.GetFaces());
                }

                foreach (StaticObjectVertex vertex in vertices)
                {
                    if (vertex.Color != null)
                    {
                        hasVertexColors = true;
                        break;
                    }
                }

                if (hasVertexColors)
                {
                    flags |= StaticObjectFlags.VERTEX_COLORS;
                }

                bw.Write(Encoding.ASCII.GetBytes("r3d2Mesh"));
                bw.Write((ushort)3);
                bw.Write((ushort)2);
                bw.Write(Encoding.ASCII.GetBytes(this.Name.PadRight(128, '\u0000')));
                bw.Write(vertices.Count);
                bw.Write(faces.Count);
                bw.Write((uint)flags);
                GetBoundingBox().Write(bw);
                bw.Write((uint)(flags & StaticObjectFlags.VERTEX_COLORS));

                vertices.ForEach(vertex => vertex.Position.Write(bw));

                if (hasVertexColors)
                {
                    foreach (StaticObjectVertex vertex in vertices)
                    {
                        if (vertex.Color != null)
                        {
                            vertex.Color.Write(bw);
                        }
                        else
                        {
                            new ColorRGBAVector4Byte(0, 0, 0, 255).Write(bw);
                        }
                    }
                }


                GetCentralPoint().Write(bw);
                faces.ForEach(face => face.Write(bw));
            }
        }

        public void WriteSCO(string fileLocation) => WriteSCO(File.Create(fileLocation));
        public void WriteSCO(Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                List<StaticObjectVertex> vertices = GetVertices();
                List<StaticObjectFace> faces = new List<StaticObjectFace>();
                bool hasVertexColors = false;

                foreach (StaticObjectSubmesh submesh in this.Submeshes)
                {
                    faces.AddRange(submesh.GetFaces());
                }

                foreach (StaticObjectVertex vertex in vertices)
                {
                    if (vertex.Color != null)
                    {
                        hasVertexColors = true;
                        break;
                    }
                }

                sw.WriteLine("[ObjectBegin]");
                sw.WriteLine("Name= " + this.Name);
                sw.WriteLine("CentralPoint= " + GetCentralPoint().ToString());

                if (this.PivotPoint != Vector3.Zero)
                {
                    sw.WriteLine("PivotPoint= " + this.PivotPoint.ToString());
                }
                if (hasVertexColors)
                {
                    sw.WriteLine("VertexColors= 1");
                }

                sw.WriteLine("Verts= " + vertices.Count);
                vertices.ForEach(vertex => vertex.Position.Write(sw, true));

                

                if (hasVertexColors)
                {
                    foreach (StaticObjectVertex vertex in vertices)
                    {
                        if (vertex.Color != null)
                        {
                            vertex.Color.Write(sw, true);
                        }
                        else
                        {
                            new ColorRGBAVector4Byte(0, 0, 0, 255).Write(sw, true);
                        }
                    }
                }


                sw.WriteLine("Faces= " + faces.Count);
                faces.ForEach(face => face.Write(sw));

                sw.WriteLine("[ObjectEnd]");
            }
        }

        public List<StaticObjectVertex> GetVertices()
        {
            List<StaticObjectVertex> vertices = new List<StaticObjectVertex>();

            foreach (StaticObjectSubmesh submesh in this.Submeshes)
            {
                vertices.AddRange(submesh.Vertices);
            }

            return vertices;
        }
        public List<uint> GetIndices()
        {
            List<uint> indices = new List<uint>();

            uint startIndex = 0;
            foreach (StaticObjectSubmesh submesh in this.Submeshes)
            {
                indices.AddRange(submesh.Indices.Select(x => x += startIndex));

                startIndex += submesh.Indices.Max();
            }

            return indices;
        }

        public R3DBox GetBoundingBox()
        {
            Vector3 min = Vector3.Infinity;
            Vector3 max = Vector3.NegativeInfinity;

            foreach (StaticObjectSubmesh submesh in this.Submeshes)
            {
                foreach (StaticObjectVertex vertex in submesh.Vertices)
                {
                    if (min.X > vertex.Position.X) min.X = vertex.Position.X;
                    if (min.Y > vertex.Position.Y) min.Y = vertex.Position.Y;
                    if (min.Z > vertex.Position.Z) min.Z = vertex.Position.Z;
                    if (max.X < vertex.Position.X) max.X = vertex.Position.X;
                    if (max.Y < vertex.Position.Y) max.Y = vertex.Position.Y;
                    if (max.Z < vertex.Position.Z) max.Z = vertex.Position.Z;
                }
            }

            return new R3DBox(min, max);
        }
        public Vector3 GetCentralPoint() => GetBoundingBox().GetCentralPoint();
    }

    [Flags]
    public enum StaticObjectFlags : uint
    {
        VERTEX_COLORS = 1,
        LOCAL_ORIGIN_LOCATOR_AND_PIVOT = 2
    }
}
