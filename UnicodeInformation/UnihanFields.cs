namespace System.Unicode
{
	[Flags]
	internal enum UnihanFields : ushort
	{
		// NumericType / NumericValue : Not exactly a bit mask here…
		PrimaryNumeric = 1,
		AccountingNumeric = 2,
		OtherNumeric = 3,

		// UnicodeRadicalStroke : Not exactly a bit mask…
		UnicodeRadicalStrokeCount = 4, // Will indicate exactly one value for Unicode_Radical_Stroke.
		UnicodeRadicalStrokeCountTwice = 8, // Will indicate exactly two values for Unicode_Radical_Stroke.
		UnicodeRadicalStrokeCountMore = 12, // Will indicate three or more values for Unicode_Radical_Stroke. This combination should never happen in the current files.

		Definition = 16,
		MandarinReading = 32,
		CantoneseReading = 64,
		JapaneseKunReading = 128,
		JapaneseOnReading = 256,
		KoreanReading = 512,
		HangulReading = 1024,
		VietnameseReading = 2048,

		SimplifiedVariant = 4096,
		TraditionalVariant = 8192,
	}
}
