using Fantome.Libraries.League.Helpers.BIN;
using Fantome.Libraries.League.Helpers.Cryptography;
using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fantome.Libraries.League.IO.BIN
{
    public class BINValue : IBINValue, IEquatable<BINValue>
    {
        public BINValueType? Type { get; private set; }
        public IBINValue Parent { get; private set; }
        public uint Property { get; set; }
        public object Value { get; private set; }
        public BINValue this[string path]
        {
            get
            {
                string[] properties = path.Split('/');
                string property = properties[0];

                //Build next recursive property path
                string nextPath = string.Empty;
                if (properties.Length != 1)
                {
                    for (int i = 1; i < properties.Length; i++)
                    {
                        nextPath += properties[i];

                        if (i + 1 != properties.Length)
                        {
                            nextPath += '/';
                        }
                    }
                }

                //Determine the property type
                if(Regex.IsMatch(property, @"^\[\d+\]"))
                {
                    int valueIndex = int.Parse(property.Substring(1, property.IndexOf(']') - 1));

                    return (this.Value as BINContainer).Values[valueIndex];
                }
                else if(property.Contains('[') && !property.Contains('.'))
                {
                    int startIndex = property.IndexOf('[');
                    int valueIndex = int.Parse(property.Substring(startIndex + 1, property.IndexOf(']') - startIndex - 1));

                    if(this.Type == BINValueType.Container && 
                        (this.Value as BINContainer).EntryType == BINValueType.Embedded ||
                        (this.Value as BINContainer).EntryType == BINValueType.Structure)
                    {
                        BINContainer container = this.Value as BINContainer;

                        return container.Values[valueIndex];
                    }
                }
                else if(property.Contains('.'))
                {
                    string[] structureProperties = property.Split('.');
                    string structureProperty = structureProperties[0];
                    string fieldProperty = structureProperties[1];
                    int? structureIndex = null;

                    //Check if structure property has an array index
                    if(structureProperty.Contains('['))
                    {
                        int startIndex = structureProperty.IndexOf('[');
                        structureIndex = int.Parse(structureProperty.Substring(startIndex + 1, structureProperty.IndexOf(']') - startIndex - 1));
                        structureProperty = structureProperty.Remove(structureProperty.IndexOf('['));
                    }

                    uint structureHash = 0;
                    uint fieldHash;
                    if (structureIndex == null && !uint.TryParse(structureProperty, out structureHash))
                    {
                        structureHash = Cryptography.FNV32Hash(structureProperty);
                    }
                    if (!uint.TryParse(fieldProperty, out fieldHash))
                    {
                        fieldHash = Cryptography.FNV32Hash(fieldProperty);
                    }

                    BINStructure structure = (structureIndex == null) ? this.Value as BINStructure : (this.Value as BINContainer).Values[(int)structureIndex].Value as BINStructure;
                    BINValue fieldValue = structure[fieldHash];
                    if(nextPath != string.Empty)
                    {
                        return structure[fieldHash][nextPath];
                    }
                    else
                    {
                        return structure[fieldHash];
                    }
                }
                else if(this.Type == BINValueType.Map)
                {
                    BINMap map = this.Value as BINMap;

                    if(map.KeyType == BINValueType.Byte)
                    {
                        return map[map.Values.Keys.Where(x => byte.Parse(property).Equals(x.Value)).First()];
                    }
                    else if(map.KeyType == BINValueType.UInt16)
                    {
                        return map[map.Values.Keys.Where(x => ushort.Parse(property).Equals(x.Value)).First()];
                    }
                    else if (map.KeyType == BINValueType.UInt32)
                    {
                        return map[map.Values.Keys.Where(x => uint.Parse(property).Equals(x.Value)).First()];
                    }
                    else if (map.KeyType == BINValueType.UInt64)
                    {
                        return map[map.Values.Keys.Where(x => ulong.Parse(property).Equals(x.Value)).First()];
                    }
                    else if (map.KeyType == BINValueType.String)
                    {
                        return map[map.Values.Keys.Where(x => string.Equals(property, x.Value)).First()];
                    }
                    else if (map.KeyType == BINValueType.Hash)
                    {
                        return map[map.Values.Keys.Where(x => uint.Parse(property).Equals(x.Value)).First()];
                    }
                    else
                    {
                        throw new Exception("Unsupported Map Key Type: " + map.KeyType);
                    }

                }
                
                return null;
            }
        }
        public BINValue this[uint hash]
        {
            get
            {
                return (this.Value as IBINValue)[hash];
            }
        }

        private bool _typeRead;

        public BINValue(IBINValue parent, uint property, object value, BINValueType type)
        {
            this.Parent = parent;
            this.Property = property;
            this.Value = value;
            this.Type = type;
        }

        public BINValue(BinaryReader br, IBINValue parent, BINValueType? type = null)
        {
            this.Parent = parent;
            this.Type = type;
            if (this.Type == null)
            {
                this.Property = br.ReadUInt32();
                this.Type = BINUtilities.UnpackType((BINValueType)br.ReadByte());
                this._typeRead = true;
            }

            if (this.Type == BINValueType.None)
            {
                this.Value = null;
            }
            else if (this.Type == BINValueType.Boolean)
            {
                this.Value = br.ReadBoolean();
            }
            else if (this.Type == BINValueType.SByte)
            {
                this.Value = br.ReadSByte();
            }
            else if (this.Type == BINValueType.Byte)
            {
                this.Value = br.ReadByte();
            }
            else if (this.Type == BINValueType.Int16)
            {
                this.Value = br.ReadInt16();
            }
            else if (this.Type == BINValueType.UInt16)
            {
                this.Value = br.ReadUInt16();
            }
            else if (this.Type == BINValueType.Int32)
            {
                this.Value = br.ReadInt32();
            }
            else if (this.Type == BINValueType.UInt32)
            {
                this.Value = br.ReadUInt32();
            }
            else if (this.Type == BINValueType.Int64)
            {
                this.Value = br.ReadInt64();
            }
            else if (this.Type == BINValueType.UInt64)
            {
                this.Value = br.ReadUInt64();
            }
            else if (this.Type == BINValueType.Float)
            {
                this.Value = br.ReadSingle();
            }
            else if (this.Type == BINValueType.FloatVector2)
            {
                this.Value = new Vector2(br);
            }
            else if (this.Type == BINValueType.FloatVector3)
            {
                this.Value = new Vector3(br);
            }
            else if (this.Type == BINValueType.FloatVector4)
            {
                this.Value = new Vector4(br);
            }
            else if (this.Type == BINValueType.Matrix44)
            {
                this.Value = new R3DMatrix44(br);
            }
            else if (this.Type == BINValueType.Color)
            {
                this.Value = new ColorRGBAVector4Byte(br);
            }
            else if (this.Type == BINValueType.String)
            {
                this.Value = Encoding.ASCII.GetString(br.ReadBytes(br.ReadUInt16()));
            }
            else if (this.Type == BINValueType.Hash)
            {
                this.Value = br.ReadUInt32();
            }
            else if (this.Type == BINValueType.Container)
            {
                this.Value = new BINContainer(br, this);
            }
            else if (this.Type == BINValueType.Structure || this.Type == BINValueType.Embedded)
            {
                this.Value = new BINStructure(br, this);
            }
            else if (this.Type == BINValueType.LinkOffset)
            {
                this.Value = br.ReadUInt32();
            }
            else if (this.Type == BINValueType.Optional)
            {
                this.Value = new BINOptional(br, this);
            }
            else if (this.Type == BINValueType.Map)
            {
                this.Value = new BINMap(br, this);
            }
            else if (this.Type == BINValueType.FlagsBoolean)
            {
                this.Value = br.ReadBoolean();
            }
            else
            {
                throw new Exception("An Unknown Value Type was encountered: " + (byte)this.Type);
            }
        }

        public void Write(BinaryWriter bw, bool writeType)
        {
            if (writeType)
            {
                bw.Write(this.Property);
                bw.Write((byte)BINUtilities.PackType(this.Type.Value));
            }

            if (this.Type == BINValueType.None)
            {

            }
            else if (this.Type == BINValueType.Boolean)
            {
                bw.Write((bool)this.Value);
            }
            else if (this.Type == BINValueType.SByte)
            {
                bw.Write((sbyte)this.Value);
            }
            else if (this.Type == BINValueType.Byte)
            {
                bw.Write((byte)this.Value);
            }
            else if (this.Type == BINValueType.Int16)
            {
                bw.Write((short)this.Value);
            }
            else if (this.Type == BINValueType.UInt16)
            {
                bw.Write((ushort)this.Value);
            }
            else if (this.Type == BINValueType.Int32)
            {
                bw.Write((int)this.Value);
            }
            else if (this.Type == BINValueType.UInt32)
            {
                bw.Write((uint)this.Value);
            }
            else if (this.Type == BINValueType.Int64)
            {
                bw.Write((long)this.Value);
            }
            else if (this.Type == BINValueType.UInt64)
            {
                bw.Write((ulong)this.Value);
            }
            else if (this.Type == BINValueType.Float)
            {
                bw.Write((float)this.Value);
            }
            else if (this.Type == BINValueType.FloatVector2)
            {
                ((Vector2)this.Value).Write(bw);
            }
            else if (this.Type == BINValueType.FloatVector3)
            {
                ((Vector2)this.Value).Write(bw);
            }
            else if (this.Type == BINValueType.FloatVector4)
            {
                (this.Value as Vector4).Write(bw);
            }
            else if (this.Type == BINValueType.Matrix44)
            {
                (this.Value as R3DMatrix44).Write(bw);
            }
            else if (this.Type == BINValueType.Color)
            {
                (this.Value as ColorRGBAVector4Byte).Write(bw);
            }
            else if (this.Type == BINValueType.String)
            {
                string value = this.Value as string;
                bw.Write((ushort)value.Length);
                bw.Write(Encoding.ASCII.GetBytes(value));
            }
            else if (this.Type == BINValueType.Hash)
            {
                bw.Write((uint)this.Value);
            }
            else if (this.Type == BINValueType.Container)
            {
                (this.Value as BINContainer).Write(bw);
            }
            else if (this.Type == BINValueType.Structure || this.Type == BINValueType.Embedded)
            {
                (this.Value as BINStructure).Write(bw);
            }
            else if (this.Type == BINValueType.LinkOffset)
            {
                bw.Write((uint)this.Value);
            }
            else if (this.Type == BINValueType.Optional)
            {
                (this.Value as BINOptional).Write(bw);
            }
            else if (this.Type == BINValueType.Map)
            {
                (this.Value as BINMap).Write(bw);
            }
            else if (this.Type == BINValueType.FlagsBoolean)
            {
                bw.Write((bool)this.Value);
            }
        }

        public uint GetSize()
        {
            uint size = this._typeRead ? (uint)5 : 0;

            switch (this.Type)
            {
                case BINValueType.None:
                    break;

                case BINValueType.Boolean:
                case BINValueType.SByte:
                case BINValueType.Byte:
                case BINValueType.FlagsBoolean:
                    size += 1;
                    break;

                case BINValueType.Int16:
                case BINValueType.UInt16:
                    size += 2;
                    break;

                case BINValueType.Int32:
                case BINValueType.UInt32:
                case BINValueType.Hash:
                case BINValueType.LinkOffset:
                case BINValueType.Float:
                case BINValueType.Color:
                    size += 4;
                    break;

                case BINValueType.Int64:
                case BINValueType.UInt64:
                case BINValueType.FloatVector2:
                    size += 8;
                    break;

                case BINValueType.FloatVector3:
                    size += 12;
                    break;
                case BINValueType.FloatVector4:
                    size += 16;
                    break;

                case BINValueType.Matrix44:
                    size += 64;
                    break;

                case BINValueType.String:
                    size += 2 + (uint)(this.Value as string).Length;
                    break;

                case BINValueType.Container:
                case BINValueType.Structure:
                case BINValueType.Embedded:
                case BINValueType.Optional:
                case BINValueType.Map:
                    size += (this.Value as IBINValue).GetSize();
                    break;
            }

            return size;
        }

        public string GetPath(bool excludeEntry = true)
        {
            string path = "";

            if (this.Property == 0 && this.Parent is BINContainer)
            {
                path += string.Format("{0}/[{1}]", this.Parent.GetPath(excludeEntry), (this.Parent as BINContainer).Values.IndexOf(this));
            }
            else if(this.Property == 0 && this.Parent is BINMap)
            {
                BINMap map = this.Parent as BINMap;
                if(map.Values.ContainsValue(this))
                {
                    path += map.Values.First(x => x.Value == this).Key.GetPath(excludeEntry);
                }
                else
                {
                    if (map.KeyType == BINValueType.Byte)
                    {
                        path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), (byte)this.Value);
                    }
                    else if (map.KeyType == BINValueType.UInt16)
                    {
                        path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), (ushort)this.Value);
                    }
                    else if (map.KeyType == BINValueType.UInt32)
                    {
                        path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), (uint)this.Value);
                    }
                    else if (map.KeyType == BINValueType.UInt64)
                    {
                        path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), (ulong)this.Value);
                    }
                    else if (map.KeyType == BINValueType.String)
                    {
                        path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), (string)this.Value);
                    }
                    else if (map.KeyType == BINValueType.Hash)
                    {
                        path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), (uint)this.Value);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            else if(this.Property == 0 && this.Parent is BINOptional)
            {
                path += this.Parent.GetPath(excludeEntry);
            }
            else if(this.Parent is BINStructure)
            {
                path += string.Format("{0}.{1}", this.Parent.GetPath(excludeEntry), BINGlobal.GetField(this.Property));
            }
            else if(this.Parent is BINEntry)
            {
                path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), BINGlobal.GetField(this.Property));
            }
            else
            {
                path += string.Format("{0}/{1}", this.Parent.GetPath(excludeEntry), BINGlobal.GetClass(this.Property));
            }

            return excludeEntry ? path.Remove(0, path.IndexOf('/') + 1) : path;
        }

        public bool Equals(BINValue other)
        {
            if(this.Property != other.Property || this.Type != other.Type)
            {
                return false;
            }

            if (this.Type == BINValueType.None)
            {
                return true;
            }
            else if (this.Type == BINValueType.Boolean)
            {
                return (bool)this.Value == (bool)other.Value;
            }
            else if (this.Type == BINValueType.SByte)
            {
                return (sbyte)this.Value == (sbyte)other.Value;
            }
            else if (this.Type == BINValueType.Byte)
            {
                return (byte)this.Value == (byte)other.Value;
            }
            else if (this.Type == BINValueType.Int16)
            {
                return (short)this.Value == (short)other.Value;
            }
            else if (this.Type == BINValueType.UInt16)
            {
                return (ushort)this.Value == (ushort)other.Value;
            }
            else if (this.Type == BINValueType.Int32)
            {
                return (int)this.Value == (int)other.Value;
            }
            else if (this.Type == BINValueType.UInt32)
            {
                return (uint)this.Value == (uint)other.Value;
            }
            else if (this.Type == BINValueType.Int64)
            {
                return (long)this.Value == (long)other.Value;
            }
            else if (this.Type == BINValueType.UInt64)
            {
                return (ulong)this.Value == (ulong)other.Value;
            }
            else if (this.Type == BINValueType.Float)
            {
                return (float)this.Value == (float)other.Value;
            }
            else if (this.Type == BINValueType.FloatVector2)
            {
                return (Vector2)this.Value == (Vector2)other.Value;
            }
            else if (this.Type == BINValueType.FloatVector3)
            {
                return (Vector3)this.Value == (Vector3)other.Value;
            }
            else if (this.Type == BINValueType.FloatVector4)
            {
                return (this.Value as Vector4).Equals(other.Value as Vector4);
            }
            else if (this.Type == BINValueType.Matrix44)
            {
                return (this.Value as R3DMatrix44).Equals(other.Value as R3DMatrix44);
            }
            else if (this.Type == BINValueType.Color)
            {
                return (this.Value as ColorRGBAVector4Byte).Equals(other.Value as ColorRGBAVector4Byte);
            }
            else if (this.Type == BINValueType.String)
            {
                return (string)this.Value == (string)other.Value;
            }
            else if (this.Type == BINValueType.Hash)
            {
                return (uint)this.Value == (uint)other.Value;
            }
            else if (this.Type == BINValueType.Container)
            {
                return (this.Value as BINContainer).Equals(other.Value as BINContainer);
            }
            else if (this.Type == BINValueType.Structure || this.Type == BINValueType.Embedded)
            {
                return (this.Value as BINStructure).Equals(other.Value as BINStructure);
            }
            else if (this.Type == BINValueType.LinkOffset)
            {
                return (uint)this.Value == (uint)other.Value;
            }
            else if (this.Type == BINValueType.Optional)
            {
                return (this.Value as BINOptional).Equals(other.Value as BINOptional);
            }
            else if (this.Type == BINValueType.Map)
            {
                return (this.Value as BINMap).Equals(other.Value as BINMap);
            }
            else if (this.Type == BINValueType.FlagsBoolean)
            {
                return (bool)this.Value == (bool)other.Value;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Type of a <see cref="BINValue"/>
    /// </summary>
    public enum BINValueType : byte
    {
        /// <summary>
        /// Represents an empty value
        /// </summary>
        None = 0,
        /// <summary>
        /// Represents a <see cref="bool"/> value
        /// </summary>
        Boolean = 1,
        /// <summary>
        /// Represents an <see cref="sbyte"/> value
        /// </summary>
        SByte = 2,
        /// <summary>
        /// Represents a <see cref="byte"/> value
        /// </summary>
        Byte = 3,
        /// <summary>
        /// Represents a <see cref="short"/> value
        /// </summary>
        Int16 = 4,
        /// <summary>
        /// Represents a <see cref="ushort"/> value
        /// </summary>
        UInt16 = 5,
        /// <summary>
        /// Represents an <see cref="int"/> value
        /// </summary>
        Int32 = 6,
        /// <summary>
        /// Represents a <see cref="uint"/> value
        /// </summary>
        UInt32 = 7,
        /// <summary>
        /// Reperesents a <see cref="long"/> value
        /// </summary>
        Int64 = 8,
        /// <summary>
        /// Represents a <see cref="ulong"/> value
        /// </summary>
        UInt64 = 9,
        /// <summary>
        /// Represents a <see cref="float"/> value
        /// </summary>
        Float = 10,
        /// <summary>
        /// Represents a <see cref="float"/> Vector2 value
        /// </summary>
        FloatVector2 = 11,
        /// <summary>
        /// Represents a <see cref="float"/> Vector3 value
        /// </summary>
        FloatVector3 = 12,
        /// <summary>
        /// Represents a <see cref="float"/> Vector4 value
        /// </summary>
        FloatVector4 = 13,
        /// <summary>
        /// Represents a <see cref="R3DMatrix44"/> value
        /// </summary>
        Matrix44 = 14,
        /// <summary>
        /// Represents a <see cref="ColorRGBAVector4Byte"/> value
        /// </summary>
        Color = 15,
        /// <summary>
        /// Represents a <see cref="string"/> value
        /// </summary>
        String = 16,
        /// <summary>
        /// Represents a <see cref="uint"/> value which is a hash
        /// </summary>
        Hash = 17,
        /// <summary>
        /// Represents a Value Container
        /// </summary>
        Container = 18,
        Container2 = 19,
        /// <summary>
        /// Represents a Structure
        /// </summary>
        Structure = 19 + 1,
        /// <summary>
        /// Represents an Embedded Structure
        /// </summary>
        Embedded = 20 + 1,
        /// <summary>
        /// Represents a <see cref="uint"/> value which links to another entry
        /// </summary>
        LinkOffset = 21 + 1,
        /// <summary>
        /// Represents an Optional Value
        /// </summary>
        Optional = 22 + 1,
        /// <summary>
        /// Represents a List which holds Key-Value values
        /// </summary>
        Map = 23 + 1,
        /// <summary>
        /// Represents a <see cref="bool"/>
        /// </summary>
        FlagsBoolean = 24 + 1
    }
}