using CommunityToolkit.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueToolkit.Core.Animation.Builders;

/// <summary>
/// Provides an interface for creating a <see cref="RigResource"/>
/// </summary>
public sealed class RigResourceBuilder
{
    /// <summary>
    /// Gets or sets the flags of the rig
    /// </summary>
    public ushort Flags { get; set; }

    /// <summary>
    /// Gets or sets the name of the rig
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the asset name of the rig
    /// </summary>
    public string AssetName { get; set; }

    /// <summary>
    /// Gets the root joints of the rig
    /// </summary>
    public IReadOnlyList<JointBuilder> Joints => this._joints;
    private readonly List<JointBuilder> _joints = new();

    /// <summary>
    /// Creates a new <see cref="RigResourceBuilder"/> object
    /// </summary>
    public RigResourceBuilder() { }

    /// <summary>
    /// Creates a new root <see cref="JointBuilder"/> object
    /// </summary>
    /// <param name="name">The name of the <see cref="JointBuilder"/></param>
    /// <returns>The created <see cref="JointBuilder"/> object</returns>
    public JointBuilder CreateJoint(string name)
    {
        Guard.IsNotNull(name, nameof(name));

        JointBuilder joint = new(name, null);
        this._joints.Add(joint);

        return joint;
    }

    /// <summary>
    /// Creates a new <see cref="RigResource"/> object
    /// </summary>
    public RigResource Build()
    {
        JointBuilder[] flatJointBuilders = TraverseJoints().ToArray();
        Joint[] joints = new Joint[flatJointBuilders.Length];
        List<short> influences = new(flatJointBuilders.Length);

        for (short i = 0; i < flatJointBuilders.Length; i++)
        {
            JointBuilder jointBuilder = flatJointBuilders[i];
            int parentId = jointBuilder.Parent is null
                ? -1
                : Array.FindIndex(flatJointBuilders, x => x == jointBuilder.Parent);

            joints[i] = new(
                jointBuilder.Name,
                jointBuilder.Flags,
                i,
                (short)parentId,
                jointBuilder.Radius,
                jointBuilder.LocalTransform,
                jointBuilder.InverseBindTransform
            );

            if (jointBuilder.IsInfluence)
                influences.Add(i);
        }

        return new(this.Flags, this.Name, this.AssetName, joints, influences);
    }

    /// <summary>
    /// Enumerates all children <see cref="JointBuilder"/> instances recursively
    /// </summary>
    public IEnumerable<JointBuilder> TraverseJoints()
    {
        foreach (JointBuilder joint in this._joints)
        {
            yield return joint;

            foreach (JointBuilder jointChild in joint.TraverseChildren())
                yield return jointChild;
        }
    }
}
