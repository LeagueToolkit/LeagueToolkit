using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.RMAN
{
    public class RMANUnknown
    {
        //If anybody feels like having a look at this go ahead :D
        public byte[] Data { get; private set; }

        public RMANUnknown(BinaryReader br)
        {
            this.Data = br.ReadBytes(36);
        }
    }
}
