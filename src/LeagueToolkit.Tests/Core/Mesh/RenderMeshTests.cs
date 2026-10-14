using System.Numerics;
using System.Text;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using Xunit;

namespace LeagueToolkit.Tests.Core.Mesh;

public sealed class RenderMeshTests
{
    [Fact]
    public void IdentifiesKnownMagicAndBothFileExtensions()
    {
        Assert.Equal(LeagueToolkit.Utils.LeagueFileType.RenderMeshGmesh, LeagueToolkit.Utils.LeagueFile.GetFileType("GMSH"u8));
        Assert.Equal(LeagueToolkit.Utils.LeagueFileType.RenderMeshGmesh, LeagueToolkit.Utils.LeagueFile.GetFileType(".gmesh".AsSpan()));
        Assert.Equal(LeagueToolkit.Utils.LeagueFileType.RenderMeshTmesh, LeagueToolkit.Utils.LeagueFile.GetFileType(".tmesh".AsSpan()));
        Assert.Equal("tmesh", LeagueToolkit.Utils.LeagueFile.GetExtension(LeagueToolkit.Utils.LeagueFileType.RenderMeshTmesh));
        Assert.Equal("gmesh", LeagueToolkit.Utils.LeagueFile.GetExtension(LeagueToolkit.Utils.LeagueFileType.RenderMeshGmesh));
    }

    // Construct bytes independently from RenderMesh.Write, matching LTK's split-stream fixture.
    private static byte[] Fixture(string magic = "GMSH", bool half = true, int vertices = 3)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write(Encoding.ASCII.GetBytes(magic));
        writer.Write(1u); writer.Write((uint)vertices); writer.Write(vertices > 0 ? 3u : 0u);
        foreach (float value in new float[] { -1, -2, -3, 10, 20, 30 }) writer.Write(value);
        writer.Write(2u);
        Layout(writer, (0u, 2u));
        Layout(writer, (2u, half ? 8u : 2u), (7u, half ? 7u : 1u), (14u, 7u));
        writer.Write((uint)(vertices * 12));
        for (int v = 0; v < vertices; v++) { writer.Write((float)v); writer.Write(v * 2f); writer.Write(v * 3f); }
        writer.Write((uint)(vertices * (half ? 16 : 24)));
        for (int v = 0; v < vertices; v++)
        {
            if (half)
            {
                foreach (float value in new float[] { 0, 1, 0, 99, v * 0.5f, -v * 0.25f })
                    writer.Write(BitConverter.HalfToUInt16Bits((Half)value));
            }
            else
            {
                foreach (float value in new float[] { 0, 1, 0, v * 0.5f, -v * 0.25f }) writer.Write(value);
            }
            writer.Write(BitConverter.HalfToUInt16Bits((Half)7));
            writer.Write(BitConverter.HalfToUInt16Bits((Half)8));
        }
        if (vertices > 0) { writer.Write((ushort)0); writer.Write((ushort)1); writer.Write((ushort)2); }
        writer.Write(vertices > 0 ? 1u : 0u);
        if (vertices > 0)
        {
            byte[] name = Encoding.UTF8.GetBytes("b\u00f3dy");
            writer.Write((uint)name.Length); writer.Write(name);
            writer.Write(0u); writer.Write(3u); writer.Write(0u); writer.Write(2u);
        }
        return stream.ToArray();
    }

    private static void Layout(BinaryWriter writer, params (uint Name, uint Format)[] elements)
    {
        writer.Write(0u); writer.Write((uint)elements.Length);
        for (int i = 0; i < 15; i++)
        {
            writer.Write(i < elements.Length ? elements[i].Name : 0xfeedfaceu);
            writer.Write(i < elements.Length ? elements[i].Format : 0xcafebabeu);
        }
    }

    [Theory]
    [InlineData("GMSH", true)]
    [InlineData("GMSH", false)]
    [InlineData("TMSH", true)]
    [InlineData("????", true)]
    public void ReadsAndRoundTripsSplitStreams(string magic, bool half)
    {
        byte[] bytes = Fixture(magic, half);
        using var input = new MemoryStream(bytes);
        RenderMesh mesh = RenderMesh.Read(input);
        Assert.True(input.CanRead);
        Assert.Equal(3, mesh.VertexCount);
        Assert.Equal(new Vector3(-1, -2, -3), mesh.BoundingBox.Min);
        Assert.Equal(new Vector3(10, 20, 30), mesh.BoundingBox.Max);
        Assert.Equal(new Vector3(2, 4, 6), mesh.ReadVector3(ElementName.Position, 2));
        Assert.Equal(Vector3.UnitY, mesh.ReadVector3(ElementName.Normal, 2));
        Assert.Equal(new Vector2(1, -0.5f), mesh.ReadVector2(ElementName.Texcoord0, 2));
        Assert.Equal(new Vector2(7, 8), mesh.ReadVector2(ElementName.Texcoord7, 1));
        Assert.Equal(new ushort[] { 0, 1, 2 }, mesh.Indices);
        Assert.Equal(new RenderMeshSubmesh("b\u00f3dy", 0, 3, 0, 2), Assert.Single(mesh.Submeshes));
        Assert.Equal(2, mesh.VertexStreams.Count);
        Assert.False(mesh.TryGetAccessor(ElementName.BlendWeight, out _));
        Assert.Throws<ArgumentOutOfRangeException>(() => mesh.ReadVector3(ElementName.Position, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => mesh.ReadVector3(ElementName.Position, 3));
        Assert.Throws<InvalidOperationException>(() => mesh.ReadVector3(ElementName.Texcoord0, 0));
        using var output = new MemoryStream();
        mesh.Write(output);
        Assert.Equal(bytes, output.ToArray());
        Assert.True(output.CanWrite);
    }

    [Fact]
    public void AcceptsEmptyVertexAndIndexBuffers()
    {
        byte[] bytes = Fixture(vertices: 0);
        using var input = new MemoryStream(bytes);
        var mesh = RenderMesh.Read(input);
        Assert.Equal(0, mesh.VertexCount);
        Assert.Empty(mesh.Indices);
        using var output = new MemoryStream();
        mesh.Write(output);
        Assert.Equal(bytes, output.ToArray());
    }

    [Theory]
    [InlineData(4, 2)] // Version.
    [InlineData(40, 0)] // No streams.
    [InlineData(44, 3)] // Invalid usage.
    [InlineData(48, 16)] // Too many elements.
    [InlineData(52, 15)] // Invalid name.
    [InlineData(56, 9)] // Invalid format.
    [InlineData(180, 0)] // Duplicate position across streams.
    [InlineData(300, 35)] // Incorrect position stream byte size.
    public void RejectsMalformedHeadersAndLayouts(int offset, uint value)
    {
        byte[] bytes = Fixture();
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(offset), value);
        using var input = new MemoryStream(bytes);
        Assert.Throws<InvalidDataException>(() => RenderMesh.Read(input));
    }

    [Fact]
    public void RejectsOutOfRangeIndicesAndSubmeshes()
    {
        byte[] bytes = Fixture();
        // 44-byte header, two 128-byte layouts, two sized streams (36 and 48 bytes).
        const int indicesOffset = 44 + 256 + 4 + 36 + 4 + 48;
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(indicesOffset), 3);
        using (var input = new MemoryStream(bytes)) Assert.Throws<InvalidDataException>(() => RenderMesh.Read(input));
        bytes = Fixture();
        int submeshStart = indicesOffset + 6 + 4 + 4 + Encoding.UTF8.GetByteCount("b\u00f3dy");
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(submeshStart), uint.MaxValue);
        using (var input = new MemoryStream(bytes)) Assert.Throws<InvalidDataException>(() => RenderMesh.Read(input));
    }

    [Fact]
    public void RejectsTruncationAtEveryByteBoundary()
    {
        byte[] bytes = Fixture();
        for (int length = 0; length < bytes.Length; length++)
        {
            using var input = new MemoryStream(bytes, 0, length);
            Assert.Throws<EndOfStreamException>(() => RenderMesh.Read(input));
        }
    }

    [Fact]
    public void ReadsNonSeekableStreamsWithShortReads()
    {
        byte[] bytes = Fixture();
        using var input = new ShortStream(bytes);
        var mesh = RenderMesh.Read(input);
        using var output = new MemoryStream();
        mesh.Write(output);
        Assert.Equal(bytes, output.ToArray());
    }

    private sealed class ShortStream(byte[] bytes) : Stream
    {
        private readonly MemoryStream input = new(bytes);
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => input.Read(buffer, offset, Math.Min(count, 3));
        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        protected override void Dispose(bool disposing) { if (disposing) input.Dispose(); base.Dispose(disposing); }
    }
}
