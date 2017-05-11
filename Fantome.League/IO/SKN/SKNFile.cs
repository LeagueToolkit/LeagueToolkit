using Fantome.League.Helpers.Exceptions;
using Fantome.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.League.IO.SKN
{
    public class SKNFile
    {
        public List<SKNSubmesh> Submeshes { get; private set; } = new List<SKNSubmesh>();
        public List<UInt16> Indices { get; private set; } = new List<UInt16>();
        public List<SKNVertex> Vertices { get; private set; } = new List<SKNVertex>();
        public SKNFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                UInt32 Magic = br.ReadUInt32();
                if (Magic != 0x00112233)
                    throw new InvalidFileMagicException();

                UInt16 Version = br.ReadUInt16();
                if (Version != 2 && Version != 4)
                    throw new UnsupportedFileVersionException();

                UInt16 ObjectCount = br.ReadUInt16();
                UInt32 SubmeshCount = br.ReadUInt32();

                for(int i = 0; i < SubmeshCount; i++)
                {
                    this.Submeshes.Add(new SKNSubmesh(br));
                }
                if (Version == 4)
                    br.ReadUInt32();

                UInt32 IndexCount = br.ReadUInt32();
                UInt32 VertexCount = br.ReadUInt32();

                UInt32 VertexSize;
                bool IsTangent = false;
                Vector3 Min;
                Vector3 Max;
                Vector3 CentralPoint;
                float Radius;

                if (Version == 4)
                {
                    VertexSize = br.ReadUInt32();
                    IsTangent = br.ReadUInt32() == 1;
                    Min = new Vector3(br);
                    Max = new Vector3(br);
                    CentralPoint = new Vector3(br);
                    Radius = br.ReadSingle();
                }

                for(int i = 0; i < IndexCount; i++)
                {
                    this.Indices.Add(br.ReadUInt16());
                }
                for(int i = 0; i < VertexCount; i++)
                {
                    this.Vertices.Add(new SKNVertex(br, IsTangent));
                }
            }
        }
    }
}
