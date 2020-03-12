using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Unicode
{
	/// <summary>Represents a collection of values for the kRSUnicode (aka. Unicode_Radical_Stroke) property.</summary>
	public readonly struct UnicodeRadicalStrokeCountCollection : IList<UnicodeRadicalStrokeCount>
	{
		/// <summary>Represents an enumerator for the <see cref="UnicodeRadicalStrokeCountCollection"/> class.</summary>
		public struct Enumerator : IEnumerator<UnicodeRadicalStrokeCount>
		{
			private readonly int _dataAddress;
			private readonly int _itemCount;
			private int _index;

			/// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
			/// <param name="dataAddress">The data address of the items to enumerate.</param>
			/// <param name="itemCount">The number of items to enumerate.</param>
			internal Enumerator(int dataAddress, int itemCount)
			{
				_dataAddress = dataAddress;
				_itemCount = itemCount;
				_index = -1;
			}

			/// <summary>Does nothing.</summary>
			public void Dispose() { }

			/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public UnicodeRadicalStrokeCount Current => ReadItem(_dataAddress, _itemCount * ItemSize, _index);
			object IEnumerator.Current => Current;

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext() => _index < _itemCount && ++_index < _itemCount;

			void IEnumerator.Reset() => _index = -1;
		}

		private const int ItemSize = sizeof(ushort);

		private static ReadOnlySpan<byte> GetDataBlock(int dataAddress, int dataLength)
			=> UnicodeData.UnihanCharacterData.Slice(dataAddress, dataLength);

		private static UnicodeRadicalStrokeCount ReadItem(int dataAddress, int dataLength, int itemIndex)
			=> MemoryMarshal.Read<UnicodeRadicalStrokeCount>(GetDataBlock(dataAddress, dataLength).Slice(itemIndex * ItemSize));

		/// <summary>Gets an empty <see cref="UnicodeRadicalStrokeCountCollection"/> struct.</summary>
		public static readonly UnicodeRadicalStrokeCountCollection Empty = default;

		private readonly int _dataAddress;
		private readonly int _itemCount;

		internal UnicodeRadicalStrokeCountCollection(int dataAddress, int itemCount)
			=> (_dataAddress, _itemCount) = (dataAddress, itemCount);

		/// <summary>Gets the <see cref="UnicodeRadicalStrokeCount"/> at the specified index.</summary>
		/// <value>The <see cref="UnicodeRadicalStrokeCount"/>.</value>
		/// <param name="index">The index.</param>
		/// <returns>The <see cref="UnicodeRadicalStrokeCount"/> at the specified index.</returns>
		public UnicodeRadicalStrokeCount this[int index]
			=> ReadItem(_dataAddress, _itemCount * ItemSize, index);

		UnicodeRadicalStrokeCount IList<UnicodeRadicalStrokeCount>.this[int index]
		{
			get => this[index];
			set => throw new NotSupportedException();
		}

		/// <summary>Gets the number of elements contained in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</summary>
		/// <value>The number of elements contained in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</value>
		public int Count => _itemCount;

		bool ICollection<UnicodeRadicalStrokeCount>.IsReadOnly => true;

		void ICollection<UnicodeRadicalStrokeCount>.Add(UnicodeRadicalStrokeCount item) => throw new NotSupportedException();
		void IList<UnicodeRadicalStrokeCount>.Insert(int index, UnicodeRadicalStrokeCount item) => throw new NotSupportedException();

		bool ICollection<UnicodeRadicalStrokeCount>.Remove(UnicodeRadicalStrokeCount item) => throw new NotSupportedException();
		void IList<UnicodeRadicalStrokeCount>.RemoveAt(int index) => throw new NotSupportedException();

		void ICollection<UnicodeRadicalStrokeCount>.Clear() => throw new NotSupportedException();

		/// <summary>Determines the index of a specific item in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</param>
		/// <returns>The index of the item if found in the list; otherwise, -1.</returns>
		public int IndexOf(UnicodeRadicalStrokeCount item)
		{
			var span = GetDataBlock(_dataAddress, _itemCount * ItemSize);

			for (int i = 0; i < _itemCount; i++)
			{
				if (MemoryMarshal.Read<UnicodeRadicalStrokeCount>(span) == item)
				{
					return i;
				}

				span = span.Slice(ItemSize);
			}

			return -1;
		}

		/// <summary>Determines whether the <see cref="UnicodeRadicalStrokeCountCollection"/> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeRadicalStrokeCountCollection"/>.</param>
		/// <returns><see langword="true" /> if item is fount in the <see cref="UnicodeRadicalStrokeCountCollection"/>; <see langword="false" /> otherwise.</returns>
		public bool Contains(UnicodeRadicalStrokeCount item) => IndexOf(item) >= 0;

		/// <summary>Copies the elements of the UnicodeRadicalStrokeCountCollection to an <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements to copy from UnicodeRadicalStrokeCountCollection. The <see cref="System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zeo-based index in array at which copy begins.</param>
		public void CopyTo(UnicodeRadicalStrokeCount[] array, int arrayIndex)
			=> MemoryMarshal.Cast<byte, UnicodeRadicalStrokeCount>(GetDataBlock(_dataAddress, _itemCount * ItemSize)).CopyTo(array.AsSpan(arrayIndex));

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="Enumerator"/> that can be used to iterate through the collection.
		/// </returns>
		public Enumerator GetEnumerator() => new Enumerator(_dataAddress, _itemCount);

		IEnumerator<UnicodeRadicalStrokeCount> IEnumerable<UnicodeRadicalStrokeCount>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
