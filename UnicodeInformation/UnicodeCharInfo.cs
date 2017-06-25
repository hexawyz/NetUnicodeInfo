using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Provides information on a specific unicode code point.</summary>
	public struct UnicodeCharInfo
	{
		/// <summary>The code point.</summary>
		private readonly int codePoint;
		/// <summary>The name of the code point.</summary>
		private readonly string name;
		internal readonly int unicodeCharacterDataIndex;
		private readonly int unihanCharacterDataIndex;
		private readonly string block;

		private ref UnicodeCharacterData UnicodeCharacterData => ref UnicodeInfo.GetUnicodeCharacterData(unicodeCharacterDataIndex);
		private ref UnihanCharacterData UnihanCharacterData => ref UnicodeInfo.GetUnihanCharacterData(unihanCharacterDataIndex);

		/// <summary>Gets the code point as an UTF-32 value.</summary>
		public int CodePoint => codePoint;

		/// <summary>Gets the code point name.</summary>
		/// <remarks>This is the Name Unicode property.</remarks>
		[ValueName("Name"), ValueName("na")]
		public string Name => name;
		/// <summary>Gets the name aliases defined for the code point.</summary>
		/// <remarks>This is the Name_Alias Unicode property.</remarks>
		[ValueName("Name_Alias")]
		public UnicodeNameAliasCollection NameAliases => new UnicodeNameAliasCollection(UnicodeCharacterData.NameAliases);

		/// <summary>Gets the category defined for the code point.</summary>
		/// <remarks>This is the General_Category Unicode property.</remarks>
		[ValueName("General_Category"), ValueName("gc")]
		public UnicodeCategory Category => UnicodeCharacterData.Category;
		/// <summary>Gets the name of the block where the code point is located.</summary>
		/// <remarks>This is the Block Unicode property.</remarks>
		[ValueName("Block"), ValueName("blk")]
		public string Block => block ?? UnicodeInfo.DefaultBlock;
		/// <summary>Gets the canonical combining class defined for the code point.</summary>
		/// <remarks>This is the Canonical_Combining_Class Unicode property.</remarks>
		[ValueName("Canonical_Combining_Class"), ValueName("ccc")]
		public CanonicalCombiningClass CanonicalCombiningClass => UnicodeCharacterData.CanonicalCombiningClass;
		/// <summary>Gets the bidirectional class defined for the code point.</summary>
		/// <remarks>This is the Bidi_Class Unicode property.</remarks>
		[ValueName("Bidi_Class"), ValueName("bc")]
		public BidirectionalClass BidirectionalClass => UnicodeCharacterData.BidirectionalClass;
		/// <summary>Gets the decomposition type defined for the code point.</summary>
		/// <remarks>This is the Decomposition_Type Unicode property.</remarks>
		[ValueName("Decomposition_Type"), ValueName("dt")]
		public CompatibilityFormattingTag DecompositionType => UnicodeCharacterData.DecompositionType;
		/// <summary>Gets the decomposition mapping defined for the code point.</summary>
		/// <remarks>This is the Decomposition_Mapping Unicode property.</remarks>
		[ValueName("Decomposition_Mapping"), ValueName("dm")]
		public string DecompositionMapping => UnicodeCharacterData.DecompositionMapping;
		/// <summary>Gets the numeric type defined for the code point.</summary>
		/// <remarks>
		/// This is the Numeric_Type Unicode property.
		/// When this value is defined to something other than <see cref="UnicodeNumericType.None"/>, the <see cref="NumericValue"/> indicates the numeric value of the code point.
		/// The value of this property may be influenced by Unihan data, which will set it to <see cref="UnicodeNumericType.Numeric"/>.
		/// In this case, the property <see cref="UnihanNumericType"/> will indicate the origin of the numeric value in Unihan data.
		/// </remarks>
		[ValueName("Numeric_Type"), ValueName("nt")]
		public UnicodeNumericType NumericType => unihanCharacterDataIndex >= 0 ? UnihanCharacterData.NumericType != UnihanNumericType.None ? UnicodeNumericType.Numeric : UnicodeNumericType.None : UnicodeCharacterData.NumericType;
		/// <summary>Gets the Unihan numeric type defined for the code point.</summary>
		/// <remarks>The value of this property indicates which of the kPrimaryNumeric, kAccountingNumeric, or kOtherNumeric Unihan property is set, if any.</remarks>
		public UnihanNumericType UnihanNumericType => unihanCharacterDataIndex >= 0 ? UnihanCharacterData.NumericType : UnihanNumericType.None;
		/// <summary>Gets the numeric value defined for the code point.</summary>
		/// <remarks>
		/// This is the Numeric_Value Unicode property.
		/// This value may come from Unihan data.
		/// When this property set, the <see cref="NumericType"/> and <see cref="UnihanNumericType"/> indicates the nature of the numeric value.
		/// </remarks>
		[ValueName("Numeric_Value"), ValueName("nv")]
		public UnicodeRationalNumber? NumericValue
		{
			get
			{
				if (unihanCharacterDataIndex >= 0)
				{
					ref var unihanCharacterData = ref UnihanCharacterData;
					if (unihanCharacterData.NumericType != UnihanNumericType.None)
					{
						return new UnicodeRationalNumber(unihanCharacterData.NumericValue, 1);
					}
				}

				return UnicodeCharacterData.NumericValue;
			}
		}

		/// <summary>Gets a value indicating whether the character is mirrored in bidirectional text.</summary>
		/// <remarks>This is the Bidi_Mirrored Unicode property.</remarks>
		[ValueName("Bidi_Mirrored")]
		public bool BidirectionalMirrored => UnicodeCharacterData.BidirectionalMirrored;
		/// <summary>Gets the Unicode 1 name of the code point.</summary>
		/// <remarks>This is the Unicode_1_Name Unicode property.</remarks>
		[ValueName("Unicode_1_Name"), ValueName("na1")]
		public string OldName => UnicodeCharacterData.OldName;
		/// <summary>Gets the simple uppercase mapping defined for the code point.</summary>
		/// <remarks>This is the Simple_Uppercase_Mapping Unicode property.</remarks>
		[ValueName("Simple_Uppercase_Mapping"), ValueName("suc")]
		public string SimpleUpperCaseMapping => UnicodeCharacterData.SimpleUpperCaseMapping;
		/// <summary>Gets the simple lowercase mapping defined for the code point.</summary>
		/// <remarks>This is the Simple_Lowercase_Mapping Unicode property.</remarks>
		[ValueName("Simple_Lowercase_Mapping"), ValueName("slc")]
		public string SimpleLowerCaseMapping => UnicodeCharacterData.SimpleLowerCaseMapping;
		/// <summary>Gets the simple titlecase mapping defined for the code point.</summary>
		/// <remarks>This is the Simple_Titlecase_Mapping Unicode property.</remarks>
		[ValueName("Simple_Titlecase_Mapping"), ValueName("stc")]
		public string SimpleTitleCaseMapping => UnicodeCharacterData.SimpleTitleCaseMapping;
		/// <summary>Gets a value indicating which of the boolean contributory properties are defined for the code point.</summary>
		/// <remarks>
		/// The Unicode standard indicates contributory properties as neither normative nor informational.
		/// However, contributory properties are used by Unicode to define the code properties.
		/// The corresponding core properties may be accessed from the <see cref="CoreProperties"/> member.
		/// </remarks>
		public ContributoryProperties ContributoryProperties => UnicodeCharacterData.ContributoryProperties;
		/// <summary>Gets a value indicating which of the boolean core properties are defined for the code point.</summary>
		/// <remarks>The core properties are computed by combining various character information together with contributory properties.</remarks>
		public CoreProperties CoreProperties => UnicodeCharacterData.CoreProperties;
		/// <summary>Gets a value indicating which of the boolean emoji properties are defined for the code point.</summary>
		/// <remarks>The emoji properties are provided by the Unicode Emoji standard, which is not directly part of UCD.</remarks>
		public EmojiProperties EmojiProperties => UnicodeCharacterData.EmojiProperties;
		/// <summary>Gets a collection of cross references associated with the code point.</summary>
		/// <remarks>The cross references have been extracted from Unicode data but are not normative.</remarks>
		public UnicodeCrossReferenceCollection CrossRerefences => new UnicodeCrossReferenceCollection(UnicodeCharacterData.CrossRerefences);
		/// <summary>Gets the radical and stroke count for the code point.</summary>
		/// <remarks>
		/// This is the Unicode_Radical_Stroke Unicode property, defined as kRSUnicode in Unihan data.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kRSUnicode"), ValueName("cjkRSUnicode"), ValueName("Unicode_Radical_Stroke"), ValueName("URS")]
		public UnicodeRadicalStrokeCountCollection UnicodeRadicalStrokeCounts => new UnicodeRadicalStrokeCountCollection(UnihanCharacterData.UnicodeRadicalStrokeCounts);

		/// <summary>Gets the definition of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kDefinition Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kDefinition")]
		public string Definition => UnihanCharacterData.Definition;
		/// <summary>Gets the Mandarin reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kMandarin Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kMandarin")]
		public string MandarinReading => UnihanCharacterData.MandarinReading;
		/// <summary>Gets the Cantonese reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kCantonese Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kCantonese")]
		public string CantoneseReading => UnihanCharacterData.CantoneseReading;
		/// <summary>Gets the Japanese Kun reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kJapaneseKun Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kJapaneseKun")]
		public string JapaneseKunReading => UnihanCharacterData.JapaneseKunReading;
		/// <summary>Gets the Japanese On reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kJapaneseOn Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kJapaneseOn")]
		public string JapaneseOnReading => UnihanCharacterData.JapaneseOnReading;
		/// <summary>Gets the Korean reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kKorean Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kKorean")]
		public string KoreanReading => UnihanCharacterData.KoreanReading;
		/// <summary>Gets the Hangul reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kHangul Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kHangul")]
		public string HangulReading => UnihanCharacterData.HangulReading;
		/// <summary>Gets the Vietnamese reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kVietnamese Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kVietnamese")]
		public string VietnameseReading => UnihanCharacterData.VietnameseReading;

		/// <summary>Gets the simplified variant of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kSimplifiedVariant Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kSimplifiedVariant")]
		public string SimplifiedVariant => UnihanCharacterData.SimplifiedVariant;
		/// <summary>Gets the traditional variant of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kTraditionalVariant Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kTraditionalVariant")]
		public string TraditionalVariant => UnihanCharacterData.TraditionalVariant;

		internal UnicodeCharInfo(int codePoint, int unicodeCharacterDataIndex, int unihanCharacterDataIndex, string block)
		{
			this.codePoint = codePoint;
			this.name = unicodeCharacterDataIndex >= 0 ? UnicodeInfo.GetName(codePoint, ref UnicodeInfo.GetUnicodeCharacterData(unicodeCharacterDataIndex)) : null;
			this.unicodeCharacterDataIndex = unicodeCharacterDataIndex;
			this.unihanCharacterDataIndex = unihanCharacterDataIndex;
			this.block = block;
		}
	}
}
