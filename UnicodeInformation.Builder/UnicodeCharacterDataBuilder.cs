using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public sealed class UnicodeCharacterDataBuilder
	{
		private readonly UnicodeCharacterRange codePointRange;
		private string name;
		private UnicodeCategory category = UnicodeCategory.OtherNotAssigned;
		private CanonicalCombiningClass canonicalCombiningClass;
		private BidirectionalClass bidirectionalClass;
		private string decompositionType;
		private UnicodeNumericType numericType;
		private UnicodeRationalNumber numericValue;
		private bool bidirectionalMirrored;
		private string oldName;
		private string simpleUpperCaseMapping;
		private string simpleLowerCaseMapping;
		private string simpleTitleCaseMapping;
		private ContributoryProperties contributoryProperties;

		private List<int> relatedCodePoints = new List<int>();

		public UnicodeCharacterRange CodePointRange { get { return codePointRange; } }

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public UnicodeCategory Category
		{
			get { return category; }
			set
			{
				if (!Enum.IsDefined(typeof(UnicodeCategory), value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				category = value;
			}
		}

		public CanonicalCombiningClass CanonicalCombiningClass
		{
			get { return canonicalCombiningClass; }
			set { canonicalCombiningClass = value; } // Even values not defined in the enum are allowed here.
		}

		public BidirectionalClass BidirectionalClass
		{
			get { return bidirectionalClass; }
			set { bidirectionalClass = value; }
		}

		public string DecompositionType
		{
			get { return decompositionType; }
			set { decompositionType = value; }
		}

		public UnicodeNumericType NumericType
		{
			get { return numericType; }
			set { numericType = value; }
		}

		public UnicodeRationalNumber NumericValue
		{
			get { return numericValue; }
			set { numericValue = value; }
		}

		public string OldName
		{
			get { return oldName; }
			set { oldName = value; }
		}

		public bool BidirectionalMirrored
		{
			get { return bidirectionalMirrored; }
			set { bidirectionalMirrored = value; }
		}

		public string SimpleUpperCaseMapping
		{
			get { return simpleUpperCaseMapping; }
			set { simpleUpperCaseMapping = value; }
		}

		public string SimpleLowerCaseMapping
		{
			get { return simpleLowerCaseMapping; }
			set { simpleLowerCaseMapping = value; }
		}

		public string SimpleTitleCaseMapping
		{
			get { return simpleTitleCaseMapping; }
			set { simpleTitleCaseMapping = value; }
		}

		public ContributoryProperties ContributoryProperties
		{
			get { return contributoryProperties; }
			set { contributoryProperties = value; }
		}

		public ICollection<int> RelatedCodePoints { get { return relatedCodePoints; } }

		public UnicodeCharacterDataBuilder(int codePoint)
			: this(new UnicodeCharacterRange(codePoint))
		{
		}

		public UnicodeCharacterDataBuilder(UnicodeCharacterRange codePointRange)
		{
			this.codePointRange = codePointRange;
			this.category = UnicodeCategory.OtherNotAssigned;
		}

		public UnicodeCharacterData ToCharacterData()
		{
			return new UnicodeCharacterData
			(
				codePointRange,
				Name,
				category,
				canonicalCombiningClass,
				bidirectionalClass,
				decompositionType,
				numericType,
				numericValue,
				bidirectionalMirrored,
				oldName,
				simpleUpperCaseMapping,
				simpleLowerCaseMapping,
				simpleTitleCaseMapping,
				contributoryProperties,
				relatedCodePoints.Count > 0 ? relatedCodePoints.ToArray() : null
			);
		}
	}
}
