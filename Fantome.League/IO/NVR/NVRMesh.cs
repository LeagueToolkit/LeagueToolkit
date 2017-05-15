using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.League.Helpers.Structures;

namespace Fantome.League.IO.NVR
{
    public class NVRMesh
    {
        public NVRNode ParentNode { get; set; }
        public NVRMeshQuality QualityLevel { get; set; }
        public int Flag { get; private set; }
        public R3DSphere BoundingSphere { get; private set; }
        public R3DBox BoundingBox { get; private set; }
        public NVRMaterial Material { get; private set; }
        public NVRDrawIndexedPrimitive[] IndexedPrimitives { get; private set; } = new NVRDrawIndexedPrimitive[2];

        //Used for writing
        public int MaterialIndex;

        public NVRMesh(BinaryReader br, NVRBuffers buffers)
        {
            this.QualityLevel = (NVRMeshQuality)br.ReadInt32();
            this.Flag = br.ReadInt32();
            this.BoundingSphere = new R3DSphere(br);
            this.BoundingBox = new R3DBox(br);
            this.Material = buffers.Materials[br.ReadInt32()];
            this.IndexedPrimitives[0] = new NVRDrawIndexedPrimitive(br, buffers, this, true);
            this.IndexedPrimitives[1] = new NVRDrawIndexedPrimitive(br, buffers, this, false);
        }

        public NVRMesh(NVRMeshQuality meshQualityLevel, int flag, NVRMaterial material, List<NVRVertex> vertices, List<int> indices)
        {
            this.QualityLevel = meshQualityLevel;
            this.Flag = flag;
            this.Material = material;
            this.IndexedPrimitives[0] = new NVRDrawIndexedPrimitive(this, vertices, indices, true);
            this.IndexedPrimitives[1] = new NVRDrawIndexedPrimitive(this, vertices, indices, false);

            float[] min = new float[3] { vertices[0].Position.X, vertices[0].Position.Y, vertices[0].Position.Z };
            float[] max = new float[3] { vertices[0].Position.X, vertices[0].Position.Y, vertices[0].Position.Z };
            for (int i = 1; i < vertices.Count; i++)
            {
                Vector3 position = vertices[i].Position;
                if (position.X < min[0]) { min[0] = position.X; }
                if (position.Y < min[1]) { min[1] = position.Y; }
                if (position.Z < min[2]) { min[2] = position.Z; }
                if (position.X > max[0]) { max[0] = position.X; }
                if (position.Y > max[1]) { max[1] = position.Y; }
                if (position.Z > max[2]) { max[2] = position.Z; }
            }
            this.BoundingBox = new R3DBox(new Vector3(min[0], min[1], min[2]), new Vector3(max[0], max[1], max[2]));

            float radius = max[0] - min[0];
            if (max[1] - min[1] > radius) { radius = max[1] - min[1]; }
            if (max[2] - min[2] > radius) { radius = max[2] - min[2]; }
            this.BoundingSphere = new R3DSphere(new Vector3((min[0] + max[0]) / 2, (min[1] + max[1]) / 2, (min[2] + max[2]) / 2), radius / 2);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((int)this.QualityLevel);
            bw.Write(this.Flag);
            this.BoundingSphere.Write(bw);
            this.BoundingBox.Write(bw);
            bw.Write(this.MaterialIndex);
            this.IndexedPrimitives[0].Write(bw);
            this.IndexedPrimitives[1].Write(bw);
        }
    }

    public enum NVRMeshQuality : int
    {
        VERY_LOW = -100,
        LOW = 0,
        MEDIUM = 1,
        HIGH = 2,
        VERY_HIGH = 3
    }
}
