using CommunityToolkit.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation.Builders;

public sealed class RigResourceBuilder
{
    public ushort Flags { get; set; }
    public string Name { get; set; }
    public string AssetName { get; set; }

    public IReadOnlyList<JointBuilder> Joints => this._joints;
    private readonly List<JointBuilder> _joints = new();

    public RigResourceBuilder() { }

    public JointBuilder CreateJoint(string name)
    {
        Guard.IsNotNull(name, nameof(name));

        JointBuilder joint = new(name, null);
        this._joints.Add(joint);

        return joint;
    }

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
