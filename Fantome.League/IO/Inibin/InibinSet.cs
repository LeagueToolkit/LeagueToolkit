using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Fantome.League.Helpers.Structures;

namespace Fantome.League.IO.Inibin
{
    public class InibinSet
    {
        public InibinFlags Type { get; private set; }
        public Dictionary<uint, object> Properties { get; private set; } = new Dictionary<uint, object>();
        public InibinSet(BinaryReader br, InibinFlags type)
        {
            this.Type = type;
            ushort valueCount = br.ReadUInt16();
            List<uint> hashes = new List<uint>();

            for(int i = 0; i < valueCount; i++)
            {
                uint hash = br.ReadUInt32();
                this.Properties.Add(hash, null);
                hashes.Add(hash);
            }

            for (int i = 0; i < valueCount; i++)
            {
                if(this.Type == InibinFlags.Int32List)
                {
                    this.Properties[hashes[i]] = br.ReadInt32();
                }
                else if(this.Type == InibinFlags.Float32List)
                {
                    this.Properties[hashes[i]] = br.ReadSingle();
                }
                else if (this.Type == InibinFlags.FixedPointFloatList)
                {
                    this.Properties[hashes[i]] = br.ReadByte() * 0.1;
                }
                else if (this.Type == InibinFlags.Int16List)
                {
                    this.Properties[hashes[i]] = br.ReadInt16();
                }
                else if (this.Type == InibinFlags.Int8List)
                {
                    this.Properties[hashes[i]] = br.ReadByte();
                }
                else if(this.Type == InibinFlags.BitList)
                {
                    byte boolean = 0;
                    boolean = (i % 8) == 0 ? br.ReadByte() : (byte)(boolean >> 1);
                    this.Properties[hashes[i]] = Convert.ToBoolean(0x1 & boolean);
                }
                else if(this.Type == InibinFlags.FixedPointFloatListVec3)
                {
                    this.Properties[hashes[i]] = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte()};
                }
                else if (this.Type == InibinFlags.Float32ListVec3)
                {
                    this.Properties[hashes[i]] = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle()};
                }
                else if (this.Type == InibinFlags.FixedPointFloatListVec2)
                {
                    this.Properties[hashes[i]] = new byte[] { br.ReadByte(), br.ReadByte() };
                }
                else if (this.Type == InibinFlags.Float32ListVec2)
                {
                    this.Properties[hashes[i]] = new float[] { br.ReadSingle(), br.ReadSingle() };
                }
                else if (this.Type == InibinFlags.FixedPointFloatListVec4)
                {
                    this.Properties[hashes[i]] = new byte[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };
                }
                else if (this.Type == InibinFlags.Float32ListVec4)
                {
                    this.Properties[hashes[i]] = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
                }
            }
        }
        public InibinSet(BinaryReader br, InibinFlags type, uint stringOffset, uint? valueCount = null)
        {
            this.Type = type;
            if(valueCount == null)
            {
                valueCount = br.ReadUInt16();
            }
            List<uint> Hashes = new List<uint>();

            for (int i = 0; i < valueCount; i++)
            {
                uint hash = br.ReadUInt32();
                this.Properties.Add(hash, null);
                Hashes.Add(hash);

                uint offset = br.ReadUInt32();
                long oldPos = br.BaseStream.Position;
                br.BaseStream.Seek(offset + stringOffset, SeekOrigin.Begin);
                string s = "";
                char c = (char)0xff;
                while (c != '\u0000')
                {
                    c = br.ReadChar();
                    s += c;
                }
                this.Properties[Hashes[i]] = s;
                br.BaseStream.Seek(oldPos, SeekOrigin.Begin);
            }
        }
    }
}
