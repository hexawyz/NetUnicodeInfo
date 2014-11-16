using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeCharInfo
	{
		private readonly int codePoint;
		private readonly UnicodeCharacterData unicodeCharacterData;
		private readonly UnihanCharacterData unihanCharacterData;
		private readonly string block;

		public int CodePoint { get { return codePoint; } }

		public string Name
		{
			get
			{
				return unicodeCharacterData.Name == null || unicodeCharacterData.CodePointRange.IsSingleCodePoint ?
					unicodeCharacterData.Name :
					unicodeCharacterData.Name + "-" + codePoint.ToString("X4");
			}
		}

		public UnicodeCategory Category { get { return unicodeCharacterData != null ? unicodeCharacterData.Category : UnicodeCategory.OtherNotAssigned; } }
		public string Block { get { return block ?? "No_Block"; } }
		public CanonicalCombiningClass CanonicalCombiningClass { get { return unicodeCharacterData.CanonicalCombiningClass; } }
		public BidirectionalClass BidirectionalClass { get { return unicodeCharacterData.BidirectionalClass; } }
		public CompatibilityFormattingTag DecompositionType { get { return unicodeCharacterData.DecompositionType; } }
		public string DecompositionMapping { get { return unicodeCharacterData.DecompositionMapping; } }
		public UnicodeNumericType NumericType { get { return unihanCharacterData != null ? unihanCharacterData.NumericType != UnihanNumericType.None ? UnicodeNumericType.Numeric : UnicodeNumericType.None : unicodeCharacterData.NumericType; } }
		public UnihanNumericType UnihanNumericType { get { return unihanCharacterData != null ? unihanCharacterData.NumericType : UnihanNumericType.None; } }
		public UnicodeRationalNumber? NumericValue { get { return unihanCharacterData != null && unihanCharacterData.NumericType != UnihanNumericType.None ? new UnicodeRationalNumber(unihanCharacterData.NumericValue, 1) : unicodeCharacterData.NumericValue; } }
		public bool BidirectionalMirrored { get { return unicodeCharacterData.BidirectionalMirrored; } }
		public string OldName { get { return unicodeCharacterData.OldName; } }
		public string SimpleUpperCaseMapping { get { return unicodeCharacterData.SimpleUpperCaseMapping; } }
		public string SimpleLowerCaseMapping { get { return unicodeCharacterData.SimpleLowerCaseMapping; } }
		public string SimpleTitleCaseMapping { get { return unicodeCharacterData.SimpleTitleCaseMapping; } }
		public ContributoryProperties ContributoryProperties { get { return unicodeCharacterData.ContributoryProperties; } }
		public CoreProperties CoreProperties { get { return unicodeCharacterData.CoreProperties; } }

		public string Definition { get { return unihanCharacterData?.Definition; } }
		public string MandarinReading { get { return unihanCharacterData?.MandarinReading; } }
		public string CantoneseReading { get { return unihanCharacterData?.CantoneseReading; } }
		public string JapaneseKunReading { get { return unihanCharacterData?.JapaneseKunReading; } }
		public string JapaneseOnReading { get { return unihanCharacterData?.JapaneseOnReading; } }
		public string KoreanReading { get { return unihanCharacterData?.KoreanReading; } }
		public string HangulReading { get { return unihanCharacterData?.HangulReading; } }
		public string VietnameseReading { get { return unihanCharacterData?.VietnameseReading; } }

		public string SimplifiedVariant { get { return unihanCharacterData?.SimplifiedVariant; } }
		public string TraditionalVariant { get { return unihanCharacterData?.TraditionalVariant; } }

		internal UnicodeCharInfo(int codePoint, UnicodeCharacterData unicodeCharacterData, UnihanCharacterData unihanCharacterData, string block)
		{
			this.codePoint = codePoint;
			this.unicodeCharacterData = unicodeCharacterData;
			this.unihanCharacterData = unihanCharacterData;
			this.block = block;
		}
	}
}
