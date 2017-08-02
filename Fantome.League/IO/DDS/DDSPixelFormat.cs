using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.League.IO.DDS
{
    public class DDSPixelFormat
    {
        public DDSPixelFormatFlags Flags { get; private set; }
        public string FourCC { get; private set; }
        public DDSPixelFormat(BinaryReader br)
        {
            uint Size = br.ReadUInt32();
            this.Flags = (DDSPixelFormatFlags)br.ReadUInt32();
            this.FourCC = Encoding.ASCII.GetString(br.ReadBytes(4));
            uint RGBBitCount = br.ReadUInt32();
            uint RBitmask = br.ReadUInt32();
            uint GBitmask = br.ReadUInt32();
            uint BBitmask = br.ReadUInt32();
            uint ABitmask = br.ReadUInt32();
        }
    }

    [Flags]
    public enum DDSPixelFormatFlags
    {
        DDPF_ALPHAPIXELS = 1,
        DDPF_ALPHA = 2,
        DDPF_FOURCC = 4,
        DDPF_RGB = 0x40,
        DDPF_YUV = 0x200,
        DDPF_LUMINANCE = 0x20000
    }
}
