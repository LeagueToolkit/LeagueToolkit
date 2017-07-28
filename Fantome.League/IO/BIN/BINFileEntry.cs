using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINFileEntry
    {
        public BINFileEntryType Type { get; private set; }
        public UInt32 Property { get; private set; }
        public List<BINFileValue> Values { get; private set; } = new List<BINFileValue>();
        public BINFileEntry(BinaryReader br)
        {
            this.Type = (BINFileEntryType)br.ReadUInt32();
        }

        public void ReadData(BinaryReader br)
        {
            uint length = br.ReadUInt32();
            this.Property = br.ReadUInt32();
            ushort valueCount = br.ReadUInt16();
            for(int i = 0; i < valueCount; i++)
            {
                this.Values.Add(new BINFileValue(br, this));
            }
        }
    }

    public enum BINFileEntryType : UInt32
    {
        CharacterRecord = 602544405,
        SkinCharacterDataProperties = 2607278582,
        CharacterAnimations = 4126869447,
        CharacterSpells = 1585338886,
        CharacterMeta = 4160558231,
        InteractionData = 1250691283,
        ItemData = 608970470,
        MapAudio = 3010308524
    }
}
