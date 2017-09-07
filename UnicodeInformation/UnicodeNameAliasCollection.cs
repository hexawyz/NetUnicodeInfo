using System.Collections;
using System.Collections.Generic;

namespace System.Unicode
{
	/// <summary>Represents a collection of name aliases.</summary>
	public struct UnicodeNameAliasCollection : IList<UnicodeNameAlias>
	{
		/// <summary>Represents an enumerator for the <see cref="UnicodeNameAliasCollection"/> class.</summary>
		public struct Enumerator : IEnumerator<UnicodeNameAlias>
		{
			private readonly UnicodeNameAlias[] items;
			private int index;

			/// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
			/// <param name="items">The items to enumerate.</param>
			internal Enumerator(UnicodeNameAlias[] items)
			{
				this.items = items;
				this.index = -1;
			}

			/// <summary>Does nothing.</summary>
			public void Dispose() { }

			/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public UnicodeNameAlias Current { get { return items[index]; } }
			object IEnumerator.Current { get { return Current; } }

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext() { return index < items.Length && ++index < items.Length; }

			void IEnumerator.Reset() { this.index = -1; }
		}

		/// <summary>Gets an empty <see cref="UnicodeNameAliasCollection"/> struct.</summary>
		public static readonly UnicodeNameAliasCollection Empty = new UnicodeNameAliasCollection(UnicodeNameAlias.EmptyArray);

		private readonly UnicodeNameAlias[] items;

		internal UnicodeNameAliasCollection(UnicodeNameAlias[] items) { this.items = items ?? UnicodeNameAlias.EmptyArray; }

		/// <summary>Gets the <see cref="UnicodeNameAlias"/> at the specified index.</summary>
		/// <value>The <see cref="UnicodeNameAlias"/>.</value>
		/// <param name="index">The index.</param>
		/// <returns>The <see cref="UnicodeNameAlias"/> at the specified index.</returns>
		public UnicodeNameAlias this[int index] { get { return items[index]; } }

		UnicodeNameAlias IList<UnicodeNameAlias>.this[int index]
		{
			get { return items[index]; }
			set { throw new NotSupportedException(); }
		}

		/// <summary>Gets the number of elements contained in the <see cref="UnicodeNameAliasCollection"/>.</summary>
		/// <value>The number of elements contained in the <see cref="UnicodeNameAliasCollection"/>.</value>
		public int Count { get { return items.Length; } }

		bool ICollection<UnicodeNameAlias>.IsReadOnly { get { return true; } }

		void ICollection<UnicodeNameAlias>.Add(UnicodeNameAlias item) { throw new NotSupportedException(); }
		void IList<UnicodeNameAlias>.Insert(int index, UnicodeNameAlias item) { throw new NotSupportedException(); }

		bool ICollection<UnicodeNameAlias>.Remove(UnicodeNameAlias item) { throw new NotSupportedException(); }
		void IList<UnicodeNameAlias>.RemoveAt(int index) { throw new NotSupportedException(); }

		void ICollection<UnicodeNameAlias>.Clear() { throw new NotSupportedException(); }

		/// <summary>Determines the index of a specific item in the <see cref="UnicodeNameAliasCollection"/>.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeNameAliasCollection"/>.</param>
		/// <returns>The index of the item if found in the list; otherwise, -1.</returns>
		public int IndexOf(UnicodeNameAlias item) { return Array.IndexOf(items, item); }
		/// <summary>Determines whether the <see cref="UnicodeNameAliasCollection"/> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeNameAliasCollection"/>.</param>
		/// <returns><see langword="true" /> if item is fount in the <see cref="UnicodeNameAliasCollection"/>; <see langword="false" /> otherwise.</returns>
		public bool Contains(UnicodeNameAlias item) { return IndexOf(item) >= 0; }

		/// <summary>
		/// Copies the elements of the UnicodeNameAliasCollection to an <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements to copy from UnicodeNameAliasCollection. The <see cref="System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zeo-based index in array at which copy begins.</param>
		public void CopyTo(UnicodeNameAlias[] array, int arrayIndex) { items.CopyTo(array, arrayIndex); }

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="Enumerator"/> that can be used to iterate through the collection.</returns>
		public Enumerator GetEnumerator() { return new Enumerator(items); }

		IEnumerator<UnicodeNameAlias> IEnumerable<UnicodeNameAlias>.GetEnumerator() { return GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
