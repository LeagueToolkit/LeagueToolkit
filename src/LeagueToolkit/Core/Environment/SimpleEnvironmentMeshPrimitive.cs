namespace LeagueToolkit.Core.Environment;

internal readonly struct SimpleEnvironmentMeshPrimitive
{
    public int VertexBufferId { get; }
    public int StartVertex { get; }
    public int VertexCount { get; }

    public int IndexBufferId { get; }
    public int StartIndex { get; }
    public int IndexCount { get; }

    public SimpleEnvironmentMeshPrimitive(
        int vertexBufferId,
        int startVertex,
        int vertexCount,
        int indexBufferId,
        int startIndex,
        int indexCount
    )
    {
        this.VertexBufferId = vertexBufferId;
        this.StartVertex = startVertex;
        this.VertexCount = vertexCount;

        this.IndexBufferId = indexBufferId;
        this.StartIndex = startIndex;
        this.IndexCount = indexCount;
    }

    public static SimpleEnvironmentMeshPrimitive Read(BinaryReader br)
    {
        int vertexBufferId = br.ReadInt32();
        int startVertex = br.ReadInt32();
        int vertexCount = br.ReadInt32();

        int indexBufferId = br.ReadInt32();
        int startIndex = br.ReadInt32();
        int indexCount = br.ReadInt32();

        return new(vertexBufferId, startVertex, vertexCount, indexBufferId, startIndex, indexCount);
    }
}
