using System;
using System.Unicode;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using Xunit;

namespace UnicodeInformation.Tests
{
	public class UnicodeInfoTests
	{
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

			Assert.Equal(true, enumerator.MoveNext());
			Assert.Equal(0x0041, enumerator.Current);
			Assert.Equal(true, enumerator.MoveNext());
			Assert.Equal(0x1F600, enumerator.Current);
			Assert.Equal(true, enumerator.MoveNext());
			Assert.Equal(0x00E9, enumerator.Current);
			Assert.Equal(false, enumerator.MoveNext());

			var genericEnumerator = ((IEnumerable<int>)enumerable).GetEnumerator();

			Assert.Equal(true, genericEnumerator.MoveNext());
			Assert.Equal(0x0041, genericEnumerator.Current);
			Assert.Equal(true, genericEnumerator.MoveNext());
			Assert.Equal(0x1F600, genericEnumerator.Current);
			Assert.Equal(true, genericEnumerator.MoveNext());
			Assert.Equal(0x00E9, genericEnumerator.Current);
			Assert.Equal(false, genericEnumerator.MoveNext());

			var legacyEnumerator = ((IEnumerable)enumerable).GetEnumerator();

			Assert.Equal(true, legacyEnumerator.MoveNext());
			Assert.Equal(0x0041, legacyEnumerator.Current);
			Assert.Equal(true, legacyEnumerator.MoveNext());
			Assert.Equal(0x1F600, legacyEnumerator.Current);
			Assert.Equal(true, legacyEnumerator.MoveNext());
			Assert.Equal(0x00E9, legacyEnumerator.Current);
			Assert.Equal(false, legacyEnumerator.MoveNext());
		}

		private static void EnumerationFailTest(string text) { foreach (int codePoint in text.AsCodePointEnumerable()) { } }

		[Fact]
		public void TestCodePointEnumeratorFailures()
		{
			Assert.Throws<ArgumentNullException>(() => EnumerationFailTest(null));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\uDA00"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\uDCD0"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\uDCD0\uDA00"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\u0041\uDA00"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\u0041\uDCD0"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\uDA00\u0041"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\uDCD0\u0041"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\uDA00\u0041\uDCD0\u0041"));
			Assert.Throws<ArgumentException>(() => EnumerationFailTest("\u0041\uDA00\u0041\uDCD0\u0041"));
		}

		[Fact]
		public void TestPermissiveCodePointEnumerator()
		{
			string text = "\u0041\U0001F600\u00E9";

			var enumerable = text.AsPermissiveCodePointEnumerable();

			Assert.Equal(text, enumerable.Text);

			var enumerator = enumerable.GetEnumerator();

			Assert.Equal(true, enumerator.MoveNext());
			Assert.Equal(0x0041, enumerator.Current);
			Assert.Equal(true, enumerator.MoveNext());
			Assert.Equal(0x1F600, enumerator.Current);
			Assert.Equal(true, enumerator.MoveNext());
			Assert.Equal(0x00E9, enumerator.Current);
			Assert.Equal(false, enumerator.MoveNext());

			var genericEnumerator = ((IEnumerable<int>)enumerable).GetEnumerator();

			Assert.Equal(true, genericEnumerator.MoveNext());
			Assert.Equal(0x0041, genericEnumerator.Current);
			Assert.Equal(true, genericEnumerator.MoveNext());
			Assert.Equal(0x1F600, genericEnumerator.Current);
			Assert.Equal(true, genericEnumerator.MoveNext());
			Assert.Equal(0x00E9, genericEnumerator.Current);
			Assert.Equal(false, genericEnumerator.MoveNext());

			var legacyEnumerator = ((IEnumerable)enumerable).GetEnumerator();

			Assert.Equal(true, legacyEnumerator.MoveNext());
			Assert.Equal(0x0041, legacyEnumerator.Current);
			Assert.Equal(true, legacyEnumerator.MoveNext());
			Assert.Equal(0x1F600, legacyEnumerator.Current);
			Assert.Equal(true, legacyEnumerator.MoveNext());
			Assert.Equal(0x00E9, legacyEnumerator.Current);
			Assert.Equal(false, legacyEnumerator.MoveNext());
		}

		private static void TestPermissiveEnumeration(string text, int[] expectedCharacters)
		{
			int i = 0;

			foreach (int codePoint in text.AsPermissiveCodePointEnumerable())
			{
				Assert.Equal(expectedCharacters[i++], codePoint);
			}

			Assert.Equal(expectedCharacters.Length, i);
		}

		[Fact]
		public void TestPermissiveCodePointEnumeratorWithDirtyValues()
		{
			Assert.Throws<ArgumentNullException>(() => { foreach (int c in (null as string).AsPermissiveCodePointEnumerable()) { } });
			TestPermissiveEnumeration(string.Empty, new int[0]);
			TestPermissiveEnumeration("\uDA00", new int[] { 0xDA00 });
			TestPermissiveEnumeration("\uDCD0", new int[] { 0xDCD0 });
			TestPermissiveEnumeration("\uDCD0\uDA00", new int[] { 0xDCD0, 0xDA00 });
			TestPermissiveEnumeration("\u0041\uDA00", new int[] { 0x0041, 0xDA00 });
			TestPermissiveEnumeration("\u0041\uDCD0", new int[] { 0x0041, 0xDCD0 });
			TestPermissiveEnumeration("\uDA00\u0041", new int[] { 0xDA00, 0x0041 });
			TestPermissiveEnumeration("\uDCD0\u0041", new int[] { 0xDCD0, 0x0041 });
			TestPermissiveEnumeration("\uDA00\u0041\uDCD0\u0041", new int[] { 0xDA00, 0x0041, 0xDCD0, 0x0041 });
			TestPermissiveEnumeration("\u0041\uDA00\u0041\uDCD0\u0041", new int[] { 0x0041, 0xDA00, 0x0041, 0xDCD0, 0x0041 });
		}

		[Fact]
		public void TestDisplayText()
		{
			for (int i = 0; i <= 0x20; ++i)
			{
				Assert.Equal(char.ConvertFromUtf32(0x2400 + i), UnicodeInfo.GetDisplayText(i));
				Assert.Equal(char.ConvertFromUtf32(0x2400 + i), UnicodeInfo.GetDisplayText(UnicodeInfo.GetCharInfo(i)));
			}

			Assert.Equal("\u0041", UnicodeInfo.GetDisplayText(0x0041));
			Assert.Equal("\U0001F600", UnicodeInfo.GetDisplayText(0x1F600));
			Assert.Equal("\u00E9", UnicodeInfo.GetDisplayText(0x00E9));
		}

		private static void AssertChar(int codePoint, UnicodeCategory category, string name, string block)
		{
			AssertChar(codePoint, category, UnicodeNumericType.None, null, name, block);
        }

		private static void AssertChar(int codePoint, UnicodeCategory category, UnicodeNumericType numericType, UnicodeRationalNumber? numericValue, string name, string block)
		{
			var info = UnicodeInfo.GetCharInfo(codePoint);
			Assert.Equal(codePoint, info.CodePoint);
			Assert.Equal(category, info.Category);
			Assert.Equal(numericType, info.NumericType);
			Assert.Equal(numericValue, info.NumericValue);
			Assert.Equal(name, info.Name);
			Assert.Equal(block, UnicodeInfo.GetBlockName(codePoint));
			Assert.Equal(block, info.Block);
		}

		[Fact]
		public void TestCharacterInfo()
		{
			AssertChar(0x0030, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, new UnicodeRationalNumber(0), "DIGIT ZERO", "Basic Latin");
			AssertChar(0x0031, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, new UnicodeRationalNumber(1), "DIGIT ONE", "Basic Latin");
			AssertChar(0x0032, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, new UnicodeRationalNumber(2), "DIGIT TWO", "Basic Latin");
			AssertChar(0x0035, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, new UnicodeRationalNumber(5), "DIGIT FIVE", "Basic Latin");
			AssertChar(0x0039, UnicodeCategory.DecimalDigitNumber, UnicodeNumericType.Decimal, new UnicodeRationalNumber(9), "DIGIT NINE", "Basic Latin");
			AssertChar(0x0041, UnicodeCategory.UppercaseLetter, "LATIN CAPITAL LETTER A", "Basic Latin");
			AssertChar(0x1F600, UnicodeCategory.OtherSymbol, "GRINNING FACE", "Emoticons");
			AssertChar(0x00E9, UnicodeCategory.LowercaseLetter, "LATIN SMALL LETTER E WITH ACUTE", "Latin-1 Supplement");
			AssertChar(0xD4DB, UnicodeCategory.OtherLetter, "HANGUL SYLLABLE PWILH", "Hangul Syllables");
			AssertChar(0x1F574, UnicodeCategory.OtherSymbol, "MAN IN BUSINESS SUIT LEVITATING", "Miscellaneous Symbols and Pictographs");
			AssertChar(0x17000, UnicodeCategory.OtherLetter, "TANGUT IDEOGRAPH-17000", "Tangut");
		}

		[Fact]
		public void TestRationalNumber()
		{
			Assert.Equal(true, default(UnicodeRationalNumber).IsDefaultValue);
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
				Assert.Equal(true, hashSet.Add(number));

			// Verify that all numbers are already in the list
			foreach (var number in numbers)
				Assert.Equal(false, hashSet.Add(number));
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

			Assert.NotEqual(0, char5E7A.UnicodeRadicalStrokeCounts.Count);
			Assert.Equal(false, char5E7A.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.Equal(char5E7A.UnicodeRadicalStrokeCounts[0].Radical, 52);
			Assert.Equal(char5E7A.UnicodeRadicalStrokeCounts[0].StrokeCount, 0);

			var char2A6D6 = UnicodeInfo.GetCharInfo(0x2A6D6);

			Assert.NotEqual(0, char2A6D6.UnicodeRadicalStrokeCounts.Count);
			Assert.Equal(false, char2A6D6.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.Equal(char2A6D6.UnicodeRadicalStrokeCounts[0].Radical, 214);
			Assert.Equal(char2A6D6.UnicodeRadicalStrokeCounts[0].StrokeCount, 20);
		}

		[Fact]
		public void TestRadicalInfo()
		{
			var radical1 = UnicodeInfo.GetCjkRadicalInfo(1);

			Assert.Equal(false, radical1.HasSimplifiedForm);
			Assert.Equal(1, radical1.RadicalIndex);
			Assert.Equal('\u2F00', radical1.TraditionalRadicalCodePoint);
			Assert.Equal('\u4E00', radical1.TraditionalCharacterCodePoint);
			Assert.Equal('\u2F00', radical1.SimplifiedRadicalCodePoint);
			Assert.Equal('\u4E00', radical1.SimplifiedCharacterCodePoint);

			var radical214 = UnicodeInfo.GetCjkRadicalInfo(214);

			Assert.Equal(false, radical214.HasSimplifiedForm);
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
			Assert.Equal(false, fullRange.IsSingleCodePoint);

			var letterA = new UnicodeCodePointRange('A');

			Assert.Equal('A', letterA.FirstCodePoint);
			Assert.Equal('A', letterA.LastCodePoint);
			Assert.Equal(true, letterA.IsSingleCodePoint);

			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(0x110000));
			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(int.MaxValue));
			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(-1, 10));
			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(10, 0x110000));
			Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(-1, 0x110000));
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

				Assert.Equal(true, enumerator.MoveNext());
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
