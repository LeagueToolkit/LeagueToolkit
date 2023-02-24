using LeagueToolkit.Core.Animation;
using LeagueToolkit.Utils;
using System.Buffers.Binary;

namespace LeagueToolkit.Tests.Utilities;

public class LeagueFileTests
{
    public class GetFileType
    {
        [Fact]
        public void Should_Return_StaticObjectBinary_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.StaticMeshBinary, LeagueFile.GetFileType("r3d2Mesh"u8));
        }

        [Fact]
        public void Should_Return_Skeleton_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.Skeleton, LeagueFile.GetFileType("r3d2sklt"u8));
        }

        [Fact]
        public void Should_Return_Animation_If_Magic_Matches_Uncompressed()
        {
            Assert.Equal(LeagueFileType.Animation, LeagueFile.GetFileType("r3d2anmd"u8));
        }

        [Fact]
        public void Should_Return_Animation_If_Magic_Matches_Compressed()
        {
            Assert.Equal(LeagueFileType.Animation, LeagueFile.GetFileType("r3d2canm"u8));
        }

        [Fact]
        public void Should_Return_WwisePackage_If_Magic_Matches()
        {
            Span<byte> magic = stackalloc byte[8];

            "r3d2"u8.CopyTo(magic);
            magic[4] = 1;

            Assert.Equal(LeagueFileType.WwisePackage, LeagueFile.GetFileType(magic));
        }

        [Fact]
        public void Should_Return_Png_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.Png, LeagueFile.GetFileType("\0PNG"u8));
        }

        [Fact]
        public void Should_Return_Dds_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.TextureDds, LeagueFile.GetFileType("DDS "u8));
        }

        [Fact]
        public void Should_Return_SimpleSKin_If_FormatToken_Matches()
        {
            Span<byte> magic = stackalloc byte[4];

            BinaryPrimitives.WriteUInt32LittleEndian(magic, 0x00112233);

            Assert.Equal(LeagueFileType.SimpleSkin, LeagueFile.GetFileType(magic));
        }

        [Fact]
        public void Should_Return_PropertyBin_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.PropertyBin, LeagueFile.GetFileType("PROP"u8));
        }

        [Fact]
        public void Should_Return_WwiseBank_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.WwiseBank, LeagueFile.GetFileType("BKHD"u8));
        }

        [Fact]
        public void Should_Return_WorldGeometry_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.WorldGeometry, LeagueFile.GetFileType("WGEO"u8));
        }

        [Fact]
        public void Should_Return_MapGeometry_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.MapGeometry, LeagueFile.GetFileType("OEGM"u8));
        }

        [Fact]
        public void Should_Return_StaticMeshAscii_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.StaticMeshAscii, LeagueFile.GetFileType("[ObjectBegin]"u8));
        }

        [Fact]
        public void Should_Return_LuaObj_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.LuaObj, LeagueFile.GetFileType("\0LuaQ"u8));
        }

        [Fact]
        public void Should_Return_Preload_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.Preload, LeagueFile.GetFileType("PreLoad"u8));
        }

        [Fact]
        public void Should_Return_RiotStringTable_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.RiotStringTable, LeagueFile.GetFileType("RST"u8));
        }

        [Fact]
        public void Should_Return_PropertyBinOverride_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.PropertyBinOverride, LeagueFile.GetFileType("PTCH"u8));
        }

        [Fact]
        public void Should_Return_Jpeg_If_Magic_Matches()
        {
            Span<byte> magic = stackalloc byte[4];

            magic[0] = 0xFF;
            magic[1] = 0xD8;
            magic[2] = 0xFF;
            magic[3] = 123;

            Assert.Equal(LeagueFileType.Jpeg, LeagueFile.GetFileType(magic));
        }

        [Fact]
        public void Should_Return_Skeleton_If_FormatToken_Matches()
        {
            Span<byte> magic = stackalloc byte[8];

            BinaryPrimitives.WriteUInt32LittleEndian(magic[4..], 0x22FD4FC3);

            Assert.Equal(LeagueFileType.Skeleton, LeagueFile.GetFileType(magic));
        }

        [Fact]
        public void Should_Return_Texture_If_Magic_Matches()
        {
            Assert.Equal(LeagueFileType.Texture, LeagueFile.GetFileType("TEX\0"u8));
        }
    }
}
