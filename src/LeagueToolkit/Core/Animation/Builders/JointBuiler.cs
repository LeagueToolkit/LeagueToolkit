using CommunityToolkit.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.Core.Animation.Builders;

[DebuggerDisplay("{GetPath(), nq}", Name = "{Name}")]
public sealed class JointBuilder
{
    public string Name { get; set; }
    public ushort Flags { get; set; }
    public bool IsInfluence { get; set; }

    public JointBuilder Parent { get; private set; }

    public float Radius { get; set; } = 2.1f;

    public Matrix4x4 LocalTransform { get; set; }
    public Matrix4x4 InverseBindTransform { get; set; }

    public IReadOnlyList<JointBuilder> Children => this._children;
    private readonly List<JointBuilder> _children = new();

    public JointBuilder(string name) : this(name, null) { }

    internal JointBuilder(string name, JointBuilder parent)
    {
        Guard.IsNotNull(name, nameof(name));

        this.Name = name;
        this.Parent = parent;
    }

    public JointBuilder WithName(string name)
    {
        Guard.IsNotNull(name, nameof(name));

        this.Name = name;

        return this;
    }

    public JointBuilder WithFlags(ushort flags)
    {
        this.Flags = flags;

        return this;
    }

    public JointBuilder WithInfluence(bool isInfluence)
    {
        this.IsInfluence = isInfluence;

        return this;
    }

    public JointBuilder WithRadius(float radius)
    {
        this.Radius = radius;

        return this;
    }

    public JointBuilder WithLocalTransform(Matrix4x4 localTransform)
    {
        this.LocalTransform = localTransform;

        return this;
    }

    public JointBuilder WithInverseBindTransform(Matrix4x4 inverseBindTransform)
    {
        this.InverseBindTransform = inverseBindTransform;

        return this;
    }

    public JointBuilder CreateJoint(string name)
    {
        JointBuilder child = new(name, this);

        this._children.Add(child);

        return child;
    }

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
