using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.NVR
{
    public class NVRDrawIndexedPrimitive
    {
        public NVRMesh Parent;
        public NVRVertexType VertexType { get; private set; }
        public List<NVRVertex> Vertices { get; private set; } = new List<NVRVertex>();
        public List<int> Indices { get; private set; } = new List<int>();

        //Used for writing only
        public int VertexBuffer;
        public int FirstVertex;
        public int VertexCount;
        public int IndexBuffer;
        public int FirstIndex;
        public int IndexCount;

        public NVRDrawIndexedPrimitive(BinaryReader br, NVRBuffers buffers, NVRMesh mesh, bool isComplex)
        {
            this.Parent = mesh;
            // Read vertices
            this.VertexBuffer = br.ReadInt32();
            this.FirstVertex = br.ReadInt32();
            this.VertexCount = br.ReadInt32();
            long currentOffset = br.BaseStream.Position;

            // Find vertex type
            int vertexSize = 12;
            if (isComplex)
            {
                this.VertexType = NVRVertex.GetVertexTypeFromMaterial(mesh.Material);
                switch (VertexType)
                {
                    case NVRVertexType.NVRVERTEX_4:
                        vertexSize = NVRVertex4.Size;
                        break;
                    case NVRVertexType.NVRVERTEX_8:
                        vertexSize = NVRVertex8.Size;
                        break;
                    case NVRVertexType.NVRVERTEX_GROUND_8:
                        vertexSize = NVRVertexGround8.Size;
                        break;
                    case NVRVertexType.NVRVERTEX_12:
                        vertexSize = NVRVertex12.Size;
                        break;
                }
            }

            //Parse vertices
            br.BaseStream.Seek(buffers.VertexBuffers[VertexBuffer].Offset + FirstVertex * vertexSize, SeekOrigin.Begin);
            for (int i = 0; i < VertexCount; i++)
            {
                NVRVertex newVertex;
                switch (VertexType)
                {
                    case NVRVertexType.NVRVERTEX_4:
                        newVertex = new NVRVertex4(br);
                        break;
                    case NVRVertexType.NVRVERTEX_8:
                        newVertex = new NVRVertex8(br);
                        break;
                    case NVRVertexType.NVRVERTEX_GROUND_8:
                        newVertex = new NVRVertexGround8(br);
                        break;
                    case NVRVertexType.NVRVERTEX_12:
                        newVertex = new NVRVertex12(br);
                        break;
                    default:
                        newVertex = new NVRVertex(br);
                        break;
                }
                this.Vertices.Add(newVertex);
            }

            // Store indices
            br.BaseStream.Seek(currentOffset, SeekOrigin.Begin);
            this.IndexBuffer = br.ReadInt32();
            this.FirstIndex = br.ReadInt32();
            this.IndexCount = br.ReadInt32();

            for (int i = FirstIndex; i < FirstIndex + IndexCount; i++)
            {
                this.Indices.Add(buffers.IndexBuffers[IndexBuffer].Indices[i]);
            }

            // Fix indices
            int indicesMin = FindMin(this.Indices);
            for (int i = 0; i < this.Indices.Count; i++)
            {
                this.Indices[i] -= indicesMin;
            }
        }

        public NVRDrawIndexedPrimitive(NVRMesh mesh, List<NVRVertex> vertices, List<int> indices, bool complex)
        {
            this.Parent = mesh;
            this.Indices.AddRange(indices);
            if (complex)
            {
                this.Vertices.AddRange(vertices);
                if (vertices.Count > 0)
                {
                    this.VertexType = vertices[0].GetVertexType();
                    NVRVertexType expectedType = NVRVertex.GetVertexTypeFromMaterial(mesh.Material);
                    if (expectedType != this.VertexType)
                    {
                        throw new InvalidVertexTypeException(mesh.Material.Type, expectedType);
                    }
                }
            }
            else
            {
                this.VertexType = NVRVertexType.NVRVERTEX;
                // Conversion to simple vertex
                foreach (NVRVertex vertex in vertices)
                {
                    this.Vertices.Add(new NVRVertex(vertex.Position));
                }
            }
        }

        private static int FindMin(List<int> list)
        {
            int min = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                }
            }
            return min;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.VertexBuffer);
            bw.Write(this.FirstVertex);
            bw.Write(this.VertexCount);
            bw.Write(this.IndexBuffer);
            bw.Write(this.FirstIndex);
            bw.Write(this.IndexCount);
        }
    }

    public class InvalidVertexTypeException : Exception
    {
        public InvalidVertexTypeException(NVRMaterialType matType, NVRVertexType expected) : base(String.Format("Invalid vertex type for the specified material ({0}), expected type: {1}.", matType, expected)) { }
    }
}
