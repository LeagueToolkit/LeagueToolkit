using Fantome.League.Helpers.Structures;
using Fantome.League.IO.OBJ;
using Fantome.League.IO.WGEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.League.Converters
{
    public class WGEOConverter
    {
        private WGEOFile WGEO { get; set; }
        public WGEOConverter(WGEOFile ToConvert)
        {
            this.WGEO = ToConvert;
        }
        public void ExportModels(string Location)
        {
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector2> UVs = new List<Vector2>();
            OBJFile obj;
            int i = 0;
            foreach(WGEOModel Model in this.WGEO.Models)
            {
                foreach(WGEOVertex Vertex in Model.Vertices)
                {
                    Vertices.Add(Vertex.Position);
                    UVs.Add(Vertex.UV);
                }
                obj = new OBJFile(Vertices, UVs, Model.Indices);
                obj.Write(Location + "//" + i + "_" + Model.Material + ".obj");
                Vertices.Clear();
                UVs.Clear();
                i++;
            }
        }
        public void ExportBucketGeometry(string Location)
        {
            OBJFile obj = new OBJFile(this.WGEO.BucketGeometry.Vertices, this.WGEO.BucketGeometry.Indices);
            obj.Write(Location);
        }
    }
}
