namespace System.Unicode
{
	internal readonly partial struct UnihanCharacterData
	{
		public readonly int CodePoint;
		public readonly UnihanNumericType NumericType;
		public readonly long NumericValue;
		public readonly UnicodeRadicalStrokeCount[] UnicodeRadicalStrokeCounts;
		public readonly string Definition;
		public readonly string MandarinReading;
		public readonly string CantoneseReading;
		public readonly string JapaneseKunReading;
		public readonly string JapaneseOnReading;
		public readonly string KoreanReading;
		public readonly string HangulReading;
		public readonly string VietnameseReading;
		public readonly string SimplifiedVariant;
		public readonly string TraditionalVariant;

		internal UnihanCharacterData
		(
			int codePoint,
			UnihanNumericType numericType,
			long numericValue,
			UnicodeRadicalStrokeCount[] unicodeRadicalStrokeCounts,
			string definition,
			string mandarinReading,
			string cantoneseReading,
			string japaneseKunReading,
			string japaneseOnReading,
			string koreanReading,
			string hangulReading,
			string vietnameseReading,
			string simplifiedVariant,
			string traditionalVariant
		)
		{
			CodePoint = codePoint;
			NumericType = numericType;
			NumericValue = numericValue;
			UnicodeRadicalStrokeCounts = unicodeRadicalStrokeCounts;
			Definition = definition;
			MandarinReading = mandarinReading;
			CantoneseReading = cantoneseReading;
			JapaneseKunReading = japaneseKunReading;
			JapaneseOnReading = japaneseOnReading;
			KoreanReading = koreanReading;
			HangulReading = hangulReading;
			VietnameseReading = vietnameseReading;
			SimplifiedVariant = simplifiedVariant;
			TraditionalVariant = traditionalVariant;
		}
	}
}
