using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public static class UnicodeInfo
	{
		private static readonly Version unicodeVersion;
		private static readonly UnicodeCharacterData[] unicodeCharacterData;
		private static readonly UnihanCharacterData[] unihanCharacterData;
		private static readonly UnicodeBlock[] blocks;

		static UnicodeInfo()
		{
			using (var stream = new DeflateStream(typeof(UnicodeInfo).GetTypeInfo().Assembly.GetManifestResourceStream("ucd.dat"), CompressionMode.Decompress, false))
			{
				ReadFromStream(stream, out unicodeVersion, out unicodeCharacterData, out unihanCharacterData, out blocks);
			}
		}

		internal static void ReadFromStream(Stream stream, out Version unicodeVersion, out UnicodeCharacterData[] unicodeCharacterData, out UnihanCharacterData[] unihanCharacterData, out UnicodeBlock[] blocks)
		{
			using (var reader = new BinaryReader(stream, Encoding.UTF8))
			{
				if (reader.ReadByte() != 'U'
					| reader.ReadByte() != 'C'
					| reader.ReadByte() != 'D')
					throw new InvalidDataException();

				byte formatVersion = reader.ReadByte();

				if (formatVersion != 1) throw new InvalidDataException();

				var fileUnicodeVersion = new Version(reader.ReadUInt16(), reader.ReadByte());

				var unicodeCharacterDataEntries = new UnicodeCharacterData[ReadCodePoint(reader)];

				for (int i = 0; i < unicodeCharacterDataEntries.Length; ++i)
				{
					unicodeCharacterDataEntries[i] = ReadUnicodeCharacterDataEntry(reader);
				}

				var blockEntries = new UnicodeBlock[reader.ReadByte()];

				for (int i = 0; i < blockEntries.Length; ++i)
				{
					blockEntries[i] = ReadBlockEntry(reader);
				}

				var unihanCharacterDataEntries = new UnihanCharacterData[ReadCodePoint(reader)];

				for (int i = 0; i < unihanCharacterDataEntries.Length; ++i)
				{
					unihanCharacterDataEntries[i] = ReadUnihanCharacterDataEntry(reader);
				}

				unicodeVersion = fileUnicodeVersion;
				unicodeCharacterData = unicodeCharacterDataEntries;
				unihanCharacterData = unihanCharacterDataEntries;
				blocks = blockEntries;
			}
		}

		private static UnicodeCharacterData ReadUnicodeCharacterDataEntry(BinaryReader reader)
		{
			var fields = (UcdFields)reader.ReadUInt16();

			var codePointRange = (fields & UcdFields.CodePointRange) != 0 ? new UnicodeCharacterRange(ReadCodePoint(reader), ReadCodePoint(reader)) : new UnicodeCharacterRange(ReadCodePoint(reader));

            string name = (fields & UcdFields.Name) != 0 ? reader.ReadString() : null;
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
			CoreProperties coreProperties = (fields & UcdFields.CoreProperties) != 0 ? (CoreProperties)ReadInt24(reader) : 0;

			return new UnicodeCharacterData
			(
				codePointRange,
				name,
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
				coreProperties,
                null
			);
        }

		private static UnihanCharacterData ReadUnihanCharacterDataEntry(BinaryReader reader)
		{
			var fields = (UnihanFields)reader.ReadUInt16();

			int codePoint = UnihanCharacterData.UnpackCodePoint(ReadCodePoint(reader));

			var numericType = (UnihanNumericType)((int)(fields & UnihanFields.OtherNumeric));
			long numericValue = numericType != UnihanNumericType.None ?
				reader.ReadInt64() :
				0;

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

			return new UnihanCharacterData
			(
				codePoint,
				numericType,
				numericValue,
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

		private static UnicodeBlock ReadBlockEntry(BinaryReader reader)
		{
			return new UnicodeBlock(new UnicodeCharacterRange(ReadCodePoint(reader), ReadCodePoint(reader)), reader.ReadString());
		}

		private static int ReadInt24(BinaryReader reader)
		{
			return reader.ReadByte() | ((reader.ReadByte() | (reader.ReadByte() << 8)) << 8);
		}

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

		public static Version UnicodeVersion { get { return unicodeVersion; } }

		private static UnicodeCharacterData FindUnicodeCodePoint(int codePoint)
		{
			int minIndex = 0;
			int maxIndex = unicodeCharacterData.Length - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = unicodeCharacterData[index].CodePointRange.CompareCodePoint(codePoint);

				if (Δ == 0) return unicodeCharacterData[index];
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return null;
		}

		private static UnihanCharacterData FindUnihanCodePoint(int codePoint)
		{
			int minIndex;
			int maxIndex;

			if (unihanCharacterData.Length == 0 || codePoint < unihanCharacterData[minIndex = 0].CodePoint || codePoint > unihanCharacterData[maxIndex = unicodeCharacterData.Length - 1].CodePoint)
			{
				return null;
			}

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = codePoint - unihanCharacterData[index].CodePoint;

				if (Δ == 0) return unihanCharacterData[index];
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return null;
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

		private static string GetBlockName(int codePoint)
		{
			int i = FindBlockIndex(codePoint);

			return i >= 0 ? blocks[i].Name : null;
        }

		public static UnicodeCharInfo GetCharInfo(int codePoint)
		{
			return new UnicodeCharInfo(codePoint, FindUnicodeCodePoint(codePoint), FindUnihanCodePoint(codePoint), GetBlockName(codePoint));
		}

		public static UnicodeCategory GetCategory(int codePoint)
		{
			var charInfo = FindUnicodeCodePoint(codePoint);

			return charInfo != null ? charInfo.Category : UnicodeCategory.OtherNotAssigned;
        }

		public static string GetDisplayText(UnicodeCharInfo charInfo)
		{
			if (charInfo.CodePoint <= 0x0020) return ((char)(0x2400 + charInfo.CodePoint)).ToString();
			else if (charInfo.Category == UnicodeCategory.NonSpacingMark) return "\u25CC" + char.ConvertFromUtf32(charInfo.CodePoint);
			else return char.ConvertFromUtf32(charInfo.CodePoint);
        }

		public static string GetDisplayText(int codePoint)
		{
			if (codePoint <= 0x0020) return ((char)(0x2400 + codePoint)).ToString();
			else if (GetCategory(codePoint) == UnicodeCategory.NonSpacingMark) return "\u25CC" + char.ConvertFromUtf32(codePoint);
			else return char.ConvertFromUtf32(codePoint);
        }

		public static UnicodeBlock[] GetBlocks()
		{
			return (UnicodeBlock[])blocks.Clone();
        }
	}
}
