using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.Inibin
{
    public class InibinFile
    {
        public List<InibinSet> Sets { get; private set; } = new List<InibinSet>();
        public InibinFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                uint version = br.ReadByte();

                uint stringDataLength = 0;
                InibinFlags flags = 0;
                if (version == 1)
                {
                    br.ReadBytes(3);
                    uint valueCount = br.ReadUInt32();
                    stringDataLength = br.ReadUInt32();

                    this.Sets.Add(new InibinSet(br, InibinFlags.StringList, (uint)br.BaseStream.Length - stringDataLength, valueCount));
                }
                else if(version == 2)
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
                else
                {
                    throw new Exception("This version is not supported");
                }
            }
        }

        public void Write(string fileLocation)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileLocation)))
            {
                ushort stringDataLength = 0;
                InibinFlags flags = 0;

                foreach (string dataString in this.Sets.Find(x => x.Type == InibinFlags.StringList).Properties.Values)
                {
                    stringDataLength += (ushort)(dataString.Length + 1);
                }

                if (this.Sets.Exists(x => x.Type == InibinFlags.BitList))
                {
                    flags |= InibinFlags.BitList;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.FixedPointFloatList))
                {
                    flags |= InibinFlags.FixedPointFloatList;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.FixedPointFloatListVec2))
                {
                    flags |= InibinFlags.FixedPointFloatListVec2;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.FixedPointFloatListVec3))
                {
                    flags |= InibinFlags.FixedPointFloatListVec3;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.FixedPointFloatListVec4))
                {
                    flags |= InibinFlags.FixedPointFloatListVec4;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.Float32List))
                {
                    flags |= InibinFlags.Float32List;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.Float32ListVec2))
                {
                    flags |= InibinFlags.Float32ListVec2;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.Float32ListVec3))
                {
                    flags |= InibinFlags.Float32ListVec3;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.Float32ListVec4))
                {
                    flags |= InibinFlags.Float32ListVec4;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.Int16List))
                {
                    flags |= InibinFlags.Int16List;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.Int32List))
                {
                    flags |= InibinFlags.Int32List;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.Int8List))
                {
                    flags |= InibinFlags.Int8List;
                }
                if (this.Sets.Exists(x => x.Type == InibinFlags.StringList))
                {
                    flags |= InibinFlags.StringList;
                }


                bw.Write((byte)2);
                bw.Write(stringDataLength);
                bw.Write((ushort)flags);

                foreach (InibinSet set in this.Sets)
                {
                    set.Write(bw);
                }
            }
        }
    }

    [Flags]
    public enum InibinFlags : ushort
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
