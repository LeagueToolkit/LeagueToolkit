using Fantome.Libraries.League.Helpers.Extensions;
using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Fantome.Libraries.League.IO.PropertyBin
{
    public abstract class BinTreeProperty : IEquatable<BinTreeProperty>
    {
        protected const int HEADER_SIZE = 5;

        public IBinTreeParent Parent { get; private set; }
        public BinPropertyType Type => this switch
        {
            BinTreeNone _ => BinPropertyType.None,
            BinTreeBool _ => BinPropertyType.Bool,
            BinTreeSByte _ => BinPropertyType.SByte,
            BinTreeByte _ => BinPropertyType.Byte,
            BinTreeInt16 _ => BinPropertyType.Int16,
            BinTreeUInt16 _ => BinPropertyType.UInt16,
            BinTreeInt32 _ => BinPropertyType.Int32,
            BinTreeUInt32 _ => BinPropertyType.UInt32,
            BinTreeInt64 _ => BinPropertyType.Int64,
            BinTreeUInt64 _ => BinPropertyType.UInt64,
            BinTreeFloat _ => BinPropertyType.Float,
            BinTreeVector2 _ => BinPropertyType.Vector2,
            BinTreeVector3 _ => BinPropertyType.Vector3,
            BinTreeVector4 _ => BinPropertyType.Vector4,
            BinTreeMatrix44 _ => BinPropertyType.Matrix44,
            BinTreeColor _ => BinPropertyType.Color,
            BinTreeString _ => BinPropertyType.String,
            BinTreeHash _ => BinPropertyType.Hash,
            BinTreeWadEntryLink _ => BinPropertyType.WadEntryLink,
            BinTreeContainer2 _ => BinPropertyType.Container2,
            BinTreeContainer _ => BinPropertyType.Container,
            BinTreeEmbedded _ => BinPropertyType.Embedded,
            BinTreeStructure _ => BinPropertyType.Structure,
            BinTreeObjectLink _ => BinPropertyType.ObjectLink,
            BinTreeOptional _ => BinPropertyType.Optional,
            BinTreeMap _ => BinPropertyType.Map,
            BinTreeBitBool _ => BinPropertyType.BitBool,
            _ => throw new InvalidOperationException("Cannot match BinTreeProperty to a valid BinPropertyType")
        };

        public uint NameHash { get; private set; }

        internal BinTreeProperty() { }
        protected BinTreeProperty(IBinTreeParent parent, uint nameHash)
        {
            this.Parent = parent;
            this.NameHash = nameHash;
        }

        internal static BinTreeProperty Read(BinaryReader br, IBinTreeParent parent, BinPropertyType? type = null)
        {
            uint nameHash = 0;
            BinPropertyType packedType;
            if (type is null)
            {
                nameHash = br.ReadUInt32();
                packedType = (BinPropertyType)br.ReadByte();
                type = BinUtilities.UnpackType(packedType);
            }

            return type switch
            {
                BinPropertyType.None => new BinTreeNone(br, parent, nameHash),
                BinPropertyType.Bool => new BinTreeBool(br, parent, nameHash),
                BinPropertyType.SByte => new BinTreeSByte(br, parent, nameHash),
                BinPropertyType.Byte => new BinTreeByte(br, parent, nameHash),
                BinPropertyType.Int16 => new BinTreeInt16(br, parent, nameHash),
                BinPropertyType.UInt16 => new BinTreeUInt16(br, parent, nameHash),
                BinPropertyType.Int32 => new BinTreeInt32(br, parent, nameHash),
                BinPropertyType.UInt32 => new BinTreeUInt32(br, parent, nameHash),
                BinPropertyType.Int64 => new BinTreeInt64(br, parent, nameHash),
                BinPropertyType.UInt64 => new BinTreeUInt64(br, parent, nameHash),
                BinPropertyType.Float => new BinTreeFloat(br, parent, nameHash),
                BinPropertyType.Vector2 => new BinTreeVector2(br, parent, nameHash),
                BinPropertyType.Vector3 => new BinTreeVector3(br, parent, nameHash),
                BinPropertyType.Vector4 => new BinTreeVector4(br, parent, nameHash),
                BinPropertyType.Matrix44 => new BinTreeMatrix44(br, parent, nameHash),
                BinPropertyType.Color => new BinTreeColor(br, parent, nameHash),
                BinPropertyType.String => new BinTreeString(br, parent, nameHash),
                BinPropertyType.Hash => new BinTreeHash(br, parent, nameHash),
                BinPropertyType.WadEntryLink => new BinTreeWadEntryLink(br, parent, nameHash),
                BinPropertyType.Container => new BinTreeContainer(br, parent, nameHash),
                BinPropertyType.Container2 => new BinTreeContainer2(br, parent, nameHash),
                BinPropertyType.Structure => new BinTreeStructure(br, parent, nameHash),
                BinPropertyType.Embedded => new BinTreeEmbedded(br, parent, nameHash),
                BinPropertyType.ObjectLink => new BinTreeObjectLink(br, parent, nameHash),
                BinPropertyType.Optional => new BinTreeOptional(br, parent, nameHash),
                BinPropertyType.Map => new BinTreeMap(br, parent, nameHash),
                BinPropertyType.BitBool => new BinTreeBitBool(br, parent, nameHash),
                _ => throw new InvalidOperationException("Invalid BinPropertyType: " + type),
            };
        }

        internal virtual void Write(BinaryWriter bw, bool writeHeader)
        {
            if (writeHeader) WriteHeader(bw);
            WriteContent(bw);
        }
        protected void WriteHeader(BinaryWriter bw)
        {
            bw.Write(this.NameHash);
            bw.Write((byte)BinUtilities.PackType(this.Type));
        }
        protected abstract void WriteContent(BinaryWriter bw);

        internal abstract int GetSize(bool includeHeader);

        public abstract bool Equals(BinTreeProperty other);
    }

    public sealed class BinTreeNone : BinTreeProperty
    {
        internal BinTreeNone(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {

        }

        protected override void WriteContent(BinaryWriter bw) { }

        internal override int GetSize(bool includeHeader) => includeHeader ? 5 : 0;

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeNone property && property.NameHash == this.NameHash;
        }
    }
    public sealed class BinTreeBool : BinTreeProperty
    {
        public bool Value { get; private set; }

        internal BinTreeBool(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadBoolean();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 1;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeBool property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator bool(BinTreeBool property) => property.Value;
    }
    public sealed class BinTreeSByte : BinTreeProperty
    {
        public sbyte Value { get; private set; }

        internal BinTreeSByte(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadSByte();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 1;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeSByte property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator sbyte(BinTreeSByte property) => property.Value;
    }
    public sealed class BinTreeByte : BinTreeProperty
    {
        public byte Value { get; private set; }

        internal BinTreeByte(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadByte();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 1;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeByte property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator byte(BinTreeByte property) => property.Value;
    }
    public sealed class BinTreeInt16 : BinTreeProperty
    {
        public short Value { get; private set; }

        internal BinTreeInt16(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadInt16();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 2;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeInt16 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator short(BinTreeInt16 property) => property.Value;
    }
    public sealed class BinTreeUInt16 : BinTreeProperty
    {
        public ushort Value { get; private set; }

        internal BinTreeUInt16(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt16();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 2;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeUInt16 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator ushort(BinTreeUInt16 property) => property.Value;
    }
    public sealed class BinTreeInt32 : BinTreeProperty
    {
        public int Value { get; private set; }

        internal BinTreeInt32(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadInt32();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeInt32 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator int(BinTreeInt32 property) => property.Value;
    }
    public sealed class BinTreeUInt32 : BinTreeProperty
    {
        public uint Value { get; private set; }

        internal BinTreeUInt32(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt32();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeUInt32 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator uint(BinTreeUInt32 property) => property.Value;
    }
    public sealed class BinTreeInt64 : BinTreeProperty
    {
        public long Value { get; private set; }

        internal BinTreeInt64(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadInt64();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 8;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeInt64 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator long(BinTreeInt64 property) => property.Value;
    }
    public sealed class BinTreeUInt64 : BinTreeProperty
    {
        public ulong Value { get; private set; }

        internal BinTreeUInt64(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt64();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 8;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeUInt64 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator ulong(BinTreeUInt64 property) => property.Value;
    }
    public sealed class BinTreeFloat : BinTreeProperty
    {
        public float Value { get; private set; }

        internal BinTreeFloat(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadSingle();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeFloat property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator float(BinTreeFloat property) => property.Value;
    }
    public sealed class BinTreeVector2 : BinTreeProperty
    {
        public Vector2 Value { get; private set; }

        internal BinTreeVector2(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadVector2();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteVector2(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 8;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeVector2 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Vector2(BinTreeVector2 property) => property.Value;
    }
    public sealed class BinTreeVector3 : BinTreeProperty
    {
        public Vector3 Value { get; private set; }

        internal BinTreeVector3(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadVector3();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteVector3(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 12;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeVector3 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Vector3(BinTreeVector3 property) => property.Value;
    }
    public sealed class BinTreeVector4 : BinTreeProperty
    {
        public Vector4 Value { get; private set; }

        internal BinTreeVector4(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadVector4();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteVector4(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 16;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeVector4 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Vector4(BinTreeVector4 property) => property.Value;
    }
    public sealed class BinTreeMatrix44 : BinTreeProperty
    {
        public Matrix4x4 Value { get; private set; }

        internal BinTreeMatrix44(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadMatrix4x4RowMajor();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteMatrix4x4RowMajor(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 64;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeMatrix44 property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Matrix4x4(BinTreeMatrix44 property) => property.Value;
    }
    public sealed class BinTreeColor : BinTreeProperty
    {
        public Color Value { get; private set; }

        internal BinTreeColor(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadColor(ColorFormat.RgbaU8);
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.WriteColor(this.Value, ColorFormat.RgbaU8);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeColor property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator Color(BinTreeColor property) => property.Value;
    }
    public sealed class BinTreeString : BinTreeProperty
    {
        public string Value { get; private set; }

        internal BinTreeString(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = Encoding.ASCII.GetString(br.ReadBytes(br.ReadUInt16()));
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write((ushort)this.Value.Length);
            bw.Write(Encoding.UTF8.GetBytes(this.Value));
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 2 + this.Value.Length;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeString property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator string(BinTreeString property) => property.Value;
    }
    public sealed class BinTreeHash : BinTreeProperty
    {
        public uint Value { get; private set; }

        internal BinTreeHash(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt32();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeHash property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator uint(BinTreeHash property) => property.Value;
    }
    public sealed class BinTreeWadEntryLink : BinTreeProperty
    {
        public ulong Value { get; private set; }

        internal BinTreeWadEntryLink() { }
        internal BinTreeWadEntryLink(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt64();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 8;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeWadEntryLink property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }
    }
    public class BinTreeContainer : BinTreeProperty, IBinTreeParent
    {
        public BinPropertyType PropertiesType { get; private set; }
        public List<BinTreeProperty> Properties { get; private set; } = new();

        internal BinTreeContainer() { }
        internal BinTreeContainer(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.PropertiesType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
            uint size = br.ReadUInt32();

            uint valueCount = br.ReadUInt32();
            for (int i = 0; i < valueCount; i++)
            {
                this.Properties.Add(BinTreeProperty.Read(br, this, this.PropertiesType));
            }
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            // Verify that all properties have the correct type and parent
            foreach (BinTreeProperty property in this.Properties)
            {
                if (property.Type != this.PropertiesType) throw new InvalidOperationException("Found a Property with an invalid Type");
                if (property.Parent != this) throw new InvalidOperationException("Found a Property with an invalid Parent");
            }

            bw.Write((byte)BinUtilities.PackType(this.PropertiesType));
            bw.Write(GetContentSize());
            bw.Write(this.Properties.Count);

            foreach (BinTreeProperty property in this.Properties)
            {
                if (property.Type != this.PropertiesType) throw new InvalidOperationException("Found a Property with an invalid Type");
                if (property.Parent != this) throw new InvalidOperationException("Found a Property with an invalid Parent");

                property.Write(bw, false);
            }
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4 + 1 + GetContentSize();
        }
        private int GetContentSize()
        {
            int size = 4;
            foreach (BinTreeProperty property in this.Properties)
            {
                size += property.GetSize(false);
            }

            return size;
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeContainer otherProperty && other is not BinTreeContainer2)
            {
                if (this.Properties.Count != otherProperty.Properties.Count) return false;
                if (this.PropertiesType != otherProperty.PropertiesType) return false;

                for (int i = 0; i < this.Properties.Count; i++)
                {
                    if (!this.Properties[i].Equals(otherProperty.Properties[i])) return false;
                }
            }

            return true;
        }
    }
    public sealed class BinTreeContainer2 : BinTreeContainer
    {
        internal BinTreeContainer2(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(br, parent, nameHash) { }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeContainer2 otherProperty)
            {
                if (this.Properties.Count != otherProperty.Properties.Count) return false;

                for (int i = 0; i < this.Properties.Count; i++)
                {
                    if (!this.Properties[i].Equals(otherProperty.Properties[i])) return false;
                }
            }

            return true;
        }
    }
    public class BinTreeStructure : BinTreeProperty, IBinTreeParent
    {
        public uint MetaClassHash { get; private set; }
        public List<BinTreeProperty> Properties { get; private set; } = new();

        internal BinTreeStructure() { }
        internal BinTreeStructure(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.MetaClassHash = br.ReadUInt32();
            if (this.MetaClassHash == 0) return; // Empty structure

            uint size = br.ReadUInt32();
            ushort propertyCount = br.ReadUInt16();
            for (int i = 0; i < propertyCount; i++)
            {
                this.Properties.Add(BinTreeProperty.Read(br, this));
            }
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.MetaClassHash);
            if (this.MetaClassHash == 0) return; // empty

            bw.Write(GetContentSize());
            bw.Write((ushort)this.Properties.Count);

            foreach (BinTreeProperty property in this.Properties)
            {
                property.Write(bw, true);
            }
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? HEADER_SIZE : 0;
            if (this.MetaClassHash == 0) // empty struct
            {
                return size + 4;
            }
            else
            {
                return size + 4 + 4 + GetContentSize();
            }
        }
        private int GetContentSize()
        {
            int size = 2;
            foreach (BinTreeProperty property in this.Properties)
            {
                size += property.GetSize(true);
            }
            return size;
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeStructure otherProperty && other is not BinTreeEmbedded)
            {
                if (this.MetaClassHash != otherProperty.MetaClassHash) return false;
                if (this.Properties.Count != otherProperty.Properties.Count) return false;

                for (int i = 0; i < this.Properties.Count; i++)
                {
                    if (!this.Properties[i].Equals(otherProperty.Properties[i])) return false;
                }
            }

            return true;
        }
    }
    public sealed class BinTreeEmbedded : BinTreeStructure
    {
        internal BinTreeEmbedded(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(br, parent, nameHash) { }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeEmbedded otherProperty)
            {
                if (this.MetaClassHash != otherProperty.MetaClassHash) return false;
                if (this.Properties.Count != otherProperty.Properties.Count) return false;

                for (int i = 0; i < this.Properties.Count; i++)
                {
                    if (!this.Properties[i].Equals(otherProperty.Properties[i])) return false;
                }
            }

            return true;
        }
    }
    public sealed class BinTreeObjectLink : BinTreeProperty
    {
        public uint Value { get; private set; }

        internal BinTreeObjectLink(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadUInt32();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? 5 : 0;
            return size + 4;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeObjectLink property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator uint(BinTreeObjectLink property) => property.Value;
    }
    public sealed class BinTreeOptional : BinTreeProperty, IBinTreeParent
    {
        public BinPropertyType ValueType { get; private set; }
        public BinTreeProperty Value { get; private set; }

        internal BinTreeOptional(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.ValueType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
            bool isSome = br.ReadBoolean();

            if (isSome)
            {
                this.Value = BinTreeProperty.Read(br, this, this.ValueType);
            }
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write((byte)BinUtilities.PackType(this.ValueType));
            bw.Write(this.Value is not null);

            if (this.Value is not null)
            {
                this.Value.Write(bw, false);
            }
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = 2 + (includeHeader ? 5 : 0);

            if (this.Value is not null)
            {
                size += this.Value.GetSize(false);
            }

            return size;
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeOptional otherProperty)
            {
                if (this.ValueType != otherProperty.ValueType) return false;
                return this.Value.Equals(otherProperty.Value);
            }

            return true;
        }
    }
    public sealed class BinTreeMap : BinTreeProperty, IBinTreeParent
    {
        public BinPropertyType KeyType { get; private set; }
        public BinPropertyType ValueType { get; private set; }
        public Dictionary<BinTreeProperty, BinTreeProperty> Map { get; private set; } = new();

        internal BinTreeMap(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.KeyType = (BinPropertyType)br.ReadByte();
            this.ValueType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte());
            uint size = br.ReadUInt32();
            uint valueCount = br.ReadUInt32();

            for (int i = 0; i < valueCount; i++)
            {
                this.Map.Add(BinTreeProperty.Read(br, this, this.KeyType), BinTreeProperty.Read(br, this, this.ValueType));
            }
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write((byte)this.KeyType);
            bw.Write((byte)BinUtilities.PackType(this.ValueType));
            bw.Write(GetContentSize());
            bw.Write(this.Map.Count);

            foreach (var pair in this.Map)
            {
                pair.Key.Write(bw, false);
                pair.Value.Write(bw, false);
            }
        }

        internal override int GetSize(bool includeHeader)
        {
            return 1 + 1 + 4 + GetContentSize() + (includeHeader ? 5 : 0);
        }
        private int GetContentSize()
        {
            int size = 4;
            foreach (var pair in this.Map)
            {
                size += pair.Key.GetSize(false) + pair.Value.GetSize(false);
            }
            return size;
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeMap otherProperty)
            {
                if (this.KeyType != otherProperty.KeyType) return false;
                if (this.ValueType != otherProperty.ValueType) return false;
                if (this.Map.Count != otherProperty.Map.Count) return false;

                foreach(var entry in this.Map)
                {
                    if (otherProperty.Map.TryGetValue(entry.Key, out BinTreeProperty value))
                    {
                        if (!entry.Value.Equals(value)) return false;
                    }
                    else return false;
                }
            }

            return true;
        }

        public static implicit operator Dictionary<BinTreeProperty, BinTreeProperty>(BinTreeMap property) => property.Map;
    }
    public sealed class BinTreeBitBool : BinTreeProperty
    {
        public byte Value { get; private set; }

        internal BinTreeBitBool(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
            this.Value = br.ReadByte();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(this.Value);
        }

        internal override int GetSize(bool includeHeader)
        {
            int size = includeHeader ? HEADER_SIZE : 0;
            return size + 1;
        }

        public override bool Equals(BinTreeProperty other)
        {
            return other is BinTreeBitBool property
                && this.NameHash == property.NameHash
                && this.Value == property.Value;
        }

        public static implicit operator byte(BinTreeBitBool property) => property.Value;
    }

    public enum BinPropertyType : byte
    {
        // PRIMITIVE TYPES \\
        None,
        Bool,
        SByte,
        Byte,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Float,
        Vector2,
        Vector3,
        Vector4,
        Matrix44,
        Color,
        String,
        Hash,
        WadEntryLink,
        // COMPLEX TYPES \\
        Container,
        Container2,
        Structure,
        Embedded,
        ObjectLink,
        Optional,
        Map,
        BitBool
    }
}
