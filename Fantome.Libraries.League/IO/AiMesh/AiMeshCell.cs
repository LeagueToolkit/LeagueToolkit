using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.AiMesh
{
    /// <summary>
    /// Represents a cell inside of an <see cref="AiMeshFile"/>
    /// </summary>
    public class AiMeshCell
    {
        /// <summary>
        /// Vertices of this <see cref="AiMeshCell"/>
        /// </summary>
        public Vector3[] Vertices { get; set; }
        /// <summary>
        /// Links to other <see cref="AiMeshCell"/>
        /// </summary>
        /// <remarks>A link can be 0xFF which means it doesnt link to anything</remarks>
        public ushort[] Links { get; set; }

        /// <summary>
        /// Reads an <see cref="AiMeshCell"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public AiMeshCell(BinaryReader br)
        {
            this.Vertices = new Vector3[] { new Vector3(br), new Vector3(br), new Vector3(br) };
            this.Links = new ushort[] { br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16() };
        }

        /// <summary>
        /// Writes this <see cref="AiMeshCell"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
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
