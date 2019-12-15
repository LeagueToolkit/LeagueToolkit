using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.MaterialLibrary
{
    public class MaterialLibraryFile
    {
        public List<MaterialLibraryMaterial> Materials = new List<MaterialLibraryMaterial>();

        public MaterialLibraryFile() { }

        public MaterialLibraryFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {

        }
        public MaterialLibraryFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    if (sr.ReadLine() == "[MaterialBegin]")
                    {
                        this.Materials.Add(new MaterialLibraryMaterial(sr));
                    }
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        public void Write(Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (MaterialLibraryMaterial material in this.Materials)
                {
                    material.Write(sw);
                    sw.WriteLine();
                }
            }
        }
    }
}
