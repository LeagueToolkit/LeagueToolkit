using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Fantome.League.IO.Inibin
{
    public class InibinFile
    {
        public List<InibinSet> Sets { get; private set; } = new List<InibinSet>();
        public InibinFile(string location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(location)))
            {
                uint version = br.ReadByte();

                uint stringDataLength = 0;
                InibinFlags flags = 0;
                if(version == 1)
                {
                    br.ReadBytes(3);
                    uint valueCount = br.ReadUInt32();
                    stringDataLength = br.ReadUInt32();

                    this.Sets.Add(new InibinSet(br ,InibinFlags.StringList, (uint)br.BaseStream.Length - stringDataLength, valueCount));
                }
                else
                {
                    stringDataLength = br.ReadUInt16();
                    flags = (InibinFlags)br.ReadUInt16();

                    if (flags.HasFlag(InibinFlags.Int32List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Int32List));
                    }
                    if (flags.HasFlag(InibinFlags.Float32List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32List));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatList))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatList));
                    }
                    if (flags.HasFlag(InibinFlags.Int16List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Int16List));
                    }
                    if (flags.HasFlag(InibinFlags.Int8List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Int8List));
                    }
                    if (flags.HasFlag(InibinFlags.BitList))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.BitList));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatListVec3))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatListVec3));
                    }
                    if (flags.HasFlag(InibinFlags.Float32ListVec3))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32ListVec3));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatListVec2))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatListVec2));
                    }
                    if (flags.HasFlag(InibinFlags.Float32ListVec2))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32ListVec2));
                    }
                    if (flags.HasFlag(InibinFlags.FixedPointFloatListVec4))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatListVec4));
                    }
                    if (flags.HasFlag(InibinFlags.Float32ListVec4))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32ListVec4));
                    }
                    if (flags.HasFlag(InibinFlags.StringList))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.StringList, (uint)br.BaseStream.Length - stringDataLength));
                    }
                }
            }
        }

        public void Write(string fileLocation)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileLocation)))
            {
                bw.Write();
            }
        }
    }

    [Flags]
    public enum InibinFlags : UInt16
    {
        Int32List = 1,
        Float32List = 1 << 1,
        FixedPointFloatList = 1 << 2,
        Int16List = 1 << 3,
        Int8List = 1 << 4,
        BitList = 1 << 5,
        FixedPointFloatListVec3 = 1 << 6,
        Float32ListVec3 = 1 << 7,
        FixedPointFloatListVec2 = 1 << 8,
        Float32ListVec2 = 1 << 9,
        FixedPointFloatListVec4 = 1 << 10,
        Float32ListVec4 = 1 << 11,
        StringList = 1 << 12
    }
}
