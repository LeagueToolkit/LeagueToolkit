using LeagueToolkit.Helpers.Structures.BucketGrid;
using LeagueToolkit.IO.NVR;
using LeagueToolkit.IO.WorldGeometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.Converters
{
    public static class WGEOConverter
    {
        /// <summary>
        /// Converts <paramref name="nvr"/> to a <see cref="WorldGeometry"/>
        /// </summary>
        /// <param name="nvr">The <see cref="NVRFile"/> to be used for models</param>
        /// <param name="bucketTemplate">The <see cref="WGEOBucketGeometry"/> to be used a a template for bucket geometry</param>
        /// <returns>A <see cref="WorldGeometry"/> converted from <paramref name="nvr"/></returns>
        public static WorldGeometry ConvertNVR(NVRFile nvr, BucketGrid bucketTemplate)
        {
            List<WorldGeometryModel> models = new List<WorldGeometryModel>();

            foreach (NVRMesh mesh in nvr.Meshes)
            {
                List<WorldGeometryVertex> vertices = new List<WorldGeometryVertex>();
                List<uint> indices = mesh.IndexedPrimitives[0].Indices.Select(x => (uint)x).ToList();

                foreach (NVRVertex vertex in mesh.IndexedPrimitives[0].Vertices)
                {
                    if (mesh.IndexedPrimitives[0].VertexType == NVRVertexType.NVRVERTEX_4)
                    {
                        NVRVertex4 vertex4 = vertex as NVRVertex4;
                        vertices.Add(new WorldGeometryVertex(vertex4.Position, NVRVertex.IsGroundType(mesh.Material) ? new Vector2(0, 0) : vertex4.UV));
                    }
                    else if (mesh.IndexedPrimitives[0].VertexType == NVRVertexType.NVRVERTEX_8)
                    {
                        NVRVertex8 vertex8 = vertex as NVRVertex8;
                        vertices.Add(new WorldGeometryVertex(vertex8.Position, NVRVertex.IsGroundType(mesh.Material) ? new Vector2(0, 0) : vertex8.UV));
                    }
                    else if (mesh.IndexedPrimitives[0].VertexType == NVRVertexType.NVRVERTEX_12)
                    {
                        NVRVertex12 vertex12 = vertex as NVRVertex12;
                        vertices.Add(new WorldGeometryVertex(vertex12.Position, vertex12.UV));
                    }
                }

                models.Add(new WorldGeometryModel(mesh.Material.Channels[0].Name, mesh.Material.Name, vertices, indices));
            }

            return new WorldGeometry(models, bucketTemplate);
        }
    }
}
