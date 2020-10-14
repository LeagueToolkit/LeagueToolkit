using Fantome.Libraries.League.Meta.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fantome.Libraries.League.Meta
{
    public sealed class MetaEnvironment
    {
        public ReadOnlyCollection<Type> RegisteredMetaClasses { get; }

        private List<Type> _registeredMetaClasses = new();

        private MetaEnvironment()
        {
            this.RegisteredMetaClasses = this._registeredMetaClasses.AsReadOnly();
        }
        internal MetaEnvironment(ICollection<Type> metaClasses)
        {
            this._registeredMetaClasses = metaClasses.ToList();
            this.RegisteredMetaClasses = this._registeredMetaClasses.AsReadOnly();

            foreach (Type metaClass in this.RegisteredMetaClasses)
            {
                MetaClassAttribute metaClassAttribute = metaClass.GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;
                if (metaClassAttribute is null) throw new ArgumentException($"MetaClass: {metaClass.Name} does not have MetaClass Attribute");
            }
        }

        public static MetaEnvironment Create(ICollection<Type> metaClasses)
        {
            return new MetaEnvironment(metaClasses);
        }

        public Type FindMetaClass(uint classNameHash)
        {
            return this.RegisteredMetaClasses.FirstOrDefault(x =>
            {
                MetaClassAttribute metaClassAttribute = x.GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;

                return metaClassAttribute?.NameHash == classNameHash;
            });
        }
    }
}
