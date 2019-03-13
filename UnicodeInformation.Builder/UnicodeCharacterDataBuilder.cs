using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace System.Unicode.Builder
{
	public sealed class UnicodeCharacterDataBuilder
	{
		private UnicodeCategory category = UnicodeCategory.OtherNotAssigned;

		private readonly List<UnicodeNameAlias> nameAliases = new List<UnicodeNameAlias>();
		private readonly List<int> crossRerefences = new List<int>();

		public UnicodeCodePointRange CodePointRange { get; }

		public string Name { get; set; }

		public IList<UnicodeNameAlias> NameAliases { get { return nameAliases; } }

		public UnicodeCategory Category
		{
			get => category;
			set => category = Enum.IsDefined(typeof(UnicodeCategory), value) ?
				value :
				throw new ArgumentOutOfRangeException(nameof(value));
		}

		public CanonicalCombiningClass CanonicalCombiningClass { get; set; } // Even values not defined in the enum are allowed here.
		public BidirectionalClass BidirectionalClass { get; set; }
		public CharacterDecompositionMapping CharacterDecompositionMapping { get; set; }
		public UnicodeNumericType NumericType { get; set; }
		public UnicodeRationalNumber NumericValue { get; set; }
		public string OldName { get; set; }
		public bool BidirectionalMirrored { get; set; }
		public string SimpleUpperCaseMapping { get; set; }
		public string SimpleLowerCaseMapping { get; set; }
		public string SimpleTitleCaseMapping { get; set; }
		public ContributoryProperties ContributoryProperties { get; set; }
		public CoreProperties CoreProperties { get; set; }
		public EmojiProperties EmojiProperties { get; set; }
		public IList<int> CrossRerefences { get { return crossRerefences; } }

		public UnicodeCharacterDataBuilder(int codePoint)
			: this(new UnicodeCodePointRange(codePoint))
		{
		}

		public UnicodeCharacterDataBuilder(UnicodeCodePointRange codePointRange)
		{
			CodePointRange = codePointRange;
			this.category = UnicodeCategory.OtherNotAssigned;
		}

		internal UnicodeCharacterData ToCharacterData()
		{
			return new UnicodeCharacterData
			(
				CodePointRange,
				Name,
				nameAliases.Count > 0 ? nameAliases.ToArray() : UnicodeNameAlias.EmptyArray,
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
				(int)CoreProperties | (int)EmojiProperties << 20,
				CrossRerefences.Count > 0 ? CrossRerefences.ToArray() : null
			);
		}

		internal void WriteToFile(BinaryWriter writer)
		{
			if (nameAliases.Count > 64) throw new InvalidDataException("Cannot handle more than 64 name aliases.");

			UcdFields fields = default(UcdFields);

			if (!CodePointRange.IsSingleCodePoint) fields = UcdFields.CodePointRange;

			if (Name != null || nameAliases.Count > 0) fields |= UcdFields.Name; // This field combines name and alias.
			if (category != UnicodeCategory.OtherNotAssigned) fields |= UcdFields.Category;
			if (CanonicalCombiningClass != CanonicalCombiningClass.NotReordered) fields |= UcdFields.CanonicalCombiningClass;
			/*if (bidirectionalClass != 0)*/
			fields |= UcdFields.BidirectionalClass;
			if (CharacterDecompositionMapping.DecompositionMapping != null) fields |= UcdFields.DecompositionMapping;
			fields |= (UcdFields)((int)NumericType << 6);
			if (BidirectionalMirrored) fields |= UcdFields.BidirectionalMirrored;
			if (OldName != null) fields |= UcdFields.OldName;
			if (SimpleUpperCaseMapping != null) fields |= UcdFields.SimpleUpperCaseMapping;
			if (SimpleLowerCaseMapping != null) fields |= UcdFields.SimpleLowerCaseMapping;
			if (SimpleTitleCaseMapping != null) fields |= UcdFields.SimpleTitleCaseMapping;
			if (ContributoryProperties != 0) fields |= UcdFields.ContributoryProperties;
			if (CoreProperties != 0 || EmojiProperties != 0) fields |= UcdFields.CorePropertiesAndEmojiProperties;
			if (crossRerefences.Count > 0) fields |= UcdFields.CrossRerefences;

			writer.Write((ushort)fields);

			writer.WriteCodePoint(CodePointRange.FirstCodePoint);
			if ((fields & UcdFields.CodePointRange) != 0) writer.WriteCodePoint(CodePointRange.LastCodePoint);

			if ((fields & UcdFields.Name) != 0)
			{
				// We write the names by optimizing for the common case.
				// i.e. Most characters have only one name.
				// The first 8 bit sequence will encore either the length of the name property alone,
				// or the number of aliases and a bit indicating the presence of the name property.

				if (nameAliases.Count > 0)
				{
					writer.WritePackedLength((byte)(Name != null ? 3 : 2), nameAliases.Count);

					if (Name != null)
						writer.WriteNamePropertyToFile(Name);

					foreach (var nameAlias in nameAliases)
						writer.WriteNameAliasToFile(nameAlias);
				}
				else
				{
					writer.WriteNamePropertyToFile(Name);
				}
			}
			if ((fields & UcdFields.Category) != 0) writer.Write((byte)category);
			if ((fields & UcdFields.CanonicalCombiningClass) != 0) writer.Write((byte)CanonicalCombiningClass);
			if ((fields & UcdFields.BidirectionalClass) != 0) writer.Write((byte)BidirectionalClass);
			if ((fields & UcdFields.DecompositionMapping) != 0)
			{
				writer.Write((byte)CharacterDecompositionMapping.DecompositionType);
				writer.Write(CharacterDecompositionMapping.DecompositionMapping);
			}
			if ((fields & UcdFields.NumericNumeric) != 0)
			{
				writer.Write(NumericValue.Numerator);
				writer.WriteVariableUInt64(NumericValue.Denominator);
			}
			if ((fields & UcdFields.OldName) != 0) writer.Write(OldName);
			if ((fields & UcdFields.SimpleUpperCaseMapping) != 0) writer.Write(SimpleUpperCaseMapping);
			if ((fields & UcdFields.SimpleLowerCaseMapping) != 0) writer.Write(SimpleLowerCaseMapping);
			if ((fields & UcdFields.SimpleTitleCaseMapping) != 0) writer.Write(SimpleTitleCaseMapping);
			if ((fields & UcdFields.ContributoryProperties) != 0) writer.Write((int)ContributoryProperties);
			if ((fields & UcdFields.CorePropertiesAndEmojiProperties) != 0) writer.WriteUInt24((int)CoreProperties | (int)EmojiProperties << 20);
			if ((fields & UcdFields.CrossRerefences) != 0)
			{
				writer.Write(checked((byte)(crossRerefences.Count - 1)));
				foreach (int crossReference in crossRerefences)
					writer.WriteCodePoint(crossReference);
			}
		}
	}
}
