using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.League.Helpers.Structures;

namespace Fantome.League.IO.NVR
{
    public class NVRNode
    {
        public static readonly float NullCoordinate = BitConverter.ToSingle(new byte[4] { 255, 255, 127, 255 }, 0);
        public R3DBox BoundingBox { get; private set; }
        public List<NVRNode> Children { get; private set; } = new List<NVRNode>();
        public List<NVRMesh> Meshes { get; private set; } = new List<NVRMesh>();

        //Values used when reading
        public int FirstMesh;
        public int MeshCount;
        public int FirstChildNode;
        public int ChildNodeCount;

        public NVRNode(BinaryReader br, NVRBuffers buffers)
        {
            this.BoundingBox = new R3DBox(br);
            this.FirstMesh = br.ReadInt32();
            this.MeshCount = br.ReadInt32();
            this.FirstChildNode = br.ReadInt32();
            this.ChildNodeCount = br.ReadInt32();

            if (this.FirstChildNode == -1)
            {
                for (int i = this.FirstMesh; i < this.FirstMesh + this.MeshCount; i++)
                {
                    this.Meshes.Add(buffers.Meshes[i]);
                    buffers.Meshes[i].ParentNode = this;
                }
            }

        }

        public int CalculateMeshCount()
        {
            if (this.Children.Count == 0)
            {
                return this.Meshes.Count;
            }
            else
            {
                int meshCount = 0;
                foreach (NVRNode child in this.Children)
                {
                    meshCount += child.CalculateMeshCount();
                }
                return meshCount;
            }
        }

        public void Write(BinaryWriter bw)
        {
            this.BoundingBox.Write(bw);
            bw.Write(this.FirstMesh);
            bw.Write(this.MeshCount);
            bw.Write(this.FirstChildNode);
            bw.Write(this.ChildNodeCount);
        }
    }
}
