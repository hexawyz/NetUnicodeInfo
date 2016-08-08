using System;
using System.Unicode;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using Xunit;
using System.Linq;
using Xunit.Abstractions;
using System.Text;

namespace UnicodeInformation.Tests
{
	public class UnicodeInfoTests
	{
		// This class is needed because apparently, somewhere in the process of unit testing, strings with invalid UTF-16 sequences are "fixed", which totally messes up the tests here.
		// This is just a wrapper over regular strings… Data is serialized as an array of chars instead of a string. This seems to do the trick.
		public class XUnitSerializableString : IEquatable<XUnitSerializableString>, IXunitSerializable
		{
			private string value;

			public XUnitSerializableString() : this(null) { }

			public XUnitSerializableString(string value)
			{
				this.value = value;
			}

			void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
			{
				var chars = info.GetValue<char[]>("Chars");

				value = chars != null ?
					new string(chars) :
					null;
			}

			void IXunitSerializable.Serialize(IXunitSerializationInfo info)
			{
				info.AddValue("Chars", value?.ToCharArray(), typeof(char[]));
			}

			public override string ToString()
			{
				if (string.IsNullOrEmpty(value)) return value;

				var sb = new StringBuilder(value.Length * 6);

				foreach (char c in value)
				{
					sb.Append(@"\u")
						.Append(((ushort)c).ToString("X4"));
				}

				return sb.ToString();
			}

			public bool Equals(XUnitSerializableString other) => value == other.value;
			public override bool Equals(object obj) => obj is XUnitSerializableString && Equals((XUnitSerializableString)obj);
			public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(value);

			public static implicit operator string(XUnitSerializableString text) => text.value;
			public static implicit operator XUnitSerializableString(string text) => new XUnitSerializableString(text);
		}

		[Fact]
		public void TestUnicodeVersion()
		{
			Assert.Equal(new Version(9, 0, 0), UnicodeInfo.UnicodeVersion);
		}

		[Fact]
		public void TestCodePointEnumerator()
		{
			string text = "\u0041\U0001F600\u00E9";

			var enumerable = text.AsCodePointEnumerable();

			Assert.Equal(text, enumerable.Text);

			var enumerator = enumerable.GetEnumerator();

			Assert.True(enumerator.MoveNext());
			Assert.Equal(0x0041, enumerator.Current);
			Assert.True(enumerator.MoveNext());
			Assert.Equal(0x1F600, enumerator.Current);
			Assert.True(enumerator.MoveNext());
			Assert.Equal(0x00E9, enumerator.Current);
			Assert.False(enumerator.MoveNext());

			var genericEnumerator = ((IEnumerable<int>)enumerable).GetEnumerator();

			Assert.True(genericEnumerator.MoveNext());
			Assert.Equal(0x0041, genericEnumerator.Current);
			Assert.True(genericEnumerator.MoveNext());
			Assert.Equal(0x1F600, genericEnumerator.Current);
			Assert.True(genericEnumerator.MoveNext());
			Assert.Equal(0x00E9, genericEnumerator.Current);
			Assert.False(genericEnumerator.MoveNext());

			var legacyEnumerator = ((IEnumerable)enumerable).GetEnumerator();

			Assert.True(legacyEnumerator.MoveNext());
			Assert.Equal(0x0041, legacyEnumerator.Current);
			Assert.True(legacyEnumerator.MoveNext());
			Assert.Equal(0x1F600, legacyEnumerator.Current);
			Assert.True(legacyEnumerator.MoveNext());
			Assert.Equal(0x00E9, legacyEnumerator.Current);
			Assert.False(legacyEnumerator.MoveNext());
		}

		private static void EnumerationFailTest(string text) { foreach (int codePoint in text.AsCodePointEnumerable()) { } }

		public void TestCodePointEnumeratorNullArgument()
		{
			Assert.Throws<ArgumentNullException>
			(
				() =>
				{
					foreach (int codePoint in (null as string).AsCodePointEnumerable())
					{
					}
				}
			);
		}

		public static readonly TheoryData<XUnitSerializableString> InvalidUtf16Strings = new TheoryData<XUnitSerializableString>
		{
			"\uDA00",
			"\uDCD0",
			"\uDCD0\uDA00",
			"\u0041\uDA00",
			"\u0041\uDCD0",
			"\uDA00\u0041",
			"\uDCD0\u0041",
			"\uDA00\u0041\uDCD0\u0041",
			"\u0041\uDA00\u0041\uDCD0\u0041",
		};

		[Theory]
		[MemberData(nameof(InvalidUtf16Strings))]
		public void TestCodePointEnumeratorFailures(XUnitSerializableString text)
		{
			Assert.Throws<ArgumentException>
			(
				() =>
				{
					foreach (int codePoint in ((string)text).AsCodePointEnumerable())
					{
					}
				}
			);
		}
		
		public static readonly TheoryData<int[], XUnitSerializableString> PermissiveCodePointEnumeratorTestData = new TheoryData<int[], XUnitSerializableString>
		{
			{  new int[0], "" },
			{ new int[] { 0xDA00 }, "\uDA00" },
			{ new int[] { 0xDCD0 }, "\uDCD0" },
			{ new int[] { 0xDCD0, 0xDA00 }, "\uDCD0\uDA00" },
			{ new int[] { 0x0041, 0xDA00 }, "\u0041\uDA00" },
			{ new int[] { 0x0041, 0xDCD0 }, "\u0041\uDCD0" },
			{ new int[] { 0xDA00, 0x0041 }, "\uDA00\u0041" },
			{ new int[] { 0xDCD0, 0x0041 }, "\uDCD0\u0041" },
			{ new int[] { 0xDA00, 0x0041, 0xDCD0, 0x0041 }, "\uDA00\u0041\uDCD0\u0041" },
			{ new int[] { 0x0041, 0xDA00, 0x0041, 0xDCD0, 0x0041 }, "\u0041\uDA00\u0041\uDCD0\u0041" },
			{ new int[] { 0x0041, 0x1F600, 0x00E9 }, "\u0041\U0001F600\u00E9" },
		};

		[Theory]
		[MemberData(nameof(PermissiveCodePointEnumeratorTestData))]
		public void TestPermissiveCodePointEnumerator(int[] expectedCharacters, XUnitSerializableString text)
		{
			var enumerable = ((string)text).AsPermissiveCodePointEnumerable();

			// Test C# foreach enumeration
			{
				int i = 0;
				foreach (int codePoint in enumerable)
				{
					Assert.Equal(expectedCharacters[i++], codePoint);
				}
				Assert.Equal(expectedCharacters.Length, i);
			}

			// Test generic enumerable
			Assert.Equal(expectedCharacters, from codePoint in enumerable select codePoint);

			// Test legacy enumeration
			{
				// We could use Enumerable.Cast<>, but we can't guarantee that the LINQ implementation we use wouldn't be smart and cast IEnumerable back to IEnumerable<int>
				var legacyEnumerator = ((IEnumerable)enumerable).GetEnumerator();

				int index = 0;

				while (legacyEnumerator.MoveNext())
				{
					Assert.True(index < expectedCharacters.Length);
					Assert.Equal(expectedCharacters[index++], Assert.IsType<int>(legacyEnumerator.Current));
				}

				Assert.Equal(expectedCharacters.Length, index);
			}
		}

		[Fact]
		public void TestPermissiveCodePointEnumeratorFailure()
		{
			Assert.Throws<ArgumentNullException>(() => { foreach (int c in (null as string).AsPermissiveCodePointEnumerable()) { } });
		}

		[Fact]
		public void TestDisplayTextForControlCharacters()
		{
			for (int i = 0; i <= 0x20; ++i)
			{
				Assert.Equal(char.ConvertFromUtf32(0x2400 + i), UnicodeInfo.GetDisplayText(i));
				Assert.Equal(char.ConvertFromUtf32(0x2400 + i), UnicodeInfo.GetDisplayText(UnicodeInfo.GetCharInfo(i)));
			}
		}

		[Theory]
		[InlineData("\u0041", 0x0041)]
		[InlineData("\U0001F600", 0x1F600)]
		[InlineData("\u00E9", 0x00E9)]
		public void TestDisplayText(string expectedText, int codePoint)
		{
			Assert.Equal(expectedText, UnicodeInfo.GetDisplayText(codePoint));
		}

		[Theory]
		[InlineData(0x0030, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, "0", "DIGIT ZERO", "Basic Latin")]
		[InlineData(0x0031, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, "1", "DIGIT ONE", "Basic Latin")]
		[InlineData(0x0032, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, "2", "DIGIT TWO", "Basic Latin")]
		[InlineData(0x0035, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, "5", "DIGIT FIVE", "Basic Latin")]
		[InlineData(0x0039, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, "9", "DIGIT NINE", "Basic Latin")]
		[InlineData(0x0041, UnicodeCategory.UppercaseLetter, UnicodeNumericType.None, null, "LATIN CAPITAL LETTER A", "Basic Latin")]
		[InlineData(0x1F600, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "GRINNING FACE", "Emoticons")]
		[InlineData(0x00E9, UnicodeCategory.LowercaseLetter, UnicodeNumericType.None, null, "LATIN SMALL LETTER E WITH ACUTE", "Latin-1 Supplement")]
		[InlineData(0xD4DB, UnicodeCategory.OtherLetter, UnicodeNumericType.None, null, "HANGUL SYLLABLE PWILH", "Hangul Syllables")]
		[InlineData(0x1F574, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "MAN IN BUSINESS SUIT LEVITATING", "Miscellaneous Symbols and Pictographs")]
		[InlineData(0x16FE0, UnicodeCategory.ModifierLetter, UnicodeNumericType.None, null, "TANGUT ITERATION MARK", "Ideographic Symbols and Punctuation")]
		[InlineData(0x17000, UnicodeCategory.OtherLetter, UnicodeNumericType.None, null, "TANGUT IDEOGRAPH-17000", "Tangut")]
		[InlineData(0x17943, UnicodeCategory.OtherLetter, UnicodeNumericType.None, null, "TANGUT IDEOGRAPH-17943", "Tangut")] // Number 4
		[InlineData(0x187EC, UnicodeCategory.OtherLetter, UnicodeNumericType.None, null, "TANGUT IDEOGRAPH-187EC", "Tangut")]
		[InlineData(0x0D76, UnicodeCategory.OtherNumber, UnicodeNumericType.Numeric, "1/16", "MALAYALAM FRACTION ONE SIXTEENTH", "Malayalam")]
		[InlineData(0x0D5D, UnicodeCategory.OtherNumber, UnicodeNumericType.Numeric, "3/20", "MALAYALAM FRACTION THREE TWENTIETHS", "Malayalam")]
		[InlineData(0x0D59, UnicodeCategory.OtherNumber, UnicodeNumericType.Numeric, "1/40", "MALAYALAM FRACTION ONE FORTIETH", "Malayalam")]
		[InlineData(0x1F953, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "BACON", "Supplemental Symbols and Pictographs")]
		public void TestCharacterInfo(int codePoint, UnicodeCategory expectedCategory, UnicodeNumericType expectedNumericType, string expectedNumericValue, string expectedName, string expectedBlock)
		{
			var info = UnicodeInfo.GetCharInfo(codePoint);
			Assert.Equal(codePoint, info.CodePoint);
			Assert.Equal(expectedCategory, info.Category);
			Assert.Equal(expectedNumericType, info.NumericType);
			if (expectedNumericValue != null) Assert.Equal(UnicodeRationalNumber.Parse(expectedNumericValue), info.NumericValue);
			else Assert.Null(info.NumericValue);
			Assert.Equal(expectedName, info.Name);
			Assert.Equal(expectedBlock, UnicodeInfo.GetBlockName(codePoint));
			Assert.Equal(expectedBlock, info.Block);
		}

		[Fact]
		public void TestRationalNumber()
		{
			Assert.True(default(UnicodeRationalNumber).IsDefaultValue);
			Assert.Equal("1", new UnicodeRationalNumber(1).ToString());
			Assert.Equal("1", new UnicodeRationalNumber(1, 1).ToString());
			Assert.Equal(new UnicodeRationalNumber(1), new UnicodeRationalNumber(1, 1));
			Assert.Equal("1/100", new UnicodeRationalNumber(1, 100).ToString());
			Assert.Equal("-20/7", new UnicodeRationalNumber(-20, 7).ToString());
			Assert.Equal("-5", new UnicodeRationalNumber(-5).ToString());
			Assert.Equal(long.MaxValue.ToString(), new UnicodeRationalNumber(long.MaxValue).ToString());
			Assert.Equal(long.MaxValue.ToString() + "/" + byte.MaxValue.ToString(), new UnicodeRationalNumber(long.MaxValue, byte.MaxValue).ToString());
			Assert.Equal(string.Empty, default(UnicodeRationalNumber).ToString());

			Assert.Equal(new UnicodeRationalNumber(0), UnicodeRationalNumber.Parse("0"));
			Assert.Equal(new UnicodeRationalNumber(1), UnicodeRationalNumber.Parse("1"));
			Assert.Equal(new UnicodeRationalNumber(1), UnicodeRationalNumber.Parse("1/1"));
			Assert.Equal(new UnicodeRationalNumber(1, 10), UnicodeRationalNumber.Parse("1/10"));
			Assert.NotEqual(new UnicodeRationalNumber(2, 10), UnicodeRationalNumber.Parse("1/10"));
			Assert.NotEqual(new UnicodeRationalNumber(1, 20), UnicodeRationalNumber.Parse("1/10"));
			Assert.NotEqual(new UnicodeRationalNumber(2, 2), new UnicodeRationalNumber(1, 1));

			Assert.Throws<ArgumentNullException>(() => UnicodeRationalNumber.Parse(null));
			Assert.Throws<ArgumentException>(() => UnicodeRationalNumber.Parse(string.Empty));

			var numbers = new[]
			{
				default(UnicodeRationalNumber),
				new UnicodeRationalNumber(0),
				new UnicodeRationalNumber(1),
				new UnicodeRationalNumber(1, 10),
				new UnicodeRationalNumber(1, 100),
				new UnicodeRationalNumber(10),
				new UnicodeRationalNumber(100),
				new UnicodeRationalNumber(1000),
				new UnicodeRationalNumber(1000000),
				new UnicodeRationalNumber(1000000000),
				new UnicodeRationalNumber(1000000000000),
			};

			var hashSet = new HashSet<UnicodeRationalNumber>();

			// Verify that all numbers are unique
			foreach (var number in numbers)
				Assert.True(hashSet.Add(number));

			// Verify that all numbers are already in the list
			foreach (var number in numbers)
				Assert.False(hashSet.Add(number));
		}

#if DEBUG
		[Theory]
		[InlineData('\0')]
		[InlineData('\uABFF')]
		[InlineData('\uD7A5')]
		public void TestHangulNameFailure(char codePoint)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => HangulInfo.GetHangulName(codePoint));
		}
#endif

		[Theory]
		[InlineData("HANGUL SYLLABLE PWILH", 0xD4DB)]
		[InlineData("HANGUL SYLLABLE PWAENG", 0xD439)]
		[InlineData("HANGUL SYLLABLE PANJ", 0xD311)]
		[InlineData("HANGUL SYLLABLE TOLM", 0xD1AA)]
		public void TestHangulName(string expectedName, int codePoint)
		{
			Assert.Equal(expectedName, UnicodeInfo.GetName(codePoint));
		}

		[Theory]
		[InlineData("Basic Latin", 0x0041)]
		[InlineData("Miscellaneous Technical", 0x2307)]
		[InlineData("Hangul Syllables", 0xD311)]
		[InlineData("Miscellaneous Symbols and Pictographs", 0x1F574)]
		public void TestBlockName(string expectedBlockName, int codePoint)
		{
			Assert.Equal(expectedBlockName, UnicodeInfo.GetBlockName(codePoint));
		}

		[Fact]
		public void TestRadicalStrokeCount()
		{
			var char5E7A = UnicodeInfo.GetCharInfo(0x5E7A);

			Assert.NotEmpty(char5E7A.UnicodeRadicalStrokeCounts);
			Assert.False(char5E7A.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.Equal(char5E7A.UnicodeRadicalStrokeCounts[0].Radical, 52);
			Assert.Equal(char5E7A.UnicodeRadicalStrokeCounts[0].StrokeCount, 0);

			var char2A6D6 = UnicodeInfo.GetCharInfo(0x2A6D6);

			Assert.NotEmpty(char2A6D6.UnicodeRadicalStrokeCounts);
			Assert.False(char2A6D6.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.Equal(char2A6D6.UnicodeRadicalStrokeCounts[0].Radical, 214);
			Assert.Equal(char2A6D6.UnicodeRadicalStrokeCounts[0].StrokeCount, 20);
		}

		[Fact]
		public void TestRadicalInfo()
		{
			var radical1 = UnicodeInfo.GetCjkRadicalInfo(1);

			Assert.False(radical1.HasSimplifiedForm);
			Assert.Equal(1, radical1.RadicalIndex);
			Assert.Equal('\u2F00', radical1.TraditionalRadicalCodePoint);
			Assert.Equal('\u4E00', radical1.TraditionalCharacterCodePoint);
			Assert.Equal('\u2F00', radical1.SimplifiedRadicalCodePoint);
			Assert.Equal('\u4E00', radical1.SimplifiedCharacterCodePoint);

			var radical214 = UnicodeInfo.GetCjkRadicalInfo(214);

			Assert.False(radical214.HasSimplifiedForm);
			Assert.Equal(214, radical214.RadicalIndex);
			Assert.Equal('\u2FD5', radical214.TraditionalRadicalCodePoint);
			Assert.Equal('\u9FA0', radical214.TraditionalCharacterCodePoint);
			Assert.Equal('\u2FD5', radical214.SimplifiedRadicalCodePoint);
			Assert.Equal('\u9FA0', radical214.SimplifiedCharacterCodePoint);

			Assert.Throws<IndexOutOfRangeException>(() => UnicodeInfo.GetCjkRadicalInfo(0));
			Assert.Throws<IndexOutOfRangeException>(() => UnicodeInfo.GetCjkRadicalInfo(215));
		}

		[Fact]
		public void TestCodePointRange()
		{
			var fullRange = new UnicodeCodePointRange(0, 0x10FFFF);

			Assert.Equal(0, fullRange.FirstCodePoint);
			Assert.Equal(0x10FFFF, fullRange.LastCodePoint);
			Assert.False(fullRange.IsSingleCodePoint);

			var letterA = new UnicodeCodePointRange('A');

			Assert.Equal('A', letterA.FirstCodePoint);
			Assert.Equal('A', letterA.LastCodePoint);
			Assert.True(letterA.IsSingleCodePoint);
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0x110000)]
		[InlineData(int.MaxValue)]
		public void TestInvalidSingleCodePointRanges(int codePoint)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(codePoint));
		}

		[Theory]
		[InlineData(-1, 10)]
		[InlineData(10, 0x110000)]
		[InlineData(-1, 0x110000)]
		public void TestInvalidCodePointRanges(int firstCodePoint, int lastCodePoint)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(firstCodePoint, lastCodePoint));
		}

		[Fact]
		public void TestCodePointRangeEnumeration()
		{
			const int start = 0xA3F;
			const int end = 0x105F;

			// Generic test
			{
				int i = start;

				foreach (int n in new UnicodeCodePointRange(start, end))
				{
					Assert.Equal(i++, n);
				}
			}

			// Nongeneric test
			{
				int i = start;

				var enumerator = (IEnumerator)new UnicodeCodePointRange(start, end).GetEnumerator();

				while (enumerator.MoveNext())
				{
					Assert.Equal(i++, enumerator.Current);
				}

				enumerator.Reset();

				Assert.True(enumerator.MoveNext());
				Assert.Equal(start, enumerator.Current);
			}
		}

		[Fact]
		public void TestGetNameSuccess()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetName(i);
			}
		}

		[Fact]
		public void TestGetDisplayTextSuccess()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetDisplayText(i);
			}
		}

		[Fact]
		public void TestGetCategorySuccess()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetCategory(i);
			}
		}

		[Fact]
		public void TestGetCharInfoSuccess()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetCharInfo(i);
			}
		}

		[Fact]
		public void TestGetCharInfoCoherence()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				var charInfo = UnicodeInfo.GetCharInfo(i);

				Assert.Equal(charInfo.Name, UnicodeInfo.GetName(i));
				Assert.Equal(charInfo.Category, UnicodeInfo.GetCategory(i));
				Assert.Equal(UnicodeInfo.GetDisplayText(charInfo), UnicodeInfo.GetDisplayText(i));
			}
		}

#if DEBUG
		[Fact]
		public void TestUnihanCodePointPacking()
		{
			for (int i = 0x3400; i < 0x4E00; ++i)
				Assert.Equal(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0x4E00; i < 0xA000; ++i)
				Assert.Equal(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0xF900; i < 0xFB00; ++i)
				Assert.Equal(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0x20000; i < 0x2F800; ++i)
				Assert.Equal(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0x2F800; i < 0x30000; ++i)
				Assert.Equal(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));

			// The PackCodePoint method should fail for code points outside of the valid range.
			Assert.Throws<ArgumentOutOfRangeException>(() => UnihanCharacterData.PackCodePoint(0xA000));
			Assert.Throws<ArgumentOutOfRangeException>(() => UnihanCharacterData.PackCodePoint(0xFB00));
			Assert.Throws<ArgumentOutOfRangeException>(() => UnihanCharacterData.PackCodePoint(0x30000));

			// The UnpackCodePoint method should fail for values outside of the valid range.
			Assert.Throws<ArgumentOutOfRangeException>(() => UnihanCharacterData.UnpackCodePoint(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => UnihanCharacterData.UnpackCodePoint(0x20000));
		}
#endif
	}
}
