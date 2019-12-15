using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.AiMesh
{
    /// <summary>
    /// Represents an AiMesh Navigation mesh
    /// </summary>
    public class AiMeshFile
    {
        /// <summary>
        /// A collection of <see cref="AiMeshCell"/>
        /// </summary>
        public List<AiMeshCell> Cells { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="AiMeshFile"/> with cells
        /// </summary>
        public AiMeshFile(List<AiMeshCell> cells)
        {
            this.Cells = cells;
        }

        /// <summary>
        /// Reads an <see cref="AiMeshFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">The location to read from</param>
        public AiMeshFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {
           
        }

        /// <summary>
        /// Reads an <see cref="AiMeshFile"/> from the specified stream
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        public AiMeshFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                if (magic != "r3d2aims")
                {
                    throw new Exception("This is not a valid AiMesh file");
                }

                uint version = br.ReadUInt32();
                if (version != 2)
                {
                    throw new Exception("This version is not supported");
                }

                uint cellCount = br.ReadUInt32();
                uint flags = br.ReadUInt32();
                uint unknownFlagConstant = br.ReadUInt32(); // If set to [1] then Flags is [1]

                for (int i = 0; i < cellCount; i++)
                {
                    this.Cells.Add(new AiMeshCell(br));
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="AiMeshFile"/> to the specified location
        /// </summary>
        /// <param name="fileLocation">The location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="AiMeshFile"/> to the specified stream
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("r3d2aims"));
                bw.Write((uint)2);
                bw.Write(this.Cells.Count);
                bw.Write((uint)0);
                bw.Write((uint)0);

                foreach (AiMeshCell cell in this.Cells)
                {
                    cell.Write(bw);
                }
            }
        }
    }
}
