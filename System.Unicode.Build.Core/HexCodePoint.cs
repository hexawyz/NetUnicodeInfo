namespace System.Unicode.Build.Core
{
	public static class HexCodePoint
	{
		public static int ParsePrefixed(string s)
		{
			if (!s.StartsWith("U+"))
			{
				throw new FormatException("Expected a code point in the form U+nnnn.");
			}
			return Parse(s, 2);
		}

		public static int Parse(string s, int index) => Parse(s, ref index);

		public static int Parse(string s, ref int index)
		{
			int i = index;
			int accum = 0;

			while (i < s.Length)
			{
				char c = s[i];

				if (c == ' ') break;

				accum <<= 4;

				if (c >= '0' && c <= '9') accum |= c - '0';
				else if (c >= 'A' && c <= 'F') accum |= c - 'A' + 0xA;
				else if (c >= 'a' && c <= 'f') accum |= c - 'a' + 0xA;
				else throw new FormatException();

				++i;
			}

			index = i;
			return accum;
		}
	}
}
