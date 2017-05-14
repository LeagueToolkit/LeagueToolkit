using Fantome.League.Helpers.Structures;
using Fantome.League.IO.OBJ;
using Fantome.League.IO.SCO;
using Fantome.League.IO.SKN;
using Fantome.League.IO.WGT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.League.Converters
{
    public static class SKNConverter
    {
        public static SKNFile Convert(WGTFile Weights, SCOFile Model)
        {
            return new SKNFile(Weights, Model);
        }
        public static OBJFile Convert(SKNFile Model)
        {
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector2> UV = new List<Vector2>();
            List<Vector3> Normals = new List<Vector3>();

            foreach(SKNVertex Vertex in Model.Vertices)
            {
                Vertices.Add(Vertex.Position);
                UV.Add(Vertex.UV);
                if(Vertex.Normal.X == 0 && Vertex.Normal.Y == 0 && Vertex.Normal.Z == 0)
                {
                    Normals.Add(new Vector3(1, 1, 1));
                }
                else
                {
                    Normals.Add(Vertex.Normal);
                }
            }

            return new OBJFile(Vertices, UV, Normals, Model.Indices);
        }
    }
}
