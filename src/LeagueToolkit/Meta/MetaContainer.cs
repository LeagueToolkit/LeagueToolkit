using System;
using System.Collections;
using System.Collections.Generic;

namespace LeagueToolkit.Meta
{
    public class MetaContainer<T> : IList<T>
    {
        public int Count => this._list.Count;
        public bool IsReadOnly => false;
        public bool IsFixedSize { get; private set; }
        public int FixedSize { get; private set; }

        private List<T> _list = new();

        public T this[int index]
        { 
            get => this._list[index];
            set => this._list[index] = value; 
        }

        public MetaContainer() { }
        public MetaContainer(ICollection<T> items)
        {
            this.IsFixedSize = false;
            this._list = new List<T>(items);
        }
        public MetaContainer(ICollection<T> items, int fixedSize)
        {
            if (items.Count > fixedSize)
            {
                throw new ArgumentException($"{nameof(items.Count)}: {items.Count} is higher than {nameof(fixedSize)}: {fixedSize}");
            }

            this.IsFixedSize = true;
            this.FixedSize = fixedSize;
            this._list = new List<T>(items);
        }

        public void Add(T item)
        {
            // List is full
            if (this.IsFixedSize && this._list.Count == this.FixedSize)
            {
                throw new InvalidOperationException("maximum list size reached: " + this.FixedSize);
            }
            else
            {
                this._list.Add(item);
            }
        }

        public void Clear()
        {
            this._list.Clear();
        }

        public bool Contains(T item) => this._list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._list.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item) => this._list.IndexOf(item);

        public void Insert(int index, T item)
        {
            if (this.IsFixedSize && index >= this.FixedSize)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"must be within bounds of {this.FixedSize}");
            }
            else
            {
                this._list.Add(item);
            }
        }

        public bool Remove(T item) => this._list.Remove(item);

        public void RemoveAt(int index)
        {
            this._list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => this._list.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => this._list.GetEnumerator();
    }

    public class MetaUnorderedContainer<T> : MetaContainer<T>
    {
        public MetaUnorderedContainer() : base() { }
    }
}
