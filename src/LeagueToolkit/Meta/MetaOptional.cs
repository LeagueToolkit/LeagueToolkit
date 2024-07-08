namespace LeagueToolkit.Meta
{
    public struct MetaOptional<T> : IMetaOptional
    {
        public bool IsSome { get; private set; }
        public T Value
        {
            get => this._value;
            set
            {
                this._value = value;
                this.IsSome = value is not null;
            }
        }

        private T _value;

        public MetaOptional()
        {
            this._value = default;
            this.IsSome = false;
        }

        public MetaOptional(T value)
        {
            this._value = value;
            this.IsSome = value is not null;
        }

        public MetaOptional(T value, bool isSome)
        {
            this.IsSome = isSome;
            this._value = value;
        }

        object IMetaOptional.GetValue()
        {
            if (this.IsSome)
                return this._value;
            else
                return null;
        }

        public static implicit operator T(MetaOptional<T> optional)
        {
            return optional.Value;
        }
    }

    internal interface IMetaOptional
    {
        internal object GetValue();
    }
}
