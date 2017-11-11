using System.Collections.Generic;

namespace Fantome.Libraries.League.IO.Skeleton
{
    /// <summary>
    /// Represents an abstract class from which all SKL Bone types inherit
    /// </summary>
    public abstract class SKLBone
    {
        /// <summary>
        /// The Name of this <see cref="SKLBone"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Children bones of this <see cref="SKLBone"/>
        /// </summary>
        public List<SKLBone> Children { get; set; }
    }
}
