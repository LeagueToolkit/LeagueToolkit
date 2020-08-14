using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantome.Libraries.League.IO.StaticObject
{
    public static class StaticObjectObjExtensions
    {
        public static OBJFile GetObj(this StaticObjectSubmesh submesh)
        {
            List<Vector3> vertices = new List<Vector3>(submesh.Vertices.Count);
            List<Vector2> uvs = new List<Vector2>(submesh.Vertices.Count);
            foreach(StaticObjectVertex vertex in submesh.Vertices)
            {
                vertices.Add(vertex.Position);
                uvs.Add(vertex.UV);
            }

            return new OBJFile(vertices, submesh.Indices, uvs);
        }
    }
}
