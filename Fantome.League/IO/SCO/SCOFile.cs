using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Fantome.Libraries.League.IO.SCO
{
    [DebuggerDisplay("[ {Name} ]")]
    public class SCOFile
    {
        public string Name { get; private set; }
        public Vector3 CentralPoint { get; private set; }
        public Vector3 PivotPoint { get; private set; }
        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public List<SCOFace> Faces { get; private set; } = new List<SCOFace>();

        public SCOFile(List<UInt16> Indices, List<Vector3> Vertices, List<Vector2> UV)
        {
            this.Vertices = Vertices;
            for(int i = 0; i < Indices.Count; i += 3)
            {
                this.Faces.Add(new SCOFace(new UInt16[] { Indices[i], Indices[i + 1], Indices[i + 2] }, "lambert1", new Vector2[] { UV[i], UV[i + 1], UV[i + 2]}));
            }
        }

        public SCOFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {

        }
        public SCOFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                char[] SplittingArray = new char[] { ' ' };
                string[] input = null;

                if (sr.ReadLine() != "[ObjectBegin]")
                    throw new Exception("File is either not an SCO file or is corrupted");

                this.Name = sr.ReadLine().Split(SplittingArray, StringSplitOptions.RemoveEmptyEntries)[1];

                input = sr.ReadLine().Split(SplittingArray, StringSplitOptions.RemoveEmptyEntries);
                this.CentralPoint = new Vector3
                (
                    float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat),
                    float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat),
                    float.Parse(input[3], CultureInfo.InvariantCulture.NumberFormat)
                );

                input = sr.ReadLine().Split(SplittingArray, StringSplitOptions.RemoveEmptyEntries);
                UInt32 VertexCount = 0;
                if (input[0] == "PivotPoint=")
                {
                    this.PivotPoint = new Vector3
                    (
                        float.Parse(input[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(input[3], CultureInfo.InvariantCulture.NumberFormat)
                    );
                    VertexCount = uint.Parse(sr.ReadLine().Split(SplittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
                }
                else if (input[0] == "Verts=")
                {
                    VertexCount = uint.Parse(input[1]);
                }

                for (int i = 0; i < VertexCount; i++)
                {
                    this.Vertices.Add(new Vector3(sr));
                }

                UInt32 FaceCount = uint.Parse(sr.ReadLine().Split(SplittingArray, StringSplitOptions.RemoveEmptyEntries)[1]);
                for (int i = 0; i < FaceCount; i++)
                {
                    this.Faces.Add(new SCOFace(sr));
                }
            }
        }
    }
}
