using Fantome.Libraries.League.Helpers.Hashing;
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
        public ReadOnlyDictionary<uint, IMetaClass> RegisteredObjects { get; }

        private List<Type> _registeredMetaClasses = new();
        private Dictionary<uint, IMetaClass> _registeredObjects = new();

        internal MetaEnvironment(ICollection<Type> metaClasses)
        {
            this._registeredMetaClasses = metaClasses.ToList();
            this.RegisteredMetaClasses = this._registeredMetaClasses.AsReadOnly();
            this.RegisteredObjects = new ReadOnlyDictionary<uint, IMetaClass>(this._registeredObjects);

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

        public void RegisterObject<T>(string path, T metaObject)
            where T : IMetaClass
        {
            MetaClassAttribute metaClassAttribute = metaObject.GetType().GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;
            if (metaClassAttribute is null) throw new ArgumentException($"{nameof(metaObject)} does not have a Meta Class attribute");

            metaClassAttribute.SetPath(path);

            uint pathHash = Fnv1a.HashLower(path);
            if (this._registeredObjects.TryAdd(pathHash, metaObject) is false)
            {
                throw new ArgumentException("An object with the same path is already registered", nameof(pathHash));
            }
        }
        public void RegisterObject<T>(uint pathHash, T metaObject)
            where T : IMetaClass
        {
            MetaClassAttribute metaClassAttribute = metaObject.GetType().GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;
            if (metaClassAttribute is null) throw new ArgumentException($"{nameof(metaObject)} does not have a Meta Class attribute");

            metaClassAttribute.SetPath(pathHash);

            if (this._registeredObjects.TryAdd(pathHash, metaObject) is false)
            {
                throw new ArgumentException("An object with the same path is already registered", nameof(pathHash));
            }
        }

        public Type FindMetaClass(uint classNameHash)
        {
            return this.RegisteredMetaClasses.FirstOrDefault(x =>
            {
                MetaClassAttribute metaClassAttribute = x.GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;

                return metaClassAttribute?.NameHash == classNameHash;
            });
        }
        public T FindObject<T>(string path)
            where T : IMetaClass
        {
            return FindObject<T>(Fnv1a.HashLower(path));
        }
        public T FindObject<T>(uint pathHash)
            where T : IMetaClass
        {
            return (T)this._registeredObjects.GetValueOrDefault(pathHash);
        }
    }
}
