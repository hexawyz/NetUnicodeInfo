namespace System.Unicode
{
	internal static class HangulInfo
	{
		// Constants defined on page 144 of the Unicode 7.0 Standard (3.12)
		private const ushort SBase = 0xAC00;
		//private const ushort LBase = 0x1100;
		//private const ushort VBase = 0x1161;
		//private const ushort TBase = 0x11A7;
		private const int LCount = 19;
		private const int VCount = 21;
		private const int TCount = 28;
		private const int NCount = VCount * TCount;
		private const int SCount = LCount * NCount;

		private static readonly string[] JamoLTable =
		{
			"G", "GG", "N", "D", "DD", "R", "M", "B", "BB",
			"S", "SS", "", "J", "JJ", "C", "K", "T", "P", "H"
		};

		private static readonly string[] JamoVTable =
		{
			"A", "AE", "YA", "YAE", "EO", "E", "YEO", "YE", "O",
			"WA", "WAE", "OE", "YO", "U", "WEO", "WE", "WI",
			"YU", "EU", "YI", "I"
		};

		private static readonly string[] JamoTTable =
		{
			"", "G", "GG", "GS", "N", "NJ", "NH", "D", "L", "LG", "LM",
			"LB", "LS", "LT", "LP", "LH", "M", "B", "BS",
			"S", "SS", "NG", "J", "C", "K", "T", "P", "H"
		};

		// Algorithm defined on page 150 of the Unicode 7.0 Standard (3.12)
		internal static string GetHangulName(char codePoint)
		{
			int sIndex = codePoint - SBase;

			if (sIndex < 0 || sIndex >= SCount) throw new ArgumentOutOfRangeException(nameof(codePoint));

			int lIndex = sIndex / NCount;
			int vIndex = sIndex % NCount / TCount;
			int tIndex = sIndex % TCount;

			return "HANGUL SYLLABLE " + JamoLTable[lIndex] + JamoVTable[vIndex] + JamoTTable[tIndex];
		}

		internal static bool IsHangul(int codePoint)
			=> codePoint >= SBase && codePoint < SBase + SCount;
	}
}
