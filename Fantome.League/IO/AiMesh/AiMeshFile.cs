using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.AiMesh
{
    public class AiMeshFile
    {
        public List<AiMeshFace> Faces = new List<AiMeshFace>();

        public AiMeshFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
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

                uint faceCount = br.ReadUInt32();
                uint flags = br.ReadUInt32();
                uint unknownFlagConstant = br.ReadUInt32(); // If set to [1] then Flags is [1]

                for (int i = 0; i < faceCount; i++)
                {
                    this.Faces.Add(new AiMeshFace(br));
                }
            }
        }

        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
            {
                bw.Write(Encoding.ASCII.GetBytes("r3d2aims"));
                bw.Write((uint)2);
                bw.Write(this.Faces.Count);
                bw.Write((uint)0);
                bw.Write((uint)0);

                foreach (AiMeshFace face in this.Faces)
                {
                    face.Write(bw);
                }
            }
        }
    }
}
