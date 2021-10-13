using System.Collections.Generic;
using System.Globalization;

namespace System.Unicode
{
	/// <summary>Provides complementary information on <see cref="UnicodeCategory"/> values.</summary>
	public readonly struct UnicodeCategoryInfo : IEquatable<UnicodeCategoryInfo>
	{
		private static readonly UnicodeCategoryInfo[] Categories =
		{
			new UnicodeCategoryInfo(UnicodeCategory.UppercaseLetter, "Lu", "Uppercase_Letter"),
			new UnicodeCategoryInfo(UnicodeCategory.LowercaseLetter, "Ll", "Lowercase_Letter"),
			new UnicodeCategoryInfo(UnicodeCategory.TitlecaseLetter, "Lt", "Titlecase_Letter"),
			new UnicodeCategoryInfo(UnicodeCategory.ModifierLetter, "Lm", "Modifier_Letter"),
			new UnicodeCategoryInfo(UnicodeCategory.OtherLetter, "Lo", "Other_Letter"),
			new UnicodeCategoryInfo(UnicodeCategory.NonSpacingMark, "Mn", "Nonspacing_Mark"),
			new UnicodeCategoryInfo(UnicodeCategory.SpacingCombiningMark, "Mc", "Spacing_Mark"),
			new UnicodeCategoryInfo(UnicodeCategory.EnclosingMark, "Me", "Enclosing_Mark"),
			new UnicodeCategoryInfo(UnicodeCategory.DecimalDigitNumber, "Nd", "Decimal_Number"),
			new UnicodeCategoryInfo(UnicodeCategory.LetterNumber, "Nl", "Letter_Number"),
			new UnicodeCategoryInfo(UnicodeCategory.OtherNumber, "No", "Other_Number"),
			new UnicodeCategoryInfo(UnicodeCategory.SpaceSeparator, "Zs", "Space_Separator"),
			new UnicodeCategoryInfo(UnicodeCategory.LineSeparator, "Zl", "Line_Separator"),
			new UnicodeCategoryInfo(UnicodeCategory.ParagraphSeparator, "Zp", "Paragraph_Separator"),
			new UnicodeCategoryInfo(UnicodeCategory.Control, "Cc", "Control"),
			new UnicodeCategoryInfo(UnicodeCategory.Format, "Cf", "Format"),
			new UnicodeCategoryInfo(UnicodeCategory.Surrogate, "Cs", "Surrogate"),
			new UnicodeCategoryInfo(UnicodeCategory.PrivateUse, "Co", "Private_Use"),
			new UnicodeCategoryInfo(UnicodeCategory.ConnectorPunctuation, "Pc", "Connector_Punctuation"),
			new UnicodeCategoryInfo(UnicodeCategory.DashPunctuation, "Pd", "Dash_Punctuation"),
			new UnicodeCategoryInfo(UnicodeCategory.OpenPunctuation, "Ps", "Open_Punctuation"),
			new UnicodeCategoryInfo(UnicodeCategory.ClosePunctuation, "Pe", "Close_Punctuation"),
			new UnicodeCategoryInfo(UnicodeCategory.InitialQuotePunctuation, "Pi", "Initial_Punctuation"),
			new UnicodeCategoryInfo(UnicodeCategory.FinalQuotePunctuation, "Pf", "Final_Punctuation"),
			new UnicodeCategoryInfo(UnicodeCategory.OtherPunctuation, "Po", "Other_Punctuation"),
			new UnicodeCategoryInfo(UnicodeCategory.MathSymbol, "Sm", "Math_Symbol"),
			new UnicodeCategoryInfo(UnicodeCategory.CurrencySymbol, "Sc", "Currency_Symbol"),
			new UnicodeCategoryInfo(UnicodeCategory.ModifierSymbol, "Sk", "Modifier_Symbol"),
			new UnicodeCategoryInfo(UnicodeCategory.OtherSymbol, "So", "Other_Symbol"),
			new UnicodeCategoryInfo(UnicodeCategory.OtherNotAssigned, "Cn", "Unassigned"),
		};

		private static readonly Dictionary<string, UnicodeCategory> UnicodeShortNameToCategoryDictionary = BuildShortNameDictionary();
		private static readonly Dictionary<string, UnicodeCategory> UnicodeLongNameToCategoryDictionary = BuildLongNameDictionary();

		private static Dictionary<string, UnicodeCategory> BuildShortNameDictionary()
		{
			var dictionary = new Dictionary<string, UnicodeCategory>(StringComparer.OrdinalIgnoreCase);

			foreach (var info in Categories)
			{
				dictionary.Add(info.ShortName, info.Category);
			}

			return dictionary;
		}

		private static Dictionary<string, UnicodeCategory> BuildLongNameDictionary()
		{
			var dictionary = new Dictionary<string, UnicodeCategory>(StringComparer.OrdinalIgnoreCase);

			foreach (var info in Categories)
			{
				dictionary.Add(info.LongName, info.Category);
			}

			return dictionary;
		}

		private static UnicodeCategory GetCategoryFromShortName(string name)
			=> UnicodeShortNameToCategoryDictionary[name];

		private static UnicodeCategory GetCategoryFromLongName(string name)
			=> UnicodeLongNameToCategoryDictionary[name];

		/// <summary>Gets an <see cref="UnicodeCategoryInfo"/> value providing information on the specified unicode category.</summary>
		/// <param name="category">The category on which information should be retrieved.</param>
		/// <returns>Information on the specified category.</returns>
		public static UnicodeCategoryInfo Get(UnicodeCategory category) => Categories[(int)category];

		/// <summary>Gets an <see cref="UnicodeCategoryInfo"/> value providing information on the unicode category, accessed by its short name, as per the Unicode standard.</summary>
		/// <param name="name">The short name for which information should be retrieved .</param>
		/// <returns>Information on the specified category.</returns>
		public static UnicodeCategoryInfo FromShortName(string name) => Get(GetCategoryFromShortName(name));

		/// <summary>Gets an <see cref="UnicodeCategoryInfo"/> value providing information on the unicode category, accessed by its long name, as per the Unicode standard.</summary>
		/// <param name="name">The long name for which information should be retrieved .</param>
		/// <returns>Information on the specified category.</returns>
		public static UnicodeCategoryInfo FromLongName(string name) => Get(GetCategoryFromLongName(name));

		/// <summary>The unicode category described.</summary>
		public readonly UnicodeCategory Category;
		/// <summary>Short name of the category, as per the Unicode standard.</summary>
		public readonly string ShortName;
		/// <summary>Long name of the category, as per the Unicode standard.</summary>
		public readonly string LongName;

		private UnicodeCategoryInfo(UnicodeCategory category, string shortName, string longName)
		{
			Category = category;
			ShortName = shortName;
			LongName = longName;
		}

		/// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
		/// <returns>A <see cref="string" /> that represents this instance.</returns>
		public override string ToString() => Category.ToString();

		/// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
		/// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
		/// <returns><see langword="true" /> if the specified <see cref="object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj) => obj is UnicodeCategoryInfo other && Equals(other);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
		public bool Equals(UnicodeCategoryInfo other) => other.Category == Category && (other.Category != 0 || other.ShortName != null);

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode() => (int)Category;

		/// <summary>Performs an implicit conversion from <see cref="UnicodeCategoryInfo"/> to <see cref="UnicodeCategory"/>.</summary>
		/// <param name="info">The information.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator UnicodeCategory(UnicodeCategoryInfo info) => info.Category;
	}
}
