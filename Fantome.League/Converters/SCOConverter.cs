using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SCB;
using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.SKN;
using Fantome.Libraries.League.IO.WGT;
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Converters
{
    public static class SCOConverter
    {
        public static SCOFile ConvertSCB(SCBFile SCB)
        {
            List<UInt16> Indices = new List<UInt16>();
            List<Vector2> UV = new List<Vector2>();
            foreach(SCBFace Face in SCB.Faces)
            {
                Indices.AddRange(Face.Indices.AsEnumerable().Cast<UInt16>());
                UV.AddRange(Face.UV);
            }
            return new SCOFile(Indices, SCB.Vertices, UV);
        }
        public static SCOFile ConvertOBJ(OBJFile OBJ)
        {
            List<UInt16> Indices = new List<UInt16>();
            bool ZeroPointIndex = false;
            foreach(OBJFace Face in OBJ.Faces)
            {
                for(int i = 0; i < 3; i++)
                {
                    if(Face.VertexIndices[i] == 0)
                    {
                        ZeroPointIndex = true;
                        break;
                    }
                }
                if (ZeroPointIndex) break;
            }
            if(ZeroPointIndex == false)
            {
                foreach (OBJFace Face in OBJ.Faces)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Face.VertexIndices[i] -= 1;
                        Face.UVIndices[i] -= 1;
                    }
                }
            }
            foreach (OBJFace Face in OBJ.Faces)
            {
                Indices.AddRange(Face.VertexIndices);
            }
            return new SCOFile(Indices, OBJ.Vertices, OBJ.UVs);
        }
        public static Tuple<SCOFile, WGTFile> ConvertToLegacy(SKNFile SKN)
        {
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector2> UV = new List<Vector2>();
            List<Vector4Byte> BoneIndices = new List<Vector4Byte>();
            List<Vector4> Weights = new List<Vector4>();

            foreach(SKNVertex Vertex in SKN.Vertices)
            {
                Vertices.Add(Vertex.Position);
                UV.Add(Vertex.UV);
                Weights.Add(Vertex.Weights);
                BoneIndices.Add(Vertex.BoneIndices);
            }
            return new Tuple<SCOFile, WGTFile>(new SCOFile(SKN.Indices, Vertices, UV), new WGTFile(Weights, BoneIndices));
        }
    }
}
