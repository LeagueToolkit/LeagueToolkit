using LeagueToolkit.Utils.Extensions;
using System.IO;

namespace LeagueToolkit.Core.Mesh
{
    /// <summary>Represents a <see cref="SkinnedMesh"/> primitive</summary>
    public readonly struct SkinnedMeshRange
    {
        /// <summary>Gets the primitive's material name</summary>
        public string Material { get; }

        /// <summary>Gets the primitive's start vertex</summary>
        public int StartVertex { get; }

        /// <summary>Gets the primitive's vertex count</summary>
        public int VertexCount { get; }

        /// <summary>Gets the primitive's start index</summary>
        public int StartIndex { get; }

        /// <summary>Gets the primitive's index count</summary>
        public int IndexCount { get; }

        /// <summary>
        /// Creates a new <see cref="SkinnedMeshRange"/> object with the specified parameters
        /// </summary>
        /// <param name="material">The material name of the <see cref="SkinnedMeshRange"/></param>
        /// <param name="startVertex">The start vertex of the <see cref="SkinnedMeshRange"/></param>
        /// <param name="vertexCount">The vertex count of the <see cref="SkinnedMeshRange"/></param>
        /// <param name="startIndex">The start index of the <see cref="SkinnedMeshRange"/></param>
        /// <param name="indexCount">The index count of the <see cref="SkinnedMeshRange"/></param>
        public SkinnedMeshRange(string material, int startVertex, int vertexCount, int startIndex, int indexCount)
        {
            this.Material = material;
            this.StartVertex = startVertex;
            this.VertexCount = vertexCount;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
        }

        internal static SkinnedMeshRange ReadFromSimpleSkin(BinaryReader br)
        {
            string material = br.ReadPaddedString(64);
            int startVertex = br.ReadInt32();
            int vertexCount = br.ReadInt32();
            int startIndex = br.ReadInt32();
            int indexCount = br.ReadInt32();

            return new(material, startVertex, vertexCount, startIndex, indexCount);
        }

        internal void WriteToSimpleSkin(BinaryWriter bw)
        {
            bw.WritePaddedString(this.Material, 64);
            bw.Write(this.StartVertex);
            bw.Write(this.VertexCount);
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
        }
    }
}
