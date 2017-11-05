using System.Collections.Generic;

namespace Fantome.Libraries.League.IO.Skeleton
{
    /// <summary>
    /// Represents an Interface from which all SKL Bone types inherit
    /// </summary>
    public interface ISKLBone
    {
        /// <summary>
        /// The Name of this <see cref="ISKLBone"/>
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The Children bones of this <see cref="ISKLBone"/>
        /// </summary>
        List<ISKLBone> Children { get; set; }

        void AddBone(ISKLBone bone);
    }
}
