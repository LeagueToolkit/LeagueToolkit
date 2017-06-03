using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Fantome.League.IO.Inibin
{
    [DebuggerDisplay("[ Version: {Version} ]")]
    public class InibinFile
    {
        public byte Version { get; private set; }
        public List<InibinSet> Sets { get; private set; } = new List<InibinSet>();
        public InibinFile(string location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(location)))
            {
                this.Version = br.ReadByte();

                UInt32 StringDataLength = 0;
                InibinFlags Flags = 0;
                if(Version == 1)
                {
                    br.ReadBytes(3);
                    UInt32 ValueCount = br.ReadUInt32();
                    StringDataLength = br.ReadUInt32();

                    this.Sets.Add(new InibinSet(br ,InibinFlags.StringList, (uint)br.BaseStream.Length - StringDataLength, ValueCount));
                }
                else
                {
                    StringDataLength = br.ReadUInt16();
                    Flags = (InibinFlags)br.ReadUInt16();

                    if (Flags.HasFlag(InibinFlags.Int32List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Int32List));
                    }
                    if (Flags.HasFlag(InibinFlags.Float32List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32List));
                    }
                    if (Flags.HasFlag(InibinFlags.FixedPointFloatList))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatList));
                    }
                    if (Flags.HasFlag(InibinFlags.Int16List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Int16List));
                    }
                    if (Flags.HasFlag(InibinFlags.Int8List))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Int8List));
                    }
                    if (Flags.HasFlag(InibinFlags.BitList))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.BitList));
                    }
                    if (Flags.HasFlag(InibinFlags.FixedPointFloatListVec3))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatListVec3));
                    }
                    if (Flags.HasFlag(InibinFlags.Float32ListVec3))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32ListVec3));
                    }
                    if (Flags.HasFlag(InibinFlags.FixedPointFloatListVec2))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatListVec2));
                    }
                    if (Flags.HasFlag(InibinFlags.Float32ListVec2))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32ListVec2));
                    }
                    if (Flags.HasFlag(InibinFlags.FixedPointFloatListVec4))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.FixedPointFloatListVec4));
                    }
                    if (Flags.HasFlag(InibinFlags.Float32ListVec4))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.Float32ListVec4));
                    }
                    if (Flags.HasFlag(InibinFlags.StringList))
                    {
                        this.Sets.Add(new InibinSet(br, InibinFlags.StringList, (uint)br.BaseStream.Length - StringDataLength));
                    }
                }
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
