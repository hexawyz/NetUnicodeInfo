using System.Collections;
using System.Collections.Generic;

namespace System.Unicode
{
	/// <summary>Represents a collection of name aliases.</summary>
	public readonly struct UnicodeBlockCollection : IList<UnicodeBlock>
	{
		/// <summary>Represents an enumerator for the <see cref="UnicodeBlockCollection"/> class.</summary>
		public struct Enumerator : IEnumerator<UnicodeBlock>
		{
			private readonly int _dataAddress;
			private readonly int _itemCount;
			private int _nameStartOffset;
			private int _index;

			/// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
			/// <param name="dataAddress">The data offset of items to enumerate.</param>
			/// <param name="itemCount">The number of items to enumerate.</param>
			internal Enumerator(int dataAddress, int itemCount)
			{
				_dataAddress = dataAddress;
				_itemCount = itemCount;
				_nameStartOffset = 0;
				_index = -1;
			}

			private ReadOnlySpan<byte> Span => UnicodeData.UnicodeCharacterData.Slice(_dataAddress, 2 * _itemCount);

			/// <summary>Does nothing.</summary>
			public void Dispose() { }

			/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public UnicodeBlock Current
			{
				get
				{
					var span = Span;
					int i = _index * 2;

					return new UnicodeBlock(new UnicodeDataString(_dataAddress + 2 * _itemCount + _nameStartOffset, span[i]), (UnicodeBlockKind)span[i + 1]);
				}
			}

			object IEnumerator.Current => Current;

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext()
			{
				int i = _index * 2;

				if (unchecked((uint)_index < (uint)_itemCount && (uint)++_index < (uint)_itemCount))
				{
					_nameStartOffset += Span[i];
					return true;
				}

				return false;
			}

			void IEnumerator.Reset() => (_nameStartOffset, _index) = (0, -1);
		}

		/// <summary>Gets an empty <see cref="UnicodeBlockCollection"/> struct.</summary>
		public static readonly UnicodeBlockCollection Empty = default;

		private readonly int _dataAddress;
		private readonly int _itemCount;

		internal UnicodeBlockCollection(int dataAddress, int itemCount)
			=> (_dataAddress, _itemCount) = (dataAddress, itemCount);

		/// <summary>Gets the <see cref="UnicodeBlock"/> at the specified index.</summary>
		/// <value>The <see cref="UnicodeBlock"/>.</value>
		/// <param name="index">The index.</param>
		/// <returns>The <see cref="UnicodeBlock"/> at the specified index.</returns>
		public UnicodeBlock this[int index]
		{
			get
			{
				int arraySize = 2 * _itemCount;
				var span = UnicodeData.UnicodeCharacterData.Slice(_dataAddress, arraySize);

				int requestedDataOffset = 0;
				int stringStartOffset = 0;

				for (int i = 0; i < index; i++, requestedDataOffset += 2)
				{
					stringStartOffset += span[i * 2];
				}

				return new UnicodeBlock
				(
					new UnicodeDataString(_dataAddress + arraySize + stringStartOffset, span[requestedDataOffset]),
					(UnicodeBlockKind)span[requestedDataOffset + 1]
				);
			}
		}

		UnicodeBlock IList<UnicodeBlock>.this[int index]
		{
			get => this[index];
			set => throw new NotSupportedException();
		}

		/// <summary>Gets the number of elements contained in the <see cref="UnicodeBlockCollection"/>.</summary>
		/// <value>The number of elements contained in the <see cref="UnicodeBlockCollection"/>.</value>
		public int Count => _itemCount;

		bool ICollection<UnicodeBlock>.IsReadOnly => true;

		void ICollection<UnicodeBlock>.Add(UnicodeBlock item) => throw new NotSupportedException();
		void IList<UnicodeBlock>.Insert(int index, UnicodeBlock item) => throw new NotSupportedException();

		bool ICollection<UnicodeBlock>.Remove(UnicodeBlock item) => throw new NotSupportedException();
		void IList<UnicodeBlock>.RemoveAt(int index) => throw new NotSupportedException();

		void ICollection<UnicodeBlock>.Clear() => throw new NotSupportedException();

		/// <summary>Determines the index of a specific item in the <see cref="UnicodeBlockCollection"/>.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeBlockCollection"/>.</param>
		/// <returns>The index of the item if found in the list; otherwise, -1.</returns>
		public int IndexOf(UnicodeBlock item)
		{
			int arraySize = 2 * _itemCount;
			var span = UnicodeData.UnicodeCharacterData.Slice(_dataAddress, arraySize);

			int currentDataOffset = 0;
			int stringStartOffset = _dataAddress + arraySize;

			for (int i = 0; i < _itemCount; i++, currentDataOffset += 2)
			{
				int stringLength = span[currentDataOffset];

				var currentItem = new UnicodeBlock
				(
					new UnicodeDataString
					(
						UnicodeData.GetDataAddress(UnicodeData.StringDataOrigin.UnicodeCharacterData, stringStartOffset),
						stringLength
					),
					(UnicodeBlockKind)span[currentDataOffset + 1]
				);

				if (item == currentItem)
				{
					return i;
				}

				stringStartOffset += stringLength;
			}

			return -1;
		}

		/// <summary>Determines whether the <see cref="UnicodeBlockCollection"/> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="UnicodeBlockCollection"/>.</param>
		/// <returns><see langword="true" /> if item is fount in the <see cref="UnicodeBlockCollection"/>; <see langword="false" /> otherwise.</returns>
		public bool Contains(UnicodeBlock item) => IndexOf(item) >= 0;

		/// <summary>
		/// Copies the elements of the UnicodeBlockCollection to an <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements to copy from UnicodeBlockCollection. The <see cref="System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zeo-based index in array at which copy begins.</param>
		public void CopyTo(UnicodeBlock[] array, int arrayIndex)
		{
			if (unchecked((uint)arrayIndex >= (uint)array.Length)) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			if (unchecked(_itemCount >= (uint)array.Length - (uint)arrayIndex)) throw new ArgumentException();

			int arraySize = 2 * _itemCount;
			var span = UnicodeData.UnicodeCharacterData.Slice(_dataAddress, arraySize);

			int currentDataOffset = 0;
			int stringStartOffset = _dataAddress + arraySize;

			for (int i = 0; i < _itemCount; i++, currentDataOffset += 2, arrayIndex += 1)
			{
				int stringLength = span[currentDataOffset];

				array[arrayIndex] = new UnicodeBlock
				(
					new UnicodeDataString
					(
						UnicodeData.GetDataAddress(UnicodeData.StringDataOrigin.UnicodeCharacterData, stringStartOffset),
						stringLength
					),
					(UnicodeBlockKind)span[currentDataOffset + 1]
				);

				stringStartOffset += stringLength;
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="Enumerator"/> that can be used to iterate through the collection.</returns>
		public Enumerator GetEnumerator() => new Enumerator(_dataAddress, _itemCount);

		IEnumerator<UnicodeBlock> IEnumerable<UnicodeBlock>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
