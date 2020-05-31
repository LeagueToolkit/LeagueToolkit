using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.NavigationGridOverlay
{
    public class NavigationGridOverlay
    {
        public List<NavigationGridOverlayRegion> Regions { get; set; } = new List<NavigationGridOverlayRegion>();

        public NavigationGridOverlay(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public NavigationGridOverlay(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                byte version = br.ReadByte();
                if(version != 1)
                {
                    throw new Exception("Unsupported Version: " + version);
                }

                byte regionCount = br.ReadByte();
                for(int i = 0; i < regionCount; i++)
                {
                    this.Regions.Add(new NavigationGridOverlayRegion(br));
                }
            }
        }
    }
}
