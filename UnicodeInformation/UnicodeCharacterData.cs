using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	internal sealed class UnicodeCharacterData
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
		public readonly CoreProperties CoreProperties;

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
			CoreProperties coreProperties,
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
			this.CoreProperties = coreProperties;
			this.CrossRerefences = crossRerefences;
		}

		public UnicodeRationalNumber? NumericValue { get { return NumericType != UnicodeNumericType.None ? numericValue : null as UnicodeRationalNumber?; } }
	}
}
