using System.Globalization;

namespace System.Unicode
{
	internal struct UnicodeCharacterData
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
		private readonly UnicodeRationalNumber numericValue;
		public readonly bool BidirectionalMirrored;
		public readonly string OldName;
		public readonly string SimpleUpperCaseMapping;
		public readonly string SimpleLowerCaseMapping;
		public readonly string SimpleTitleCaseMapping;
		public readonly ContributoryProperties ContributoryProperties;
		private readonly int corePropertiesAndEmojiProperties;
		public CoreProperties CoreProperties => (CoreProperties)(corePropertiesAndEmojiProperties & 0x000FFFFF);
		public EmojiProperties EmojiProperties => (EmojiProperties)(corePropertiesAndEmojiProperties >> 20);

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
			this.CodePointRange = codePointRange;
			this.Name = name;
			this.NameAliases = nameAliases;
			this.Category = category;
			this.CanonicalCombiningClass = canonicalCombiningClass;
			this.BidirectionalClass = bidirectionalClass;
			this.DecompositionType = decompositionType;
			this.DecompositionMapping = decompositionMapping;
			this.NumericType = numericType;
			this.numericValue = numericValue;
			this.BidirectionalMirrored = bidirectionalMirrored;
			this.OldName = oldName;
			this.SimpleUpperCaseMapping = simpleUpperCaseMapping;
			this.SimpleLowerCaseMapping = simpleLowerCaseMapping;
			this.SimpleTitleCaseMapping = simpleTitleCaseMapping;
			this.ContributoryProperties = contributoryProperties;
			this.corePropertiesAndEmojiProperties = corePropertiesAndEmojiProperties;
			this.CrossRerefences = crossRerefences;
		}

		public UnicodeRationalNumber? NumericValue => NumericType != UnicodeNumericType.None ? numericValue : null as UnicodeRationalNumber?;
	}
}
