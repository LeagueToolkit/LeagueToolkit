using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Meta.Dump
{
    public sealed class MetaDumpClass
    {
        [JsonProperty(PropertyName = "alignment")] public uint Alignment { get; private set; }
        [JsonProperty(PropertyName = "classSize")] public uint ClassSize { get; private set; }
        [JsonProperty(PropertyName = "constructor")] public uint Constructor { get; private set; }
        [JsonProperty(PropertyName = "destructor")] public uint Destructor { get; private set; }
        [JsonProperty(PropertyName = "hash")] public uint Hash { get; private set; }
        [JsonProperty(PropertyName = "initfunction")] public uint InitFunction { get; private set; }
        [JsonProperty(PropertyName = "inplaceconstructor")] public uint InplaceConstructor { get; private set; }
        [JsonProperty(PropertyName = "inplacedestructor")] public uint InplaceDestructor { get; private set; }
        [JsonProperty(PropertyName = "isInterface")] public bool IsInterface { get; private set; }
        [JsonProperty(PropertyName = "isPropertyBase")] public bool IsPropertyBase { get; private set; }
        [JsonProperty(PropertyName = "isSecondaryBase")] public bool IsSecondaryBase { get; private set; }
        [JsonProperty(PropertyName = "isUnk5")] public bool IsUnknown5 { get; private set; }
        [JsonProperty(PropertyName = "isValue")] public bool IsValue { get; private set; }
        [JsonProperty(PropertyName = "parentClass")] public uint ParentClass { get; private set; }
        [JsonProperty(PropertyName = "properties")] public List<MetaDumpProperty> Properties { get; private set; }
        [JsonProperty(PropertyName = "secondaryBases")] public List<uint[]> Implements { get; private set; }
        [JsonProperty(PropertyName = "secondaryChildren")] public List<uint[]> ImplementedBy { get; private set; }
        [JsonProperty(PropertyName = "upcastSecondary")] public uint UpcastSecondary { get; private set; }

        internal List<uint> GetInterfaces(List<MetaDumpClass> classes, bool includeMainParent)
        {
            List<uint> interfaces = new();

            if (includeMainParent && classes.FirstOrDefault(x => x.Hash == this.ParentClass && x.IsInterface) is MetaDumpClass parentInterface)
            {
                interfaces.Add(parentInterface.Hash);
                interfaces.AddRange(parentInterface.GetInterfacesRecursive(classes, true));
            }

            for(int i = 0; i < this.Implements.Count; i++)
            {
                interfaces.Add(this.Implements[i][0]);
            }

            return interfaces;
        }
        internal List<uint> GetInterfacesRecursive(List<MetaDumpClass> classes, bool includeMainParent)
        {
            List<uint> interfaces = new();

            if (includeMainParent && classes.FirstOrDefault(x => x.Hash == this.ParentClass && x.IsInterface) is MetaDumpClass parentInterface)
            {
                interfaces.Add(parentInterface.Hash);
                interfaces.AddRange(parentInterface.GetInterfacesRecursive(classes, true));
            }

            for (int i = 0; i < this.Implements.Count; i++)
            {
                uint interfaceHash = this.Implements[i][0];
                MetaDumpClass interfaceClass = classes.FirstOrDefault(x => x.Hash == interfaceHash && x.IsInterface);

                interfaces.Add(interfaceHash);
                
                if (interfaceClass is not null)
                {
                    interfaces.AddRange(interfaceClass.GetInterfacesRecursive(classes, true));
                }
                else throw new InvalidOperationException("Failed to find interface: " + interfaceHash);
                
            }

            return interfaces;
        }
    }
}
