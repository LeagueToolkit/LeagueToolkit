using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    /// <summary>
    /// Represents a <see cref="BINFileEntry"/> inside of a <see cref="BINFile"/>
    /// </summary>
    public class BINFileEntry : IBINFileValue
    {
        /// <summary>
        /// Hash of the type of this <see cref="BINFileEntry"/>
        /// </summary>
        public uint Type { get; private set; }
        /// <summary>
        /// Hash of the name of this <see cref="BINFileEntry"/>
        /// </summary>
        public uint Property { get; private set; }
        /// <summary>
        /// A Collection of <see cref="BINFileValue"/>
        /// </summary>
        public List<BINFileValue> Values { get; private set; } = new List<BINFileValue>();

        /// <summary>
        /// Initializes a new <see cref="BINFileEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public BINFileEntry(BinaryReader br)
        {
            this.Type = br.ReadUInt32();
        }

        /// <summary>
        /// Reads the data of this <see cref="BINFileEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public void ReadData(BinaryReader br)
        {
            uint length = br.ReadUInt32();
            this.Property = br.ReadUInt32();
            ushort valueCount = br.ReadUInt16();
            for (int i = 0; i < valueCount; i++)
            {
                this.Values.Add(new BINFileValue(br, this));
            }
        }

        /// <summary>
        /// Writes this <see cref="BINFileEntry"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(GetSize());
            bw.Write(this.Property);
            bw.Write((ushort)this.Values.Count);
            foreach (BINFileValue value in this.Values)
            {
                value.Write(bw, true);
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="BINFileEntry"/> in bytes
        /// </summary>
        public int GetSize()
        {
            int size = 0;
            foreach (BINFileValue value in this.Values)
            {
                size += value.GetSize();
            }

            return size + 4 + 2;
        }
    }
}
