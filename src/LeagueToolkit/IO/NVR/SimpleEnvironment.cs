using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Helpers.Exceptions;
using System.Text;

namespace LeagueToolkit.IO.NVR;

public class SimpleEnvironment
{
    public SimpleEnvironment(string fileLocation) : this(File.OpenRead(fileLocation)) { }

    public SimpleEnvironment(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.UTF8.GetString(br.ReadBytes(4));
        if (magic is not "NVR\0")
            throw new InvalidFileSignatureException();

        ushort major = br.ReadUInt16();
        ushort minor = br.ReadUInt16();

        //Reading the counts
        int materialsCount = br.ReadInt32();
        int vertexBufferCount = br.ReadInt32();
        int indexBufferCount = br.ReadInt32();
        int meshesCount = br.ReadInt32();
        int nodesCount = br.ReadInt32();

        SimpleEnvironmentMaterial[] materials = new SimpleEnvironmentMaterial[materialsCount];
        long[] vertexBufferOffsets = new long[vertexBufferCount];
        IndexBuffer[] indexBuffers = new IndexBuffer[indexBufferCount];
        SimpleEnvironmentMesh[] meshes = new SimpleEnvironmentMesh[meshesCount];

        for (int i = 0; i < materialsCount; i++)
        {
            materials[i] = (major, minor) switch
            {
                (8, 1) => SimpleEnvironmentMaterial.ReadOld(br),
                _ => SimpleEnvironmentMaterial.Read(br)
            };
        }

        for (int i = 0; i < vertexBufferCount; i++)
        {
            int vertexBufferSize = br.ReadInt32();
            vertexBufferOffsets[i] = br.BaseStream.Position;

            br.BaseStream.Seek(vertexBufferSize, SeekOrigin.Current);
        }
        for (int i = 0; i < indexBufferCount; i++)
        {
            int indexBufferSize = br.ReadInt32();
            SimpleEnvironmentIndexFormat indexFormat = (SimpleEnvironmentIndexFormat)br.ReadInt32();
            MemoryOwner<byte> indexBufferOwner = MemoryOwner<byte>.Allocate(indexBufferSize);

            br.Read(indexBufferOwner.Span);

            indexBuffers[i] = IndexBuffer.Create(
                indexFormat is SimpleEnvironmentIndexFormat.D3DFMT_INDEX16 ? IndexFormat.U16 : IndexFormat.U32,
                indexBufferOwner
            );
        }
        for (int i = 0; i < meshesCount; i++)
        {
            meshes[i] = (major, minor) switch
            {
                (8, 1) => SimpleEnvironmentMesh.ReadOld(br),
                _ => SimpleEnvironmentMesh.Read(br)
            };
        }
    }
}

public enum SimpleEnvironmentIndexFormat
{
    D3DFMT_INDEX16 = 0x65,
    D3DFMT_INDEX32 = 0x66,
}
