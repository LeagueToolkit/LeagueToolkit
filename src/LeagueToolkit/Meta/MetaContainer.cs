using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Meta.Properties;
using System.Collections;
using System.Collections.Generic;

namespace LeagueToolkit.Meta
{
    /// <summary>
    /// Represents a collection of elements serialized from a <see cref="BinTreeContainer"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection</typeparam>
    public class MetaContainer<T> : IList<T>
    {
        /// <inheritdoc/>
        public int Count => this._list.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets a value indicating whether the container has a fixed size
        /// </summary>
        public bool IsFixedSize { get; private set; }

        /// <summary>
        /// Gets the fixed size of the container
        /// </summary>
        public int FixedSize { get; private set; }

        private readonly List<T> _list = new();

        /// <inheritdoc/>
        public T this[int index]
        {
            get => this._list[index];
            set => this._list[index] = value;
        }

        /// <summary>
        /// Creates a new <see cref="MetaContainer{T}"/> object
        /// </summary>
        public MetaContainer() { }

        /// <summary>
        /// Creates a new <see cref="MetaContainer{T}"/> object with elements copied from the specified collection
        /// </summary>
        /// <param name="collection">The elements of the <see cref="MetaContainer{T}"/></param>
        public MetaContainer(IEnumerable<T> collection)
        {
            Guard.IsNotNull(collection);

            this.IsFixedSize = false;
            this._list = new(collection);
        }

        /// <summary>
        /// Creates a new fixed size <see cref="MetaContainer{T}"/> object with elements copied from the specified collection
        /// </summary>
        /// <param name="collection">The elements of the <see cref="MetaContainer{T}"/></param>
        /// <param name="fixedSize">The fixed size of the <see cref="MetaContainer{T}"/></param>
        /// <remarks>
        /// If the count of elements in <paramref name="collection"/> is lower than <paramref name="fixedSize"/>,
        /// the remaining elements will be initialized to their default value
        /// </remarks>
        public MetaContainer(IEnumerable<T> collection, int fixedSize)
        {
            Guard.IsNotNull(collection);
            Guard.IsGreaterThanOrEqualTo(fixedSize, 0, nameof(fixedSize));

            this.IsFixedSize = true;
            this.FixedSize = fixedSize;

            this._list = new(collection);
            if (this._list.Count > fixedSize)
                ThrowHelper.ThrowArgumentException(
                    $"{nameof(collection)} Count is higher than {nameof(fixedSize)}: {fixedSize}"
                );

            int appendCount = fixedSize - this._list.Count;
            for (int i = 0; i < appendCount; i++)
                this._list.Add(default);
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            if (this.IsFixedSize)
                ThrowHelper.ThrowInvalidOperationException(
                    $"Cannot add element into a fixed size {nameof(MetaContainer<T>)}"
                );

            this._list.Add(item);
        }

        /// <inheritdoc/>
        public void Clear() => this._list.Clear();

        /// <inheritdoc/>
        public bool Contains(T item) => this._list.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex) => this._list.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public int IndexOf(T item) => this._list.IndexOf(item);

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            if (this.IsFixedSize)
                ThrowHelper.ThrowInvalidOperationException(
                    $"Cannot insert element into a fixed size {nameof(MetaContainer<T>)}"
                );

            this._list.Insert(index, item);
        }

        /// <inheritdoc/>
        public bool Remove(T item) => this._list.Remove(item);

        /// <inheritdoc/>
        public void RemoveAt(int index) => this._list.RemoveAt(index);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this._list.GetEnumerator();

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => this._list.GetEnumerator();
    }

    public class MetaUnorderedContainer<T> : MetaContainer<T>
    {
        public MetaUnorderedContainer() : base() { }
    }
}
