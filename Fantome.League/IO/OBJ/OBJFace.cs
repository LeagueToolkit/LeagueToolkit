using System.IO;

namespace Fantome.Libraries.League.IO.OBJ
{
    public class OBJFace
    {
        public uint[] VertexIndices { get; set; }
        public uint[] UVIndices { get; set; }
        public uint[] NormalIndices { get; set; }

        public OBJFace(uint[] vertexIndices)
        {
            this.VertexIndices = vertexIndices;
        }

        public OBJFace(uint[] vertexIndices, uint[] uvIndices)
        {
            this.VertexIndices = vertexIndices;
            this.UVIndices = uvIndices;
        }

        public OBJFace(uint[] vertexIndices, uint[] uvIndices, uint[] normalIndices)
        {
            this.VertexIndices = vertexIndices;
            this.UVIndices = uvIndices;
            this.NormalIndices = normalIndices;
        }

        public void Write(StreamWriter sw)
        {
            if (this.UVIndices != null && this.NormalIndices == null)
            {
                sw.WriteLine(
                    "f {0}/{1} {2}/{3} {4}/{5}",
                    this.VertexIndices[0] + 1,
                    this.UVIndices[0] + 1,
                    this.VertexIndices[1] + 1,
                    this.UVIndices[1] + 1,
                    this.VertexIndices[2] + 1,
                    this.UVIndices[2] + 1
                    );
            }
            else if (this.UVIndices != null && this.NormalIndices != null)
            {
                sw.WriteLine(
                    "f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8}",
                    this.VertexIndices[0] + 1,
                    this.UVIndices[0] + 1,
                    this.NormalIndices[0] + 1,
                    this.VertexIndices[1] + 1,
                    this.UVIndices[1] + 1,
                    this.NormalIndices[1] + 1,
                    this.VertexIndices[2] + 1,
                    this.UVIndices[2] + 1,
                    this.NormalIndices[2] + 1
                    );
            }
            else
            {
                sw.WriteLine("f {0} {1} {2}", this.VertexIndices[0] + 1, this.VertexIndices[1] + 1, this.VertexIndices[2] + 1);
            }
        }
    }
}
