using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace System.Unicode
{
	internal readonly struct UnicodeData
	{
		public readonly Version UnicodeVersion;
		public readonly UnicodeCharacterData[] UnicodeCharacterData;
		public readonly UnihanCharacterData[] UnihanCharacterData;
		public readonly UnicodeBlock[] Blocks;
		public readonly CjkRadicalData[] Radicals;
		public readonly int MaxContiguousIndex;

		private UnicodeData(Version unicodeVersion, UnicodeCharacterData[] unicodeCharacterData, UnihanCharacterData[] unihanCharacterData, UnicodeBlock[] blocks, CjkRadicalData[] radicals, int maxContiguousIndex)
		{
			UnicodeVersion = unicodeVersion;
			UnicodeCharacterData = unicodeCharacterData;
			UnihanCharacterData = unihanCharacterData;
			Blocks = blocks;
			Radicals = radicals;
			MaxContiguousIndex = maxContiguousIndex;
		}

		public static UnicodeData ReadFromResources()
		{
			using (var stream = new DeflateStream(typeof(UnicodeData).GetTypeInfo().Assembly.GetManifestResourceStream("ucd.dat"), CompressionMode.Decompress, false))
			{
				return ReadFromStream(stream);
			}
		}

		public static UnicodeData ReadFromStream(Stream stream)
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
				int maxContiguousIndex = 0;

				for (i = 0; i < unicodeCharacterDataEntries.Length; ++i)
				{
					ReadUnicodeCharacterDataEntry(reader, nameBuffer, out unicodeCharacterDataEntries[i]);
					if (unicodeCharacterDataEntries[i].CodePointRange.Contains(i))
					{
						maxContiguousIndex = i;
					}
					else
					{
						++i;
						break;
					}
				}

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

				return new UnicodeData
				(
					fileUnicodeVersion,
					unicodeCharacterDataEntries,
					unihanCharacterDataEntries,
					blockEntries,
					cjkRadicalEntries,
					maxContiguousIndex
				);
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
						nameAliases[i] = new UnicodeNameAlias(reader.ReadString(), (UnicodeNameAliasKind)reader.ReadByte());
					}
				}
			}

			var category = (fields & UcdFields.Category) != 0 ? (UnicodeCategory)reader.ReadByte() : UnicodeCategory.OtherNotAssigned;
			var canonicalCombiningClass = (fields & UcdFields.CanonicalCombiningClass) != 0 ? (CanonicalCombiningClass)reader.ReadByte() : CanonicalCombiningClass.NotReordered;
			var bidirectionalClass = (fields & UcdFields.BidirectionalClass) != 0 ? (BidirectionalClass)reader.ReadByte() : 0;
			var decompositionType = (fields & UcdFields.DecompositionMapping) != 0 ? (CompatibilityFormattingTag)reader.ReadByte() : CompatibilityFormattingTag.Canonical;
			string decompositionMapping = (fields & UcdFields.DecompositionMapping) != 0 ? reader.ReadString() : null;
			var numericType = (UnicodeNumericType)((int)(fields & UcdFields.NumericNumeric) >> 6);
			var numericValue = numericType != UnicodeNumericType.None ?
				new UnicodeRationalNumber(reader.ReadInt64(), ReadVariableUInt16(reader)) :
				default;
			string oldName = (fields & UcdFields.OldName) != 0 ? reader.ReadString() : null;
			string simpleUpperCaseMapping = (fields & UcdFields.SimpleUpperCaseMapping) != 0 ? reader.ReadString() : null;
			string simpleLowerCaseMapping = (fields & UcdFields.SimpleLowerCaseMapping) != 0 ? reader.ReadString() : null;
			string simpleTitleCaseMapping = (fields & UcdFields.SimpleTitleCaseMapping) != 0 ? reader.ReadString() : null;
			var contributoryProperties = (fields & UcdFields.ContributoryProperties) != 0 ? (ContributoryProperties)reader.ReadInt32() : 0;
			int corePropertiesAndEmojiProperties = (fields & UcdFields.CorePropertiesAndEmojiProperties) != 0 ? ReadEmojiAndCoreProperties(reader) : 0;
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

			int codePoint = Unicode.UnihanCharacterData.UnpackCodePoint(ReadCodePoint(reader));

			var numericType = (UnihanNumericType)(int)(fields & UnihanFields.OtherNumeric);
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
#if NETSTANDARD1_1 || NET45
				UnicodeRadicalStrokeCount.EmptyArray;
#else
				Array.Empty<UnicodeRadicalStrokeCount>();
#endif

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

		private static ushort ReadVariableUInt16(BinaryReader reader)
		{
			byte b = reader.ReadByte();
			ushort value = unchecked((ushort)(b & 0x7F));

			if (unchecked((sbyte)b) < 0)
			{
				b = reader.ReadByte();
				value |= unchecked((ushort)((b & 0x7F) << 7));

				if (unchecked((sbyte)b) < 0)
				{
					b = reader.ReadByte();
					value |= unchecked((ushort)((b & 0x7F) << 14));
				}
			}

			return value;
		}

		private static int ReadInt24(BinaryReader reader) => reader.ReadByte() | ((reader.ReadByte() | (reader.ReadByte() << 8)) << 8);

		private static int ReadEmojiAndCoreProperties(BinaryReader reader)
		{
			int value = 0;
			byte flags = reader.ReadByte();
			byte high = (byte)(flags & 0x3F);

			if ((sbyte)flags < 0)
			{
				value = high << 24;
			}
			if ((flags & 64) != 0)
			{
				if ((sbyte)flags < 0)
				{
					value |= ReadInt24(reader);
				}
				else
				{
					value = high << 16 | reader.ReadUInt16();
				}
			}

			return value;
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
	}
}
