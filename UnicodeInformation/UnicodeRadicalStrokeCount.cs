using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeRadicalStrokeCount
	{
		internal static readonly UnicodeRadicalStrokeCount[] EmptyArray = new UnicodeRadicalStrokeCount[0];

		private readonly byte radical;
		private readonly byte strokeCount;

		internal UnicodeRadicalStrokeCount(byte rawRadical, byte rawStrokeCount)
		{
			radical = rawRadical;
			strokeCount = rawStrokeCount;
		}

		internal UnicodeRadicalStrokeCount(byte radical, byte strokeCount, bool isSimplified)
		{
			this.radical = radical;
			if (isSimplified) strokeCount |= 0x80;
			this.strokeCount = strokeCount;
		}

		public byte Radical { get { return radical; } }
		public byte StrokeCount { get { return (byte)(strokeCount & 0x7F); } }
		public bool IsSimplified { get { return (strokeCount & 0x80) != 0; } }
    }
}
