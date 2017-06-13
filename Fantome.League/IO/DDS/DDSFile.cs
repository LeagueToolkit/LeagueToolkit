using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.League.IO.DDS
{
    public class DDSFile
    {
        public DDSFlags Flags { get; private set; }
        public uint Heigth { get; private set; }
        public uint Width { get; private set; }
        public DDSPixelFormat PixelFormat { get; private set; }
        public DDSFile(string location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(location)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                uint HeaderSize = br.ReadUInt32();
                this.Flags = (DDSFlags)br.ReadUInt32();
                this.Heigth = br.ReadUInt32();
                this.Width = br.ReadUInt32();
                uint PitchOrLinearSize = br.ReadUInt32();
                uint Depth = br.ReadUInt32();
                uint MipMapCount = br.ReadUInt32();
                uint[] Reserved = new uint[11];
                for(int i = 0; i < 11; i++)
                {
                    Reserved[i] = br.ReadUInt32();
                }

                this.PixelFormat = new DDSPixelFormat(br);
                uint Caps1 = br.ReadUInt32();
                uint Caps2 = br.ReadUInt32();
                uint Caps3 = br.ReadUInt32();
                uint Caps4 = br.ReadUInt32();
                uint Reserved2 = br.ReadUInt32();
            }
        }
    }

    [Flags]
    public enum DDSFlags : UInt32
    {
        DDSD_CAPS = 1,
        DDSD_HEIGHT = 2,
        DDSD_WIDTH = 4,
        DDSD_PITCH = 8,
        DDSD_PIXELFORMAT = 0x1000,
        DDSD_MIPMAPCOUNT = 0x20000,
        DDSD_LINEARSIZE = 0x80000,
        DDSD_DEPTH = 0x800000
    }
}
