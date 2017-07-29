using Fantome.Libraries.League.IO.NVR;
using Fantome.Libraries.League.IO.WGEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Converters
{
    public static class WGEOConverter
    {
        public static WGEOFile ConvertNVR(NVRFile nvr, WGEOBucketGeometry bucketTemplate)
        {
            List<WGEOModel> models = new List<WGEOModel>();

            foreach(NVRMesh mesh in nvr.Meshes)
            {
                List<WGEOVertex> vertices = new List<WGEOVertex>();
                List<ushort> indices = mesh.IndexedPrimitives[0].Indices.Select(x => (ushort)x).ToList();

                foreach (NVRVertex vertex in mesh.IndexedPrimitives[0].Vertices)
                {
                    if(mesh.IndexedPrimitives[0].VertexType == NVRVertexType.NVRVERTEX_4)
                    {
                        NVRVertex4 vertex4 = vertex as NVRVertex4;
                        vertices.Add(new WGEOVertex(vertex4.Position, vertex4.UV));
                    }
                    else if(mesh.IndexedPrimitives[0].VertexType == NVRVertexType.NVRVERTEX_8)
                    {
                        NVRVertex8 vertex8 = vertex as NVRVertex8;
                        vertices.Add(new WGEOVertex(vertex8.Position, vertex8.UV));
                    }
                    else if(mesh.IndexedPrimitives[0].VertexType == NVRVertexType.NVRVERTEX_12)
                    {
                        NVRVertex12 vertex12 = vertex as NVRVertex12;
                        vertices.Add(new WGEOVertex(vertex12.Position, vertex12.UV));
                    }
                }

                models.Add(new WGEOModel(mesh.Material.Channels[0].Name, mesh.Material.Name, vertices, indices));
            }

            return new WGEOFile(models, bucketTemplate);
        }
    }
}
