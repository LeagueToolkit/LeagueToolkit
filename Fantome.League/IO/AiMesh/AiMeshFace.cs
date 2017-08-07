using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.AiMesh
{
    public class AiMeshFace
    {
        public Vector3[] Vertices { get; set; }
        public ushort[] Links { get; set; }

        public AiMeshFace(BinaryReader br)
        {
            this.Vertices = new Vector3[] { new Vector3(br), new Vector3(br), new Vector3(br) };
            this.Links = new ushort[] { br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16() };
        }

        public void Write(BinaryWriter bw)
        {
            for (int i = 0; i < 3; i++)
            {
                this.Vertices[i].Write(bw);
            }
            for (int i = 0; i < 3; i++)
            {
                bw.Write(this.Links[i]);
            }
        }
    }
}
