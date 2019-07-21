using System.Globalization;

namespace System.Unicode
{
	internal readonly struct UnicodeCharacterData
	{
		public readonly UnicodeCodePointRange CodePointRange;
		public readonly string Name;
		public readonly UnicodeNameAlias[] NameAliases;
		public readonly UnicodeCategory Category;
		public readonly CanonicalCombiningClass CanonicalCombiningClass;
		public readonly BidirectionalClass BidirectionalClass;
		public readonly CompatibilityFormattingTag DecompositionType;
		public readonly string DecompositionMapping;
		public readonly UnicodeNumericType NumericType;
		private readonly UnicodeRationalNumber _numericValue;
		public readonly bool BidirectionalMirrored;
		public readonly string OldName;
		public readonly string SimpleUpperCaseMapping;
		public readonly string SimpleLowerCaseMapping;
		public readonly string SimpleTitleCaseMapping;
		public readonly ContributoryProperties ContributoryProperties;
		private readonly int _corePropertiesAndEmojiProperties;
		public CoreProperties CoreProperties => (CoreProperties)(_corePropertiesAndEmojiProperties & 0x003FFFFF);
		public EmojiProperties EmojiProperties => (EmojiProperties)(_corePropertiesAndEmojiProperties >> 24);

		public readonly int[] CrossRerefences; // NB: It seems that parsing NamesList is required in order to provide data for this field ?

		internal UnicodeCharacterData
		(
			UnicodeCodePointRange codePointRange,
			string name,
			UnicodeNameAlias[] nameAliases,
			UnicodeCategory category,
			CanonicalCombiningClass canonicalCombiningClass,
			BidirectionalClass bidirectionalClass,
			CompatibilityFormattingTag decompositionType,
			string decompositionMapping,
			UnicodeNumericType numericType,
			UnicodeRationalNumber numericValue,
			bool bidirectionalMirrored,
			string oldName,
			string simpleUpperCaseMapping,
			string simpleLowerCaseMapping,
			string simpleTitleCaseMapping,
			ContributoryProperties contributoryProperties,
			int corePropertiesAndEmojiProperties,
			int[] crossRerefences
		)
		{
			CodePointRange = codePointRange;
			Name = name;
			NameAliases = nameAliases;
			Category = category;
			CanonicalCombiningClass = canonicalCombiningClass;
			BidirectionalClass = bidirectionalClass;
			DecompositionType = decompositionType;
			DecompositionMapping = decompositionMapping;
			NumericType = numericType;
			_numericValue = numericValue;
			BidirectionalMirrored = bidirectionalMirrored;
			OldName = oldName;
			SimpleUpperCaseMapping = simpleUpperCaseMapping;
			SimpleLowerCaseMapping = simpleLowerCaseMapping;
			SimpleTitleCaseMapping = simpleTitleCaseMapping;
			ContributoryProperties = contributoryProperties;
			_corePropertiesAndEmojiProperties = corePropertiesAndEmojiProperties;
			CrossRerefences = crossRerefences;
		}

		public UnicodeRationalNumber? NumericValue => NumericType != UnicodeNumericType.None ? _numericValue : null as UnicodeRationalNumber?;
	}
}
