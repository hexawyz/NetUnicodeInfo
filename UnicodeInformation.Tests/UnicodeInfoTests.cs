using System;
using System.Unicode;
using System.Globalization;
using Xunit;

namespace UnicodeInformation.Tests
{
	public class UnicodeInfoTests
	{
		[Fact]
		public void UnicodeVersionShouldBeTheLatestSupported()
		{
			Assert.Equal(new Version(12, 0, 0), UnicodeInfo.UnicodeVersion);
		}
		
		[Fact]
		public void ControlCharactersShouldHaveSpecificDisplayText()
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
		public void DisplayTextShouldReturnExpectedResult(string expectedText, int codePoint)
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
		[InlineData(0x11FC0, UnicodeCategory.OtherNumber, UnicodeNumericType.Numeric, "1/320", "TAMIL FRACTION ONE THREE-HUNDRED-AND-TWENTIETH", "Tamil Supplement")]
		[InlineData(0x1F953, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "BACON", "Supplemental Symbols and Pictographs")]
		[InlineData(0x1F966, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "BROCCOLI", "Supplemental Symbols and Pictographs")]
		[InlineData(0x1F99E, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "LOBSTER", "Supplemental Symbols and Pictographs")]
		[InlineData(0x1F9A6, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "OTTER", "Supplemental Symbols and Pictographs")]
		public void CharacterInfoShouldHaveExpectedResults(int codePoint, UnicodeCategory expectedCategory, UnicodeNumericType expectedNumericType, string expectedNumericValue, string expectedName, string expectedBlock)
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

#if DEBUG
		[Theory]
		[InlineData('\0')]
		[InlineData('\uABFF')]
		[InlineData('\uD7A5')]
		public void HangulNameShouldFailForNonHangulCodePoints(char codePoint)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => HangulInfo.GetHangulName(codePoint));
		}
#endif

		[Theory]
		[InlineData("HANGUL SYLLABLE PWILH", 0xD4DB)]
		[InlineData("HANGUL SYLLABLE PWAENG", 0xD439)]
		[InlineData("HANGUL SYLLABLE PANJ", 0xD311)]
		[InlineData("HANGUL SYLLABLE TOLM", 0xD1AA)]
		public void HangulNameShouldReturnExpectedResult(string expectedName, int codePoint)
		{
			Assert.Equal(expectedName, UnicodeInfo.GetName(codePoint));
		}

		[Theory]
		[InlineData("Basic Latin", 0x0041)]
		[InlineData("Miscellaneous Technical", 0x2307)]
		[InlineData("Hangul Syllables", 0xD311)]
		[InlineData("Miscellaneous Symbols and Pictographs", 0x1F574)]
		public void MethodGetBlockNameShouldHaveExpectedResult(string expectedBlockName, int codePoint)
		{
			Assert.Equal(expectedBlockName, UnicodeInfo.GetBlockName(codePoint));
		}

		[Fact]
		public void RadicalStrokeCountShouldHaveExpectedResults()
		{
			var char5E7A = UnicodeInfo.GetCharInfo(0x5E7A);

			Assert.NotEmpty(char5E7A.UnicodeRadicalStrokeCounts);
			Assert.False(char5E7A.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.Equal(52, char5E7A.UnicodeRadicalStrokeCounts[0].Radical);
			Assert.Equal(0, char5E7A.UnicodeRadicalStrokeCounts[0].StrokeCount);

			var char2A6D6 = UnicodeInfo.GetCharInfo(0x2A6D6);

			Assert.NotEmpty(char2A6D6.UnicodeRadicalStrokeCounts);
			Assert.False(char2A6D6.UnicodeRadicalStrokeCounts[0].IsSimplified);
			Assert.Equal(214, char2A6D6.UnicodeRadicalStrokeCounts[0].Radical);
			Assert.Equal(20, char2A6D6.UnicodeRadicalStrokeCounts[0].StrokeCount);
		}

		[Fact]
		public void RadicalInfoShouldHaveExpectedResults()
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
		public void MethodGetNameShouldNeverFail()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetName(i);
			}
		}

		[Fact]
		public void MethodGetDisplayTextShouldNeverFail()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetDisplayText(i);
			}
		}

		[Fact]
		public void MethodGetCategoryShouldNeverFail()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetCategory(i);
			}
		}

		[Fact]
		public void MethodGetCharInfoShouldNeverFail()
		{
			for (int i = 0; i <= 0x10FFFF; i++)
			{
				UnicodeInfo.GetCharInfo(i);
			}
		}

		[Fact]
		public void MethodGetCharInfoShouldHaveCoherentResults()
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
		public void UnihanCodePointPackingShouldHaveExpectedResults()
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
