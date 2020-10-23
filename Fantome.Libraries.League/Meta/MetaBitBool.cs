using System;
using System.Collections.Generic;
using System.Text;

namespace Fantome.Libraries.League.Meta
{
    public struct MetaBitBool
    {
        public byte Value { get; set; }

        public MetaBitBool(byte value)
        {
            this.Value = value;
        }

        public static implicit operator byte(MetaBitBool bitBool) => bitBool.Value;
    }
}
