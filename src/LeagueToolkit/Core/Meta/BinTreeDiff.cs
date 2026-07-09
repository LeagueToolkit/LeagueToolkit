namespace LeagueToolkit.Core.Meta;

/// <summary>
/// Describes how an element changed between two property bins.
/// </summary>
public enum BinTreeDiffKind
{
    Added,
    Removed,
    Modified
}

/// <summary>
/// Represents the object graph differences between two property bins.
/// </summary>
public sealed class BinTreeDiff
{
    /// <summary>
    /// Gets the object differences, ordered by object path hash.
    /// </summary>
    public IReadOnlyList<BinTreeObjectDiff> Objects { get; }

    /// <summary>
    /// Gets a value indicating whether both property bins contain equal object graphs.
    /// </summary>
    public bool IsEmpty => this.Objects.Count is 0;

    internal BinTreeDiff(IReadOnlyList<BinTreeObjectDiff> objects) => this.Objects = objects;
}

/// <summary>
/// Represents a semantic change to an object in a property bin.
/// </summary>
public abstract class BinTreeObjectDiff
{
    /// <summary>
    /// Gets the kind of change.
    /// </summary>
    public abstract BinTreeDiffKind Kind { get; }

    /// <summary>
    /// Gets the path hash of the changed object.
    /// </summary>
    public uint PathHash { get; }

    private protected BinTreeObjectDiff(uint pathHash) => this.PathHash = pathHash;
}

/// <summary>
/// Represents an object added to a property bin.
/// </summary>
public sealed class AddedBinTreeObjectDiff : BinTreeObjectDiff
{
    /// <inheritdoc/>
    public override BinTreeDiffKind Kind => BinTreeDiffKind.Added;

    /// <summary>
    /// Gets the added object.
    /// </summary>
    public BinTreeObject Object { get; }

    internal AddedBinTreeObjectDiff(BinTreeObject treeObject) : base(treeObject.PathHash) => this.Object = treeObject;
}

/// <summary>
/// Represents an object removed from a property bin.
/// </summary>
public sealed class RemovedBinTreeObjectDiff : BinTreeObjectDiff
{
    /// <inheritdoc/>
    public override BinTreeDiffKind Kind => BinTreeDiffKind.Removed;

    /// <summary>
    /// Gets the removed object.
    /// </summary>
    public BinTreeObject Object { get; }

    internal RemovedBinTreeObjectDiff(BinTreeObject treeObject) : base(treeObject.PathHash) => this.Object = treeObject;
}

/// <summary>
/// Represents an object modified between two property bins.
/// </summary>
public sealed class ModifiedBinTreeObjectDiff : BinTreeObjectDiff
{
    /// <inheritdoc/>
    public override BinTreeDiffKind Kind => BinTreeDiffKind.Modified;

    /// <summary>
    /// Gets the object before the modification.
    /// </summary>
    public BinTreeObject OldObject { get; }

    /// <summary>
    /// Gets the object after the modification.
    /// </summary>
    public BinTreeObject NewObject { get; }

    /// <summary>
    /// Gets the property differences, ordered by property path.
    /// </summary>
    public IReadOnlyList<BinTreePropertyDiff> Properties { get; }

    internal ModifiedBinTreeObjectDiff(
        BinTreeObject oldObject,
        BinTreeObject newObject,
        IReadOnlyList<BinTreePropertyDiff> properties
    ) : base(oldObject.PathHash)
    {
        this.OldObject = oldObject;
        this.NewObject = newObject;
        this.Properties = properties;
    }
}

/// <summary>
/// Represents a semantic change to a property. <see cref="Path"/> contains property name hashes
/// from the object root to the changed property.
/// </summary>
public abstract class BinTreePropertyDiff
{
    /// <summary>
    /// Gets the kind of change.
    /// </summary>
    public abstract BinTreeDiffKind Kind { get; }

    /// <summary>
    /// Gets the property name hashes from the object root to the changed property.
    /// </summary>
    public IReadOnlyList<uint> Path { get; }

    private protected BinTreePropertyDiff(IReadOnlyList<uint> path) => this.Path = path;
}

/// <summary>
/// Represents a property added to an object.
/// </summary>
public sealed class AddedBinTreePropertyDiff : BinTreePropertyDiff
{
    /// <inheritdoc/>
    public override BinTreeDiffKind Kind => BinTreeDiffKind.Added;

    /// <summary>
    /// Gets the added property.
    /// </summary>
    public BinTreeProperty Property { get; }

    internal AddedBinTreePropertyDiff(IReadOnlyList<uint> path, BinTreeProperty property) : base(path) =>
        this.Property = property;
}

/// <summary>
/// Represents a property removed from an object.
/// </summary>
public sealed class RemovedBinTreePropertyDiff : BinTreePropertyDiff
{
    /// <inheritdoc/>
    public override BinTreeDiffKind Kind => BinTreeDiffKind.Removed;

    /// <summary>
    /// Gets the removed property.
    /// </summary>
    public BinTreeProperty Property { get; }

    internal RemovedBinTreePropertyDiff(IReadOnlyList<uint> path, BinTreeProperty property) : base(path) =>
        this.Property = property;
}

/// <summary>
/// Represents a property modified between two objects.
/// </summary>
public sealed class ModifiedBinTreePropertyDiff : BinTreePropertyDiff
{
    /// <inheritdoc/>
    public override BinTreeDiffKind Kind => BinTreeDiffKind.Modified;

    /// <summary>
    /// Gets the property before the modification.
    /// </summary>
    public BinTreeProperty OldProperty { get; }

    /// <summary>
    /// Gets the property after the modification.
    /// </summary>
    public BinTreeProperty NewProperty { get; }

    internal ModifiedBinTreePropertyDiff(
        IReadOnlyList<uint> path,
        BinTreeProperty oldProperty,
        BinTreeProperty newProperty
    ) : base(path)
    {
        this.OldProperty = oldProperty;
        this.NewProperty = newProperty;
    }
}
