using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public sealed class UnicodeCharacterData
	{
		public readonly UnicodeCharacterRange CodePointRange;
		public readonly string Name;
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

		public readonly int[] RelatedCodePoints;

		internal UnicodeCharacterData
		(
			UnicodeCharacterRange codePointRange,
			string name,
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
            int[] relatedCodePoints
		)
		{
			this.CodePointRange = codePointRange;
			this.Name = name;
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
			this.RelatedCodePoints = relatedCodePoints;
        }

		public UnicodeRationalNumber? NumericValue { get { return NumericType != UnicodeNumericType.None ? numericValue : null as UnicodeRationalNumber?; } }
	}
}
