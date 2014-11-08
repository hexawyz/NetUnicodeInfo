using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
		private CharacterDecompositionMapping characterDecompositionMapping;
		private UnicodeNumericType numericType;
		private UnicodeRationalNumber numericValue;
		private bool bidirectionalMirrored;
		private string oldName;
		private string simpleUpperCaseMapping;
		private string simpleLowerCaseMapping;
		private string simpleTitleCaseMapping;
		private ContributoryProperties contributoryProperties;
		private CoreProperties coreProperties;

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

		public CharacterDecompositionMapping CharacterDecompositionMapping
		{
			get { return characterDecompositionMapping; }
			set { characterDecompositionMapping = value; }
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

		public CoreProperties CoreProperties
		{
			get { return coreProperties; }
			set { coreProperties = value; }
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

		internal UnicodeCharacterData ToCharacterData()
		{
			return new UnicodeCharacterData
			(
				codePointRange,
				Name,
				category,
				canonicalCombiningClass,
				bidirectionalClass,
				characterDecompositionMapping.DecompositionType,
				characterDecompositionMapping.DecompositionMapping,
				numericType,
				numericValue,
				bidirectionalMirrored,
				oldName,
				simpleUpperCaseMapping,
				simpleLowerCaseMapping,
				simpleTitleCaseMapping,
				contributoryProperties,
				coreProperties,
				relatedCodePoints.Count > 0 ? relatedCodePoints.ToArray() : null
			);
		}

		internal void WriteToFile(BinaryWriter writer)
		{
			UcdFields fields = default(UcdFields);

			if (!codePointRange.IsSingleCodePoint) fields = UcdFields.CodePointRange;

			if (name != null) fields |= UcdFields.Name;
			if (category != UnicodeCategory.OtherNotAssigned) fields |= UcdFields.Category;
			if (canonicalCombiningClass != CanonicalCombiningClass.NotReordered) fields |= UcdFields.CanonicalCombiningClass;
			/*if (bidirectionalClass != 0)*/fields |= UcdFields.BidirectionalClass;
			if (characterDecompositionMapping.DecompositionMapping != null) fields |= UcdFields.DecompositionMapping;
			fields |= (UcdFields)((int)numericType << 6);
			if (bidirectionalMirrored) fields |= UcdFields.BidirectionalMirrored;
			if (oldName != null) fields |= UcdFields.OldName;
			if (simpleUpperCaseMapping != null) fields |= UcdFields.SimpleUpperCaseMapping;
			if (simpleLowerCaseMapping != null) fields |= UcdFields.SimpleLowerCaseMapping;
			if (simpleTitleCaseMapping != null) fields |= UcdFields.SimpleTitleCaseMapping;
			if (contributoryProperties != 0) fields |= UcdFields.ContributoryProperties;
			if (coreProperties != 0) fields |= UcdFields.CoreProperties;

			writer.Write((ushort)fields);

			writer.WriteCodePoint(codePointRange.FirstCodePoint);
			if ((fields & UcdFields.CodePointRange) != 0) writer.WriteCodePoint(CodePointRange.LastCodePoint);

			if ((fields & UcdFields.Name) != 0) writer.Write(name);
			if ((fields & UcdFields.Category) != 0) writer.Write((byte)category);
			if ((fields & UcdFields.CanonicalCombiningClass) != 0) writer.Write((byte)canonicalCombiningClass);
			if ((fields & UcdFields.BidirectionalClass) != 0) writer.Write((byte)bidirectionalClass);
			if ((fields & UcdFields.DecompositionMapping) != 0)
			{
				writer.Write((byte)characterDecompositionMapping.DecompositionType);
				writer.Write(characterDecompositionMapping.DecompositionMapping);
			}
			if ((fields & UcdFields.NumericNumeric) != 0)
			{
				writer.Write(numericValue.Numerator);
				writer.Write(numericValue.Denominator);
			}
			if ((fields & UcdFields.OldName) != 0) writer.Write(oldName);
			if ((fields & UcdFields.SimpleUpperCaseMapping) != 0) writer.Write(simpleUpperCaseMapping);
			if ((fields & UcdFields.SimpleLowerCaseMapping) != 0) writer.Write(simpleLowerCaseMapping);
			if ((fields & UcdFields.SimpleTitleCaseMapping) != 0) writer.Write(simpleTitleCaseMapping);
			if ((fields & UcdFields.ContributoryProperties) != 0) writer.Write((int)contributoryProperties);
			if ((fields & UcdFields.CoreProperties) != 0) writer.WriteUInt24((int)coreProperties);
		}
	}
}
