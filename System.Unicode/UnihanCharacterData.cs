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

		// This method packs code points by predicted order of importance (it may be wrong)
		// Its purpose is to avoid skipping numbers so that file encoding can be more efficient.
		internal static int PackCodePoint(int codePoint)
		{
			if (codePoint >= 0x3400)
			{
				// 3400..4DBF; CJK Unified Ideographs Extension A
				if (codePoint < 0x4DC0) return codePoint + 0x1E00;
				else if (codePoint >= 0x4E00)
				{
					// 4E00..9FFF; CJK Unified Ideographs
					if (codePoint < 0xA000) return codePoint - 0x4E00;
					// F900..FAFF; CJK Compatibility Ideographs
					else if (codePoint >= 0xF900 && codePoint < 0xFB00) return codePoint + 0xFD00;
					else if (codePoint >= 0x20000)
					{
						// 20000..2A6DF; CJK Unified Ideographs Extension B
						// 2A700..2B73F; CJK Unified Ideographs Extension C
						// 2B740..2B81F; CJK Unified Ideographs Extension D
						// 2B820..2CEAF; CJK Unified Ideographs Extension E
						// 2CEB0..2EBEF; CJK Unified Ideographs Extension F
						// ..2F7FF; NA
						if (codePoint < 0x2F800) return codePoint - 0x19400;
						// 2F800..2FA1F; CJK Compatibility Ideographs Supplement
						else if (codePoint < 0x2FA20) return codePoint - 0x10000;
						// 30000..3134F; CJK Unified Ideographs Extension G
						else if (codePoint >= 0x3000 && codePoint < 0x31350) return 0;
					}
				}
			}

			throw new ArgumentOutOfRangeException(nameof(codePoint));
		}

		// Reverses the packing done by the PackCodePoint method.
		internal static int UnpackCodePoint(int packedCodePoint)
		{
			if (packedCodePoint >= 0)
			{
				// 4E00..9FFF; CJK Unified Ideographs
				if (packedCodePoint < 0x05200) return packedCodePoint + 0x4E00;
				// 3400..4DBF; CJK Unified Ideographs Extension A
				else if (packedCodePoint < 0x06C00) return packedCodePoint - 0x1E00;
				// 20000..2A6DF; CJK Unified Ideographs Extension B
				// 2A700..2B73F; CJK Unified Ideographs Extension C
				// 2B740..2B81F; CJK Unified Ideographs Extension D
				// 2B820..2CEAF; CJK Unified Ideographs Extension E
				// 2CEB0..2EBEF; CJK Unified Ideographs Extension F
				else if (packedCodePoint < 0x1F600) return packedCodePoint + 0x19400;
				// F900..FAFF; CJK Compatibility Ideographs
				else if (packedCodePoint < 0x1F800) return packedCodePoint - 0xFD00;
				// 2F800..2FA1F; CJK Compatibility Ideographs Supplement
				else if (packedCodePoint < 0x20000) return packedCodePoint + 0x10000;
				// 30000..3134F; CJK Unified Ideographs Extension G
				else if (packedCodePoint < 0x20000) return packedCodePoint + 0x10000;
			}
			throw new ArgumentOutOfRangeException(nameof(packedCodePoint));
		}
	}
}
