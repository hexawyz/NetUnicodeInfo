using System.Globalization;
using Xunit;

namespace System.Unicode.Tests
{
	public class UnicodeInfoTests
	{
		[Fact]
		public void UnicodeVersionShouldBeTheLatestSupported()
			=> Assert.Equal(RequestedUnicodeVersion.Value, UnicodeInfo.UnicodeVersion);

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
			=> Assert.Equal(expectedText, UnicodeInfo.GetDisplayText(codePoint));

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
		[InlineData(0x32FF, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "SQUARE ERA NAME REIWA", "Enclosed CJK Letters and Months")]
		[InlineData(0x1F9A4, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "DODO", "Supplemental Symbols and Pictographs")]
		[InlineData(0x1F9AD, UnicodeCategory.OtherSymbol, UnicodeNumericType.None, null, "SEAL", "Supplemental Symbols and Pictographs")]
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

		// The purpose of this test is to ensure that each core property is valid
		[Theory]
		[InlineData(0x61, CoreProperties.Lowercase)] // LATIN SMALL LETTER A
		[InlineData(0x41, CoreProperties.Uppercase)] // LATIN CAPITAL LETTER A
		[InlineData(0x61, CoreProperties.Cased)] // LATIN SMALL LETTER A
		[InlineData(0x41, CoreProperties.Cased)] // LATIN CAPITAL LETTER A
		[InlineData(0x27, CoreProperties.CaseIgnorable)] // APOSTROPHE
		[InlineData(0xFE55, CoreProperties.CaseIgnorable)] // SMALL COLON
		[InlineData(0x1F3FB, CoreProperties.CaseIgnorable)] // 	EMOJI MODIFIER FITZPATRICK TYPE-1-2
		[InlineData(0x41, CoreProperties.ChangesWhenLowercased)] // LATIN CAPITAL LETTER A
		[InlineData(0x62, CoreProperties.ChangesWhenUppercased)] // LATIN SMALL LETTER B
		[InlineData(0x25B, CoreProperties.ChangesWhenUppercased)] // LATIN SMALL LETTER OPEN E
		[InlineData(0x1F7D, CoreProperties.ChangesWhenUppercased)] // GREEK SMALL LETTER OMEGA WITH OXIA
		[InlineData(0xA695, CoreProperties.ChangesWhenUppercased)] // CYRILLIC SMALL LETTER HWE
		[InlineData(0x1E922, CoreProperties.ChangesWhenUppercased)] // ADLAM SMALL LETTER ALIF
		[InlineData(0x7A, CoreProperties.ChangesWhenTitlecased)] // LATIN SMALL LETTER Z
		[InlineData(0x52F, CoreProperties.ChangesWhenTitlecased)] // CYRILLIC SMALL LETTER EL WITH DESCENDER
		[InlineData(0xFB06, CoreProperties.ChangesWhenTitlecased)] // LATIN SMALL LIGATURE ST
		[InlineData(0x1E943, CoreProperties.ChangesWhenTitlecased)] // ADLAM SMALL LETTER SHA
		[InlineData(0x41, CoreProperties.ChangesWhenCasefolded)] // LATIN CAPITAL LETTER A
		[InlineData(0x11A, CoreProperties.ChangesWhenCasefolded)] // LATIN CAPITAL LETTER E WITH CARON
		[InlineData(0x466, CoreProperties.ChangesWhenCasefolded)] // CYRILLIC CAPITAL LETTER LITTLE YUS
		[InlineData(0x212A, CoreProperties.ChangesWhenCasefolded)] // KELVIN SIGN
		[InlineData(0xA76C, CoreProperties.ChangesWhenCasefolded)] // LATIN CAPITAL LETTER IS
		[InlineData(0x1E921, CoreProperties.ChangesWhenCasefolded)] // 	ADLAM CAPITAL LETTER SHA
		[InlineData(0x41, CoreProperties.ChangesWhenCasemapped)] // LATIN CAPITAL LETTER A
		[InlineData(0x18C, CoreProperties.ChangesWhenCasemapped)] // LATIN SMALL LETTER D WITH TOPBAR
		[InlineData(0x13A0, CoreProperties.ChangesWhenCasemapped)] // CHEROKEE LETTER A
		[InlineData(0x2D25, CoreProperties.ChangesWhenCasemapped)] // GEORGIAN SMALL LETTER HOE
		[InlineData(0x1E943, CoreProperties.ChangesWhenCasemapped)] // ADLAM SMALL LETTER SHA
		[InlineData(0x41, CoreProperties.Alphabetic)] // LATIN CAPITAL LETTER A
		[InlineData(0x2188, CoreProperties.Alphabetic)] // ROMAN NUMERAL ONE HUNDRED THOUSAND
		[InlineData(0x1F150, CoreProperties.Alphabetic)] // NEGATIVE CIRCLED LATIN CAPITAL LETTER A
		[InlineData(0x2FA1D, CoreProperties.Alphabetic)] // CJK COMPATIBILITY IDEOGRAPH-2FA1D
		[InlineData(0xAD, CoreProperties.DefaultIgnorableCodePoint)] // SOFT HYPHEN
		[InlineData(0x34F, CoreProperties.DefaultIgnorableCodePoint)] // COMBINING GRAPHEME JOINER
		[InlineData(0x61C, CoreProperties.DefaultIgnorableCodePoint)] // ARABIC LETTER MARK
		[InlineData(0x115F, CoreProperties.DefaultIgnorableCodePoint)] // HANGUL CHOSEONG FILLER
		[InlineData(0x1160, CoreProperties.DefaultIgnorableCodePoint)] // HANGUL JUNGSEONG FILLER
		[InlineData(0x2060, CoreProperties.DefaultIgnorableCodePoint)] // WORD JOINER
		[InlineData(0xFEFF, CoreProperties.DefaultIgnorableCodePoint)] // ZERO WIDTH NO-BREAK SPACE
		[InlineData(0xFFA0, CoreProperties.DefaultIgnorableCodePoint)] // HALFWIDTH HANGUL FILLER
		[InlineData(0xE0FFF, CoreProperties.DefaultIgnorableCodePoint)] // null
		[InlineData(0x20, CoreProperties.GraphemeBase)] // SPACE
		[InlineData(0x7E, CoreProperties.GraphemeBase)] // TILDE
		[InlineData(0xA0, CoreProperties.GraphemeBase)] // NO-BREAK SPACE
		[InlineData(0x1A80, CoreProperties.GraphemeBase)] // TAI THAM HORA DIGIT ZERO
		[InlineData(0x1FB4, CoreProperties.GraphemeBase)] // GREEK SMALL LETTER ALPHA WITH OXIA AND YPOGEGRAMMENI
		[InlineData(0x2C5E, CoreProperties.GraphemeBase)] // GLAGOLITIC SMALL LETTER LATINATE MYSLITE
		[InlineData(0xA830, CoreProperties.GraphemeBase)] // NORTH INDIC FRACTION ONE QUARTER
		[InlineData(0x10133, CoreProperties.GraphemeBase)] // AEGEAN NUMBER NINETY THOUSAND
		[InlineData(0x11838, CoreProperties.GraphemeBase)] // DOGRA SIGN VISARGA
		[InlineData(0x1EE62, CoreProperties.GraphemeBase)] // ARABIC MATHEMATICAL STRETCHED JEEM
		[InlineData(0x1F976, CoreProperties.GraphemeBase)] // FREEZING FACE
		[InlineData(0x2FA1D, CoreProperties.GraphemeBase)] // CJK COMPATIBILITY IDEOGRAPH-2FA1D
		[InlineData(0x300, CoreProperties.GraphemeExtend)] // COMBINING GRAVE ACCENT
		[InlineData(0x36F, CoreProperties.GraphemeExtend)] // COMBINING LATIN SMALL LETTER X
		[InlineData(0x5BF, CoreProperties.GraphemeExtend)] // HEBREW POINT RAFE
		[InlineData(0x823, CoreProperties.GraphemeExtend)] // SAMARITAN VOWEL SIGN A
		[InlineData(0x9C1, CoreProperties.GraphemeExtend)] // BENGALI VOWEL SIGN U
		[InlineData(0xDCF, CoreProperties.GraphemeExtend)] // SINHALA VOWEL SIGN AELA-PILLA
		[InlineData(0xA672, CoreProperties.GraphemeExtend)] // COMBINING CYRILLIC THOUSAND MILLIONS SIGN
		[InlineData(0x1D16E, CoreProperties.GraphemeExtend)] // MUSICAL SYMBOL COMBINING FLAG-1
		[InlineData(0x1E021, CoreProperties.GraphemeExtend)] // COMBINING GLAGOLITIC LETTER YATI
		[InlineData(0xE0020, CoreProperties.GraphemeExtend)] // TAG SPACE
		[InlineData(0xE01EF, CoreProperties.GraphemeExtend)] // VARIATION SELECTOR-256
		[InlineData(0x94D, CoreProperties.GraphemeLink)] // DEVANAGARI SIGN VIRAMA
		[InlineData(0x1714, CoreProperties.GraphemeLink)] // TAGALOG SIGN VIRAMA
		[InlineData(0x2D7F, CoreProperties.GraphemeLink)] // TIFINAGH CONSONANT JOINER
		[InlineData(0x119E0, CoreProperties.GraphemeLink)] // NANDINAGARI SIGN VIRAMA
		[InlineData(0x11D97, CoreProperties.GraphemeLink)] // GUNJALA GONDI VIRAMA
		[InlineData(0x2B, CoreProperties.Math)] // PLUS SIGN
		[InlineData(0x3D0, CoreProperties.Math)] // GREEK BETA SYMBOL
		[InlineData(0x211D, CoreProperties.Math)] // DOUBLE-STRUCK CAPITAL R
		[InlineData(0x2663, CoreProperties.Math)] // BLACK CLUB SUIT
		[InlineData(0x1EEF1, CoreProperties.Math)] // ARABIC MATHEMATICAL OPERATOR HAH WITH DAL
		[InlineData(0x41, CoreProperties.IdentifierStart)] // LATIN CAPITAL LETTER A
		[InlineData(0x41, CoreProperties.IdentifierContinue)] // LATIN CAPITAL LETTER A
		[InlineData(0x41, CoreProperties.ExtendedIdentifierStart)] // LATIN CAPITAL LETTER A
		[InlineData(0x1D539, CoreProperties.ExtendedIdentifierStart)] // MATHEMATICAL DOUBLE-STRUCK CAPITAL B
		[InlineData(0x2FA1D, CoreProperties.ExtendedIdentifierStart)] // CJK COMPATIBILITY IDEOGRAPH-2FA1D
		[InlineData(0x30, CoreProperties.ExtendedIdentifierContinue)] // DIGIT ZERO
		[InlineData(0x41, CoreProperties.ExtendedIdentifierContinue)] // LATIN CAPITAL LETTER A
		[InlineData(0x6E8, CoreProperties.ExtendedIdentifierContinue)] // ARABIC SMALL HIGH NOON
		[InlineData(0xC60, CoreProperties.ExtendedIdentifierContinue)] // TELUGU LETTER VOCALIC RR
		[InlineData(0x2149, CoreProperties.ExtendedIdentifierContinue)] // DOUBLE-STRUCK ITALIC SMALL J
		[InlineData(0x2B734, CoreProperties.ExtendedIdentifierContinue)] // CJK UNIFIED IDEOGRAPH-2B734
		[InlineData(0xE01EF, CoreProperties.ExtendedIdentifierContinue)] // VARIATION SELECTOR-256
		public void CodePointShouldHaveCoreProperties(int codePoint, CoreProperties coreProperties)
			=> Assert.Equal(coreProperties, UnicodeInfo.GetCharInfo(codePoint).CoreProperties & coreProperties);

		[Theory]
		[InlineData(0x23, EmojiProperties.Emoji)] // NUMBER SIGN
		[InlineData(0x2A, EmojiProperties.Emoji)] // ASTERISK
		[InlineData(0x30, EmojiProperties.Emoji)] // DIGIT ZERO
		[InlineData(0x31, EmojiProperties.Emoji)] // DIGIT ONE
		[InlineData(0x39, EmojiProperties.Emoji)] // DIGIT NINE
		[InlineData(0xAE, EmojiProperties.Emoji)] // REGISTERED SIGN
		[InlineData(0x231B, EmojiProperties.Emoji)] // HOURGLASS
		[InlineData(0x2618, EmojiProperties.Emoji)] // SHAMROCK
		[InlineData(0x265F, EmojiProperties.Emoji)] // BLACK CHESS PAWN
		[InlineData(0x3299, EmojiProperties.Emoji)] // CIRCLED IDEOGRAPH SECRET
		[InlineData(0x1F004, EmojiProperties.Emoji)] // MAHJONG TILE RED DRAGON
		[InlineData(0x1F18E, EmojiProperties.Emoji)] // NEGATIVE SQUARED AB
		[InlineData(0x1F232, EmojiProperties.Emoji)] // SQUARED CJK UNIFIED IDEOGRAPH-7981
		[InlineData(0x1F3F5, EmojiProperties.Emoji)] // ROSETTE
		[InlineData(0x1F9FF, EmojiProperties.Emoji)] // NAZAR AMULET
		[InlineData(0x1FA80, EmojiProperties.Emoji)] // YO-YO
		[InlineData(0x1FA95, EmojiProperties.Emoji)] // BANJO
		[InlineData(0x1F9A4, EmojiProperties.Emoji)] // DODO
		[InlineData(0x231A, EmojiProperties.EmojiPresentation)] // WATCH
		[InlineData(0x267F, EmojiProperties.EmojiPresentation)] // WHEELCHAIR SYMBOL
		[InlineData(0x2B55, EmojiProperties.EmojiPresentation)] // HEAVY LARGE CIRCLE
		[InlineData(0x1F19A, EmojiProperties.EmojiPresentation)] // SQUARED VS
		[InlineData(0x1F32D, EmojiProperties.EmojiPresentation)] // HOT DOG
		[InlineData(0x1F3F8, EmojiProperties.EmojiPresentation)] // BADMINTON RACQUET AND SHUTTLECOCK
		[InlineData(0x1FA73, EmojiProperties.EmojiPresentation)] // SHORTS
		[InlineData(0x1FA95, EmojiProperties.EmojiPresentation)] // BANJO
		[InlineData(0x1F9A4, EmojiProperties.EmojiPresentation)] // DODO
		[InlineData(0x1F3FB, EmojiProperties.EmojiModifier)] // EMOJI MODIFIER FITZPATRICK TYPE-1-2
		[InlineData(0x1F3FC, EmojiProperties.EmojiModifier)] // EMOJI MODIFIER FITZPATRICK TYPE-3
		[InlineData(0x1F3FD, EmojiProperties.EmojiModifier)] // EMOJI MODIFIER FITZPATRICK TYPE-4
		[InlineData(0x1F3FE, EmojiProperties.EmojiModifier)] // EMOJI MODIFIER FITZPATRICK TYPE-5
		[InlineData(0x1F3FF, EmojiProperties.EmojiModifier)] // EMOJI MODIFIER FITZPATRICK TYPE-6
		[InlineData(0x261D, EmojiProperties.EmojiModifierBase)] // WHITE UP POINTING INDEX
		[InlineData(0x1F466, EmojiProperties.EmojiModifierBase)] // BOY
		[InlineData(0x1F590, EmojiProperties.EmojiModifierBase)] // RAISED HAND WITH FINGERS SPLAYED
		[InlineData(0x1F9BB, EmojiProperties.EmojiModifierBase)] // EAR WITH HEARING AID
		[InlineData(0x1F9DD, EmojiProperties.EmojiModifierBase)] // ELF
		[InlineData(0x23, EmojiProperties.EmojiComponent)] // NUMBER SIGN
		[InlineData(0x2A, EmojiProperties.EmojiComponent)] // ASTERISK
		[InlineData(0x30, EmojiProperties.EmojiComponent)] // DIGIT ZERO
		[InlineData(0x31, EmojiProperties.EmojiComponent)] // DIGIT ONE
		[InlineData(0x39, EmojiProperties.EmojiComponent)] // DIGIT NINE
		[InlineData(0x200D, EmojiProperties.EmojiComponent)] // ZERO WIDTH JOINER
		[InlineData(0x20E3, EmojiProperties.EmojiComponent)] // COMBINING ENCLOSING KEYCAP
		[InlineData(0xFE0F, EmojiProperties.EmojiComponent)] // VARIATION SELECTOR-16
		[InlineData(0x1F1E6, EmojiProperties.EmojiComponent)] // REGIONAL INDICATOR SYMBOL LETTER A
		[InlineData(0x1F3FB, EmojiProperties.EmojiComponent)] // EMOJI MODIFIER FITZPATRICK TYPE-1-2
		[InlineData(0x1F3FF, EmojiProperties.EmojiComponent)] // EMOJI MODIFIER FITZPATRICK TYPE-6
		[InlineData(0x1F9B0, EmojiProperties.EmojiComponent)] // EMOJI COMPONENT RED HAIR
		[InlineData(0x1F9B3, EmojiProperties.EmojiComponent)] // EMOJI COMPONENT WHITE HAIR
		[InlineData(0xE0020, EmojiProperties.EmojiComponent)] // TAG SPACE
		[InlineData(0xE007F, EmojiProperties.EmojiComponent)] // CANCEL TAG
		[InlineData(0xA9, EmojiProperties.ExtendedPictographic)] // COPYRIGHT SIGN
		[InlineData(0xAE, EmojiProperties.ExtendedPictographic)] // REGISTERED SIGN
		[InlineData(0x203C, EmojiProperties.ExtendedPictographic)] // DOUBLE EXCLAMATION MARK
		[InlineData(0x2328, EmojiProperties.ExtendedPictographic)] // KEYBOARD
		[InlineData(0x1F9B0, EmojiProperties.ExtendedPictographic)] // EMOJI COMPONENT RED HAIR
		[InlineData(0x1F9B3, EmojiProperties.ExtendedPictographic)] // EMOJI COMPONENT WHITE HAIR
		[InlineData(0x1FA95, EmojiProperties.ExtendedPictographic)] // BANJO
		public void CodePointShouldHaveEmojiProperties(int codePoint, EmojiProperties emojiProperties)
			=> Assert.Equal(emojiProperties, UnicodeInfo.GetCharInfo(codePoint).EmojiProperties & emojiProperties);

		[Theory]
		[InlineData(0x41, EmojiProperties.Emoji)] // LATIN CAPITAL LETTER A
		[InlineData(0x23, EmojiProperties.EmojiPresentation)] // NUMBER SIGN
		[InlineData(0x263A, EmojiProperties.EmojiPresentation)] // WHITE SMILING FACE
		[InlineData(0x26E9, EmojiProperties.EmojiPresentation)] // SHINTO SHRINE
		[InlineData(0x3030, EmojiProperties.EmojiPresentation)] // WAVY DASH
		[InlineData(0x23, EmojiProperties.EmojiModifier)] // NUMBER SIGN
		[InlineData(0x30, EmojiProperties.EmojiModifier)] // DIGIT ZERO
		[InlineData(0x41, EmojiProperties.EmojiModifier)] // LATIN CAPITAL LETTER A
		[InlineData(0x263A, EmojiProperties.EmojiModifier)] // WHITE SMILING FACE
		[InlineData(0x1FA73, EmojiProperties.EmojiModifier)] // SHORTS
		[InlineData(0x23, EmojiProperties.EmojiModifierBase)] // NUMBER SIGN
		[InlineData(0x30, EmojiProperties.EmojiModifierBase)] // DIGIT ZERO
		[InlineData(0x41, EmojiProperties.EmojiModifierBase)] // LATIN CAPITAL LETTER A
		[InlineData(0x263A, EmojiProperties.EmojiModifierBase)] // WHITE SMILING FACE
		[InlineData(0x1FA73, EmojiProperties.EmojiModifierBase)] // SHORTS
		[InlineData(0xA9, EmojiProperties.EmojiComponent)] // COPYRIGHT SIGN
		[InlineData(0xAE, EmojiProperties.EmojiComponent)] // REGISTERED SIGN
		[InlineData(0x23F3, EmojiProperties.EmojiComponent)] // HOURGLASS WITH FLOWING SAND
		[InlineData(0x263A, EmojiProperties.EmojiComponent)] // WHITE SMILING FACE
		[InlineData(0x2692, EmojiProperties.EmojiComponent)] // HAMMER AND PICK
		[InlineData(0x1F200, EmojiProperties.EmojiComponent)] // SQUARE HIRAGANA HOKA
		[InlineData(0x1F5DE, EmojiProperties.EmojiComponent)] // ROLLED-UP NEWSPAPER
		[InlineData(0x1FA70, EmojiProperties.EmojiComponent)] // BALLET SHOES
		[InlineData(0x1FA95, EmojiProperties.EmojiComponent)] // BANJO
		[InlineData(0x23, EmojiProperties.ExtendedPictographic)] // NUMBER SIGN
		[InlineData(0x30, EmojiProperties.ExtendedPictographic)] // DIGIT ZERO
		[InlineData(0x39, EmojiProperties.ExtendedPictographic)] // DIGIT NINE
		[InlineData(0x41, EmojiProperties.ExtendedPictographic)] // LATIN CAPITAL LETTER A
		[InlineData(0x200D, EmojiProperties.ExtendedPictographic)] // ZERO WIDTH JOINER
		[InlineData(0x20E3, EmojiProperties.ExtendedPictographic)] // COMBINING ENCLOSING KEYCAP
		[InlineData(0xFE0F, EmojiProperties.ExtendedPictographic)] // VARIATION SELECTOR-16
		[InlineData(0x1F1E6, EmojiProperties.ExtendedPictographic)] // REGIONAL INDICATOR SYMBOL LETTER A
		[InlineData(0x1F3FB, EmojiProperties.ExtendedPictographic)] // EMOJI MODIFIER FITZPATRICK TYPE-1-2
		[InlineData(0x1F3FF, EmojiProperties.ExtendedPictographic)] // EMOJI MODIFIER FITZPATRICK TYPE-6
		[InlineData(0xE0020, EmojiProperties.ExtendedPictographic)] // TAG SPACE
		[InlineData(0xE007F, EmojiProperties.ExtendedPictographic)] // CANCEL TAG
		public void CodePointShouldNotHaveEmojiProperties(int codePoint, EmojiProperties emojiProperties)
			=> Assert.Equal((EmojiProperties)0, UnicodeInfo.GetCharInfo(codePoint).EmojiProperties & emojiProperties);

#if FIXME
		[Theory]
		[InlineData('\0')]
		[InlineData('\uABFF')]
		[InlineData('\uD7A5')]
		public void HangulNameShouldFailForNonHangulCodePoints(char codePoint)
			=> Assert.Throws<ArgumentOutOfRangeException>(() => HangulInfo.GetHangulName(codePoint));
#endif

		[Theory]
		[InlineData("HANGUL SYLLABLE PWILH", 0xD4DB)]
		[InlineData("HANGUL SYLLABLE PWAENG", 0xD439)]
		[InlineData("HANGUL SYLLABLE PANJ", 0xD311)]
		[InlineData("HANGUL SYLLABLE TOLM", 0xD1AA)]
		public void HangulNameShouldReturnExpectedResult(string expectedName, int codePoint)
			=> Assert.Equal(expectedName, UnicodeInfo.GetName(codePoint));

		[Theory]
		[InlineData("Basic Latin", 0x0041)]
		[InlineData("Miscellaneous Technical", 0x2307)]
		[InlineData("Hangul Syllables", 0xD311)]
		[InlineData("Miscellaneous Symbols and Pictographs", 0x1F574)]
		public void MethodGetBlockNameShouldHaveExpectedResult(string expectedBlockName, int codePoint)
			=> Assert.Equal(expectedBlockName, UnicodeInfo.GetBlockName(codePoint));

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

#if FIXME
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
