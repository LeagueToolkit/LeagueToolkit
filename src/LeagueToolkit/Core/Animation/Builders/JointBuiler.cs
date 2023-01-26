using CommunityToolkit.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Animation.Builders;

/// <summary>
/// Provides an interface for building a <see cref="Joint"/> object
/// </summary>
[DebuggerDisplay("{GetPath(), nq}", Name = "{Name}")]
public sealed class JointBuilder
{
    /// <summary>
    /// Gets or sets the name of the joint
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the flags of the joint
    /// </summary>
    public ushort Flags { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the joint is a skin influence
    /// </summary>
    public bool IsInfluence { get; set; }

    /// <summary>
    /// Gets the parent of the joint
    /// </summary>
    /// <remarks>
    /// Set to <see langword="null"/> if the joint has no parent
    /// </remarks>
    public JointBuilder Parent { get; private set; }

    /// <summary>
    /// Gets or sets the radius of the joint
    /// </summary>
    public float Radius { get; set; } = 2.1f;

    /// <summary>
    /// Gets or sets the local transform of the joint
    /// </summary>
    public Matrix4x4 LocalTransform { get; set; }

    /// <summary>
    /// Gets or sets the inverse bind transform of the joint
    /// </summary>
    public Matrix4x4 InverseBindTransform { get; set; }

    /// <summary>
    /// Gets the children of the joint
    /// </summary>
    public IReadOnlyList<JointBuilder> Children => this._children;
    private readonly List<JointBuilder> _children = new();

    /// <summary>
    /// Creates a new <see cref="JointBuilder"/> object with the specified parameters
    /// </summary>
    /// <param name="name">The name of the joint</param>
    /// <remarks>
    /// This creates a root joint (without a parent)
    /// </remarks>
    public JointBuilder(string name) : this(name, null) { }

    internal JointBuilder(string name, JointBuilder parent)
    {
        Guard.IsNotNull(name, nameof(name));

        this.Name = name;
        this.Parent = parent;
    }

    /// <summary>
    /// Sets the name of the <see cref="JointBuilder"/>
    /// </summary>
    /// <param name="name">The name of the <see cref="JointBuilder"/></param>
    public JointBuilder WithName(string name)
    {
        Guard.IsNotNull(name, nameof(name));

        this.Name = name;

        return this;
    }

    /// <summary>
    /// Sets the flags of the <see cref="JointBuilder"/>
    /// </summary>
    /// <param name="flags">The flags of the <see cref="JointBuilder"/></param>
    public JointBuilder WithFlags(ushort flags)
    {
        this.Flags = flags;

        return this;
    }

    /// <summary>
    /// Sets a value indicating whether the <see cref="JointBuilder"/> is a skin influence
    /// </summary>
    /// <param name="isInfluence">The value indicating whether the <see cref="JointBuilder"/> is a skin influence</param>
    public JointBuilder WithInfluence(bool isInfluence)
    {
        this.IsInfluence = isInfluence;

        return this;
    }

    /// <summary>
    /// Sets the radius of the <see cref="JointBuilder"/>
    /// </summary>
    /// <param name="radius">The radius of the <see cref="JointBuilder"/></param>
    public JointBuilder WithRadius(float radius)
    {
        this.Radius = radius;

        return this;
    }

    /// <summary>
    /// Sets the local transform of the <see cref="JointBuilder"/>
    /// </summary>
    /// <param name="localTransform">The local transform of the <see cref="JointBuilder"/></param>
    public JointBuilder WithLocalTransform(Matrix4x4 localTransform)
    {
        this.LocalTransform = localTransform;

        return this;
    }

    /// <summary>
    /// Sets the inverse bind transform of the <see cref="JointBuilder"/>
    /// </summary>
    /// <param name="inverseBindTransform">The inverse bind transform of the <see cref="JointBuilder"/></param>
    public JointBuilder WithInverseBindTransform(Matrix4x4 inverseBindTransform)
    {
        this.InverseBindTransform = inverseBindTransform;

        return this;
    }

    /// <summary>
    /// Creates a new <see cref="JointBuilder"/> and adds it to <see cref="Children"/>
    /// </summary>
    /// <param name="name">The name of the child <see cref="JointBuilder"/></param>
    /// <returns>The created child <see cref="JointBuilder"/></returns>
    public JointBuilder CreateJoint(string name)
    {
        JointBuilder child = new(name, this);

        this._children.Add(child);

        return child;
    }

    /// <summary>
    /// Enumerates all children <see cref="JointBuilder"/> instances recursively
    /// </summary>
    public IEnumerable<JointBuilder> TraverseChildren()
    {
        foreach (JointBuilder joint in this._children)
        {
            yield return joint;

            foreach (JointBuilder jointChild in joint.TraverseChildren())
                yield return jointChild;
        }
    }

    private string GetPath() =>
        this.Parent switch
        {
            null => this.Name,
            _ => $"{this.Parent.GetPath()} | {this.Name}"
        };
}
