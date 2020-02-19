using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANBundleChunk
    {
        public uint CompressedSize { get; private set; }
        public uint UncompressedSize { get; private set; }
        public ulong ID { get; private set; }

        public RMANBundleChunk(BinaryReader br)
        {
            uint trash1 = br.ReadUInt32();
            this.CompressedSize = br.ReadUInt32();
            this.UncompressedSize = br.ReadUInt32();
            this.ID = br.ReadUInt64();
        }
    }
}
