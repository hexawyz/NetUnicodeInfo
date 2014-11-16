using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeCategoryInfo : IEquatable<UnicodeCategoryInfo>
	{
		private static readonly UnicodeCategoryInfo[] categories =
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

		private static readonly Dictionary<string, UnicodeCategory> unicodeShortNameToCategoryDictionary = BuildShortNameDictionary();
		private static readonly Dictionary<string, UnicodeCategory> unicodeLongNameToCategoryDictionary = BuildLongNameDictionary();

		private static Dictionary<string, UnicodeCategory> BuildShortNameDictionary()
		{
			var dictionary = new Dictionary<string, UnicodeCategory>(StringComparer.OrdinalIgnoreCase);

			foreach (var info in categories)
			{
				dictionary.Add(info.ShortName, info.Category);
			}

			return dictionary;
		}

		private static Dictionary<string, UnicodeCategory> BuildLongNameDictionary()
		{
			var dictionary = new Dictionary<string, UnicodeCategory>(StringComparer.OrdinalIgnoreCase);

			foreach (var info in categories)
			{
				dictionary.Add(info.LongName, info.Category);
			}

			return dictionary;
		}

		private static UnicodeCategory GetCategoryFromShortName(string name)
		{
			return unicodeShortNameToCategoryDictionary[name];
		}

		private static UnicodeCategory GetCategoryFromLongName(string name)
		{
			return unicodeLongNameToCategoryDictionary[name];
		}

		public static UnicodeCategoryInfo Get(UnicodeCategory category)
		{
			return categories[(int)category];
		}

		public static UnicodeCategoryInfo FromShortName(string name)
		{
			return Get(GetCategoryFromShortName(name));
		}

		public static UnicodeCategoryInfo FromLongName(string name)
		{
			return Get(GetCategoryFromLongName(name));
		}

		public readonly UnicodeCategory Category;
		public readonly string ShortName;
		public readonly string LongName;

		private UnicodeCategoryInfo(UnicodeCategory category, string shortName, string longName)
		{
			this.Category = category;
			this.ShortName = shortName;
			this.LongName = longName;
		}

		public override string ToString()
		{
			return Category.ToString();
		}

		public override bool Equals(object obj)
		{
			return obj is UnicodeCategoryInfo && Equals((UnicodeCategoryInfo)obj);
		}

		public bool Equals(UnicodeCategoryInfo other)
		{
			return other.Category == Category && (other.Category != 0 || other.ShortName != null);
		}

		public override int GetHashCode()
		{
			return (int)Category;
		}

		public static implicit operator UnicodeCategory(UnicodeCategoryInfo info)
		{
			return info.Category;
		}
	}
}
