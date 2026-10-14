using LeagueToolkit.Core.Memory;

namespace LeagueToolkit.Core.Mesh;

/// <summary>A vertex stream of a render mesh, retaining the serialized layout and vertex bytes.</summary>
public sealed class RenderMeshVertexStream
{
    public VertexBufferDescription Description { get; }
    public ReadOnlyMemory<byte> Data { get; }
    public int VertexCount { get; }
    public int VertexStride { get; }
    internal byte[] SerializedDescription { get; }

    internal RenderMeshVertexStream(VertexBufferDescription description, byte[] serializedDescription, byte[] data, int vertexCount)
    {
        this.Description = description;
        this.SerializedDescription = serializedDescription;
        this.Data = data;
        this.VertexCount = vertexCount;
        this.VertexStride = description.Elements.Sum(element => element.GetSize());
    }

    public bool TryGetAccessor(ElementName name, out VertexElementAccessor accessor)
    {
        int offset = 0;
        foreach (VertexElement element in this.Description.Elements)
        {
            if (element.Name == name && this.VertexCount > 0)
            {
                accessor = new VertexElementAccessor(element, this.Data, this.VertexStride, offset);
                return true;
            }
            offset += element.GetSize();
        }
        accessor = default;
        return false;
    }
}
