using System;

namespace Fantome.Libraries.League.Meta
{
    public struct MetaEmbedded<T> where T : IMetaClass
    {
        public T Value
        {
            get => this._value;
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));
                else this._value = value;
            }
        }

        private T _value;

        public MetaEmbedded(T value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            else this._value = value;
        }

        public static implicit operator T(MetaEmbedded<T> embedded)
        {
            return embedded.Value;
        }
    }
}
