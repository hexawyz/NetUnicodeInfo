using System.Collections;
using System.Collections.Generic;

namespace System.Unicode
{
	/// <summary>Represents a collection of name aliases.</summary>
	public readonly struct UnicodeNameAliasCollection : IList<UnicodeNameAlias>
	{
		/// <summary>Represents an enumerator for the <see cref="UnicodeNameAliasCollection"/> class.</summary>
		public struct Enumerator : IEnumerator<UnicodeNameAlias>
		{
			private readonly UnicodeNameAlias[] _items;
			private int _index;

			/// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
			/// <param name="items">The items to enumerate.</param>
			internal Enumerator(UnicodeNameAlias[] items)
			{
				_items = items;
				_index = -1;
			}

			/// <summary>Does nothing.</summary>
			public void Dispose() { }

			/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public UnicodeNameAlias Current => _items[_index];
			object IEnumerator.Current => Current;

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext() => _index < _items.Length && ++_index < _items.Length;

			void IEnumerator.Reset() => _index = -1;
		}

		/// <summary>Gets an empty <see cref="UnicodeNameAliasCollection"/> struct.</summary>
		public static readonly UnicodeNameAliasCollection Empty = new UnicodeNameAliasCollection(UnicodeNameAlias.EmptyArray);

		private readonly UnicodeNameAlias[] _items;

		internal UnicodeNameAliasCollection(UnicodeNameAlias[] items) => _items = items ?? UnicodeNameAlias.EmptyArray;

		/// <summary>Gets the <see cref="UnicodeNameAlias"/> at the specified index.</summary>
		/// <value>The <see cref="UnicodeNameAlias"/>.</value>
		/// <param name="index">The index.</param>
		/// <returns>The <see cref="UnicodeNameAlias"/> at the specified index.</returns>
		public UnicodeNameAlias this[int index] => _items[index];

		UnicodeNameAlias IList<UnicodeNameAlias>.this[int index]
		{
			get => _items[index];
			set => throw new NotSupportedException();
		}

		/// <summary>Gets the number of elements contained in the <see cref="UnicodeNameAliasCollection"/>.</summary>
		/// <value>The number of elements contained in the <see cref="UnicodeNameAliasCollection"/>.</value>
		public int Count => _items.Length;

		bool ICollection<UnicodeNameAlias>.IsReadOnly => true;

		void ICollection<UnicodeNameAlias>.Add(UnicodeNameAlias item) => throw new NotSupportedException();
		void IList<UnicodeNameAlias>.Insert(int index, UnicodeNameAlias item) => throw new NotSupportedException();

		bool ICollection<UnicodeNameAlias>.Remove(UnicodeNameAlias item) => throw new NotSupportedException();
		void IList<UnicodeNameAlias>.RemoveAt(int index) => throw new NotSupportedException();

		void ICollection<UnicodeNameAlias>.Clear() => throw new NotSupportedException();

		/// <summary>Determines the index of a specific item in the <see cref="UnicodeNameAliasCollection"/>.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeNameAliasCollection"/>.</param>
		/// <returns>The index of the item if found in the list; otherwise, -1.</returns>
		public int IndexOf(UnicodeNameAlias item) => Array.IndexOf(_items, item);

		/// <summary>Determines whether the <see cref="UnicodeNameAliasCollection"/> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeNameAliasCollection"/>.</param>
		/// <returns><see langword="true" /> if item is fount in the <see cref="UnicodeNameAliasCollection"/>; <see langword="false" /> otherwise.</returns>
		public bool Contains(UnicodeNameAlias item) => IndexOf(item) >= 0;

		/// <summary>
		/// Copies the elements of the UnicodeNameAliasCollection to an <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements to copy from UnicodeNameAliasCollection. The <see cref="System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zeo-based index in array at which copy begins.</param>
		public void CopyTo(UnicodeNameAlias[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="Enumerator"/> that can be used to iterate through the collection.</returns>
		public Enumerator GetEnumerator() => new Enumerator(_items);

		IEnumerator<UnicodeNameAlias> IEnumerable<UnicodeNameAlias>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
