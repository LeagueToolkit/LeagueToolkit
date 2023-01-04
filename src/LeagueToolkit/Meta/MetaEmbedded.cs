using System;

namespace LeagueToolkit.Meta
{
    public class MetaEmbedded<T> : IMetaEmbedded where T : IMetaClass
    {
        public T Value
        {
            get => this._value;
            set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));
                this._value = value;
            }
        }

        private T _value;

        public MetaEmbedded(T value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            this._value = value;
        }

        object IMetaEmbedded.GetValue()
        {
            return this.Value;
        }

        public static implicit operator T(MetaEmbedded<T> embedded)
        {
            return embedded.Value;
        }
    }

    internal interface IMetaEmbedded
    {
        internal object GetValue();
    }
}
