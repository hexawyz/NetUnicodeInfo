using System.Collections;
using System.Collections.Generic;

namespace System.Unicode
{
	/// <summary>Represents a collection of code point cross-references.</summary>
	public readonly struct UnicodeCrossReferenceCollection : IList<int>
	{
#if NETSTANDARD1_1 || NET45
		private static readonly int[] EmptyArray = new int[0];
#endif

		/// <summary>Represents an enumerator for the <see cref="UnicodeCrossReferenceCollection"/> class.</summary>
		public struct Enumerator : IEnumerator<int>
		{
			private readonly int[] _items;
			private int _index;

			/// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
			/// <param name="items">The items to enumerate.</param>
			internal Enumerator(int[] items)
			{
				_items = items;
				_index = -1;
			}

			/// <summary>Does nothing.</summary>
			public void Dispose() { }

			/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public int Current => _items[_index];
			object IEnumerator.Current => Current;

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext() => _index < _items.Length && ++_index < _items.Length;

			void IEnumerator.Reset() => _index = -1;
		}

		/// <summary>Gets an empty <see cref="UnicodeCrossReferenceCollection"/> struct.</summary>
		public static readonly UnicodeCrossReferenceCollection Empty =
#if NETSTANDARD1_1 || NET45
			new UnicodeCrossReferenceCollection(EmptyArray);
#else
			new UnicodeCrossReferenceCollection(Array.Empty<int>());
#endif

		private readonly int[] _items;

		internal UnicodeCrossReferenceCollection(int[] items)
			=> _items = items
#if NETSTANDARD1_1 || NET45
			?? EmptyArray;
#else
			?? Array.Empty<int>();
#endif

		/// <summary>Gets the cross-referenced code point at the specified index.</summary>
		/// <value>The cross-referenced code point.</value>
		/// <param name="index">The index.</param>
		/// <returns>The cross-referenced code point at the specified index.</returns>
		public int this[int index] => _items[index];

		int IList<int>.this[int index]
		{
			get => _items[index];
			set => throw new NotSupportedException();
		}

		/// <summary>Gets the number of elements contained in the <see cref="UnicodeCrossReferenceCollection"/>.</summary>
		/// <value>The number of elements contained in the <see cref="UnicodeCrossReferenceCollection"/>.</value>
		public int Count => _items.Length;

		bool ICollection<int>.IsReadOnly => true;

		void ICollection<int>.Add(int item) => throw new NotSupportedException();
		void IList<int>.Insert(int index, int item) => throw new NotSupportedException();

		bool ICollection<int>.Remove(int item) => throw new NotSupportedException();
		void IList<int>.RemoveAt(int index) => throw new NotSupportedException();

		void ICollection<int>.Clear() => throw new NotSupportedException();

		/// <summary>Determines the index of a specific item in the <see cref="UnicodeCrossReferenceCollection"/>.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeCrossReferenceCollection"/>.</param>
		/// <returns>The index of the item if found in the list; otherwise, -1.</returns>
		public int IndexOf(int item) => Array.IndexOf(_items, item);

		/// <summary>Determines whether the <see cref="UnicodeCrossReferenceCollection"/> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeCrossReferenceCollection"/>.</param>
		/// <returns><see langword="true" /> if item is fount in the <see cref="UnicodeCrossReferenceCollection"/>; <see langword="false" /> otherwise.</returns>
		public bool Contains(int item) => IndexOf(item) >= 0;

		/// <summary>
		/// Copies the elements of the UnicodeCrossReferenceCollection to an <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements to copy from UnicodeCrossReferenceCollection. The <see cref="System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zeo-based index in array at which copy begins.</param>
		public void CopyTo(int[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="Enumerator"/> that can be used to iterate through the collection.</returns>
		public Enumerator GetEnumerator() => new Enumerator(_items);

		IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
