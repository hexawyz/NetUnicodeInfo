using System;
using System.Collections.Generic;
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
			FirstCodePoint = codePoint;
			LastCodePoint = codePoint;
		}

		public UnicodeCharacterRange(int firstCodePoint, int lastCodePoint)
		{
			FirstCodePoint = firstCodePoint;
			LastCodePoint = lastCodePoint;
		}
	}
}
