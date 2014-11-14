using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	internal sealed class UnihanCharacterData
	{
		public readonly int CodePoint;
		public readonly UnihanNumericType NumericType;
		public readonly ulong NumericValue;
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

		internal static int PackCodePoint(int codePoint)
		{
			if (codePoint >= 0x3400)
			{
				if (codePoint < 0x4E00) return codePoint + 0x1E00;
				else if (codePoint < 0xA000) return codePoint - 0x4E00;
				else if (codePoint >= 0xF900 && codePoint < 0xFB00) return codePoint + 0xFD00;
				else if (codePoint >= 0x20000)
				{
					if (codePoint < 0x2F800) return codePoint - 0x19400;
					else if (codePoint < 0x30000) return codePoint - 0x10000;
				}
            }

			throw new ArgumentOutOfRangeException("codePoint");
        }

		internal static int UnpackCodePoint(int packedCodePoint)
		{
			if (packedCodePoint >= 0)
			{
				if (packedCodePoint < 0x05200) return packedCodePoint + 0x4E00;
				else if (packedCodePoint < 0x06C00) return packedCodePoint - 0x1E00;
				else if (packedCodePoint < 0x1F600) return packedCodePoint + 0x19400;
				else if (packedCodePoint < 0x1F800) return packedCodePoint - 0xFD00;
				else if (packedCodePoint < 0x20000) return packedCodePoint + 0x10000;
			}
			throw new ArgumentOutOfRangeException("packedCodePoint");
		}
	}
}
