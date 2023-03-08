using CommunityToolkit.Diagnostics;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LeagueToolkit.Meta
{
    public sealed class MetaEnvironment
    {
        public IReadOnlyDictionary<uint, Type> RegisteredMetaClasses => this._registeredMetaClasses;
        public IReadOnlyDictionary<uint, IMetaClass> RegisteredObjects => this._registeredObjects;

        private readonly Dictionary<uint, Type> _registeredMetaClasses = new();
        private readonly Dictionary<uint, IMetaClass> _registeredObjects = new();

        internal MetaEnvironment(IEnumerable<Type> metaClasses)
        {
            Guard.IsNotNull(metaClasses, nameof(metaClasses));

            foreach (Type metaClass in metaClasses)
            {
                Attribute customAttribute = metaClass.GetCustomAttribute(typeof(MetaClassAttribute));
                if (customAttribute is not MetaClassAttribute metaClassAttribute)
                    throw new ArgumentException(
                        $"{metaClass.Name} does not have {nameof(MetaClassAttribute)}",
                        nameof(metaClasses)
                    );

                this._registeredMetaClasses.Add(metaClassAttribute.NameHash, metaClass);
            }
        }

        /// <summary>
        /// Creates a new <see cref="MetaEnvironment"/> object with the specified parameters
        /// </summary>
        /// <param name="metaClasses">The <see cref="IMetaClass"/> classes used by the <see cref="MetaEnvironment"/></param>
        /// <returns>The created <see cref="MetaEnvironment"/> object</returns>
        public static MetaEnvironment Create(IEnumerable<Type> metaClasses)
        {
            Guard.IsNotNull(metaClasses, nameof(metaClasses));

            return new(metaClasses);
        }

        public void RegisterObject<T>(string path, T metaObject) where T : IMetaClass =>
            RegisterObject(Fnv1a.HashLower(path), metaObject);

        public void RegisterObject<T>(uint pathHash, T metaObject) where T : IMetaClass
        {
            if (metaObject.GetType().GetCustomAttribute(typeof(MetaClassAttribute)) is not MetaClassAttribute)
                ThrowHelper.ThrowArgumentException(
                    nameof(metaObject),
                    $"{nameof(metaObject)} does not have a {nameof(MetaClassAttribute)}"
                );

            if (this._registeredObjects.TryAdd(pathHash, metaObject) is false)
                ThrowHelper.ThrowArgumentException(
                    nameof(pathHash),
                    $"Object with {nameof(pathHash)}: {pathHash} is already registered"
                );
        }

        public bool DeregisterObject(string path) => DeregisterObject(Fnv1a.HashLower(path));

        public bool DeregisterObject(uint pathHash) => this._registeredObjects.Remove(pathHash);

        public Type GetMetaClassTypeOrDefault(uint classNameHash) =>
            this._registeredMetaClasses.GetValueOrDefault(classNameHash);

        public T GetObject<T>(string path) where T : IMetaClass => GetObject<T>(Fnv1a.HashLower(path));

        public T GetObject<T>(uint pathHash) where T : IMetaClass =>
            (T)this._registeredObjects.GetValueOrDefault(pathHash);
    }
}
