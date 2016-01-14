using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Unicode;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;

namespace UnicodeInformation.Tests
{
	[TestClass]
	public class UnicodeInfoTests
	{
		[TestMethod]
		public void CodePointEnumeratorTest()
		{
			string text = "\u0041\U0001F600\u00E9";

			var enumerable = text.AsCodePointEnumerable();

			Assert.AreEqual(text, enumerable.Text);

			var enumerator = enumerable.GetEnumerator();

			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(0x0041, enumerator.Current);
			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(0x1F600, enumerator.Current);
			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(0x00E9, enumerator.Current);
			Assert.AreEqual(false, enumerator.MoveNext());

			var genericEnumerator = ((IEnumerable<int>)enumerable).GetEnumerator();

			Assert.AreEqual(true, genericEnumerator.MoveNext());
			Assert.AreEqual(0x0041, genericEnumerator.Current);
			Assert.AreEqual(true, genericEnumerator.MoveNext());
			Assert.AreEqual(0x1F600, genericEnumerator.Current);
			Assert.AreEqual(true, genericEnumerator.MoveNext());
			Assert.AreEqual(0x00E9, genericEnumerator.Current);
			Assert.AreEqual(false, genericEnumerator.MoveNext());

			var legacyEnumerator = ((IEnumerable)enumerable).GetEnumerator();

			Assert.AreEqual(true, legacyEnumerator.MoveNext());
			Assert.AreEqual(0x0041, legacyEnumerator.Current);
			Assert.AreEqual(true, legacyEnumerator.MoveNext());
			Assert.AreEqual(0x1F600, legacyEnumerator.Current);
			Assert.AreEqual(true, legacyEnumerator.MoveNext());
			Assert.AreEqual(0x00E9, legacyEnumerator.Current);
			Assert.AreEqual(false, legacyEnumerator.MoveNext());
		}

		private static void EnumerationFailTest(string text) { foreach (int codePoint in text.AsCodePointEnumerable()) { } }

		[TestMethod]
		public void CodePointEnumeratorDirtyTest()
		{
			AssertEx.ThrowsExactly<ArgumentNullException>(() => EnumerationFailTest(null));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\uDA00"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\uDCD0"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\uDCD0\uDA00"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\u0041\uDA00"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\u0041\uDCD0"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\uDA00\u0041"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\uDCD0\u0041"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\uDA00\u0041\uDCD0\u0041"));
			AssertEx.ThrowsExactly<ArgumentException>(() => EnumerationFailTest("\u0041\uDA00\u0041\uDCD0\u0041"));
		}

		[TestMethod]
		public void PermissiveCodePointEnumeratorTest()
		{
			string text = "\u0041\U0001F600\u00E9";

			var enumerable = text.AsPermissiveCodePointEnumerable();

			Assert.AreEqual(text, enumerable.Text);

			var enumerator = enumerable.GetEnumerator();

			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(0x0041, enumerator.Current);
			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(0x1F600, enumerator.Current);
			Assert.AreEqual(true, enumerator.MoveNext());
			Assert.AreEqual(0x00E9, enumerator.Current);
			Assert.AreEqual(false, enumerator.MoveNext());

			var genericEnumerator = ((IEnumerable<int>)enumerable).GetEnumerator();

			Assert.AreEqual(true, genericEnumerator.MoveNext());
			Assert.AreEqual(0x0041, genericEnumerator.Current);
			Assert.AreEqual(true, genericEnumerator.MoveNext());
			Assert.AreEqual(0x1F600, genericEnumerator.Current);
			Assert.AreEqual(true, genericEnumerator.MoveNext());
			Assert.AreEqual(0x00E9, genericEnumerator.Current);
			Assert.AreEqual(false, genericEnumerator.MoveNext());

			var legacyEnumerator = ((IEnumerable)enumerable).GetEnumerator();

			Assert.AreEqual(true, legacyEnumerator.MoveNext());
			Assert.AreEqual(0x0041, legacyEnumerator.Current);
			Assert.AreEqual(true, legacyEnumerator.MoveNext());
			Assert.AreEqual(0x1F600, legacyEnumerator.Current);
			Assert.AreEqual(true, legacyEnumerator.MoveNext());
			Assert.AreEqual(0x00E9, legacyEnumerator.Current);
			Assert.AreEqual(false, legacyEnumerator.MoveNext());
		}

		private static void PermissiveEnumerationTest(string text, int[] expectedCharacters)
		{
			int i = 0;

			foreach (int codePoint in text.AsPermissiveCodePointEnumerable())
			{
				Assert.AreEqual(expectedCharacters[i++], codePoint);
			}

			Assert.AreEqual(expectedCharacters.Length, i);
		}

		[TestMethod]
		public void PermissiveCodePointEnumeratorDirtyTest()
		{
			AssertEx.ThrowsExactly<ArgumentNullException>(() => { foreach (int c in (null as string).AsPermissiveCodePointEnumerable()) { } });
			PermissiveEnumerationTest(string.Empty, new int[0]);
			PermissiveEnumerationTest("\uDA00", new int[] { 0xDA00 });
			PermissiveEnumerationTest("\uDCD0", new int[] { 0xDCD0 });
			PermissiveEnumerationTest("\uDCD0\uDA00", new int[] { 0xDCD0, 0xDA00 });
			PermissiveEnumerationTest("\u0041\uDA00", new int[] { 0x0041, 0xDA00 });
			PermissiveEnumerationTest("\u0041\uDCD0", new int[] { 0x0041, 0xDCD0 });
			PermissiveEnumerationTest("\uDA00\u0041", new int[] { 0xDA00, 0x0041 });
			PermissiveEnumerationTest("\uDCD0\u0041", new int[] { 0xDCD0, 0x0041 });
			PermissiveEnumerationTest("\uDA00\u0041\uDCD0\u0041", new int[] { 0xDA00, 0x0041, 0xDCD0, 0x0041 });
			PermissiveEnumerationTest("\u0041\uDA00\u0041\uDCD0\u0041", new int[] { 0x0041, 0xDA00, 0x0041, 0xDCD0, 0x0041 });
		}

		[TestMethod]
		public void DisplayTextTest()
		{
			for (int i = 0; i <= 0x20; ++i)
			{
				Assert.AreEqual(char.ConvertFromUtf32(0x2400 + i), UnicodeInfo.GetDisplayText(i));
				Assert.AreEqual(char.ConvertFromUtf32(0x2400 + i), UnicodeInfo.GetDisplayText(UnicodeInfo.GetCharInfo(i)));
			}

			Assert.AreEqual("\u0041", UnicodeInfo.GetDisplayText(0x0041));
			Assert.AreEqual("\U0001F600", UnicodeInfo.GetDisplayText(0x1F600));
			Assert.AreEqual("\u00E9", UnicodeInfo.GetDisplayText(0x00E9));
		}

		private static void AssertChar(int codePoint, UnicodeCategory category, string name, string block)
		{
			AssertChar(codePoint, category, UnicodeNumericType.None, null, name, block);
        }

		private static void AssertChar(int codePoint, UnicodeCategory category, UnicodeNumericType numericType, UnicodeRationalNumber? numericValue, string name, string block)
		{
			var info = UnicodeInfo.GetCharInfo(codePoint);
			Assert.AreEqual(codePoint, info.CodePoint);
			Assert.AreEqual(category, info.Category);
			Assert.AreEqual(numericType, info.NumericType);
			Assert.AreEqual(numericValue, info.NumericValue);
			Assert.AreEqual(name, info.Name);
			Assert.AreEqual(block, UnicodeInfo.GetBlockName(codePoint));
			Assert.AreEqual(block, info.Block);
		}

		[TestMethod]
		public void CharacterInfoTest()
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
		}

		[TestMethod]
		public void RationalNumberTest()
		{
			Assert.AreEqual(true, default(UnicodeRationalNumber).IsDefaultValue);
			Assert.AreEqual("1", new UnicodeRationalNumber(1).ToString());
			Assert.AreEqual("1", new UnicodeRationalNumber(1, 1).ToString());
			Assert.AreEqual(new UnicodeRationalNumber(1), new UnicodeRationalNumber(1, 1));
			Assert.AreEqual("1/100", new UnicodeRationalNumber(1, 100).ToString());
			Assert.AreEqual("-20/7", new UnicodeRationalNumber(-20, 7).ToString());
			Assert.AreEqual("-5", new UnicodeRationalNumber(-5).ToString());
			Assert.AreEqual(long.MaxValue.ToString(), new UnicodeRationalNumber(long.MaxValue).ToString());
			Assert.AreEqual(long.MaxValue.ToString() + "/" + byte.MaxValue.ToString(), new UnicodeRationalNumber(long.MaxValue, byte.MaxValue).ToString());
			Assert.AreEqual(string.Empty, default(UnicodeRationalNumber).ToString());

			Assert.AreEqual(new UnicodeRationalNumber(0), UnicodeRationalNumber.Parse("0"));
			Assert.AreEqual(new UnicodeRationalNumber(1), UnicodeRationalNumber.Parse("1"));
			Assert.AreEqual(new UnicodeRationalNumber(1), UnicodeRationalNumber.Parse("1/1"));
			Assert.AreEqual(new UnicodeRationalNumber(1, 10), UnicodeRationalNumber.Parse("1/10"));
			Assert.AreNotEqual(new UnicodeRationalNumber(2, 10), UnicodeRationalNumber.Parse("1/10"));
			Assert.AreNotEqual(new UnicodeRationalNumber(1, 20), UnicodeRationalNumber.Parse("1/10"));
			Assert.AreNotEqual(new UnicodeRationalNumber(2, 2), new UnicodeRationalNumber(1, 1));

			AssertEx.ThrowsExactly<ArgumentNullException>(() => UnicodeRationalNumber.Parse(null), "UnicodeRationalNumber.Parse");
			AssertEx.ThrowsExactly<ArgumentException>(() => UnicodeRationalNumber.Parse(string.Empty), "UnicodeRationalNumber.Parse");

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
				Assert.AreEqual(true, hashSet.Add(number));

			// Verify that all numbers are already in the list
			foreach (var number in numbers)
				Assert.AreEqual(false, hashSet.Add(number));
		}

		[TestMethod]
		public void HangulNameTest()
		{
#if DEBUG
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => HangulInfo.GetHangulName('\0'));
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => HangulInfo.GetHangulName('\uABFF'));
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => HangulInfo.GetHangulName('\uD7A5'));
#endif
			Assert.AreEqual("HANGUL SYLLABLE PWILH", UnicodeInfo.GetName(0xD4DB));
			Assert.AreEqual("HANGUL SYLLABLE PWAENG", UnicodeInfo.GetName(0xD439));
			Assert.AreEqual("HANGUL SYLLABLE PANJ", UnicodeInfo.GetName(0xD311));
			Assert.AreEqual("HANGUL SYLLABLE TOLM", UnicodeInfo.GetName(0xD1AA));
		}

		[TestMethod]
		public void BlockNameTest()
		{
			Assert.AreEqual("Basic Latin", UnicodeInfo.GetBlockName(0x0041));
			Assert.AreEqual("Miscellaneous Technical", UnicodeInfo.GetBlockName(0x2307));
			Assert.AreEqual("Hangul Syllables", UnicodeInfo.GetBlockName(0xD311));
			Assert.AreEqual("Miscellaneous Symbols and Pictographs", UnicodeInfo.GetBlockName(0x1F574));
		}

		[TestMethod]
		public void RadicalStrokeCountTest()
		{
			var char5E7A = UnicodeInfo.GetCharInfo(0x5E7A);

			Assert.AreNotEqual(0, char5E7A.UnicodeRadicalStrokeCounts);
			Assert.AreEqual(false, char5E7A.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.AreEqual(char5E7A.UnicodeRadicalStrokeCounts[0].Radical, 52);
			Assert.AreEqual(char5E7A.UnicodeRadicalStrokeCounts[0].StrokeCount, 0);

			var char2A6D6 = UnicodeInfo.GetCharInfo(0x2A6D6);

			Assert.AreNotEqual(0, char2A6D6.UnicodeRadicalStrokeCounts);
			Assert.AreEqual(false, char2A6D6.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.AreEqual(char2A6D6.UnicodeRadicalStrokeCounts[0].Radical, 214);
			Assert.AreEqual(char2A6D6.UnicodeRadicalStrokeCounts[0].StrokeCount, 20);
		}

		[TestMethod]
		public void RadicalInfoTest()
		{
			var radical1 = UnicodeInfo.GetCjkRadicalInfo(1);

			Assert.AreEqual(false, radical1.HasSimplifiedForm);
			Assert.AreEqual(1, radical1.RadicalIndex);
			Assert.AreEqual('\u2F00', radical1.TraditionalRadicalCodePoint);
			Assert.AreEqual('\u4E00', radical1.TraditionalCharacterCodePoint);
			Assert.AreEqual('\u2F00', radical1.SimplifiedRadicalCodePoint);
			Assert.AreEqual('\u4E00', radical1.SimplifiedCharacterCodePoint);

			var radical214 = UnicodeInfo.GetCjkRadicalInfo(214);

			Assert.AreEqual(false, radical214.HasSimplifiedForm);
			Assert.AreEqual(214, radical214.RadicalIndex);
			Assert.AreEqual('\u2FD5', radical214.TraditionalRadicalCodePoint);
			Assert.AreEqual('\u9FA0', radical214.TraditionalCharacterCodePoint);
			Assert.AreEqual('\u2FD5', radical214.SimplifiedRadicalCodePoint);
			Assert.AreEqual('\u9FA0', radical214.SimplifiedCharacterCodePoint);

			AssertEx.ThrowsExactly<IndexOutOfRangeException>(() => UnicodeInfo.GetCjkRadicalInfo(0), nameof(UnicodeInfo.GetCjkRadicalInfo));
			AssertEx.ThrowsExactly<IndexOutOfRangeException>(() => UnicodeInfo.GetCjkRadicalInfo(215), nameof(UnicodeInfo.GetCjkRadicalInfo));
		}

		[TestMethod]
		public void CodePointRangeTest()
		{
			var fullRange = new UnicodeCodePointRange(0, 0x10FFFF);

			Assert.AreEqual(0, fullRange.FirstCodePoint);
			Assert.AreEqual(0x10FFFF, fullRange.LastCodePoint);
			Assert.AreEqual(false, fullRange.IsSingleCodePoint);

			var letterA = new UnicodeCodePointRange('A');

			Assert.AreEqual('A', letterA.FirstCodePoint);
			Assert.AreEqual('A', letterA.LastCodePoint);
			Assert.AreEqual(true, letterA.IsSingleCodePoint);

			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(-1));
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(0x110000));
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(int.MaxValue));
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(-1, 10));
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(10, 0x110000));
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(-1, 0x110000));
		}

		[TestMethod]
		public void CodePointRangeEnumerationTest()
		{
			const int start = 0xA3F;
			const int end = 0x105F;

			// Generic test
			{
				int i = start;

				foreach (int n in new UnicodeCodePointRange(start, end))
				{
					Assert.AreEqual(i++, n);
				}
			}

			// Nongeneric test
			{
				int i = start;

				var enumerator = (IEnumerator)new UnicodeCodePointRange(start, end).GetEnumerator();

				while (enumerator.MoveNext())
				{
					Assert.AreEqual(i++, enumerator.Current);
				}

				enumerator.Reset();

				Assert.AreEqual(true, enumerator.MoveNext());
				Assert.AreEqual(start, enumerator.Current);
			}
        }

        [TestMethod]
        public void GetNameSuccessTest()
        {
            for (int i = 0; i <= 0x10FFFF; i++)
            {
                UnicodeInfo.GetName(i);
            }
        }

        [TestMethod]
        public void GetDisplayTextSuccessTest()
        {
            for (int i = 0; i <= 0x10FFFF; i++)
            {
                UnicodeInfo.GetDisplayText(i);
            }
        }

        [TestMethod]
        public void GetCategorySuccessTest()
        {
            for (int i = 0; i <= 0x10FFFF; i++)
            {
                UnicodeInfo.GetCategory(i);
            }
        }

        [TestMethod]
        public void GetCharInfoSuccessTest()
        {
            for (int i = 0; i <= 0x10FFFF; i++)
            {
                UnicodeInfo.GetCharInfo(i);
            }
        }

        [TestMethod]
        public void GetCharInfoCoherenceTest()
        {
            for (int i = 0; i <= 0x10FFFF; i++)
            {
                var charInfo = UnicodeInfo.GetCharInfo(i);

                Assert.AreEqual(charInfo.Name, UnicodeInfo.GetName(i));
                Assert.AreEqual(charInfo.Category, UnicodeInfo.GetCategory(i));
                Assert.AreEqual(UnicodeInfo.GetDisplayText(charInfo), UnicodeInfo.GetDisplayText(i));
            }
        }

#if DEBUG
        [TestMethod]
		public void UnihanCodePointPackingTest()
		{
			for (int i = 0x3400; i < 0x4E00; ++i)
				Assert.AreEqual(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0x4E00; i < 0xA000; ++i)
				Assert.AreEqual(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0xF900; i < 0xFB00; ++i)
				Assert.AreEqual(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0x20000; i < 0x2F800; ++i)
				Assert.AreEqual(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));
			for (int i = 0x2F800; i < 0x30000; ++i)
				Assert.AreEqual(i, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(i)));

			const string packCodePointErrorMessage = "The PackCodePoint method should fail for code points outside of the valid range.";
			const string unpackCodePointErrorMessage = "The PackCodePoint method should fail for values outside of the valid range.";

			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => UnihanCharacterData.PackCodePoint(0xA000), nameof(UnihanCharacterData.PackCodePoint), packCodePointErrorMessage);
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => UnihanCharacterData.PackCodePoint(0xFB00), nameof(UnihanCharacterData.PackCodePoint), packCodePointErrorMessage);
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => UnihanCharacterData.PackCodePoint(0x30000), nameof(UnihanCharacterData.PackCodePoint), packCodePointErrorMessage);

			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => UnihanCharacterData.UnpackCodePoint(-1), nameof(UnihanCharacterData.UnpackCodePoint), unpackCodePointErrorMessage);
			AssertEx.ThrowsExactly<ArgumentOutOfRangeException>(() => UnihanCharacterData.UnpackCodePoint(0x20000), nameof(UnihanCharacterData.UnpackCodePoint), unpackCodePointErrorMessage);
		}
#endif
	}
}
