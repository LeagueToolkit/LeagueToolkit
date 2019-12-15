using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Fantome.Libraries.League.IO.SCO
{
    public class SCOFile
    {
        public string Name { get; private set; }
        public Vector3 CentralPoint { get; private set; }
        public Vector3 PivotPoint { get; private set; }
        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public Dictionary<string, List<SCOFace>> Materials { get; private set; } = new Dictionary<string, List<SCOFace>>();

        public SCOFile(List<Vector3> vertices, Dictionary<string, List<SCOFace>> materials)
        {
            this.Vertices = vertices;
            this.Materials = materials;
        }

        public SCOFile(List<Vector3> vertices, List<uint> indices, List<Vector2> uvs)
        {
            this.Vertices = vertices;

            List<SCOFace> faces = new List<SCOFace>();
            for (int i = 0; i < indices.Count; i += 3)
            {
                faces.Add(new SCOFace(new uint[] { indices[i], indices[i + 1], indices[i + 2] }, "lambert1", new Vector2[] { uvs[i], uvs[i + 1], uvs[i + 2] }));
            }

            this.Materials.Add("lambert1", faces);
        }

        public SCOFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public SCOFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                char[] splittingArray = new char[] { ' ' };
                string[] input = null;

                if (sr.ReadLine() != "[ObjectBegin]")
                    throw new Exception("File is either not an SCO file or is corrupted");

                this.Name = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries)[1];

                input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
                this.CentralPoint = new Vector3(float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat),
                    float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat),
                    float.Parse(input[3], CultureInfo.InvariantCulture.NumberFormat));

                input = sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries);
                uint vertexCount = 0;
                if (input[0] == "PivotPoint=")
                {
                    this.PivotPoint = new Vector3(float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[3], CultureInfo.InvariantCulture.NumberFormat));
                    vertexCount = uint.Parse(sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
                }
                else if (input[0] == "Verts=")
                {
                    vertexCount = uint.Parse(input[1]);
                }

                for (int i = 0; i < vertexCount; i++)
                {
                    this.Vertices.Add(new Vector3(sr));
                }

                uint faceCount = uint.Parse(sr.ReadLine().Split(splittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
                for (int i = 0; i < faceCount; i++)
                {
                    SCOFace face = new SCOFace(sr);

                    if (!this.Materials.ContainsKey(face.Material))
                    {
                        this.Materials.Add(face.Material, new List<SCOFace>());
                    }

                    this.Materials[face.Material].Add(face);
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
                sw.WriteLine("[ObjectBegin]");
                sw.WriteLine("Name= " + this.Name);

                Vector3 centralPoint = CalculateCentralPoint();
                sw.WriteLine(string.Format("CentralPoint= {0} {1} {2}", centralPoint.X, centralPoint.Y, centralPoint.Z));

                if (this.PivotPoint != null)
                {
                    sw.WriteLine(string.Format("PivotPoint= {0} {1} {2}", this.PivotPoint.X, this.PivotPoint.Y, this.PivotPoint.Z));
                }

                sw.WriteLine("Verts= " + this.Vertices.Count);
                foreach (Vector3 vertex in this.Vertices)
                {
                    sw.WriteLine(string.Format("{0} {1} {2}", vertex.X, vertex.Y, vertex.Z));
                }

                uint faceCount = 0;
                foreach (KeyValuePair<string, List<SCOFace>> material in this.Materials)
                {
                    faceCount += (uint)material.Value.Count;
                }
                sw.WriteLine("Faces= " + faceCount);

                foreach (KeyValuePair<string, List<SCOFace>> material in this.Materials)
                {
                    foreach (SCOFace face in material.Value)
                    {
                        face.Write(sw);
                    }
                }

                sw.WriteLine("[ObjectEnd]");
            }
        }

        public R3DBoundingBox CalculateBoundingBox()
        {
            Vector3 min = this.Vertices[0];
            Vector3 max = this.Vertices[0];

            foreach (Vector3 vertex in this.Vertices)
            {
                if (min.X > vertex.X) min.X = vertex.X;
                if (min.Y > vertex.Y) min.Y = vertex.Y;
                if (min.Z > vertex.Z) min.Z = vertex.Z;
                if (max.X < vertex.X) max.X = vertex.X;
                if (max.Y < vertex.Y) max.Y = vertex.Y;
                if (max.Z < vertex.Z) max.Z = vertex.Z;
            }

            return new R3DBoundingBox(min, new Vector3(Math.Abs(max.X - min.X), Math.Abs(max.Y - min.Y), Math.Abs(max.Z - min.Z)));
        }

        public Vector3 CalculateCentralPoint()
        {
            R3DBoundingBox boundingBox = CalculateBoundingBox();
            return new Vector3(
                0.5f * (boundingBox.Size.X + boundingBox.Org.X),
                0.5f * (boundingBox.Size.Y + boundingBox.Org.Y),
                0.5f * (boundingBox.Size.Z + boundingBox.Org.Z)
                );
        }

        public Vector3 CalculateCentralPoint(R3DBoundingBox boundingBox)
        {
            return new Vector3(
                0.5f * (boundingBox.Size.X + boundingBox.Org.X),
                0.5f * (boundingBox.Size.Y + boundingBox.Org.Y),
                0.5f * (boundingBox.Size.Z + boundingBox.Org.Z)
                );
        }
    }
}
