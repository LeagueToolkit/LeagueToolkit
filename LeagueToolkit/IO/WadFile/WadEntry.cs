using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.IO.WadFile
{
    public class WadEntry : IComparable<WadEntry>
    {
        internal const int TOC_SIZE_V3 = 32;

        public ulong XXHash { get; private set; }

        public int CompressedSize { get; internal set; }
        public int UncompressedSize { get; internal set; }

        public WadEntryType Type { get; private set; }

        public WadEntryChecksumType ChecksumType { get; private set; }
        public byte[] Checksum { get; internal set; }

        public string FileRedirection { get; private set; }

        internal uint _dataOffset;
        internal bool _isDuplicated;

        internal readonly Wad _wad;
 
        internal WadEntry(Wad wad, ulong xxhash, int compressedSize, int uncompressedSize, WadEntryType entryType,
            WadEntryChecksumType checksumType, byte[] checksum, string fileRedirection, uint dataOffset)
        {
            this._wad = wad;
            this.XXHash = xxhash;
            this.CompressedSize = compressedSize;
            this.UncompressedSize = uncompressedSize;
            this.Type = entryType;
            this.ChecksumType = checksumType;
            this.Checksum = checksum;
            this.FileRedirection = fileRedirection;
            this._dataOffset = dataOffset;
        }
        internal WadEntry(Wad wad, BinaryReader br, byte major, byte minor)
        {
            this._wad = wad;
            this.XXHash = br.ReadUInt64();
            this._dataOffset = br.ReadUInt32();
            this.CompressedSize = br.ReadInt32();
            this.UncompressedSize = br.ReadInt32();
            this.Type = (WadEntryType)br.ReadByte();
            this._isDuplicated = br.ReadBoolean();
            br.ReadUInt16(); // pad 
            if (major >= 2)
            {
                this.Checksum = br.ReadBytes(8);

                if (major == 3 && minor == 1) this.ChecksumType = WadEntryChecksumType.XXHash3;
                else this.ChecksumType = WadEntryChecksumType.SHA256;
            }

            if (this.Type == WadEntryType.FileRedirection)
            {
                long currentPosition = br.BaseStream.Position;
                br.BaseStream.Seek(this._dataOffset, SeekOrigin.Begin);
                this.FileRedirection = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            }
        }

        internal void Write(BinaryWriter bw, uint major)
        {
            bw.Write(this.XXHash);
            bw.Write(this._dataOffset);
            bw.Write(this.CompressedSize);
            bw.Write(this.UncompressedSize);
            bw.Write((byte)this.Type);
            bw.Write(this._isDuplicated);
            bw.Write((ushort)0); // pad
            if (major >= 2)
            {
                bw.Write(this.Checksum);
            }
        }

        public WadEntryDataHandle GetDataHandle()
        {
            return new WadEntryDataHandle(this);
        }

        public int CompareTo(WadEntry other)
        {
            return this.XXHash.CompareTo(other.XXHash);
        }
    }

    public enum WadEntryType : byte
    {
        /// <summary>
        /// The Data of the <see cref="WadEntry"/> is uncompressed
        /// </summary>
        Uncompressed,
        /// <summary>
        /// The Data of the <see cref="WadEntry"/> is compressed with GZip
        /// </summary>
        GZipCompressed,
        /// <summary>
        /// The Data of this <see cref="WadEntry"/> is a file redirection
        /// </summary>
        FileRedirection,
        /// <summary>
        /// The Data of this <see cref="WadEntry"/> is compressed with ZStandard
        /// </summary>
        ZStandardCompressed
    }

    public enum WadEntryChecksumType
    {
        SHA256,
        XXHash3
    }
}
