using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Unicode
{
	/// <summary>Represents a range of Unicode code points.</summary>
	public struct UnicodeCodePointRange : IEnumerable<int>
	{
		/// <summary>Represents an enumerator which enumerated through all the code points in the <see cref="UnicodeCodePointRange"/>.</summary>
		public struct Enumerator : IEnumerator<int>
		{
			private readonly int start;
			private readonly int end;
			private int index;

			/// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
			/// <param name="start">The start of the range.</param>
			/// <param name="end">The end of the range.</param>
			internal Enumerator(int start, int end)
			{
				this.start = start;
				this.end = end;
				this.index = start - 1;
			}

			/// <summary>Does nothing.</summary>
			public void Dispose() { }

			/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			public int Current { get { return index; } }

			object IEnumerator.Current { get { return index; } }

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext() { return index < end && ++index == index; }

			void IEnumerator.Reset() { index = start - 1; }
		}

		/// <summary>The first code point in the range.</summary>
		public readonly int FirstCodePoint;
		/// <summary>The last code point in the range.</summary>
		public readonly int LastCodePoint;

		/// <summary>Gets a value indicating whether this value represents a single code point.</summary>
		/// <value><see langword="true" /> if this value represents a single code point; otherwise, <see langword="false" />.</value>
		public bool IsSingleCodePoint { get { return FirstCodePoint == LastCodePoint; } }

		/// <summary>Initializes a new instance of the <see cref="UnicodeCodePointRange"/> struct for a single code point.</summary>
		/// <param name="codePoint">The code point.</param>
		/// <exception cref="System.ArgumentOutOfRangeException"></exception>
		public UnicodeCodePointRange(int codePoint)
		{
			if (codePoint < 0 || codePoint > 0x10FFFF) throw new ArgumentOutOfRangeException(nameof(codePoint));

			FirstCodePoint = codePoint;
			LastCodePoint = codePoint;
		}

		/// <summary>Initializes a new instance of the <see cref="UnicodeCodePointRange"/> struct with specified bounds.</summary>
		/// <param name="firstCodePoint">The first code point in the range.</param>
		/// <param name="lastCodePoint">The last code point in the range.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="firstCodePoint"/> is less than 0 or greated than 0x10FFFF,
		/// or <paramref name="lastCodePoint"/> is less than <paramref name="firstCodePoint"/> or greated than 0x10FFFF.
		/// </exception>
		public UnicodeCodePointRange(int firstCodePoint, int lastCodePoint)
		{
			if (firstCodePoint < 0 || firstCodePoint > 0x10FFFF) throw new ArgumentOutOfRangeException(nameof(firstCodePoint));
			if (lastCodePoint < firstCodePoint || lastCodePoint > 0x10FFFF) throw new ArgumentOutOfRangeException(nameof(lastCodePoint));

			FirstCodePoint = firstCodePoint;
			LastCodePoint = lastCodePoint;
		}

		/// <summary>Determines whether the range contains the specific code point.</summary>
		/// <remarks>This method does not validate its inputs, but will always return <see langword="false"/> for any invalid code point.</remarks>
		/// <param name="i">The integer to check against the range.</param>
		/// <returns><see langword="true"/> if the range contains the specified code point; otherwise, <see langword="false"/>.</returns>
		public bool Contains(int i)
		{
			// Since the first and last code points have been checked or are at their default value of zero, the method will always exlcude invalid code points.
			return i >= FirstCodePoint & i <= LastCodePoint;
		}

		internal int CompareCodePoint(int codePoint)
		{
			return FirstCodePoint <= codePoint ? LastCodePoint < codePoint ? 1 : 0 : -1;
		}

		/// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return FirstCodePoint == LastCodePoint ? FirstCodePoint.ToString("X4") : FirstCodePoint.ToString("X4") + ".." + LastCodePoint.ToString("X4");
		}

		/// <summary>Parses the specified into a <see cref="UnicodeCodePointRange"/>.</summary>
		/// <remarks>Code point ranges are encoded as one unprefixed hexadecimal number for single code points, or a pair of unprefixed hexadecimal numbers separated by the characters "..".</remarks>
		/// <param name="s">The text to parse.</param>
		/// <returns>The parsed <see cref="UnicodeCodePointRange"/> value.</returns>
		/// <exception cref="System.FormatException">The parameter <paramref name="s"/> was not in an allowed format.</exception>
		public static UnicodeCodePointRange Parse(string s)
		{
			int start, end;

			var rangeSeparatorOffset = s.IndexOf("..");

			if (rangeSeparatorOffset == 0) throw new FormatException();
			else if (rangeSeparatorOffset < 0)
			{
				start = end = int.Parse(s, NumberStyles.HexNumber);
			}
			else
			{
				start = int.Parse(s.Substring(0, rangeSeparatorOffset), NumberStyles.HexNumber);
				end = int.Parse(s.Substring(rangeSeparatorOffset + 2), NumberStyles.HexNumber);
			}

			return new UnicodeCodePointRange(start, end);
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="Enumerator"/> that can be used to iterate through the collection.</returns>
		public Enumerator GetEnumerator() { return new Enumerator(FirstCodePoint, LastCodePoint); }
		IEnumerator<int> IEnumerable<int>.GetEnumerator() { return GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
