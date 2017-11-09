using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOFile
    {
        public List<MGEOModel> Meshes { get; set; } = new List<MGEOModel>();
        public MGEOBucketGeometry BucketGeometry { get; set; }

        public MGEOFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public MGEOFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "OEGM")
                {
                    throw new Exception("This is not a valid Map Geometry File");
                }

                uint version = br.ReadUInt32();
                if (version != 5)
                {
                    throw new Exception("Version: " + version + " of Map Geometry is not supported");
                }

                bool unknownFlag = br.ReadByte() == 1;

                List<uint> unknownList = new List<uint>();
                uint unknownCount = (br.ReadUInt32() << 7) / 4;
                for (int i = 0; i < unknownCount; i++)
                {
                    unknownList.Add(br.ReadUInt32());
                }

                List<uint> vertexBufferOffsets = new List<uint>();
                uint vertexBufferCount = br.ReadUInt32();
                for (int i = 0; i < vertexBufferCount; i++)
                {
                    uint vertexBufferSize = br.ReadUInt32();
                    vertexBufferOffsets.Add((uint)br.BaseStream.Position - 4);
                    br.BaseStream.Seek(vertexBufferSize, SeekOrigin.Current);
                }

                List<uint> indexBufferOffsets = new List<uint>();
                uint indexBufferCount = br.ReadUInt32();
                for (int i = 0; i < indexBufferCount; i++)
                {
                    uint indexBufferSize = br.ReadUInt32();
                    indexBufferOffsets.Add((uint)br.BaseStream.Position - 4);
                    br.BaseStream.Seek(indexBufferSize, SeekOrigin.Current);
                }

                uint meshCount = br.ReadUInt32();
                for (int i = 0; i < meshCount; i++)
                {
                    this.Meshes.Add(new MGEOModel(br, vertexBufferOffsets, indexBufferOffsets, unknownFlag));
                }

                //File.WriteAllText("kek.txt", JsonConvert.SerializeObject(this.Meshes, Formatting.Indented));

                this.BucketGeometry = new MGEOBucketGeometry(br);
            }
        }
    }
}
