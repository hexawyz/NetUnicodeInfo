using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Unicode
{
	/// <summary>Provides access to unicode information.</summary>
	public static class UnicodeInfo
	{
		// NB: These fields will be used as a default value for UnicodeCharacterData and UnihanCharacterData, and passed by reference.
		// They should be considered as being redonly, even if they aren't explicitely.
		private static readonly UnicodeCharacterData DefaultUnicodeCharacterData = new UnicodeCharacterData(default, null, null, UnicodeCategory.OtherNotAssigned, 0, 0, 0, null, 0, default, false, null, null, null, null, 0, 0, null);
		private static readonly UnihanCharacterData DefaultUnihanCharacterData = default;

		/// <summary>The block name returned when no block is assigned to a specific code point.</summary>
		public const string DefaultBlock = "No_Block";

		/// <summary>Gets the version of the Unicode standard supported by the class.</summary>
		public static Version UnicodeVersion => UnicodeData.UnicodeVersion;

		private static int FindUnicodeCodePointIndex(int codePoint)
			=> codePoint <= UnicodeData.MaxContiguousIndex ?
				codePoint : // For the first code points (this includes all of ASCII, and quite a bit more), the index in the table will be the code point itself.
				BinarySearchUnicodeCodePointIndex(codePoint); // For other code points, we will use a classic binary search with adjusted search indexes.

		private static int BinarySearchUnicodeCodePointIndex(int codePoint)
		{
			var unicodeCharacterData = UnicodeData.UnicodeCharacterData;

			// NB: Due to the strictly ordered nature of the table, we know that a code point can never happen after the index which is the code point itself.
			// This will greatly reduce the range to scan for characters close to maxContiguousIndex, and will have a lesser impact on other characters.
			int minIndex = UnicodeData.MaxContiguousIndex + 1;
			int maxIndex = codePoint < unicodeCharacterData.Length ? codePoint - 1 : unicodeCharacterData.Length - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int δ = unicodeCharacterData[index].CodePointRange.CompareCodePoint(codePoint);

				if (δ == 0) return index;
				else if (δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		private static int FindUnihanCodePointIndex(int codePoint)
		{
			var unihanCharacterData = UnicodeData.UnihanCharacterData;

			int minIndex;
			int maxIndex;

			if (unihanCharacterData.Length == 0 || codePoint < unihanCharacterData[minIndex = 0].CodePoint || codePoint > unihanCharacterData[maxIndex = unihanCharacterData.Length - 1].CodePoint)
			{
				return -1;
			}

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int δ = codePoint - unihanCharacterData[index].CodePoint;

				if (δ == 0) return index;
				else if (δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		private static int FindBlockIndex(int codePoint)
		{
			var blocks = UnicodeData.Blocks;

			int minIndex = 0;
			int maxIndex = blocks.Length - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int δ = blocks[index].CodePointRange.CompareCodePoint(codePoint);

				if (δ == 0) return index;
				else if (δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		internal static ref readonly UnicodeCharacterData GetUnicodeCharacterData(int index)
		{
			if (index >= 0) return ref UnicodeData.UnicodeCharacterData[index];
			return ref DefaultUnicodeCharacterData;
		}

		internal static ref readonly UnihanCharacterData GetUnihanCharacterData(int index)
		{
			if (index >= 0) return ref UnicodeData.UnihanCharacterData[index];
			return ref DefaultUnihanCharacterData;
		}

		/// <summary>Gets the name of the Unicode block containing the character.</summary>
		/// <remarks>If the character has not been assigned to a block, the value of <see cref="DefaultBlock"/> will be returned.</remarks>
		/// <param name="codePoint">The Unicode code point whose block should be retrieved.</param>
		/// <returns>The name of the block the code point was assigned to.</returns>
		public static string GetBlockName(int codePoint)
			=> FindBlockIndex(codePoint) is int i && i >= 0 ?
				UnicodeData.Blocks[i].Name :
				DefaultBlock;

		/// <summary>Gets Unicode information on the specified code point.</summary>
		/// <remarks>
		/// This method will consolidate the data from a few different sources.
		/// There are more efficient way of retrieving the data for some properties if only one of those is needed at a time:
		/// <list type="bullet">
		/// <listheader>
		/// <term>Property</term>
		/// <description>Method</description>
		/// </listheader>
		/// <item>
		/// <term>Name</term>
		/// <description><see cref="GetName(int)"/></description>
		/// </item>
		/// <item>
		/// <term>Category</term>
		/// <description><see cref="GetCategory(int)"/></description>
		/// </item>
		/// <item>
		/// <term>Block</term>
		/// <description><see cref="GetBlockName(int)"/></description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <param name="codePoint">The Unicode code point for which the data must be retrieved.</param>
		/// <returns>The name of the code point, if defined; <see langword="null"/> otherwise.</returns>
		public static UnicodeCharInfo GetCharInfo(int codePoint)
			=> new UnicodeCharInfo(codePoint, FindUnicodeCodePointIndex(codePoint), FindUnihanCodePointIndex(codePoint), GetBlockName(codePoint));

		/// <summary>Gets the category of the specified code point.</summary>
		/// <remarks>
		/// The name referred to is the unicode General_Category property.
		/// If you only need the category of a character, calling this method is faster than calling <see cref="GetCharInfo(int)"/> and retrieving <see cref="UnicodeCharInfo.Category"/>, because there is less information to lookup.
		/// </remarks>
		/// <param name="codePoint">The Unicode code point for which the category must be retrieved.</param>
		/// <returns>The category of the code point.</returns>
		public static UnicodeCategory GetCategory(int codePoint)
			=> FindUnicodeCodePointIndex(codePoint) is int unicodeCharacterDataIndex && unicodeCharacterDataIndex >= 0 ?
				GetUnicodeCharacterData(unicodeCharacterDataIndex).Category :
				UnicodeCategory.OtherNotAssigned;

		/// <summary>Gets a display text for the specified code point.</summary>
		/// <param name="charInfo">The information for the code point.</param>
		/// <returns>A display text for the code point, which may be the representation of the code point itself.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetDisplayText(this UnicodeCharInfo charInfo)
			=> GetDisplayText(charInfo.CodePoint, charInfo._unicodeCharacterDataIndex);

		/// <summary>Gets a display text for the specified code point.</summary>
		/// <param name="codePoint">The Unicode code point, for which a display text should be returned.</param>
		/// <returns>A display text for the code point, which may be the representation of the code point itself.</returns>
		public static string GetDisplayText(int codePoint)
		{
			if (codePoint <= 0x0020) return ((char)(0x2400 + codePoint)).ToString(); // Provide a display text for control characters, including space.
			else if (GetCategory(codePoint) == UnicodeCategory.NonSpacingMark) return "\u25CC" + char.ConvertFromUtf32(codePoint);
			else if (codePoint >= 0xD800 && codePoint <= 0xDFFF) return "\xFFFD";
			else if (codePoint >= 0xE0020 && codePoint < 0xE007F) return char.ConvertFromUtf32(codePoint - 0xE0000); // Handle "TAG" ASCII subset by remapping it to regular ASCII
			else return char.ConvertFromUtf32(codePoint);
		}

		private static string GetDisplayText(int codePoint, int unicodeCharacterDataIndex)
		{
			if (codePoint <= 0x0020) return ((char)(0x2400 + codePoint)).ToString(); // Provide a display text for control characters, including space.
			else if (GetUnicodeCharacterData(unicodeCharacterDataIndex).Category == UnicodeCategory.NonSpacingMark) return "\u25CC" + char.ConvertFromUtf32(codePoint);
			else if (codePoint >= 0xD800 && codePoint <= 0xDFFF) return "\xFFFD";
			else if (codePoint >= 0xE0020 && codePoint < 0xE007F) return char.ConvertFromUtf32(codePoint - 0xE0000); // Handle "TAG" ASCII subset by remapping it to regular ASCII
			else return char.ConvertFromUtf32(codePoint);
		}

		/// <summary>Gets the name of the specified code point.</summary>
		/// <remarks>
		/// The name referred to is the unicode Name property.
		/// If you only need the name of a character, calling this method is faster than calling <see cref="GetCharInfo(int)"/> and retrieving <see cref="UnicodeCharInfo.Name"/>, because there is less information to lookup.
		/// </remarks>
		/// <param name="codePoint">The Unicode code point for which the name must be retrieved.</param>
		/// <returns>The name of the code point, if defined; <see langword="null"/> otherwise.</returns>
		public static string GetName(int codePoint)
			=> HangulInfo.IsHangul(codePoint) ?
				HangulInfo.GetHangulName((char)codePoint) :
				GetNameInternal(codePoint);

		private static string GetNameInternal(int codePoint)
			=> FindUnicodeCodePointIndex(codePoint) is int codePointInfoIndex && codePointInfoIndex >= 0 ?
				GetName(codePoint, GetUnicodeCharacterData(codePointInfoIndex)) :
				null;

		internal static string GetName(int codePoint, in UnicodeCharacterData characterData)
		{
			if (characterData.CodePointRange.IsSingleCodePoint) return characterData.Name.ToString();
			else if (HangulInfo.IsHangul(codePoint)) return HangulInfo.GetHangulName((char)codePoint);
			else if (characterData.Name != null) return characterData.Name + "-" + codePoint.ToString("X4");
			else return null;
		}

		/// <summary>Returns information for a CJK radical.</summary>
		/// <param name="radicalIndex">The index of the radical. Must be between 1 and <see cref="CjkRadicalCount"/>.</param>
		/// <returns>Information on the specified radical.</returns>
		/// <exception cref="IndexOutOfRangeException">The <paramref name="radicalIndex"/> parameter is out of range.</exception>
		public static CjkRadicalInfo GetCjkRadicalInfo(int radicalIndex)
			=> new CjkRadicalInfo(checked((byte)radicalIndex), UnicodeData.Radicals[radicalIndex - 1]);

		/// <summary>Returns the number of CJK radicals in the Unicode data.</summary>
		/// <remarks>This value will be 214 for the foreseeable future.</remarks>
		public static int CjkRadicalCount => UnicodeData.Radicals.Length;

		/// <summary>Gets all the blocks defined in the Unicode data.</summary>
		/// <remarks><see cref="DefaultBlock"/> is not the name of a block, but only a value indicating the abscence of block information for a given code point.</remarks>
		/// <returns>An array containing an entry for every block defined in the Unicode data.</returns>
		public static UnicodeBlock[] GetBlocks() => (UnicodeBlock[])UnicodeData.Blocks.Clone();
	}
}
