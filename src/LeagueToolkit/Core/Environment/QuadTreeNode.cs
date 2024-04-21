using System.Numerics;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

internal class QuadTreeNode
{
    public static readonly float NullCoordinate = BitConverter.ToSingle(new byte[4] { 255, 255, 127, 255 }, 0);
    public Box BoundingBox { get; private set; }
    public List<QuadTreeNode> Children { get; private set; } = new List<QuadTreeNode>();
    public List<SimpleEnvironmentMesh> Meshes { get; private set; } = new List<SimpleEnvironmentMesh>();

    //Values used when reading
#pragma warning disable CS0649
    public int FirstMesh;
    public int MeshCount;
    public int FirstChildNode;
    public int ChildNodeCount;
#pragma warning restore CS0649

    //Values used when writing
    public Box CentralPointsBoundingBox;

    public QuadTreeNode(Box centralPointsBox, QuadTreeNode parentNode)
    {
        // Used if we create it from a parent node.
        this.CentralPointsBoundingBox = centralPointsBox;
        this.Meshes.AddRange(
            parentNode.Meshes.FindAll(x => this.CentralPointsBoundingBox.ContainsPoint(x.BoundingSphere.Position))
        );
        parentNode.Children.Add(this);
        this.BoundingBox = CalculateBoundingBox();
    }

    public QuadTreeNode(List<SimpleEnvironmentMesh> meshes)
    {
        // Used when creating the big parent node.
        this.Meshes.AddRange(meshes);
        this.BoundingBox = CalculateBoundingBox();
        this.CentralPointsBoundingBox = CalculateCentralPointsBoundingBox();
    }

    private Box CalculateBoundingBox()
    {
        if (Meshes.Count > 0)
        {
            Vector3 min = new Vector3(
                Meshes[0].BoundingBox.Min.X,
                Meshes[0].BoundingBox.Min.Y,
                Meshes[0].BoundingBox.Min.Z
            );
            Vector3 max = new Vector3(
                Meshes[0].BoundingBox.Max.X,
                Meshes[0].BoundingBox.Max.Y,
                Meshes[0].BoundingBox.Max.Z
            );
            for (int i = 1; i < Meshes.Count; i++)
            {
                Box box = Meshes[i].BoundingBox;
                if (box.Min.X < min.X)
                {
                    min.X = box.Min.X;
                }
                if (box.Min.Y < min.Y)
                {
                    min.Y = box.Min.Y;
                }
                if (box.Min.Z < min.Z)
                {
                    min.Z = box.Min.Z;
                }
                if (box.Max.X > max.X)
                {
                    max.X = box.Max.X;
                }
                if (box.Max.Y > max.Y)
                {
                    max.Y = box.Max.Y;
                }
                if (box.Max.Z > max.Z)
                {
                    max.Z = box.Max.Z;
                }
            }
            return new Box(min, max);
        }
        else
        {
            // No meshes inside, set bounding box to
            return new Box(
                new Vector3(NullCoordinate, NullCoordinate, NullCoordinate),
                new Vector3(NullCoordinate, NullCoordinate, NullCoordinate)
            );
        }
    }

    private Box CalculateCentralPointsBoundingBox()
    {
        if (Meshes.Count > 0)
        {
            Vector3 min = new Vector3(
                Meshes[0].BoundingSphere.Position.X,
                Meshes[0].BoundingSphere.Position.Y,
                Meshes[0].BoundingSphere.Position.Z
            );
            Vector3 max = new Vector3(
                Meshes[0].BoundingSphere.Position.X,
                Meshes[0].BoundingSphere.Position.Y,
                Meshes[0].BoundingSphere.Position.Z
            );
            for (int i = 1; i < Meshes.Count; i++)
            {
                Vector3 spherePosition = Meshes[i].BoundingSphere.Position;
                if (spherePosition.X < min.X)
                {
                    min.X = spherePosition.X;
                }
                if (spherePosition.Y < min.Y)
                {
                    min.Y = spherePosition.Y;
                }
                if (spherePosition.Z < min.Z)
                {
                    min.Z = spherePosition.Z;
                }
                if (spherePosition.X > max.X)
                {
                    max.X = spherePosition.X;
                }
                if (spherePosition.Y > max.Y)
                {
                    max.Y = spherePosition.Y;
                }
                if (spherePosition.Z > max.Z)
                {
                    max.Z = spherePosition.Z;
                }
            }
            return new Box(min, max);
        }
        return new Box(
            new Vector3(NullCoordinate, NullCoordinate, NullCoordinate),
            new Vector3(NullCoordinate, NullCoordinate, NullCoordinate)
        );
    }

    public void Split()
    {
        Box pBox = CentralPointsBoundingBox;
        float middleX = (pBox.Min.X + pBox.Max.X) / 2;
        float middleZ = (pBox.Min.Z + pBox.Max.Z) / 2;
        // Node 1 (bottom-left)
        Vector3 node1Min = new Vector3(pBox.Min.X, pBox.Min.Y, pBox.Min.Z);
        Vector3 node1Max = new Vector3(middleX, pBox.Max.Y, middleZ);
        QuadTreeNode node1 = new QuadTreeNode(new Box(node1Min, node1Max), this);

        // Node 2 (top-left)
        Vector3 node2Min = new Vector3(pBox.Min.X, pBox.Min.Y, middleZ);
        Vector3 node2Max = new Vector3(middleX, pBox.Max.Y, pBox.Max.Z);
        QuadTreeNode node2 = new QuadTreeNode(new Box(node2Min, node2Max), this);

        // Node 3 (top-right)
        Vector3 node3Min = new Vector3(middleX, pBox.Min.Y, middleZ);
        Vector3 node3Max = new Vector3(pBox.Max.X, pBox.Max.Y, pBox.Max.Z);
        QuadTreeNode node3 = new QuadTreeNode(new Box(node3Min, node3Max), this);

        // Node 4 (bottom-right)
        Vector3 node4Min = new Vector3(middleX, pBox.Min.Y, pBox.Min.Z);
        Vector3 node4Max = new Vector3(pBox.Max.X, pBox.Max.Y, middleZ);
        QuadTreeNode node4 = new QuadTreeNode(new Box(node4Min, node4Max), this);

        foreach (QuadTreeNode childNode in Children)
        {
            Vector3 proportions = childNode.CentralPointsBoundingBox.GetSize();
            if (childNode.Meshes.Count > 1 && (proportions.X > 100 || proportions.Z > 100))
            {
                childNode.Split();
            }
        }
    }

    public void Write(BinaryWriter bw)
    {
        bw.WriteBox(this.BoundingBox);
        bw.Write(this.FirstMesh);
        bw.Write(this.MeshCount);
        bw.Write(this.FirstChildNode);
        bw.Write(this.ChildNodeCount);
    }
}
