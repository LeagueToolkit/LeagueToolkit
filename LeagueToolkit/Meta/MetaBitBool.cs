using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueToolkit.Meta
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
