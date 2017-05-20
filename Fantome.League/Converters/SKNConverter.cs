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

        public static OBJFile Convert(SKNFile Model, bool RecalculateNormals)
        {
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector2> UV = new List<Vector2>();
            List<Vector3> Normals = new List<Vector3>();

            if (RecalculateNormals)
            {
                for (int i = 0; i < Model.Indices.Count; i += 3)
                {
                    Vector3 cp = Vector3.Cross(
                        Model.Vertices[Model.Indices[i + 1]].Position - Model.Vertices[Model.Indices[i]].Position,
                        Model.Vertices[Model.Indices[i + 2]].Position - Model.Vertices[Model.Indices[i]].Position);

                    Model.Vertices[Model.Indices[i]].SetNormal(Model.Vertices[Model.Indices[i]].Normal + cp);
                    Model.Vertices[Model.Indices[i + 1]].SetNormal(Model.Vertices[Model.Indices[i + 1]].Normal + cp);
                    Model.Vertices[Model.Indices[i + 2]].SetNormal(Model.Vertices[Model.Indices[i + 1]].Normal + cp);
                }
                foreach (SKNVertex Vertex in Model.Vertices)
                {
                    float s = Vertex.Normal.X + Vertex.Normal.Y + Vertex.Normal.Z;
                    Vertex.SetNormal(new Vector3(
                        Vertex.Normal.X / s,
                        Vertex.Normal.Y / s,
                        Vertex.Normal.Z / s
                        )
                        );
                }
            }
            foreach (SKNVertex Vertex in Model.Vertices)
            {
                Vertices.Add(Vertex.Position);
                UV.Add(Vertex.UV);
                Normals.Add(Vertex.Normal);
            }

            return new OBJFile(Vertices, UV, Normals, Model.Indices);
        }
    }
}
