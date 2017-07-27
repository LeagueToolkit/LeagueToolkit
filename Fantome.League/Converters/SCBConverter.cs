using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using Fantome.Libraries.League.IO.SCB;
using Fantome.Libraries.League.IO.SCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Converters
{
    public static class SCBConverter
    {
        public static SCBFile ConvertSCO(SCOFile SCO)
        {
            List<UInt32> Indices = new List<UInt32>();
            List<Vector2> UV = new List<Vector2>();
            foreach (SCOFace Face in SCO.Faces)
            {
                Indices.AddRange(Face.Indices.Cast<UInt32>());
                UV.AddRange(Face.UV);
            }
            return new SCBFile(Indices, SCO.Vertices, UV);
        }
        public static SCBFile ConvertOBJ(OBJFile OBJ)
        {
            List<UInt32> Indices = new List<UInt32>();
            bool ZeroPointIndex = false;
            foreach (OBJFace Face in OBJ.Faces)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Face.VertexIndices[i] == 0)
                    {
                        ZeroPointIndex = true;
                        break;
                    }
                }
                if (ZeroPointIndex) break;
            }
            if (ZeroPointIndex == false)
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
                Indices.AddRange(Face.VertexIndices.Cast<UInt32>());
            }
            return new SCBFile(Indices, OBJ.Vertices, OBJ.UVs);
        }
    }
}
