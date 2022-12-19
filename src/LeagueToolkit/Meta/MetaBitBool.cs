using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueToolkit.Meta
{
    public struct MetaBitBool
    {
        public byte Value { get; set; }

        public MetaBitBool(bool value)
        {
            this.Value = (byte)(value ? 1 : 0);
        }
        public MetaBitBool(byte value)
        {
            this.Value = value;
        }

        public static implicit operator byte(MetaBitBool bitBool) => bitBool.Value;
        public static implicit operator bool(MetaBitBool bitBool) => bitBool.Value == 1 ? true : false;
    }
}
