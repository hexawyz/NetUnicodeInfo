using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Unicode
{
	/// <summary>Provides access to unicode information.</summary>
	public static class UnicodeInfo
	{
		// NB: These fields will be used as a default value for UnicodeCharacterData and UnihanCharacterData, and passed by reference.
		// They should be considered as being redonly, even if they aren't explicitely.
		private static /*readonly*/ UnicodeCharacterData DefaultUnicodeCharacterData = new UnicodeCharacterData(default(UnicodeCodePointRange), null, null, UnicodeCategory.OtherNotAssigned, 0, 0, 0, null, 0, default(UnicodeRationalNumber), false, null, null, null, null, 0, 0, null);
		private static /*readonly*/ UnihanCharacterData DefaultUnihanCharacterData = default(UnihanCharacterData);

		/// <summary>The block name returned when no block is assigned to a specific code point.</summary>
		public const string DefaultBlock = "No_Block";

		private static readonly Version unicodeVersion;
		private static readonly UnicodeCharacterData[] unicodeCharacterData;
		private static readonly UnihanCharacterData[] unihanCharacterData;
		private static readonly UnicodeBlock[] blocks;
		private static readonly CjkRadicalData[] radicals;
		private static readonly int maxContiguousIndex;

		static UnicodeInfo()
		{
			using (var stream = new DeflateStream(typeof(UnicodeInfo).GetTypeInfo().Assembly.GetManifestResourceStream("ucd.dat"), CompressionMode.Decompress, false))
			{
				ReadFromStream(stream, out unicodeVersion, out unicodeCharacterData, out unihanCharacterData, out radicals, out blocks, out maxContiguousIndex);
			}
		}

		internal static void ReadFromStream(Stream stream, out Version unicodeVersion, out UnicodeCharacterData[] unicodeCharacterData, out UnihanCharacterData[] unihanCharacterData, out CjkRadicalData[] radicals, out UnicodeBlock[] blocks, out int maxContiguousIndex)
		{
			using (var reader = new BinaryReader(stream, Encoding.UTF8))
			{
				int i;

				if (reader.ReadByte() != 'U'
					| reader.ReadByte() != 'C'
					| reader.ReadByte() != 'D')
					throw new InvalidDataException();

				byte formatVersion = reader.ReadByte();

				if (formatVersion != 2) throw new InvalidDataException();

				var fileUnicodeVersion = new Version(reader.ReadUInt16(), reader.ReadByte(), reader.ReadByte());
				
				var unicodeCharacterDataEntries = new UnicodeCharacterData[ReadCodePoint(reader)]; // Allocate one extra entry to act as a dummy entry.
				byte[] nameBuffer = new byte[128];
				int mci = 0;

				for (i = 0; i < unicodeCharacterDataEntries.Length; ++i)
				{
					ReadUnicodeCharacterDataEntry(reader, nameBuffer, out unicodeCharacterDataEntries[i]);
					if (unicodeCharacterDataEntries[i].CodePointRange.Contains(i))
					{
						mci = i;
					}
					else
					{
						++i;
						break;
					}
				}

				maxContiguousIndex = mci;

				for (; i < unicodeCharacterDataEntries.Length; ++i)
				{
					 ReadUnicodeCharacterDataEntry(reader, nameBuffer, out unicodeCharacterDataEntries[i]);
				}

				var blockEntries = new UnicodeBlock[reader.ReadUInt16()];

				for (i = 0; i < blockEntries.Length; ++i)
				{
					ReadBlockEntry(reader, out blockEntries[i]);
				}

				var cjkRadicalEntries = new CjkRadicalData[reader.ReadByte()];

				for (i = 0; i < cjkRadicalEntries.Length; ++i)
				{
					ReadCjkRadicalInfo(reader, out cjkRadicalEntries[i]);
				}

				var unihanCharacterDataEntries = new UnihanCharacterData[ReadCodePoint(reader)];

				for (i = 0; i < unihanCharacterDataEntries.Length; ++i)
				{
					ReadUnihanCharacterDataEntry(reader, out unihanCharacterDataEntries[i]);
				}

				unicodeVersion = fileUnicodeVersion;
				unicodeCharacterData = unicodeCharacterDataEntries;
				unihanCharacterData = unihanCharacterDataEntries;
				radicals = cjkRadicalEntries;
				blocks = blockEntries;
			}
		}

		private static void ReadUnicodeCharacterDataEntry(BinaryReader reader, byte[] nameBuffer, out UnicodeCharacterData value)
		{
			var fields = (UcdFields)reader.ReadUInt16();

			var codePointRange = (fields & UcdFields.CodePointRange) != 0 ? new UnicodeCodePointRange(ReadCodePoint(reader), ReadCodePoint(reader)) : new UnicodeCodePointRange(ReadCodePoint(reader));

			string name = null;
			UnicodeNameAlias[] nameAliases = UnicodeNameAlias.EmptyArray;

			// Read all the official names of the character.
			if ((fields & UcdFields.Name) != 0)
			{
				int length = reader.ReadByte();
				byte @case = (byte)(length & 0xC0);

				if (@case < 0x80)   // Handles the case where only the name is present.
				{
					length = (length & 0x7F) + 1;
					if (reader.Read(nameBuffer, 0, length) != length) throw new EndOfStreamException();

					name = Encoding.UTF8.GetString(nameBuffer, 0, length);
				}
				else
				{
					nameAliases = new UnicodeNameAlias[(length & 0x3F) + 1];

					if ((@case & 0x40) != 0)
					{
						length = reader.ReadByte() + 1;
						if (length > 128) throw new InvalidDataException("Did not expect names longer than 128 bytes.");
						if (reader.Read(nameBuffer, 0, length) != length) throw new EndOfStreamException();
						name = Encoding.UTF8.GetString(nameBuffer, 0, length);
					}

					for (int i = 0; i < nameAliases.Length; ++i)
					{
						nameAliases[i] = new UnicodeNameAlias(reader.ReadString(), (UnicodeNameAliasKind)(reader.ReadByte()));
					}
				}
			}

			var category = (fields & UcdFields.Category) != 0 ? (UnicodeCategory)reader.ReadByte() : UnicodeCategory.OtherNotAssigned;
			var canonicalCombiningClass = (fields & UcdFields.CanonicalCombiningClass) != 0 ? (CanonicalCombiningClass)reader.ReadByte() : CanonicalCombiningClass.NotReordered;
			var bidirectionalClass = (fields & UcdFields.BidirectionalClass) != 0 ? (BidirectionalClass)reader.ReadByte() : 0;
			CompatibilityFormattingTag decompositionType = (fields & UcdFields.DecompositionMapping) != 0 ? (CompatibilityFormattingTag)reader.ReadByte() : CompatibilityFormattingTag.Canonical;
			string decompositionMapping = (fields & UcdFields.DecompositionMapping) != 0 ? reader.ReadString() : null;
			var numericType = (UnicodeNumericType)((int)(fields & UcdFields.NumericNumeric) >> 6);
			UnicodeRationalNumber numericValue = numericType != UnicodeNumericType.None ?
				new UnicodeRationalNumber(reader.ReadInt64(), reader.ReadByte()) :
				default(UnicodeRationalNumber);
			string oldName = (fields & UcdFields.OldName) != 0 ? reader.ReadString() : null;
			string simpleUpperCaseMapping = (fields & UcdFields.SimpleUpperCaseMapping) != 0 ? reader.ReadString() : null;
			string simpleLowerCaseMapping = (fields & UcdFields.SimpleLowerCaseMapping) != 0 ? reader.ReadString() : null;
			string simpleTitleCaseMapping = (fields & UcdFields.SimpleTitleCaseMapping) != 0 ? reader.ReadString() : null;
			ContributoryProperties contributoryProperties = (fields & UcdFields.ContributoryProperties) != 0 ? (ContributoryProperties)reader.ReadInt32() : 0;
			int corePropertiesAndEmojiProperties = (fields & UcdFields.CorePropertiesAndEmojiProperties) != 0 ? ReadInt24(reader) : 0;
			int[] crossReferences = (fields & UcdFields.CrossRerefences) != 0 ? new int[reader.ReadByte() + 1] : null;

			if (crossReferences != null)
			{
				for (int i = 0; i < crossReferences.Length; ++i)
					crossReferences[i] = ReadCodePoint(reader);
			}

			value = new UnicodeCharacterData
			(
				codePointRange,
				name,
				nameAliases,
				category,
				canonicalCombiningClass,
				bidirectionalClass,
				decompositionType,
				decompositionMapping,
				numericType,
				numericValue,
				(fields & UcdFields.BidirectionalMirrored) != 0,
				oldName,
				simpleUpperCaseMapping,
				simpleLowerCaseMapping,
				simpleTitleCaseMapping,
				contributoryProperties,
				corePropertiesAndEmojiProperties,
				crossReferences
			);
		}

		private static void ReadUnihanCharacterDataEntry(BinaryReader reader, out UnihanCharacterData value)
		{
			var fields = (UnihanFields)reader.ReadUInt16();

			int codePoint = UnihanCharacterData.UnpackCodePoint(ReadCodePoint(reader));

			var numericType = (UnihanNumericType)((int)(fields & UnihanFields.OtherNumeric));
			long numericValue = numericType != UnihanNumericType.None ?
				reader.ReadInt64() :
				0;

			UnicodeRadicalStrokeCount[] unicodeRadicalStrokeCounts = (fields & UnihanFields.UnicodeRadicalStrokeCountMore) != 0 ?
				new UnicodeRadicalStrokeCount
				[
					(fields & UnihanFields.UnicodeRadicalStrokeCountMore) == UnihanFields.UnicodeRadicalStrokeCountMore ?
						reader.ReadByte() + 3 :
						((byte)(fields & UnihanFields.UnicodeRadicalStrokeCountMore) >> 2)
				] :
				UnicodeRadicalStrokeCount.EmptyArray;

			for (int i = 0; i < unicodeRadicalStrokeCounts.Length; ++i)
				unicodeRadicalStrokeCounts[i] = new UnicodeRadicalStrokeCount(reader.ReadByte(), reader.ReadByte());

			string definition = (fields & UnihanFields.Definition) != 0 ? reader.ReadString() : null;
			string mandarinReading = (fields & UnihanFields.MandarinReading) != 0 ? reader.ReadString() : null;
			string cantoneseReading = (fields & UnihanFields.CantoneseReading) != 0 ? reader.ReadString() : null;
			string japaneseKunReading = (fields & UnihanFields.JapaneseKunReading) != 0 ? reader.ReadString() : null;
			string japaneseOnReading = (fields & UnihanFields.JapaneseOnReading) != 0 ? reader.ReadString() : null;
			string koreanReading = (fields & UnihanFields.KoreanReading) != 0 ? reader.ReadString() : null;
			string hangulReading = (fields & UnihanFields.HangulReading) != 0 ? reader.ReadString() : null;
			string vietnameseReading = (fields & UnihanFields.VietnameseReading) != 0 ? reader.ReadString() : null;
			string simplifiedVariant = (fields & UnihanFields.SimplifiedVariant) != 0 ? reader.ReadString() : null;
			string traditionalVariant = (fields & UnihanFields.TraditionalVariant) != 0 ? reader.ReadString() : null;

			value = new UnihanCharacterData
			(
				codePoint,
				numericType,
				numericValue,
				unicodeRadicalStrokeCounts,
				definition,
				mandarinReading,
				cantoneseReading,
				japaneseKunReading,
				japaneseOnReading,
				koreanReading,
				hangulReading,
				vietnameseReading,
				simplifiedVariant,
				traditionalVariant
			);
		}

		private static void ReadCjkRadicalInfo(BinaryReader reader, out CjkRadicalData value)
		{
			char tr;
			char tc;

			tr = (char)reader.ReadUInt16();
			tc = (char)reader.ReadUInt16();

			value = (tr & 0x8000) == 0 ?
				new CjkRadicalData(tr, tc) :
				new CjkRadicalData((char)(tr & 0x7FFF), tc, (char)reader.ReadUInt16(), (char)reader.ReadUInt16());
		}

		private static void ReadBlockEntry(BinaryReader reader, out UnicodeBlock value)
			=> value = new UnicodeBlock(new UnicodeCodePointRange(ReadCodePoint(reader), ReadCodePoint(reader)), reader.ReadString());

		private static int ReadInt24(BinaryReader reader) => reader.ReadByte() | ((reader.ReadByte() | (reader.ReadByte() << 8)) << 8);

#if DEBUG
		internal static int ReadCodePoint(BinaryReader reader)
#else
		private static int ReadCodePoint(BinaryReader reader)
#endif
		{
			byte b = reader.ReadByte();

			if (b < 0xA0) return b;
			else if (b < 0xC0)
			{
				return 0xA0 + (((b & 0x1F) << 8) | reader.ReadByte());
			}
			else if (b < 0xE0)
			{
				return 0x20A0 + (((b & 0x1F) << 8) | reader.ReadByte());
			}
			else
			{
				return 0x40A0 + (((((b & 0x1F) << 8) | reader.ReadByte()) << 8) | reader.ReadByte());
			}
		}

		/// <summary>Gets the version of the Unicode standard supported by the class.</summary>
		public static Version UnicodeVersion { get { return unicodeVersion; } }

		private static int FindUnicodeCodePointIndex(int codePoint)
			=> codePoint <= maxContiguousIndex ?
				codePoint : // For the first code points (this includes all of ASCII, and quite a bit more), the index in the table will be the code point itself.
				BinarySearchUnicodeCodePointIndex(codePoint); // For other code points, we will use a classic binary search with adjusted search indexes.

		private static int BinarySearchUnicodeCodePointIndex(int codePoint)
		{
			// NB: Due to the strictly ordered nature of the table, we know that a code point can never happen after the index which is the code point itself.
			// This will greatly reduce the range to scan for characters close to maxContiguousIndex, and will have a lesser impact on other characters.
			int minIndex = maxContiguousIndex + 1;
			int maxIndex = codePoint < unicodeCharacterData.Length ? codePoint - 1 : unicodeCharacterData.Length - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = unicodeCharacterData[index].CodePointRange.CompareCodePoint(codePoint);

				if (Δ == 0) return index;
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		private static int FindUnihanCodePointIndex(int codePoint)
		{
			int minIndex;
			int maxIndex;

			if (unihanCharacterData.Length == 0 || codePoint < unihanCharacterData[minIndex = 0].CodePoint || codePoint > unihanCharacterData[maxIndex = unihanCharacterData.Length - 1].CodePoint)
			{
				return -1;
			}

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = codePoint - unihanCharacterData[index].CodePoint;

				if (Δ == 0) return index;
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		private static int FindBlockIndex(int codePoint)
		{
			int minIndex = 0;
			int maxIndex = blocks.Length - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = blocks[index].CodePointRange.CompareCodePoint(codePoint);

				if (Δ == 0) return index;
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		internal static ref UnicodeCharacterData GetUnicodeCharacterData(int index)
		{
			if (index >= 0) return ref unicodeCharacterData[index];
			return ref DefaultUnicodeCharacterData;
		}

		internal static ref UnihanCharacterData GetUnihanCharacterData(int index)
		{
			if (index >= 0) return ref unihanCharacterData[index];
			return ref DefaultUnihanCharacterData;
		}

		/// <summary>Gets the name of the Unicode block containing the character.</summary>
		/// <remarks>If the character has not been assigned to a block, the value of <see cref="DefaultBlock"/> will be returned.</remarks>
		/// <param name="codePoint">The Unicode code point whose block should be retrieved.</param>
		/// <returns>The name of the block the code point was assigned to.</returns>
		public static string GetBlockName(int codePoint)
			=> FindBlockIndex(codePoint) is int i && i >= 0 ?
				blocks[i].Name :
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
			=> GetDisplayText(charInfo.CodePoint, charInfo.unicodeCharacterDataIndex);

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
				GetName(codePoint, ref GetUnicodeCharacterData(codePointInfoIndex)) :
				null;

		internal static string GetName(int codePoint, ref UnicodeCharacterData characterData)
		{
            if (characterData.CodePointRange.IsSingleCodePoint) return characterData.Name;
            else if (HangulInfo.IsHangul(codePoint)) return HangulInfo.GetHangulName((char)codePoint);
            else if (characterData.Name != null) return characterData.Name + "-" + codePoint.ToString("X4");
            else return null;
        }

		/// <summary>Returns information for a CJK radical.</summary>
		/// <param name="radicalIndex">The index of the radical. Must be between 1 and <see cref="CjkRadicalCount"/>.</param>
		/// <returns>Information on the specified radical.</returns>
		/// <exception cref="IndexOutOfRangeException">The <paramref name="radicalIndex"/> parameter is out of range.</exception>
		public static CjkRadicalInfo GetCjkRadicalInfo(int radicalIndex)
			=> new CjkRadicalInfo(checked((byte)radicalIndex), radicals[radicalIndex - 1]);

		/// <summary>Returns the number of CJK radicals in the Unicode data.</summary>
		/// <remarks>This value will be 214 for the foreseeable future.</remarks>
		public static int CjkRadicalCount => radicals.Length;

		/// <summary>Gets all the blocks defined in the Unicode data.</summary>
		/// <remarks><see cref="DefaultBlock"/> is not the name of a block, but only a value indicating the abscence of block information for a given code point.</remarks>
		/// <returns>An array containing an entry for every block defined in the Unicode data.</returns>
		public static UnicodeBlock[] GetBlocks() => (UnicodeBlock[])blocks.Clone();
	}
}
