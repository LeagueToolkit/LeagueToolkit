using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.BIN
{
    /// <summary>
    /// Represents a <see cref="KeyValuePair{TKey, TValue}"/> in a <see cref="BINFile"/>
    /// </summary>
    public class BINFileValuePair : IBINFileValue
    {
        /// <summary>
        /// The <see cref="BINFileValueType"/> of this <see cref="BINFileValuePair"/>'s Key 
        /// </summary>
        public BINFileValueType KeyType { get; private set; }
        /// <summary>
        /// The <see cref="BINFileValueType"/> of this <see cref="BINFileValuePair"/>'s Value 
        /// </summary>
        public BINFileValueType ValueType { get; private set; }
        /// <summary>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> of this <see cref="BINFileValuePair"/>
        /// </summary>
        public KeyValuePair<BINFileValue, BINFileValue> Pair { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="BINFileValuePair"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="keyType">The <see cref="BINFileValueType"/> of the Key of this <see cref="BINFileValuePair"/></param>
        /// <param name="valueType">The <see cref="BINFileValueType"/> of the Value of this <see cref="BINFileValuePair"/></param>
        public BINFileValuePair(BinaryReader br, BINFileValueType keyType, BINFileValueType valueType)
        {
            this.KeyType = keyType;
            this.ValueType = valueType;
            this.Pair = new KeyValuePair<BINFileValue, BINFileValue>(new BINFileValue(br, this, keyType), new BINFileValue(br, this, valueType));
        }

        /// <summary>
        /// Writes this <see cref="BINFileValuePair"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        /// <param name="writeType">Whether the Property and Type of the <see cref="KeyValuePair{TKey, TVal}"/> of this <see cref="BINFileValuePair"/> should be written</param>
        public void Write(BinaryWriter bw, bool writeType)
        {
            this.Pair.Key.Write(bw, writeType);
            this.Pair.Value.Write(bw, writeType);
        }

        /// <summary>
        /// Gets the size of this <see cref="BINFileValuePair"/>
        /// </summary>
        public int GetSize()
        {
            return this.Pair.Key.GetSize() + this.Pair.Value.GetSize();
        }
    }
}
