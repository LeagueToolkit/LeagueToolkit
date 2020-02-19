using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANBundle
    {
        public ulong ID { get; private set; }
        public List<RMANBundleChunk> Chunks { get; private set; } = new List<RMANBundleChunk>();
        public uint HeaderSize16Unknown { get; private set; }

        public RMANBundle(BinaryReader br)
        {
            uint trash1 = br.ReadUInt32();
            uint headerSize = br.ReadUInt32();
            this.ID = br.ReadUInt64();

            if(headerSize == 16)
            {
                this.HeaderSize16Unknown = br.ReadUInt32();
            }
            else if(headerSize > 16)
            {
                throw new Exception("Bundle Header Size is " + headerSize);
            }

            uint chunkCount = br.ReadUInt32();
            for (int i = 0; i < chunkCount; i++)
            {
                uint chunkOffset = br.ReadUInt32();
                long returnOffset = br.BaseStream.Position;

                br.BaseStream.Seek(chunkOffset + returnOffset - 4, SeekOrigin.Begin);
                this.Chunks.Add(new RMANBundleChunk(br));

                br.BaseStream.Seek(returnOffset, SeekOrigin.Begin);
            }
        }
    }
}
