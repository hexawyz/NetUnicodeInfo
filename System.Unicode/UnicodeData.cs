using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Buffers;
using System.Buffers.Text;

namespace System.Unicode
{
	internal static partial class UnicodeData
	{
		public static readonly Version UnicodeVersion;

		private const int UnicodeCharacterEntryCount = 0;
		private const int UnihanCharacterEntryCount = 0;
		private const int UnicodeBlockEntryCount = 0;
		private const int CjkRadicalEntryCount = 0;

		private static ReadOnlySpan<byte> UnicodeCharacterIndex => new byte[0];
		internal static ReadOnlySpan<byte> UnicodeCharacterData => new byte[0];

		private static ReadOnlySpan<byte> UnihanCharacterIndex => new byte[0];
		internal static ReadOnlySpan<byte> UnihanCharacterData => new byte[0];

		private static ReadOnlySpan<byte> UnicodeBlockData => new byte[0];
		private static ReadOnlySpan<byte> CjkRadicalData => new byte[0];

		[StructLayout(LayoutKind.Sequential)]
		private readonly struct UnicodeCharacterEntry
		{
			public readonly uint FirstCodePoint;
			public readonly uint LastCodePoint;
			public readonly uint DataOffset;
			public readonly uint DataLength;
		}

		internal enum StringDataOrigin : byte
		{
			UnicodeCharacterData,
			UnihanCharacterData,
			UnicodeBlockData,
			CjkRadicalData,
			HangulCodePointGenerator,
			CodePointSequenceGenerator,
		}

		internal static bool IsStaticString(in int dataAddress)
			=> IsStaticString(GetStringDataOrigin(dataAddress));

		private static bool IsStaticString(StringDataOrigin origin)
			=> origin < StringDataOrigin.HangulCodePointGenerator;

		internal static ReadOnlySpan<byte> GetStringSpan(in int dataAddress, byte length)
			=> GetStaticStringSpan(GetStringDataOrigin(dataAddress)).Slice(dataAddress & 0x00FF_FFFF, length);

		private static StringDataOrigin GetStringDataOrigin(in int dataAddress)
			=> (StringDataOrigin)Unsafe.Add(ref Unsafe.As<int, byte>(ref Unsafe.AsRef(dataAddress)), BitConverter.IsLittleEndian ? 3 : 0);

		private static ReadOnlySpan<byte> GetStaticStringSpan(StringDataOrigin origin)
			=> origin switch
			{
				StringDataOrigin.UnicodeCharacterData => UnicodeCharacterData,
				StringDataOrigin.UnihanCharacterData => UnihanCharacterData,
				StringDataOrigin.UnicodeBlockData => UnicodeBlockData,
				StringDataOrigin.CjkRadicalData => CjkRadicalData,
				_ => throw new InvalidOperationException()
			};

		internal static bool TryWriteString(Span<byte> destination, in int dataAddress, byte length, int codePoint)
			=> GetStringDataOrigin(dataAddress) switch
			{
				StringDataOrigin.HangulCodePointGenerator => HangulInfo.TryWriteHangulName(destination, codePoint, out _),
				StringDataOrigin.CodePointSequenceGenerator => TryWriteCodePointNameSequence(destination, dataAddress & 0x00FF_FFFF, length, codePoint),
				_ => throw new InvalidOperationException()
			};

		private static bool TryWriteCodePointNameSequence(Span<byte> destination, int dataAddress, byte length, int codePoint)
		{
			if (destination.Length < length) return false;

			int codePointLength = GetCodePointLength(codePoint);
			int baseLength = length - codePointLength - 1;

			if (baseLength < 0) throw new InvalidOperationException();

			UnicodeCharacterData.Slice(dataAddress, baseLength).CopyTo(destination);
			destination[baseLength] = (byte)'-';
			return Utf8Formatter.TryFormat(codePoint, destination.Slice(baseLength + 1), out _, new StandardFormat('X', 4));
		}

		private static int GetCodePointLength(int codePoint)
			=> codePoint >= 0x01_0000 ? codePoint >= 0x10_0000 ? 6 : 5 : 4;

		internal static int GetDataAddress(StringDataOrigin origin, int offset)
		{
			if (unchecked((uint)offset > 0x_00FF_FFFF)) throw new ArgumentOutOfRangeException(nameof(offset));

			return offset | ((byte)origin << 24);
		}

		public static readonly int MaxContiguousIndex = 0;

		private static UnicodeCharacterData ReadUnicodeCharacterDataEntry(SpanReader reader)
		{
			var fields = (UcdFields)reader.ReadUInt16LittleEndian();

			UnicodeDataString name = default;
			UnicodeNameAliasCollection nameAliases = default;

			// Read all the official names of the character.
			if ((fields & UcdFields.Name) != 0)
			{
				byte length = reader.ReadByte();
				byte @case = (byte)(length & 0xC0);

				if (@case < 0x80)   // Handles the case where only the name is present.
				{
					length = (byte)((length & 0x7F) + 1);
					name = new UnicodeDataString(GetDataAddress(StringDataOrigin.UnicodeCharacterData, reader.Position), length);
					reader.Skip(length);
				}
				else
				{
					int nameAliasCount = (length & 0x3F) + 1;

					// Read the name if present.
					if ((@case & 0x40) != 0)
					{
						length = (byte)(reader.ReadByte() + 1);
						if (length > 128) throw new InvalidDataException("Did not expect names longer than 128 bytes.");
						name = new UnicodeDataString(GetDataAddress(StringDataOrigin.UnicodeCharacterData, reader.Position), length);
					}

					nameAliases = new UnicodeNameAliasCollection(reader.Position, nameAliasCount);

					// Skip the name alias data block.
					int nameAliasTextLength = 0;
					for (int i = 0; i < nameAliasCount; ++i)
					{
						nameAliasTextLength += reader.ReadByte();
						reader.Skip(1);
					}
					reader.Skip(nameAliasTextLength);
				}
			}

			var category = (fields & UcdFields.Category) != 0 ? (UnicodeCategory)reader.ReadByte() : UnicodeCategory.OtherNotAssigned;
			var canonicalCombiningClass = (fields & UcdFields.CanonicalCombiningClass) != 0 ? (CanonicalCombiningClass)reader.ReadByte() : CanonicalCombiningClass.NotReordered;
			var bidirectionalClass = (fields & UcdFields.BidirectionalClass) != 0 ? (BidirectionalClass)reader.ReadByte() : 0;
			var decompositionType = (fields & UcdFields.DecompositionMapping) != 0 ? (CompatibilityFormattingTag)reader.ReadByte() : CompatibilityFormattingTag.Canonical;
			var decompositionMapping = (fields & UcdFields.DecompositionMapping) != 0 ? ReadString(StringDataOrigin.UnicodeCharacterData, ref reader) : default;
			var numericType = (UnicodeNumericType)((int)(fields & UcdFields.NumericNumeric) >> 6);
			var numericValue = numericType != UnicodeNumericType.None ?
				new UnicodeRationalNumber(reader.ReadInt64LittleEndian(), reader.ReadVariableUInt16LittleEndian()) :
				default;
			var oldName = (fields & UcdFields.OldName) != 0 ? ReadString(StringDataOrigin.UnicodeCharacterData, ref reader) : default;
			var simpleUpperCaseMapping = (fields & UcdFields.SimpleUpperCaseMapping) != 0 ? ReadString(StringDataOrigin.UnicodeCharacterData, ref reader) : default;
			var simpleLowerCaseMapping = (fields & UcdFields.SimpleLowerCaseMapping) != 0 ? ReadString(StringDataOrigin.UnicodeCharacterData, ref reader) : default;
			var simpleTitleCaseMapping = (fields & UcdFields.SimpleTitleCaseMapping) != 0 ? ReadString(StringDataOrigin.UnicodeCharacterData, ref reader) : default;
			var contributoryProperties = (fields & UcdFields.ContributoryProperties) != 0 ? (ContributoryProperties)reader.ReadInt32LittleEndian() : 0;
			int corePropertiesAndEmojiProperties = (fields & UcdFields.CorePropertiesAndEmojiProperties) != 0 ? ReadEmojiAndCoreProperties(ref reader) : 0;
			int[] crossReferences = (fields & UcdFields.CrossRerefences) != 0 ? new int[reader.ReadByte() + 1] : null;

			if (crossReferences != null)
			{
				for (int i = 0; i < crossReferences.Length; ++i)
					crossReferences[i] = reader.ReadCodePoint();
			}

			return new UnicodeCharacterData
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

		private static UnicodeDataString ReadString(StringDataOrigin block, ref SpanReader reader)
		{
			byte length = checked(reader.ReadByte());
			int address = GetDataAddress(block, reader.Position);
			reader.Skip(length);

			return new UnicodeDataString(address, length);
		}

		private static UnihanCharacterData ReadUnihanCharacterDataEntry(SpanReader reader)
		{
			var fields = (UnihanFields)reader.ReadUInt16LittleEndian();

			int codePoint = Unicode.UnihanCharacterData.UnpackCodePoint(reader.ReadCodePoint());

			var numericType = (UnihanNumericType)(int)(fields & UnihanFields.OtherNumeric);
			long numericValue = numericType != UnihanNumericType.None ?
				reader.ReadInt64LittleEndian() :
				0;

			int unicodeRadicalStrokeCountItemCount = (fields & UnihanFields.UnicodeRadicalStrokeCountMore) != 0 ?
				(fields & UnihanFields.UnicodeRadicalStrokeCountMore) == UnihanFields.UnicodeRadicalStrokeCountMore ?
					reader.ReadByte() + 3 :
					((byte)(fields & UnihanFields.UnicodeRadicalStrokeCountMore) >> 2)
				: 0;

			var unicodeRadicalStrokeCounts = new UnicodeRadicalStrokeCountCollection(reader.Position, unicodeRadicalStrokeCountItemCount);

			var definition = (fields & UnihanFields.Definition) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var mandarinReading = (fields & UnihanFields.MandarinReading) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var cantoneseReading = (fields & UnihanFields.CantoneseReading) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var japaneseKunReading = (fields & UnihanFields.JapaneseKunReading) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var japaneseOnReading = (fields & UnihanFields.JapaneseOnReading) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var koreanReading = (fields & UnihanFields.KoreanReading) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var hangulReading = (fields & UnihanFields.HangulReading) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var vietnameseReading = (fields & UnihanFields.VietnameseReading) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var simplifiedVariant = (fields & UnihanFields.SimplifiedVariant) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;
			var traditionalVariant = (fields & UnihanFields.TraditionalVariant) != 0 ? ReadString(StringDataOrigin.UnihanCharacterData, ref reader) : default;

			return new UnihanCharacterData
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

		private static int ReadEmojiAndCoreProperties(ref SpanReader reader)
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
					value |= reader.ReadUInt24LittleEndian();
				}
				else
				{
					value = high << 16 | reader.ReadUInt16LittleEndian();
				}
			}

			return value;
		}
	}
}
