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
		private readonly UnicodeCodePointRange codePointRange;
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

		private readonly List<UnicodeNameAlias> nameAliases = new List<UnicodeNameAlias>();
		private readonly List<int> crossRerefences = new List<int>();

		public UnicodeCodePointRange CodePointRange { get { return codePointRange; } }

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public IList<UnicodeNameAlias> NameAliases { get { return nameAliases; } }

		public UnicodeCategory Category
		{
			get { return category; }
			set
			{
				if (!Enum.IsDefined(typeof(UnicodeCategory), value))
				{
					throw new ArgumentOutOfRangeException(nameof(value));
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

		public IList<int> CrossRerefences { get { return crossRerefences; } }

		public UnicodeCharacterDataBuilder(int codePoint)
			: this(new UnicodeCodePointRange(codePoint))
		{
		}

		public UnicodeCharacterDataBuilder(UnicodeCodePointRange codePointRange)
		{
			this.codePointRange = codePointRange;
			this.category = UnicodeCategory.OtherNotAssigned;
		}

		internal UnicodeCharacterData ToCharacterData()
		{
			return new UnicodeCharacterData
			(
				CodePointRange,
				Name,
				NameAliases.Count > 0 ? NameAliases.ToArray() : UnicodeNameAlias.EmptyArray,
				Category,
				CanonicalCombiningClass,
				BidirectionalClass,
				CharacterDecompositionMapping.DecompositionType,
				CharacterDecompositionMapping.DecompositionMapping,
				NumericType,
				NumericValue,
				BidirectionalMirrored,
				OldName,
				SimpleUpperCaseMapping,
				SimpleLowerCaseMapping,
				SimpleTitleCaseMapping,
				ContributoryProperties,
				CoreProperties,
				CrossRerefences.Count > 0 ? CrossRerefences.ToArray() : null
			);
		}

		internal void WriteToFile(BinaryWriter writer)
		{
			if (nameAliases.Count > 64) throw new InvalidDataException("Cannot handle more than 64 name aliases.");

			UcdFields fields = default(UcdFields);

			if (!codePointRange.IsSingleCodePoint) fields = UcdFields.CodePointRange;

			if (name != null || nameAliases.Count > 0) fields |= UcdFields.Name; // This field combines name and alias.
			if (category != UnicodeCategory.OtherNotAssigned) fields |= UcdFields.Category;
			if (canonicalCombiningClass != CanonicalCombiningClass.NotReordered) fields |= UcdFields.CanonicalCombiningClass;
			/*if (bidirectionalClass != 0)*/
			fields |= UcdFields.BidirectionalClass;
			if (characterDecompositionMapping.DecompositionMapping != null) fields |= UcdFields.DecompositionMapping;
			fields |= (UcdFields)((int)numericType << 6);
			if (bidirectionalMirrored) fields |= UcdFields.BidirectionalMirrored;
			if (oldName != null) fields |= UcdFields.OldName;
			if (simpleUpperCaseMapping != null) fields |= UcdFields.SimpleUpperCaseMapping;
			if (simpleLowerCaseMapping != null) fields |= UcdFields.SimpleLowerCaseMapping;
			if (simpleTitleCaseMapping != null) fields |= UcdFields.SimpleTitleCaseMapping;
			if (contributoryProperties != 0) fields |= UcdFields.ContributoryProperties;
			if (coreProperties != 0) fields |= UcdFields.CoreProperties;
			if (crossRerefences.Count > 0) fields |= UcdFields.CrossRerefences;

			writer.Write((ushort)fields);

			writer.WriteCodePoint(codePointRange.FirstCodePoint);
			if ((fields & UcdFields.CodePointRange) != 0) writer.WriteCodePoint(CodePointRange.LastCodePoint);

			if ((fields & UcdFields.Name) != 0)
			{
				// We write the names by optimizing for the common case.
				// i.e. Most characters have only one name.
				// The first 8 bit sequence will encore either the length of the name property alone,
				// or the number of aliases and a bit indicating the presence of the name property.

				if (nameAliases.Count > 0)
				{
					writer.WritePackedLength((byte)(name != null ? 3 : 2), nameAliases.Count);

					if (name != null)
						writer.WriteNamePropertyToFile(name);

					foreach (var nameAlias in nameAliases)
						writer.WriteNameAliasToFile(nameAlias);
				}
				else
				{
					writer.WriteNamePropertyToFile(name);
				}
			}
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
			if ((fields & UcdFields.CrossRerefences) != 0)
			{
				writer.Write(checked((byte)(crossRerefences.Count - 1)));
				foreach (int crossReference in crossRerefences)
					writer.WriteCodePoint(crossReference);
			}
		}
	}
}
