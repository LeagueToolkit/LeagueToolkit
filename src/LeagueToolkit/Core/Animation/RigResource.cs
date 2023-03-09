using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Hashing;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Animation;

/// <summary>
/// Represents a skeleton used for a <see cref="SkinnedMesh"/>
/// </summary>
public sealed class RigResource
{
    internal const int FORMAT_TOKEN = 0x22FD4FC3; // FNV hash of the format token string

    /// <summary>
    /// Gets the flags
    /// </summary>
    public ushort Flags { get; private set; }

    /// <summary>
    /// Gets the name
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the asset name
    /// </summary>
    public string AssetName { get; private set; }

    /// <summary>
    /// Gets a read-only list of joints
    /// </summary>
    public IReadOnlyList<Joint> Joints => this._joints;
    private Joint[] _joints;

    /// <summary>
    /// Gets a read-only list of influence id's
    /// </summary>
    /// <remarks>
    /// Use this to map <see cref="VertexElement.BLEND_INDEX"/> values
    /// <code>
    /// short jointId1 = rigResource.Influences[blendIndex.x];
    /// </code>
    /// </remarks>
    public IReadOnlyList<short> Influences => this._influences;
    private short[] _influences;

    internal RigResource(
        ushort flags,
        string name,
        string assetName,
        IEnumerable<Joint> joints,
        IEnumerable<short> influences
    )
    {
        this.Flags = flags;
        this.Name = name;
        this.AssetName = assetName;
        this._joints = joints.ToArray();
        this._influences = influences.ToArray();
    }

    /// <summary>
    /// Creates a new <see cref="RigResource"/> object by reading it from the specified stream
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from</param>
    public RigResource(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        br.BaseStream.Seek(4, SeekOrigin.Begin);
        uint magic = br.ReadUInt32();
        br.BaseStream.Seek(0, SeekOrigin.Begin);

        if (magic is FORMAT_TOKEN)
            Read(br);
        else
            ReadLegacy(br);
    }

    private void Read(BinaryReader br)
    {
        uint fileSize = br.ReadUInt32();
        uint formatToken = br.ReadUInt32();
        uint version = br.ReadUInt32();
        if (version != 0)
            throw new InvalidOperationException($"Invalid version: {version}");

        this.Flags = br.ReadUInt16();
        ushort jointCount = br.ReadUInt16();
        uint influencesCount = br.ReadUInt32();
        int jointsOffset = br.ReadInt32();
        int jointIndicesOffset = br.ReadInt32();
        int influencesOffset = br.ReadInt32();
        int nameOffset = br.ReadInt32();
        int assetNameOffset = br.ReadInt32();
        int boneNamesOffset = br.ReadInt32();

        // extension offsets
        int reservedOffset1 = br.ReadInt32();
        int reservedOffset2 = br.ReadInt32();
        int reservedOffset3 = br.ReadInt32();
        int reservedOffset4 = br.ReadInt32();
        int reservedOffset5 = br.ReadInt32();

        this._joints = new Joint[jointCount];
        this._influences = new short[influencesCount];

        // These are sorted by hash in ascending order
        (short id, uint hash)[] jointHashIds = new (short id, uint hash)[jointCount];

        if (jointsOffset > 0)
        {
            br.BaseStream.Seek(jointsOffset, SeekOrigin.Begin);

            // Read joints
            for (int i = 0; i < jointCount; i++)
            {
                RigResourceJoint joint = RigResourceJoint.Read(br);

                this._joints[i] = new(
                    joint.Name,
                    0,
                    joint.Id,
                    joint.ParentId,
                    joint.Radius,
                    joint.LocalTransform,
                    joint.InverseBindTransform
                );
            }
        }
        if (influencesOffset > 0)
        {
            br.BaseStream.Seek(influencesOffset, SeekOrigin.Begin);

            for (int i = 0; i < influencesCount; i++)
                this._influences[i] = br.ReadInt16();
        }
        if (jointIndicesOffset > 0)
        {
            br.BaseStream.Seek(jointIndicesOffset, SeekOrigin.Begin);

            for (int i = 0; i < jointCount; i++)
            {
                short id = br.ReadInt16();
                br.ReadInt16(); //pad
                uint hash = br.ReadUInt32();

                jointHashIds[i] = (id, hash);
            }
        }

        if (nameOffset > 0)
        {
            br.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
            this.Name = br.ReadNullTerminatedString();
        }
        if (assetNameOffset > 0)
        {
            br.BaseStream.Seek(assetNameOffset, SeekOrigin.Begin);
            this.AssetName = br.ReadNullTerminatedString();
        }
    }

    private void ReadLegacy(BinaryReader br)
    {
        string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
        if (magic != "r3d2sklt")
            throw new InvalidFileSignatureException();

        uint version = br.ReadUInt32();
        if (version is not (1 or 2))
            throw new InvalidFileVersionException();

        uint skeletonID = br.ReadUInt32();

        uint jointCount = br.ReadUInt32();
        RigResourceLegacyJoint[] joints = new RigResourceLegacyJoint[jointCount];
        for (short i = 0; i < jointCount; i++)
        {
            joints[i] = RigResourceLegacyJoint.Read(br, i);
        }

        // Validate hierarchy
        if (joints.Any(joint => joint.Id <= joint.ParentId))
            throw new InvalidOperationException("Joints must be ordered hierarchically");

        // Compute transforms
        Matrix4x4[] localTransforms = new Matrix4x4[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            RigResourceLegacyJoint joint = joints[i];

            if (joint.ParentId is -1)
            {
                localTransforms[i] = joint.GlobalTransform;
            }
            else
            {
                Matrix4x4.Invert(joints[joint.ParentId].GlobalTransform, out Matrix4x4 parentInverseBind);
                localTransforms[i] = joint.GlobalTransform * parentInverseBind;
            }
        }

        this._joints = new Joint[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            RigResourceLegacyJoint joint = joints[i];

            Matrix4x4.Invert(joint.GlobalTransform, out Matrix4x4 inverseBindTransform);

            this._joints[i] = new(
                joint.Name,
                0,
                joint.Id,
                joint.ParentId,
                joint.Radius,
                localTransforms[i],
                inverseBindTransform
            );
        }

        if (version == 2)
        {
            uint influencesCount = br.ReadUInt32();
            this._influences = new short[influencesCount];
            for (int i = 0; i < influencesCount; i++)
                this._influences[i] = (short)br.ReadUInt32();
        }
        else if (version == 1)
        {
            this._influences = new short[jointCount];
            for (short i = 0; i < jointCount; i++)
                this._influences[i] = i;
        }
    }

    /// <summary>
    /// Writes the <see cref="RigResource"/> object into the specified stream
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to write to</param>
    public void Write(Stream stream)
    {
        using BinaryWriter bw = new(stream, Encoding.UTF8, true);

        bw.Write(0); //File Size, will Seek to start and write it at the end
        bw.Write(FORMAT_TOKEN);
        bw.Write(0); //Version
        bw.Write(this.Flags); //Flags
        bw.Write((ushort)this._joints.Length);
        bw.Write(this.Influences.Count);

        int jointsSectionSize = this._joints.Length * 100;
        int jointHashIdsSectionSize = this._joints.Length * 8;
        int influencesSectionSize = this.Influences.Count * 2;

        int jointsOffset = 64;
        int jointHashIdsOffset = jointsOffset + jointsSectionSize;
        int influencesOffset = jointHashIdsOffset + jointHashIdsSectionSize;
        int jointNamesOffset = influencesOffset + influencesSectionSize;

        bw.Write(jointsOffset);
        bw.Write(jointHashIdsOffset);
        bw.Write(influencesOffset);

        long nameOffsetOffset = bw.BaseStream.Position;
        bw.Write(-1); // Name offset, write later

        long assetNameOffsetOffset = bw.BaseStream.Position;
        bw.Write(-1); // AssetName offset, write later

        bw.Write(jointNamesOffset);
        bw.Write(0xFFFFFFFF); //Write reserved offset field
        bw.Write(0xFFFFFFFF); //Write reserved offset field
        bw.Write(0xFFFFFFFF); //Write reserved offset field
        bw.Write(0xFFFFFFFF); //Write reserved offset field
        bw.Write(0xFFFFFFFF); //Write reserved offset field

        // Write joint names and store offsets
        Span<int> jointNameOffsets = stackalloc int[this._joints.Length];
        bw.Seek(jointNamesOffset, SeekOrigin.Begin);
        for (int i = 0; i < this._joints.Length; i++)
        {
            jointNameOffsets[i] = (int)bw.BaseStream.Position;

            bw.WriteNullTerminatedString(this.Joints[i].Name);
        }

        // Write joints
        bw.Seek(jointsOffset, SeekOrigin.Begin);
        for (int i = 0; i < this.Joints.Count; i++)
            this._joints[i].Write(bw, jointNameOffsets[i]);

        // Write influences
        bw.Seek(influencesOffset, SeekOrigin.Begin);
        foreach (short influence in this.Influences)
            bw.Write(influence);

        // Write joint id hashes in ascending order by hash
        bw.Seek(jointHashIdsOffset, SeekOrigin.Begin);
        foreach (Joint joint in this.Joints.OrderBy(x => Elf.HashLower(x.Name)))
        {
            bw.Write(joint.Id);
            bw.Write((ushort)0);
            bw.Write(Elf.HashLower(joint.Name));
        }

        bw.BaseStream.Seek(0, SeekOrigin.End);

        // Write Name
        long nameOffset = bw.BaseStream.Position;
        bw.WriteNullTerminatedString(this.Name ?? string.Empty);

        // Write Asset Name
        long assetNameOffset = bw.BaseStream.Position;
        bw.WriteNullTerminatedString(this.AssetName ?? string.Empty);

        // Write Name offset to header
        bw.BaseStream.Seek(nameOffsetOffset, SeekOrigin.Begin);
        bw.Write((int)nameOffset);

        // Write Asset Name offset to header
        bw.BaseStream.Seek(assetNameOffsetOffset, SeekOrigin.Begin);
        bw.Write((int)assetNameOffset);

        // Write file size to header
        uint fileSize = (uint)bw.BaseStream.Length;
        bw.BaseStream.Seek(0, SeekOrigin.Begin);
        bw.Write(fileSize);
    }
}

internal readonly struct RigResourceJoint
{
    public string Name { get; init; }
    public ushort Flags { get; init; }
    public short Id { get; init; }
    public short ParentId { get; init; }
    public float Radius { get; init; }
    public Matrix4x4 LocalTransform { get; init; }
    public Matrix4x4 InverseBindTransform { get; init; }

    public static RigResourceJoint Read(BinaryReader br)
    {
        ushort flags = br.ReadUInt16();
        short id = br.ReadInt16();
        short parentId = br.ReadInt16();
        br.ReadInt16(); //padding
        uint nameHash = br.ReadUInt32();
        float radius = br.ReadSingle();

        Vector3 localTranslation = br.ReadVector3();
        Vector3 localScale = br.ReadVector3();
        Quaternion localRotation = br.ReadQuaternion();

        Vector3 inverseBindTranslation = br.ReadVector3();
        Vector3 inverseBindScale = br.ReadVector3();
        Quaternion inverseBindRotation = br.ReadQuaternion();

        int nameOffset = br.ReadInt32();
        long returnOffset = br.BaseStream.Position;

        br.BaseStream.Seek(returnOffset - 4 + nameOffset, SeekOrigin.Begin);
        string name = br.ReadNullTerminatedString();
        br.BaseStream.Seek(returnOffset, SeekOrigin.Begin);

        return new()
        {
            Name = name,
            Flags = flags,
            Id = id,
            ParentId = parentId,
            Radius = radius,
            LocalTransform =
                Matrix4x4.CreateScale(localScale)
                * Matrix4x4.CreateFromQuaternion(localRotation)
                * Matrix4x4.CreateTranslation(localTranslation),
            InverseBindTransform =
                Matrix4x4.CreateScale(inverseBindScale)
                * Matrix4x4.CreateFromQuaternion(inverseBindRotation)
                * Matrix4x4.CreateTranslation(inverseBindTranslation),
        };
    }
}

internal readonly struct RigResourceLegacyJoint
{
    public string Name { get; init; }
    public short Id { get; init; }
    public short ParentId { get; init; }
    public float Radius { get; init; }
    public Matrix4x4 GlobalTransform { get; init; }

    public static RigResourceLegacyJoint Read(BinaryReader br, short id)
    {
        string name = br.ReadPaddedString(32);
        short parentId = (short)br.ReadInt32();
        float radius = br.ReadSingle();
        float[,] transform = new float[4, 4];
        transform[0, 3] = 0;
        transform[1, 3] = 0;
        transform[2, 3] = 0;
        transform[3, 3] = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                transform[j, i] = br.ReadSingle();
            }
        }

        return new()
        {
            Name = name,
            Id = id,
            ParentId = parentId,
            Radius = radius,
            GlobalTransform = new Matrix4x4()
            {
                M11 = transform[0, 0],
                M12 = transform[0, 1],
                M13 = transform[0, 2],
                M14 = transform[0, 3],
                M21 = transform[1, 0],
                M22 = transform[1, 1],
                M23 = transform[1, 2],
                M24 = transform[1, 3],
                M31 = transform[2, 0],
                M32 = transform[2, 1],
                M33 = transform[2, 2],
                M34 = transform[2, 3],
                M41 = transform[3, 0],
                M42 = transform[3, 1],
                M43 = transform[3, 2],
                M44 = transform[3, 3],
            }
        };
    }
}
