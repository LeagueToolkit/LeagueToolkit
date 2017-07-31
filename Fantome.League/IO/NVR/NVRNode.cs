using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.NVR
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

        //Values used when writing
        public R3DBox CentralPointsBoundingBox;

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
                }
            }
        }

        public NVRNode(R3DBox centralPointsBox, NVRNode parentNode)
        {
            // Used if we create it from a parent node.
            this.CentralPointsBoundingBox = centralPointsBox;
            this.Meshes.AddRange(parentNode.Meshes.FindAll(x => this.CentralPointsBoundingBox.ContainsPoint(x.BoundingSphere.Position)));
            parentNode.Children.Add(this);
            this.BoundingBox = this.CalculateBoundingBox();
        }

        public NVRNode(List<NVRMesh> meshes)
        {
            // Used when creating the big parent node.
            this.Meshes.AddRange(meshes);
            this.BoundingBox = this.CalculateBoundingBox();
            this.CentralPointsBoundingBox = this.CalculateCentralPointsBoundingBox();
        }

        private R3DBox CalculateBoundingBox()
        {
            if (Meshes.Count > 0)
            {
                Vector3 min = new Vector3(Meshes[0].BoundingBox.Min.X, Meshes[0].BoundingBox.Min.Y, Meshes[0].BoundingBox.Min.Z);
                Vector3 max = new Vector3(Meshes[0].BoundingBox.Max.X, Meshes[0].BoundingBox.Max.Y, Meshes[0].BoundingBox.Max.Z);
                for (int i = 1; i < Meshes.Count; i++)
                {
                    R3DBox box = Meshes[i].BoundingBox;
                    if (box.Min.X < min.X) { min.X = box.Min.X; }
                    if (box.Min.Y < min.Y) { min.Y = box.Min.Y; }
                    if (box.Min.Z < min.Z) { min.Z = box.Min.Z; }
                    if (box.Max.X > max.X) { max.X = box.Max.X; }
                    if (box.Max.Y > max.Y) { max.Y = box.Max.Y; }
                    if (box.Max.Z > max.Z) { max.Z = box.Max.Z; }
                }
                return new R3DBox(min, max);
            }
            else
            {
                // No meshes inside, set bounding box to 
                return new R3DBox(new Vector3(NullCoordinate, NullCoordinate, NullCoordinate), new Vector3(NullCoordinate, NullCoordinate, NullCoordinate));
            }
        }

        private R3DBox CalculateCentralPointsBoundingBox()
        {
            if (Meshes.Count > 0)
            {
                Vector3 min = new Vector3(Meshes[0].BoundingSphere.Position.X, Meshes[0].BoundingSphere.Position.Y, Meshes[0].BoundingSphere.Position.Z);
                Vector3 max = new Vector3(Meshes[0].BoundingSphere.Position.X, Meshes[0].BoundingSphere.Position.Y, Meshes[0].BoundingSphere.Position.Z);
                for (int i = 1; i < Meshes.Count; i++)
                {
                    Vector3 spherePosition = Meshes[i].BoundingSphere.Position;
                    if (spherePosition.X < min.X) { min.X = spherePosition.X; }
                    if (spherePosition.Y < min.Y) { min.Y = spherePosition.Y; }
                    if (spherePosition.Z < min.Z) { min.Z = spherePosition.Z; }
                    if (spherePosition.X > max.X) { max.X = spherePosition.X; }
                    if (spherePosition.Y > max.Y) { max.Y = spherePosition.Y; }
                    if (spherePosition.Z > max.Z) { max.Z = spherePosition.Z; }
                }
                return new R3DBox(min, max);
            }
            return null;
        }

        public void Split()
        {
            R3DBox pBox = CentralPointsBoundingBox;
            float middleX = (pBox.Min.X + pBox.Max.X) / 2;
            float middleZ = (pBox.Min.Z + pBox.Max.Z) / 2;
            // Node 1 (bottom-left)
            Vector3 node1Min = new Vector3(pBox.Min.X, pBox.Min.Y, pBox.Min.Z);
            Vector3 node1Max = new Vector3(middleX, pBox.Max.Y, middleZ);
            NVRNode node1 = new NVRNode(new R3DBox(node1Min, node1Max), this);

            // Node 2 (top-left)
            Vector3 node2Min = new Vector3(pBox.Min.X, pBox.Min.Y, middleZ);
            Vector3 node2Max = new Vector3(middleX, pBox.Max.Y, pBox.Max.Z);
            NVRNode node2 = new NVRNode(new R3DBox(node2Min, node2Max), this);

            // Node 3 (top-right)
            Vector3 node3Min = new Vector3(middleX, pBox.Min.Y, middleZ);
            Vector3 node3Max = new Vector3(pBox.Max.X, pBox.Max.Y, pBox.Max.Z);
            NVRNode node3 = new NVRNode(new R3DBox(node3Min, node3Max), this);

            // Node 4 (bottom-right)
            Vector3 node4Min = new Vector3(middleX, pBox.Min.Y, pBox.Min.Z);
            Vector3 node4Max = new Vector3(pBox.Max.X, pBox.Max.Y, middleZ);
            NVRNode node4 = new NVRNode(new R3DBox(node4Min, node4Max), this);

            foreach (NVRNode childNode in Children)
            {
                Vector3 proportions = childNode.CentralPointsBoundingBox.GetProportions();
                if ((childNode.Meshes.Count > 1) && (proportions.X > 100 || proportions.Z > 100))
                {
                    childNode.Split();
                }
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
