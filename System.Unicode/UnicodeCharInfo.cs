using System.Diagnostics;
using System.Globalization;

namespace System.Unicode
{
	/// <summary>Provides information on a specific unicode code point.</summary>
	[DebuggerDisplay(@"{CodePoint.ToString(""X4""),nq} - {Name,nq}")]
	public readonly struct UnicodeCharInfo
	{
		/// <summary>Gets the code point as an UTF-32 value.</summary>
		public int CodePoint { get; }

		/// <summary>Gets the code point name.</summary>
		/// <remarks>This is the Name Unicode property.</remarks>
		[ValueName("Name"), ValueName("na")]
		public string Name { get; }

		private readonly UnicodeCharacterData _unicodeCharacterData;
		private readonly UnihanCharacterData _unihanCharacterData;
		private readonly int _blockIndex;

		/// <summary>Gets the name aliases defined for the code point.</summary>
		/// <remarks>This is the Name_Alias Unicode property.</remarks>
		[ValueName("Name_Alias")]
		public UnicodeNameAliasCollection NameAliases => _unicodeCharacterData.NameAliases;

		/// <summary>Gets the category defined for the code point.</summary>
		/// <remarks>This is the General_Category Unicode property.</remarks>
		[ValueName("General_Category"), ValueName("gc")]
		public UnicodeCategory Category => _unicodeCharacterData.Category;

		/// <summary>Gets the name of the block where the code point is located.</summary>
		/// <remarks>This is the Block Unicode property.</remarks>
		[ValueName("Block"), ValueName("blk")]
		public UnicodeBlock Block => ;

		/// <summary>Gets the canonical combining class defined for the code point.</summary>
		/// <remarks>This is the Canonical_Combining_Class Unicode property.</remarks>
		[ValueName("Canonical_Combining_Class"), ValueName("ccc")]
		public CanonicalCombiningClass CanonicalCombiningClass => _unicodeCharacterData.CanonicalCombiningClass;

		/// <summary>Gets the bidirectional class defined for the code point.</summary>
		/// <remarks>This is the Bidi_Class Unicode property.</remarks>
		[ValueName("Bidi_Class"), ValueName("bc")]
		public BidirectionalClass BidirectionalClass => _unicodeCharacterData.BidirectionalClass;

		/// <summary>Gets the decomposition type defined for the code point.</summary>
		/// <remarks>This is the Decomposition_Type Unicode property.</remarks>
		[ValueName("Decomposition_Type"), ValueName("dt")]
		public CompatibilityFormattingTag DecompositionType => _unicodeCharacterData.DecompositionType;

		/// <summary>Gets the decomposition mapping defined for the code point.</summary>
		/// <remarks>This is the Decomposition_Mapping Unicode property.</remarks>
		[ValueName("Decomposition_Mapping"), ValueName("dm")]
		public UnicodeDataString DecompositionMapping => _unicodeCharacterData.DecompositionMapping;

		/// <summary>Gets the numeric type defined for the code point.</summary>
		/// <remarks>
		/// This is the Numeric_Type Unicode property.
		/// When this value is defined to something other than <see cref="UnicodeNumericType.None"/>, the <see cref="NumericValue"/> indicates the numeric value of the code point.
		/// The value of this property may be influenced by Unihan data, which will set it to <see cref="UnicodeNumericType.Numeric"/>.
		/// In this case, the property <see cref="UnihanNumericType"/> will indicate the origin of the numeric value in Unihan data.
		/// </remarks>
		[ValueName("Numeric_Type"), ValueName("nt")]
		public UnicodeNumericType NumericType => _unihanCharacterDataIndex >= 0 ? _unihanCharacterData.NumericType != UnihanNumericType.None ? UnicodeNumericType.Numeric : UnicodeNumericType.None : _unicodeCharacterData.NumericType;

		/// <summary>Gets the Unihan numeric type defined for the code point.</summary>
		/// <remarks>The value of this property indicates which of the kPrimaryNumeric, kAccountingNumeric, or kOtherNumeric Unihan property is set, if any.</remarks>
		public UnihanNumericType UnihanNumericType => _unihanCharacterDataIndex >= 0 ? _unihanCharacterData.NumericType : UnihanNumericType.None;

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
				if (_unihanCharacterDataIndex >= 0)
				{
					ref readonly var unihanCharacterData = ref UnihanCharacterData;
					if (_unihanCharacterData.NumericType != UnihanNumericType.None)
					{
						return new UnicodeRationalNumber(_unihanCharacterData.NumericValue, 1);
					}
				}

				return _unicodeCharacterData.NumericValue;
			}
		}

		/// <summary>Gets a value indicating whether the character is mirrored in bidirectional text.</summary>
		/// <remarks>This is the Bidi_Mirrored Unicode property.</remarks>
		[ValueName("Bidi_Mirrored")]
		public bool BidirectionalMirrored => _unicodeCharacterData.BidirectionalMirrored;

		/// <summary>Gets the Unicode 1 name of the code point.</summary>
		/// <remarks>This is the Unicode_1_Name Unicode property.</remarks>
		[ValueName("Unicode_1_Name"), ValueName("na1")]
		public UnicodeDataString OldName => _unicodeCharacterData.OldName;

		/// <summary>Gets the simple uppercase mapping defined for the code point.</summary>
		/// <remarks>This is the Simple_Uppercase_Mapping Unicode property.</remarks>
		[ValueName("Simple_Uppercase_Mapping"), ValueName("suc")]
		public UnicodeDataString SimpleUpperCaseMapping => _unicodeCharacterData.SimpleUpperCaseMapping;

		/// <summary>Gets the simple lowercase mapping defined for the code point.</summary>
		/// <remarks>This is the Simple_Lowercase_Mapping Unicode property.</remarks>
		[ValueName("Simple_Lowercase_Mapping"), ValueName("slc")]
		public UnicodeDataString SimpleLowerCaseMapping => _unicodeCharacterData.SimpleLowerCaseMapping;

		/// <summary>Gets the simple titlecase mapping defined for the code point.</summary>
		/// <remarks>This is the Simple_Titlecase_Mapping Unicode property.</remarks>
		[ValueName("Simple_Titlecase_Mapping"), ValueName("stc")]
		public UnicodeDataString SimpleTitleCaseMapping => _unicodeCharacterData.SimpleTitleCaseMapping;

		/// <summary>Gets a value indicating which of the boolean contributory properties are defined for the code point.</summary>
		/// <remarks>
		/// The Unicode standard indicates contributory properties as neither normative nor informational.
		/// However, contributory properties are used by Unicode to define the code properties.
		/// The corresponding core properties may be accessed from the <see cref="CoreProperties"/> member.
		/// </remarks>
		public ContributoryProperties ContributoryProperties => _unicodeCharacterData.ContributoryProperties;

		/// <summary>Gets a value indicating which of the boolean core properties are defined for the code point.</summary>
		/// <remarks>The core properties are computed by combining various character information together with contributory properties.</remarks>
		public CoreProperties CoreProperties => _unicodeCharacterData.CoreProperties;

		/// <summary>Gets a value indicating which of the boolean emoji properties are defined for the code point.</summary>
		/// <remarks>The emoji properties are provided by the Unicode Emoji standard, which is not directly part of UCD.</remarks>
		public EmojiProperties EmojiProperties => _unicodeCharacterData.EmojiProperties;

		/// <summary>Gets a collection of cross references associated with the code point.</summary>
		/// <remarks>The cross references have been extracted from Unicode data but are not normative.</remarks>
		public UnicodeCrossReferenceCollection CrossRerefences => new UnicodeCrossReferenceCollection(_unicodeCharacterData.CrossRerefences);

		/// <summary>Gets the radical and stroke count for the code point.</summary>
		/// <remarks>
		/// This is the Unicode_Radical_Stroke Unicode property, defined as kRSUnicode in Unihan data.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kRSUnicode"), ValueName("cjkRSUnicode"), ValueName("Unicode_Radical_Stroke"), ValueName("URS")]
		public UnicodeRadicalStrokeCountCollection UnicodeRadicalStrokeCounts => new UnicodeRadicalStrokeCountCollection(_unihanCharacterData.UnicodeRadicalStrokeCounts);

		/// <summary>Gets the definition of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kDefinition Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kDefinition")]
		public string Definition => _unihanCharacterData.Definition;

		/// <summary>Gets the Mandarin reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kMandarin Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kMandarin")]
		public string MandarinReading => _unihanCharacterData.MandarinReading;

		/// <summary>Gets the Cantonese reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kCantonese Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kCantonese")]
		public string CantoneseReading => _unihanCharacterData.CantoneseReading;

		/// <summary>Gets the Japanese Kun reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kJapaneseKun Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kJapaneseKun")]
		public string JapaneseKunReading => _unihanCharacterData.JapaneseKunReading;

		/// <summary>Gets the Japanese On reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kJapaneseOn Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kJapaneseOn")]
		public string JapaneseOnReading => _unihanCharacterData.JapaneseOnReading;

		/// <summary>Gets the Korean reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kKorean Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kKorean")]
		public string KoreanReading => _unihanCharacterData.KoreanReading;

		/// <summary>Gets the Hangul reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kHangul Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kHangul")]
		public string HangulReading => _unihanCharacterData.HangulReading;

		/// <summary>Gets the Vietnamese reading of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kVietnamese Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kVietnamese")]
		public string VietnameseReading => _unihanCharacterData.VietnameseReading;


		/// <summary>Gets the simplified variant of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kSimplifiedVariant Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kSimplifiedVariant")]
		public string SimplifiedVariant => _unihanCharacterData.SimplifiedVariant;

		/// <summary>Gets the traditional variant of the character from the Unihan data.</summary>
		/// <remarks>
		/// This is the kTraditionalVariant Unicode property.
		/// This property is only ever useful when the character is a CJK ideograph.
		/// </remarks>
		[ValueName("kTraditionalVariant")]
		public string TraditionalVariant => _unihanCharacterData.TraditionalVariant;

		internal UnicodeCharInfo(int codePoint, UnicodeCharacterData unicodeCharacterData, UnihanCharacterData unihanCharacterData, string block)
		{
			CodePoint = codePoint;
			Name = unicodeCharacterDataIndex >= 0 ? UnicodeInfo.GetName(codePoint, unicodeCharacterData) : null;
			unicodeCharacterData = unicodeCharacterData;
			unihanCharacterData = unihanCharacterData;
			_block = block;
		}
	}
}
