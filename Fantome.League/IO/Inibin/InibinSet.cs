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
    [DebuggerDisplay("[ {Type} ]")]
    public class InibinSet
    {
        public InibinFlags Type { get; private set; }
        public Dictionary<UInt32, object> Properities = new Dictionary<uint, object>();
        public InibinSet(BinaryReader br, InibinFlags type)
        {
            this.Type = type;
            UInt16 ValueCount = br.ReadUInt16();
            List<UInt32> Hashes = new List<UInt32>();

            for(int i = 0; i < ValueCount; i++)
            {
                UInt32 hash = br.ReadUInt32();
                this.Properities.Add(hash, null);
                Hashes.Add(hash);
            }

            for (int i = 0; i < ValueCount; i++)
            {
                if(this.Type == InibinFlags.Int32List)
                {
                    this.Properities[Hashes[i]] = br.ReadInt32();
                }
                else if(this.Type == InibinFlags.Float32List)
                {
                    this.Properities[Hashes[i]] = br.ReadSingle();
                }
                else if (this.Type == InibinFlags.FixedPointFloatList)
                {
                    this.Properities[Hashes[i]] = br.ReadByte() * 0.1;
                }
                else if (this.Type == InibinFlags.Int16List)
                {
                    this.Properities[Hashes[i]] = br.ReadInt16();
                }
                else if (this.Type == InibinFlags.Int8List)
                {
                    this.Properities[Hashes[i]] = br.ReadByte();
                }
                else if(this.Type == InibinFlags.BitList)
                {
                    byte boolean = 0;
                    boolean = (i % 8) == 0 ? br.ReadByte() : (byte)(boolean >> 1);
                    this.Properities[Hashes[i]] = Convert.ToBoolean(0x1 & boolean);
                }
                else if(this.Type == InibinFlags.FixedPointFloatListVec3)
                {
                    this.Properities[Hashes[i]] = new Vector3Byte(br);
                }
                else if (this.Type == InibinFlags.Float32ListVec3)
                {
                    this.Properities[Hashes[i]] = new Vector3(br);
                }
                else if (this.Type == InibinFlags.FixedPointFloatListVec2)
                {
                    this.Properities[Hashes[i]] = new Vector2Byte(br);
                }
                else if (this.Type == InibinFlags.Float32ListVec2)
                {
                    this.Properities[Hashes[i]] = new Vector2(br);
                }
                else if (this.Type == InibinFlags.FixedPointFloatListVec4)
                {
                    this.Properities[Hashes[i]] = new Vector4Byte(br);
                }
                else if (this.Type == InibinFlags.Float32ListVec4)
                {
                    this.Properities[Hashes[i]] = new Vector4(br);
                }
            }
        }
        public InibinSet(BinaryReader br, InibinFlags type, uint stringOffset)
        {
            this.Type = type;
            UInt16 ValueCount = br.ReadUInt16();
            List<UInt32> Hashes = new List<UInt32>();

            for (int i = 0; i < ValueCount; i++)
            {
                UInt32 hash = br.ReadUInt32();
                this.Properities.Add(hash, null);
                Hashes.Add(hash);
            }

            for(int i = 0; i < ValueCount; i++)
            {
                if (this.Type == InibinFlags.StringList)
                {
                    UInt16 offset = br.ReadUInt16();
                    long oldPos = br.BaseStream.Position;
                    br.BaseStream.Seek(offset + stringOffset, SeekOrigin.Begin);
                    string s = "";
                    char c = (char)0xff;
                    while (c != '\u0000')
                    {
                        c = br.ReadChar();
                        s += c;
                    }
                    this.Properities[Hashes[i]] = s;
                    br.BaseStream.Seek(oldPos, SeekOrigin.Begin);
                }
            }
        }
        public InibinSet(BinaryReader br, InibinFlags type, uint stringOffset, UInt32 valueCount)
        {
            this.Type = type;
            List<UInt32> Hashes = new List<UInt32>();

            for (int i = 0; i < valueCount; i++)
            {
                UInt32 hash = br.ReadUInt32();
                this.Properities.Add(hash, null);
                Hashes.Add(hash);

                UInt32 offset = br.ReadUInt32();
                long oldPos = br.BaseStream.Position;
                br.BaseStream.Seek(offset + stringOffset, SeekOrigin.Begin);
                string s = "";
                char c = (char)0xff;
                while (c != '\u0000')
                {
                    c = br.ReadChar();
                    s += c;
                }
                this.Properities[Hashes[i]] = s;
                br.BaseStream.Seek(oldPos, SeekOrigin.Begin);
            }
        }
    }
}
