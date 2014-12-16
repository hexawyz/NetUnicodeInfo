using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Represents a collection of values for the kRSUnicode (aka. Unicode_Radical_Stroke) property.</summary>
	public struct UnicodeRadicalStrokeCountCollection : IList<UnicodeRadicalStrokeCount>
	{
		/// <summary>Represents an enumerator for the <see cref="UnicodeRadicalStrokeCountCollection"/> class.</summary>
		public struct Enumerator : IEnumerator<UnicodeRadicalStrokeCount>
		{
			private readonly UnicodeRadicalStrokeCount[] items;
			private int index;

			/// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
			/// <param name="items">The items to enumerate.</param>
			internal Enumerator(UnicodeRadicalStrokeCount[] items)
			{
				this.items = items;
				this.index = -1;
			}

			/// <summary>Does nothing.</summary>
			public void Dispose() { }

			/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public UnicodeRadicalStrokeCount Current { get { return items[index]; } }
			object IEnumerator.Current { get { return Current; } }

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext() { return index < items.Length && ++index < items.Length; }

			void IEnumerator.Reset() { this.index = -1; }
		}

		private readonly UnicodeRadicalStrokeCount[] items;

		/// <summary>Initializes a new instance of the <see cref="UnicodeRadicalStrokeCountCollection"/> struct.</summary>
		public UnicodeRadicalStrokeCountCollection() { items = UnicodeRadicalStrokeCount.EmptyArray; }
		internal UnicodeRadicalStrokeCountCollection(UnicodeRadicalStrokeCount[] items) { this.items = items ?? UnicodeRadicalStrokeCount.EmptyArray; }

		/// <summary>Gets the <see cref="UnicodeRadicalStrokeCount"/> at the specified index.</summary>
		/// <value>The <see cref="UnicodeRadicalStrokeCount"/>.</value>
		/// <param name="index">The index.</param>
		/// <returns>The <see cref="UnicodeRadicalStrokeCount"/> at the specified index.</returns>
		public UnicodeRadicalStrokeCount this[int index] { get { return items[index]; } }

		UnicodeRadicalStrokeCount IList<UnicodeRadicalStrokeCount>.this[int index]
		{
			get { return items[index]; }
			set { throw new NotSupportedException(); }
		}

		/// <summary>Gets the number of elements contained in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</summary>
		/// <value>The number of elements contained in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</value>
		public int Count { get { return items.Length; } }

		bool ICollection<UnicodeRadicalStrokeCount>.IsReadOnly { get { return true; } }

		void ICollection<UnicodeRadicalStrokeCount>.Add(UnicodeRadicalStrokeCount item) { throw new NotSupportedException(); }
		void IList<UnicodeRadicalStrokeCount>.Insert(int index, UnicodeRadicalStrokeCount item) { throw new NotSupportedException(); }

		bool ICollection<UnicodeRadicalStrokeCount>.Remove(UnicodeRadicalStrokeCount item) { throw new NotSupportedException(); }
		void IList<UnicodeRadicalStrokeCount>.RemoveAt(int index) { throw new NotSupportedException(); }

		void ICollection<UnicodeRadicalStrokeCount>.Clear() { throw new NotSupportedException(); }

		/// <summary>Determines the index of a specific item in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</param>
		/// <returns>The index of the item if found in the list; otherwise, -1.</returns>
		public int IndexOf(UnicodeRadicalStrokeCount item) { return Array.IndexOf(items, item); }

		/// <summary>Determines whether the <see cref="UnicodeRadicalStrokeCountCollection"/> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</param>
		/// <returns><see langword="true" /> if item is fount in the <see cref="UnicodeRadicalStrokeCountCollection"/>; <see langword="false" /> otherwise.</returns>
		public bool Contains(UnicodeRadicalStrokeCount item) { return IndexOf(item) >= 0; }

		/// <summary>Copies the elements of the UnicodeRadicalStrokeCountCollection to an <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements to copy from UnicodeRadicalStrokeCountCollection. The <see cref="System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zeo-based index in array at which copy begins.</param>
		public void CopyTo(UnicodeRadicalStrokeCount[] array, int arrayIndex) { items.CopyTo(array, arrayIndex); }

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="Enumerator"/> that can be used to iterate through the collection.
		/// </returns>
		public Enumerator GetEnumerator() { return new Enumerator(items); }

		IEnumerator<UnicodeRadicalStrokeCount> IEnumerable<UnicodeRadicalStrokeCount>.GetEnumerator() { return GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
