using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Meta.Properties;
using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Exceptions;
using System;
using System.Text;

namespace LeagueToolkit.Core.Meta;

/// <summary>
/// Represents a property bin with a tree structure
/// </summary>
public sealed class BinTree
{
    public bool IsOverride { get; private set; }

    /// <summary>
    /// Gets the dependencies
    /// </summary>
    /// <remarks>
    /// Property bins can depend on other property bins in a similar fashion like importing code libraries
    /// </remarks>
    public List<string> Dependencies { get; } = new();

    /// <summary>
    /// Gets the objects
    /// </summary>
    public IReadOnlyList<BinTreeObject> Objects => this._objects;
    private readonly List<BinTreeObject> _objects = new();

    public BinTree(string path) : this(File.OpenRead(path)) { }

    /// <summary>
    /// Creates a new <see cref="BinTree"/> by reading it from the specified stream
    /// </summary>
    /// <param name="stream">The stream to read from</param>
    public BinTree(Stream stream)
    {
        using BinaryReader br = new(stream, Encoding.UTF8, true);

        string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
        if (magic != "PROP" && magic != "PTCH")
            throw new InvalidFileSignatureException();

        if (magic is "PTCH")
        {
            this.IsOverride = true;

            uint overrideVersion = br.ReadUInt32();
            if (overrideVersion is not 1)
                throw new UnsupportedFileVersionException();

            // It might be possible to create an override property bin
            // and set the original file as a dependency
            uint maybeOverrideObjectCount = br.ReadUInt32(); // this seems to be object count of override section

            magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (magic is not "PROP")
                throw new InvalidFileSignatureException("Expected PROP section after PTCH, got: " + magic);
        }

        uint version = br.ReadUInt32();
        if (version is not (1 or 2 or 3))
            throw new UnsupportedFileVersionException();

        // Read dependencies
        if (version >= 2)
        {
            uint dependencyCount = br.ReadUInt32();
            for (int i = 0; i < dependencyCount; i++)
                this.Dependencies.Add(Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt16())));
        }

        // Read object classes
        uint objectCount = br.ReadUInt32();
        uint[] objectClasses = new uint[objectCount];
        br.Read(objectClasses.AsSpan().Cast<uint, byte>());

        // This is another mega brain 5Head piece of code
        // Since riot is a bit retarded, they created a new property type (WadChunkLink) in the middle of the fucking enum
        // I'm too lazy to think of another hacky method to do this in a better way so here we go
        long objectsOffset = br.BaseStream.Position;
        try
        {
            // Read objects
            for (int i = 0; i < objectCount; i++)
                this._objects.Add(BinTreeObject.Read(objectClasses[i], br, useLegacyType: false));
        }
        catch (InvalidPropertyTypeException)
        {
            // Oopsie woopsie fucky wucky we hit a "legacy" property bin
            // Reset position to objects start and read in "legacy" mode
            br.BaseStream.Seek(objectsOffset, SeekOrigin.Begin);
            this._objects.Clear();

            // Read objects
            for (int i = 0; i < objectCount; i++)
                this._objects.Add(BinTreeObject.Read(objectClasses[i], br, useLegacyType: true));
        }

        // Read override section
        if (version >= 3 && this.IsOverride)
            ReadPatchSection(br);
    }

    private void ReadPatchSection(BinaryReader br)
    {
        uint dataOverrideCount = br.ReadUInt32();
        for (int i = 0; i < dataOverrideCount; i++)
        {
            uint pathHash = br.ReadUInt32();
            uint size = br.ReadUInt32();
            BinPropertyType type = (BinPropertyType)br.ReadByte();
            string objectPath = Encoding.ASCII.GetString(br.ReadBytes(br.ReadUInt16()));

            BinTreeProperty property = BinTreeProperty.ReadPropertyContent(0, type, br);
        }
    }

    public void Write(string fileLocation) => Write(File.Create(fileLocation));

    public void Write(Stream stream, bool leaveOpen = false)
    {
        using BinaryWriter bw = new(stream, Encoding.UTF8, leaveOpen);

        if (this.IsOverride)
        {
            bw.Write("PTCH"u8);
            bw.Write(1); // override version
            bw.Write(0); // unknown
        }

        bw.Write("PROP"u8);
        bw.Write(3); // version

        bw.Write(this.Dependencies.Count);
        foreach (string dependency in this.Dependencies)
        {
            bw.Write((ushort)dependency.Length);
            bw.Write(dependency.AsSpan());
        }

        bw.Write(this._objects.Count);
        foreach (BinTreeObject treeObject in this._objects)
            bw.Write(treeObject.ClassHash);

        foreach (BinTreeObject treeObject in this._objects)
            treeObject.Write(bw);

        if (this.IsOverride)
        {
            //bw.Write(this._patchObjectCount);
            //foreach (BinTreePatchObject patchObject in this._patchObjects)
            //{
            //    patchObject.Write(bw);
            //}
        }
    }

    public void AddObject(BinTreeObject treeObject)
    {
        if (this._objects.Any(x => x.PathHash == treeObject.PathHash))
        {
            throw new InvalidOperationException("An object with the same path already exists");
        }
        else
        {
            this._objects.Add(treeObject);
        }
    }

    public void RemoveObject(uint pathHash)
    {
        if (this._objects.FirstOrDefault(x => x.PathHash == pathHash) is BinTreeObject treeObject)
        {
            this._objects.Remove(treeObject);
        }
        else
            throw new ArgumentException("Failed to find an object with the specified path hash", nameof(pathHash));
    }
}

public interface IBinTreeParent { }
