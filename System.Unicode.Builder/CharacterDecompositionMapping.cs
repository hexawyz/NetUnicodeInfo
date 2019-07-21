namespace System.Unicode.Builder
{
	public struct CharacterDecompositionMapping
	{
		public readonly CompatibilityFormattingTag DecompositionType;
		public readonly string DecompositionMapping;

		public CharacterDecompositionMapping(CompatibilityFormattingTag decompositionType, string decompositionMapping)
		{
			DecompositionType = decompositionType;
			DecompositionMapping = decompositionMapping;
		}

		public unsafe static CharacterDecompositionMapping Parse(string s)
		{
			if (string.IsNullOrEmpty(s)) return default;

			var tag = CompatibilityFormattingTag.Canonical;

			int index;

			if (s[0] == '<')
			{
				if (!EnumHelper<CompatibilityFormattingTag>.TryGetNamedValue(s.Substring(1, (index = s.IndexOf('>')) - 1), out tag))
					throw new FormatException();
				++index;
			}
			else
			{
				index = 0;
			}

			var buffer = stackalloc char[36];  // From the Unicode docs, a decomposition cannot have more than 18 code points.
			int charIndex = 0;

			while (index < s.Length && charIndex < 35)
			{
				char c = s[index];

				if (c == ' ') ++index;
				else
				{
					int codePoint = HexCodePoint.Parse(s, ref index);

					if (codePoint < 0x10000)
						buffer[charIndex++] = (char)codePoint;
					else if (codePoint < 0x10FFFF)
					{
						codePoint -= 0x10000;
						buffer[charIndex++] = (char)((codePoint >> 10) + 0xD800);
						buffer[charIndex++] = (char)((codePoint & 0x3FF) + 0xDC00);
					}
					else
					{
						throw new FormatException("The code point was outside of the allowed range.");
					}
				}
			}

			return new CharacterDecompositionMapping(tag, new string(buffer, 0, charIndex));
		}
	}
}
