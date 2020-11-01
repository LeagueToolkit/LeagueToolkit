using LeagueToolkit.IO.OBJ;
using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.IO.StaticObjectFile
{
    public static class StaticObjectObjExtensions
    {
        public static List<(string MaterialName, OBJFile Obj)> ToObj(this StaticObject staticObject)
        {
            var objs = new List<(string, OBJFile)>();

            foreach(StaticObjectSubmesh submesh in staticObject.Submeshes)
            {
                objs.Add((submesh.Name, submesh.ToObj()));
            }

            return objs;
        }

        public static OBJFile ToObj(this StaticObjectSubmesh submesh)
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
