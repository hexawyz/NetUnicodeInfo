using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	[Flags]
	public enum ContributoryProperties : int
	{
		[Display(Name = "ASCII_Hex_Digit", Description = "ASCII characters commonly used for the representation of hexadecimal numbers.")]
		ASCIIHexDigit = 0x00000001,
		[Display(Name = "Bidi_Control", Description = "Format control characters which have specific functions in the Unicode Bidirectional Algorithm [UAX9].")]
		BidiControl = 0x00000002,
		[Display(Name = "Dash", Description = "Punctuation characters explicitly called out as dashes in the Unicode Standard, plus their compatibility equivalents. Most of these have the General_Category value Pd, but some have the General_Category value Sm because of their use in mathematics.")]
		Dash = 0x00000004,
		[Display(Name = "Deprecated", Description = "For a machine-readable list of deprecated characters. No characters will ever be removed from the standard, but the usage of deprecated characters is strongly discouraged.")]
		Deprecated = 0x00000008,
		[Display(Name = "Diacritic", Description = "Characters that linguistically modify the meaning of another character to which they apply. Some diacritics are not combining characters, and some combining characters are not diacritics.")]
		Diacritic = 0x00000010,
		[Display(Name = "Extender", Description = "Characters whose principal function is to extend the value or shape of a preceding alphabetic character. Typical of these are length and iteration marks.")]
		Extender = 0x00000020,
		[Display(Name = "Hex_Digit", Description = "Characters commonly used for the representation of hexadecimal numbers, plus their compatibility equivalents.")]
		HexDigit = 0x00000040,
		[Display(Name = "Hyphen", Description = "Dashes which are used to mark connections between pieces of words, plus the Katakana middle dot. The Katakana middle dotfunctions like a hyphen, but is shaped like a dot rather than a dash.")]
		Hyphen = 0x00000080,
		[Display(Name = "Ideographic", Description = "Characters considered to be CJKV (Chinese, Japanese, Korean, and Vietnamese) ideographs. This property roughly defines the class of \"Chinese characters\" and does not include characters of other logographic scripts such as Cuneiform or Egyptian Hieroglyphs.")]
		Ideographic = 0x00000100,
		[Display(Name = "IDS_Binary_Operator", Description = "Used in Ideographic Description Sequences.")]
		IDSBinaryOperator = 0x00000200,
		[Display(Name = "IDS_Trinary_Operator", Description = "Used in Ideographic Description Sequences.")]
		IDSTrinaryOperator = 0x00000400,
		[Display(Name = "Join_Control", Description = "Format control characters which have specific functions for control of cursive joining and ligation.")]
		JoinControl = 0x00000800,
		[Display(Name = "Logical_Order_Exception", Description = "A small number of spacing vowel letters occurring in certain Southeast Asian scripts such as Thai and Lao, which use a visual order display model. These letters are stored in text ahead of syllable-initial consonants, and require special handling for processes such as searching and sorting.")]
		LogicalOrderException = 0x00001000,
		[Display(Name = "Noncharacter_Code_Point", Description = "Code points permanently reserved for internal use.")]
		NoncharacterCodePoint = 0x00002000,
		[Display(Name = "Other_Alphabetic", Description = "Used in deriving the Alphabetic property.")]
		OtherAlphabetic = 0x00004000,
		[Display(Name = "Other_Default_Ignorable_Code_Point", Description = "Used in deriving the Default_Ignorable_Code_Point property.")]
		OtherDefaultIgnorableCodePoint = 0x00008000,
		[Display(Name = "Other_Grapheme_Extend", Description = "Used in deriving  the Grapheme_Extend property.")]
		OtherGraphemeExtend = 0x00010000,
		[Display(Name = "Other_ID_Continue", Description = "Used to maintain backward compatibility of ID_Continue.")]
		OtherIDContinue = 0x00020000,
		[Display(Name = "Other_ID_Start", Description = "Used to maintain backward compatibility of ID_Start.")]
		OtherIDStart = 0x00040000,
		[Display(Name = "Other_Lowercase", Description = "Used in deriving the Lowercase property.")]
		OtherLowercase = 0x00080000,
		[Display(Name = "Other_Math", Description = "Used in deriving the Math property.")]
		OtherMath = 0x00100000,
		[Display(Name = "Other_Uppercase", Description = "Used in deriving the Uppercase property.")]
		OtherUppercase = 0x00200000,
		[Display(Name = "Pattern_Syntax", Description = "Used for pattern syntax as described in Unicode Standard Annex #31, \"Unicode Identifier and Pattern Syntax\" [UAX31].")]
		PatternSyntax = 0x00400000,
		[Display(Name = "Pattern_White_Space")]
		PatternWhiteSpace = 0x00800000,
		[Display(Name = "Quotation_Mark", Description = "Punctuation characters that function as quotation marks.")]
		QuotationMark = 0x01000000,
		[Display(Name = "Radical", Description = "Used in Ideographic Description Sequences.")]
		Radical = 0x02000000,
		[Display(Name = "Soft_Dotted", Description = "Characters with a \"soft dot\", like i or j. An accent placed on these characters causes the dot to disappear. An explicit dot abovecan be added where required, such as in Lithuanian.")]
		SoftDotted = 0x04000000,
		[Display(Name = "STerm", Description = "Sentence Terminal. Used in Unicode Standard Annex #29, \"Unicode Text Segmentation\" [UAX29].")]
		STerm = 0x08000000,
		[Display(Name = "Terminal_Punctuation", Description = "Punctuation characters that generally mark the end of textual units.")]
		TerminalPunctuation = 0x10000000,
		[Display(Name = "Unified_Ideograph", Description = "A property which specifies the exact set of Unified CJK Ideographs in the standard. This set excludes CJK Compatibility Ideographs (which have canonical decompositions to Unified CJK Ideographs), as well as characters from the CJK Symbols and Punctuation block. The property is used in the definition of Ideographic Description Sequences.")]
		UnifiedIdeograph = 0x20000000,
		[Display(Name = "Variation_Selector", Description = "Indicates characters that are Variation Selectors. For details on the behavior of these characters, seeStandardizedVariants.html, Section 23.4, Variation Selectors in [Unicode], and Unicode Technical Standard #37, \"Unicode Ideographic Variation Database\" [UTS37].")]
		VariationSelector = 0x40000000,
		[Display(Name = "White_Space", Description = "Spaces, separator characters and other control characters which should be treated by programming languages as \"white space\" for the purpose of parsing elements. See also Line_Break, Grapheme_Cluster_Break, Sentence_Break, and Word_Break, which classify space characters and related controls somewhat differently for particular text segmentation contexts.")]
		WhiteSpace = -0x7FFFFFFF,
	}
}
