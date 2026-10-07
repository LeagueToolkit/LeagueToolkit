using System.Numerics;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Core.Primitives;
using Xunit;

namespace LeagueToolkit.Tests.Core.Mesh;

public sealed class StaticMeshTests
{
    [Fact]
    public void BinaryWriterStoresBgraAndPreservesVertexColorsOnRead()
    {
        Color[] colors =
        {
            new((byte)255, (byte)0, (byte)0, (byte)128),
            new((byte)17, (byte)83, (byte)211, (byte)0),
            new((byte)0, (byte)0, (byte)255, (byte)255)
        };
        var mesh = new StaticMesh("colors",
            new[] { new StaticMeshFace("material", (0, 1, 2), (Vector2.Zero, Vector2.UnitX, Vector2.UnitY)) },
            new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY }, colors);
        using var output = new MemoryStream();

        mesh.WriteBinary(output);

        // SCB 3.2: 180-byte header followed by three float3 positions, then BGRA bytes.
        Assert.Equal(new byte[] { 0, 0, 255, 128, 211, 83, 17, 0, 255, 0, 0, 255 },
            output.ToArray().AsSpan(180 + 36, 12).ToArray());
        output.Position = 0;
        StaticMesh loaded = StaticMesh.ReadBinary(output);
        Assert.True(loaded.HasVertexColors);
        Assert.Equal(colors, loaded.VertexColors);
        Assert.Equal(mesh.Vertices, loaded.Vertices);
        Assert.Equal(mesh.Faces, loaded.Faces);
    }

    [Fact]
    public void BinaryRoundTripPreservesMeshesWithoutVertexColors()
    {
        var mesh = new StaticMesh("plain",
            new[] { new StaticMeshFace("material", (0, 1, 2), (Vector2.Zero, Vector2.UnitX, Vector2.UnitY)) },
            new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY });
        using var output = new MemoryStream();

        mesh.WriteBinary(output);
        output.Position = 0;
        StaticMesh loaded = StaticMesh.ReadBinary(output);

        Assert.False(loaded.HasVertexColors);
        Assert.Equal(mesh.Vertices, loaded.Vertices);
        Assert.Equal(mesh.Faces, loaded.Faces);
    }
}
