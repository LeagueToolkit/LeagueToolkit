using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.PropertyBin.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.PropertyBin
{
    public abstract class BinTreeProperty : IEquatable<BinTreeProperty>
    {
        protected const int HEADER_SIZE = 5;

        public IBinTreeParent Parent { get; internal set; }
        public abstract BinPropertyType Type { get; }

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
                BinPropertyType.UnorderedContainer => new BinTreeUnorderedContainer(br, parent, nameHash),
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
        UnorderedContainer,
        Structure,
        Embedded,
        ObjectLink,
        Optional,
        Map,
        BitBool
    }
}
