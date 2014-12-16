using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Provides information on radical and additional stroke count for a code point.</summary>
	/// <remarks>Values of this type are usually associated with the property kRSUnicode (aka. Unicode_Radical_Stroke).</remarks>
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

		/// <summary>Gets the index of the Kangxi radical of the character.</summary>
		/// <remarks>The Kangxi radicals are numbered from 1 to 214 inclusive.</remarks>
		/// <value>The index of the Kangxi radical.</value>
		public byte Radical { get { return radical; } }
		/// <summary>Gets the additional stroke count.</summary>
		/// <value>
		/// The stroke count.
		/// </value>
		public byte StrokeCount { get { return (byte)(strokeCount & 0x7F); } }
		/// <summary>Gets a value indicating whether the information is based on the simplified form of the radical.</summary>
		/// <value><see langword="true" /> if the information is based on the simplified form of the radical; otherwise, <see langword="false" />.</value>
		public bool IsSimplified { get { return (strokeCount & 0x80) != 0; } }
    }
}
