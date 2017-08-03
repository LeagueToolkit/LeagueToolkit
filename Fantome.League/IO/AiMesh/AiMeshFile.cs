using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Fantome.Libraries.League.Helpers.Exceptions;
using System.Diagnostics;

namespace Fantome.Libraries.League.IO.AiMesh
{
    [DebuggerDisplay("[ Version: {Version}]")]
    public class AiMeshFile
    {
        public UInt32 Version { get; private set; }
        public List<AiMeshFace> Faces = new List<AiMeshFace>();

        public AiMeshFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                if (Magic != "r3d2aims")
                    throw new InvalidFileMagicException();

                this.Version = br.ReadUInt32();
                if (this.Version != 2)
                    throw new UnsupportedFileVersionException();

                UInt32 FaceCount = br.ReadUInt32();
                UInt32 Flags = br.ReadUInt32();
                UInt32 UnknownFlagConstant = br.ReadUInt32(); // If set to [1] then Flags is [1]

                for (int i = 0; i < FaceCount; i++)
                {
                    this.Faces.Add(new AiMeshFace(br));
                }
            }
        }

        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
            {
                bw.Write("r3d2aims".ToCharArray());
                bw.Write((UInt32)2);
                bw.Write((UInt32)this.Faces.Count);
                bw.Write((UInt32)0);
                bw.Write((UInt32)0);

                foreach (AiMeshFace Face in this.Faces)
                {
                    Face.Write(bw);
                }
            }
        }
    }
}
