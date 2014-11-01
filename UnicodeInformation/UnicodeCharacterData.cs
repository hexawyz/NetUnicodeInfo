using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public sealed class UnicodeCharacterData
	{
		public readonly int CodePoint;
		public readonly string Name;
		public readonly UnicodeCategory Category;
		public readonly CanonicalCombiningClass CanonicalCombiningClass;
		public readonly string BidirectionalClass;
		public readonly string DecompositionType;
		public readonly UnicodeNumericType NumericType;
		public readonly UnicodeRationalNumber NumericValue;
		public readonly bool BidirectionalMirrored;
		public readonly string OldName;
		public readonly string SimpleUpperCaseMapping;
		public readonly string SimpleLowerCaseMapping;
		public readonly string SimpleTitleCaseMapping;
		public readonly ContributoryProperties ContributoryProperties;

		public readonly int[] RelatedCodePoints;

		internal UnicodeCharacterData
		(
			int codePoint,
			string name,
			UnicodeCategory category,
			CanonicalCombiningClass canonicalCombiningClass,
			string bidirectionalClass,
			string decompositionType,
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
			this.CodePoint = codePoint;
			this.Name = name;
			this.Category = category;
			this.CanonicalCombiningClass = canonicalCombiningClass;
			this.BidirectionalClass = bidirectionalClass;
			this.DecompositionType = decompositionType;
			this.NumericType = numericType;
			this.NumericValue = numericValue;
			this.BidirectionalMirrored = bidirectionalMirrored;
			this.OldName = oldName;
			this.SimpleUpperCaseMapping = simpleUpperCaseMapping;
			this.SimpleLowerCaseMapping = simpleLowerCaseMapping;
			this.SimpleTitleCaseMapping = simpleTitleCaseMapping;
			this.ContributoryProperties = contributoryProperties;
			this.RelatedCodePoints = relatedCodePoints;
        }
	}
}
