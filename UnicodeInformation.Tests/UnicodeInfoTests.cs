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

		private static void AssertChar(int codePoint, UnicodeCategory category, string name)
		{
			var info = UnicodeInfo.GetCharInfo(codePoint);
			Assert.AreEqual(codePoint, info.CodePoint);
            Assert.AreEqual(category, info.Category);
			Assert.AreEqual(name, info.Name);
		}

		[TestMethod]
		public void CharacterInfoTest()
		{
			AssertChar(0x0041, UnicodeCategory.UppercaseLetter, "LATIN CAPITAL LETTER A");
			AssertChar(0x1F600, UnicodeCategory.OtherSymbol, "GRINNING FACE");
			AssertChar(0x00E9, UnicodeCategory.LowercaseLetter, "LATIN SMALL LETTER E WITH ACUTE");
			AssertChar(0xD4DB, UnicodeCategory.OtherLetter, "HANGUL SYLLABLE PWILH");
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
			Assert.AreEqual("HANGUL SYLLABLE PWILH", UnicodeInfo.GetName(0xD4DB));
			Assert.AreEqual("HANGUL SYLLABLE PWAENG", UnicodeInfo.GetName(0xD439));
			Assert.AreEqual("HANGUL SYLLABLE PANJ", UnicodeInfo.GetName(0xD311));
			Assert.AreEqual("HANGUL SYLLABLE TOLM", UnicodeInfo.GetName(0xD1AA));
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

			try
			{
				UnihanCharacterData.PackCodePoint(0xA000);
				Assert.Fail("The PackCodePoint method should fail for code points outside of the valid range.");
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			try
			{
				UnihanCharacterData.PackCodePoint(0xFB00);
				Assert.Fail("The PackCodePoint method should fail for code points outside of the valid range.");
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			try
			{
				UnihanCharacterData.PackCodePoint(0x30000);
				Assert.Fail("The PackCodePoint method should fail for code points outside of the valid range.");
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			try
			{
				UnihanCharacterData.UnpackCodePoint(0x20000);
				Assert.Fail("The UnpackCodePoint method should fail for code points outside of the valid range.");
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}
#endif
	}
}
