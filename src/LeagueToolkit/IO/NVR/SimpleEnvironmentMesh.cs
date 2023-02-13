using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Helpers.Extensions;

namespace LeagueToolkit.IO.NVR;

public readonly struct SimpleEnvironmentMesh
{
    public EnvironmentQuality Quality { get; }
    public uint Flags { get; }

    public Sphere BoundingSphere { get; }
    public Box BoundingBox { get; }

    public int MaterialId { get; }
    public SimpleEnvironmentMeshPrimitive[] Primitives { get; }

    public SimpleEnvironmentMesh(
        EnvironmentQuality quality,
        uint flags,
        Sphere boundingSphere,
        Box boundingBox,
        int materialId,
        SimpleEnvironmentMeshPrimitive[] primitives
    )
    {
        this.Quality = quality;
        this.Flags = flags;
        this.BoundingSphere = boundingSphere;
        this.BoundingBox = boundingBox;
        this.MaterialId = materialId;
        this.Primitives = primitives;
    }

    public static SimpleEnvironmentMesh Read(BinaryReader br)
    {
        EnvironmentQuality quality = (EnvironmentQuality)br.ReadUInt32();
        uint flags = br.ReadUInt32();
        Sphere boundingSphere = br.ReadSphere();
        Box boundingBox = br.ReadBox();
        int materialId = br.ReadInt32();

        SimpleEnvironmentMeshPrimitive[] primitives = new[]
        {
            SimpleEnvironmentMeshPrimitive.Read(br),
            SimpleEnvironmentMeshPrimitive.Read(br)
        };

        return new(quality, flags, boundingSphere, boundingBox, materialId, primitives);
    }

    public static SimpleEnvironmentMesh ReadOld(BinaryReader br)
    {
        EnvironmentQuality quality = (EnvironmentQuality)br.ReadUInt32();
        Sphere boundingSphere = br.ReadSphere();
        Box boundingBox = br.ReadBox();
        int materialId = br.ReadInt32();

        SimpleEnvironmentMeshPrimitive[] primitives = new[]
        {
            SimpleEnvironmentMeshPrimitive.Read(br),
            SimpleEnvironmentMeshPrimitive.Read(br)
        };

        return new(quality, 0, boundingSphere, boundingBox, materialId, primitives);
    }
}
