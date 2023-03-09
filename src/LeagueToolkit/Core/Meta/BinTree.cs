using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Utils.Exceptions;
using System.Text;

namespace LeagueToolkit.Core.Meta;

/// <summary>
/// Represents a property bin with a tree structure
/// </summary>
public sealed class BinTree
{
    /// <summary>
    /// Gets a value indicating whether the property bin is an override
    /// </summary>
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
    public Dictionary<uint, BinTreeObject> Objects { get; } = new();

    /// <summary>
    /// Gets the property overrides
    /// </summary>
    public IReadOnlyList<BinTreeDataOverride> DataOverrides => this._dataOverrides;
    private readonly List<BinTreeDataOverride> _dataOverrides = new();

    /// <summary>
    /// Creates a new empty <see cref="BinTree"/> object
    /// </summary>
    public BinTree() { }

    /// <summary>
    /// Creates a new <see cref="BinTree"/> object with the specified parameters
    /// </summary>
    /// <param name="objects">The objects of the property bin</param>
    /// <param name="dependencies">The dependencies of the property bin</param>
    public BinTree(IEnumerable<BinTreeObject> objects, IEnumerable<string> dependencies)
    {
        Guard.IsNotNull(objects, nameof(objects));
        Guard.IsNotNull(dependencies, nameof(dependencies));

        this.Objects = objects.ToDictionary(x => x.PathHash);
        this.Dependencies = dependencies.ToList();
    }

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
                throw new InvalidFileVersionException();

            // It might be possible to create an override property bin
            // and set the original file as a dependency
            uint maybeOverrideObjectCount = br.ReadUInt32(); // this seems to be object count of override section

            magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (magic is not "PROP")
                throw new InvalidFileSignatureException("Expected PROP section after PTCH, got: " + magic);
        }

        uint version = br.ReadUInt32();
        if (version is not (1 or 2 or 3))
            throw new InvalidFileVersionException();

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
            {
                BinTreeObject treeObject = BinTreeObject.Read(objectClasses[i], br, useLegacyType: false);

                this.Objects.Add(treeObject.PathHash, treeObject);
            }
        }
        catch (InvalidPropertyTypeException)
        {
            // Oopsie woopsie fucky wucky we hit a "legacy" property bin
            // Reset position to objects start and read in "legacy" mode
            br.BaseStream.Seek(objectsOffset, SeekOrigin.Begin);
            this.Objects.Clear();

            // Read objects
            for (int i = 0; i < objectCount; i++)
            {
                BinTreeObject treeObject = BinTreeObject.Read(objectClasses[i], br, useLegacyType: true);

                this.Objects.Add(treeObject.PathHash, treeObject);
            }
        }

        // Read data overrides
        if (version >= 3 && this.IsOverride)
        {
            uint dataOverrideCount = br.ReadUInt32();
            for (int i = 0; i < dataOverrideCount; i++)
                this._dataOverrides.Add(BinTreeDataOverride.Read(br));
        }
    }

    /// <summary>
    /// Writes the property bin to the specified path
    /// </summary>
    /// <param name="path">The path to the written property bin</param>
    public void Write(string path) => Write(File.Create(path));

    /// <summary>
    /// Writes the property bin into the specified stream
    /// </summary>
    /// <param name="stream">The stream to write to</param>
    public void Write(Stream stream)
    {
        using BinaryWriter bw = new(stream, Encoding.UTF8, true);

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

        bw.Write(this.Objects.Count);
        foreach (var (_, treeObject) in this.Objects.OrderBy(x => x.Key))
            bw.Write(treeObject.ClassHash);

        foreach (var (_, treeObject) in this.Objects.OrderBy(x => x.Key))
            treeObject.Write(bw);

        if (this.IsOverride)
        {
            bw.Write(this._dataOverrides.Count);
            foreach (BinTreeDataOverride dataOverride in this._dataOverrides)
                dataOverride.Write(bw);
        }
    }
}
