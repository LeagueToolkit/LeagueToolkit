using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.OBJ
{
    public class OBJGroup
    {
        public string Name { get; set; } = "";
        public string Material { get; set; } = "";
        public List<OBJFace> Faces { get; set; } = new List<OBJFace>();

        public OBJGroup(string groupName, string material, List<uint> vertexIndices)
        {
            Name = groupName;
            Material = material;

            for (int i = 0; i < vertexIndices.Count; i += 3)
            {
                uint[] faceIndices = new uint[] { vertexIndices[i], vertexIndices[i + 1], vertexIndices[i + 2] };

                this.Faces.Add(new OBJFace(faceIndices));
            }
        }

        public OBJGroup(string groupName, string material, List<uint> vertexIndices, List<uint> uvIndices)
        {
            Name = groupName;
            Material = material;

            for (int i = 0; i < vertexIndices.Count; i += 3)
            {
                uint[] faceVertexIndices = new uint[] { vertexIndices[i], vertexIndices[i + 1], vertexIndices[i + 2] };

                uint[] faceUVIndices = new uint[] { uvIndices[i], uvIndices[i + 1], uvIndices[i + 2] };

                this.Faces.Add(new OBJFace(faceVertexIndices, faceUVIndices));
            }
        }

        public OBJGroup(string groupName, string material, List<uint> vertexIndices, List<uint> uvIndices, List<uint> normalIndices)
        {
            Name = groupName;
            Material = material;

            for (int i = 0; i < vertexIndices.Count; i += 3)
            {
                uint[] faceVertexIndices = new uint[] { vertexIndices[i], vertexIndices[i + 1], vertexIndices[i + 2] };

                uint[] faceUVIndices = new uint[] { uvIndices[i], uvIndices[i + 1], uvIndices[i + 2] };

                uint[] faceNormalIndices = new uint[] { normalIndices[i], normalIndices[i + 1], normalIndices[i + 2] };

                this.Faces.Add(new OBJFace(faceVertexIndices, faceUVIndices, faceNormalIndices));
            }
        }

        public OBJGroup(string groupName, string material, List<OBJFace> faces)
        {
            Name = groupName;
            Material = material;
            this.Faces = faces;
        }

        public void Write(StreamWriter sw)
        {
            if (Name != "")
            {
                sw.WriteLine(string.Format("g {0}", Name));
            }
            if (Material != "")
            {
                sw.WriteLine(string.Format("usemtl {0}", Material));
            }
            foreach (OBJFace face in Faces)
            {
                face.Write(sw);
            }
        }
    }
}