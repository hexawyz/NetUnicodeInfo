using System.Diagnostics;

namespace System.Unicode
{
	/// <summary>Provides information on a specific CJK radical.</summary>
	[DebuggerDisplay("{RadicalIndex} - {TraditionalRadicalCodePoint.ToString(),nq} / {SimplifiedRadicalCodePoint.ToString(),nq}")]
	public readonly struct CjkRadicalInfo
	{
		/// <summary>The index of the radical in the Kangxi dictionary.</summary>
		/// <remarks>There are 214 radicals, numbered from 1 to 214.</remarks>
		public byte RadicalIndex { get; }

		private readonly CjkRadicalData _radicalData;

		/// <summary>Gets a code point representing the CJK radical in its traditional form.</summary>
		public char TraditionalRadicalCodePoint => _radicalData.TraditionalRadicalCodePoint;
		/// <summary>Gets the code point of a traditional character composed only of the CJK radical.</summary>
		/// <remarks>
		/// Usually, the glyph of this code point will be the same as the one used for <see cref="TraditionalRadicalCodePoint"/>.
		/// However, the code point returned will have a meaning associated, contrary to the one returned by <see cref="TraditionalRadicalCodePoint"/>, which only represents the radical.
		/// </remarks>
		public char TraditionalCharacterCodePoint => _radicalData.TraditionalCharacterCodePoint;
		/// <summary>Gets a code point representing the CJK radical in its simplified form, which may be the same as the traditional form.</summary>
		/// <remarks>Most of the time, the value returned will be the same as <see cref="TraditionalRadicalCodePoint"/>.</remarks>
		public char SimplifiedRadicalCodePoint => _radicalData.SimplifiedRadicalCodePoint;
		/// <summary>Gets the code point of a simplified character composed only of the CJK radical.</summary>
		/// <remarks>
		/// Usually, the glyph of this code point will be the same as the one used for <see cref="SimplifiedRadicalCodePoint"/>.
		/// However, the code point returned will have a meaning associated, contrary to the one returned by <see cref="SimplifiedRadicalCodePoint"/>, which only represents the radical.
		/// </remarks>
		public char SimplifiedCharacterCodePoint => _radicalData.SimplifiedCharacterCodePoint;

		/// <summary>Gets a value indicating whether a simplified form exists for the given radical.</summary>
		public bool HasSimplifiedForm => _radicalData.HasSimplifiedForm;

		internal CjkRadicalInfo(byte radicalIndex, CjkRadicalData radicalData)
		{
			RadicalIndex = radicalIndex;
			_radicalData = radicalData;
		}
	}
}
