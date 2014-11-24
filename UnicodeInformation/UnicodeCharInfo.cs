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
		private readonly string name;
		private readonly UnicodeCharacterData unicodeCharacterData;
		private readonly UnihanCharacterData unihanCharacterData;
		private readonly string block;

		public int CodePoint { get { return codePoint; } }

		[ValueName("Name"), ValueName("na")]
		public string Name { get { return name; } }
		[ValueName("Name_Alias")]
		public UnicodeNameAliasCollection NameAliases { get { return new UnicodeNameAliasCollection(unicodeCharacterData?.NameAliases); } }

		[ValueName("General_Category"), ValueName("gc")]
		public UnicodeCategory Category { get { return unicodeCharacterData?.Category ?? UnicodeCategory.OtherNotAssigned; } }
		[ValueName("Block"), ValueName("blk")]
		public string Block { get { return block ?? UnicodeInfo.DefaultBlock; } }
		[ValueName("Canonical_Combining_Class"), ValueName("ccc")]
		public CanonicalCombiningClass CanonicalCombiningClass { get { return unicodeCharacterData?.CanonicalCombiningClass ?? CanonicalCombiningClass.NotReordered; } }
		[ValueName("Bidi_Class"), ValueName("bc")]
		public BidirectionalClass BidirectionalClass { get { return unicodeCharacterData?.BidirectionalClass ?? BidirectionalClass.LeftToRight; } }
		[ValueName("Decomposition_Type"), ValueName("dt")]
		public CompatibilityFormattingTag DecompositionType { get { return unicodeCharacterData?.DecompositionType ?? CompatibilityFormattingTag.Canonical; } }
		[ValueName("Decomposition_Mapping"), ValueName("dm")]
		public string DecompositionMapping { get { return unicodeCharacterData?.DecompositionMapping; } }
		[ValueName("Numeric_Type"), ValueName("nt")]
		public UnicodeNumericType NumericType { get { return unihanCharacterData != null ? unihanCharacterData.NumericType != UnihanNumericType.None ? UnicodeNumericType.Numeric : UnicodeNumericType.None : unicodeCharacterData?.NumericType ?? UnicodeNumericType.None; } }
		public UnihanNumericType UnihanNumericType { get { return unihanCharacterData != null ? unihanCharacterData.NumericType : UnihanNumericType.None; } }
		[ValueName("Numeric_Value"), ValueName("nv")]
		public UnicodeRationalNumber? NumericValue { get { return unihanCharacterData != null && unihanCharacterData.NumericType != UnihanNumericType.None ? new UnicodeRationalNumber(unihanCharacterData.NumericValue, 1) : unicodeCharacterData?.NumericValue; } }
		[ValueName("Bidi_Mirrored")]
		public bool BidirectionalMirrored { get { return unicodeCharacterData?.BidirectionalMirrored ?? false; } }
		[ValueName("Unicode_1_Name"), ValueName("na1")]
		public string OldName { get { return unicodeCharacterData?.OldName; } }
		[ValueName("Simple_Uppercase_Mapping"), ValueName("suc")]
		public string SimpleUpperCaseMapping { get { return unicodeCharacterData?.SimpleUpperCaseMapping; } }
		[ValueName("Simple_Lowercase_Mapping"), ValueName("slc")]
		public string SimpleLowerCaseMapping { get { return unicodeCharacterData?.SimpleLowerCaseMapping; } }
		[ValueName("Simple_Titlecase_Mapping"), ValueName("stc")]
		public string SimpleTitleCaseMapping { get { return unicodeCharacterData?.SimpleTitleCaseMapping; } }
		public ContributoryProperties ContributoryProperties { get { return unicodeCharacterData?.ContributoryProperties ?? 0; } }
		public CoreProperties CoreProperties { get { return unicodeCharacterData?.CoreProperties ?? 0; } }
		public UnicodeCrossReferenceCollection CrossRerefences { get { return new UnicodeCrossReferenceCollection(unicodeCharacterData?.CrossRerefences); } }
		[ValueName("kRSUnicode"), ValueName("cjkRSUnicode"), ValueName("Unicode_Radical_Stroke"), ValueName("URS")]
		public UnicodeRadicalStrokeCountCollection UnicodeRadicalStrokeCounts { get { return new UnicodeRadicalStrokeCountCollection(unihanCharacterData?.UnicodeRadicalStrokeCounts); } }

		[ValueName("kDefinition")]
		public string Definition { get { return unihanCharacterData?.Definition; } }
		[ValueName("kMandarin")]
		public string MandarinReading { get { return unihanCharacterData?.MandarinReading; } }
		[ValueName("kCantonese")]
		public string CantoneseReading { get { return unihanCharacterData?.CantoneseReading; } }
		[ValueName("kJapaneseKun")]
		public string JapaneseKunReading { get { return unihanCharacterData?.JapaneseKunReading; } }
		[ValueName("kJapaneseOn")]
		public string JapaneseOnReading { get { return unihanCharacterData?.JapaneseOnReading; } }
		[ValueName("kKorean")]
		public string KoreanReading { get { return unihanCharacterData?.KoreanReading; } }
		[ValueName("kHangul")]
		public string HangulReading { get { return unihanCharacterData?.HangulReading; } }
		[ValueName("kVietnamese")]
		public string VietnameseReading { get { return unihanCharacterData?.VietnameseReading; } }

		[ValueName("kSimplifiedVariant")]
		public string SimplifiedVariant { get { return unihanCharacterData?.SimplifiedVariant; } }
		[ValueName("kTraditionalVariant")]
		public string TraditionalVariant { get { return unihanCharacterData?.TraditionalVariant; } }

		internal UnicodeCharInfo(int codePoint, UnicodeCharacterData unicodeCharacterData, UnihanCharacterData unihanCharacterData, string block)
		{
			this.codePoint = codePoint;
			this.name = UnicodeInfo.GetName(codePoint, unicodeCharacterData);
			this.unicodeCharacterData = unicodeCharacterData;
			this.unihanCharacterData = unihanCharacterData;
			this.block = block;
		}
	}
}
