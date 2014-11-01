using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public struct UnicodeCharacterRange
	{
		public readonly int FirstCodePoint;
		public readonly int LastCodePoint;

		public UnicodeCharacterRange(int codePoint)
		{
			if (codePoint < 0 || codePoint > 0x10FFFF) throw new ArgumentOutOfRangeException("codePoint");

			FirstCodePoint = codePoint;
			LastCodePoint = codePoint;
		}

		public UnicodeCharacterRange(int firstCodePoint, int lastCodePoint)
		{
			if (firstCodePoint < 0 || firstCodePoint > 0x10FFFF) throw new ArgumentOutOfRangeException("firstCodePoint");
			if (lastCodePoint < firstCodePoint || lastCodePoint > 0x10FFFF) throw new ArgumentOutOfRangeException("lastCodePoint");

            FirstCodePoint = firstCodePoint;
			LastCodePoint = lastCodePoint;
		}

		public override string ToString()
		{
			return FirstCodePoint == LastCodePoint ? FirstCodePoint.ToString("X4") : FirstCodePoint.ToString("X4") + ".." + LastCodePoint.ToString("X4");
		}

		public static UnicodeCharacterRange Parse(string s)
		{
			int start, end;

			var rangeSeparatorOffset = s.IndexOf("..");

			if (rangeSeparatorOffset == 0) throw new FormatException();
			else if (rangeSeparatorOffset < 0)
			{
				start = end = int.Parse(s, NumberStyles.HexNumber);
			}
			else
			{
				start = int.Parse(s.Substring(0, rangeSeparatorOffset), NumberStyles.HexNumber);
				end = int.Parse(s.Substring(rangeSeparatorOffset + 2), NumberStyles.HexNumber);
            }

			return new UnicodeCharacterRange(start, end);
		}
	}
}
