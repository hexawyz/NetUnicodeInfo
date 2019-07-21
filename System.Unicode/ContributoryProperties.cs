using System.ComponentModel.DataAnnotations;

namespace System.Unicode
{
	/// <summary>A bitmask of the various available contributory properties.</summary>
	/// <remarks>As per the standard, contributory properties are neither normative nor informative, but are used to derive <see cref="ContributoryProperties"/>.</remarks>
	[Flags]
	public enum ContributoryProperties : int
	{
		/// <summary>Represents the ASCII_Hex_Digit property.</summary>
		/// <remarks>ASCII characters commonly used for the representation of hexadecimal numbers.</remarks>
		[ValueName("ASCII_Hex_Digit"), Display(Name = "ASCII_Hex_Digit", Description = "ASCII characters commonly used for the representation of hexadecimal numbers.")]
		AsciiHexDigit = 0x00000001,
		/// <summary>Represents the Bidi_Control property.</summary>
		/// <remarks>Format control characters which have specific functions in the Unicode Bidirectional Algorithm [UAX9].</remarks>
		[ValueName("Bidi_Control"), Display(Name = "Bidi_Control", Description = "Format control characters which have specific functions in the Unicode Bidirectional Algorithm [UAX9].")]
		BidiControl = 0x00000002,
		/// <summary>Represents the Dash property.</summary>
		/// <remarks>Punctuation characters explicitly called out as dashes in the Unicode Standard, plus their compatibility equivalents. Most of these have the General_Category value Pd, but some have the General_Category value Sm because of their use in mathematics.</remarks>
		[ValueName("Dash"), Display(Name = "Dash", Description = "Punctuation characters explicitly called out as dashes in the Unicode Standard, plus their compatibility equivalents. Most of these have the General_Category value Pd, but some have the General_Category value Sm because of their use in mathematics.")]
		Dash = 0x00000004,
		/// <summary>Represents the Deprecated property.</summary>
		/// <remarks>For a machine-readable list of deprecated characters. No characters will ever be removed from the standard, but the usage of deprecated characters is strongly discouraged.</remarks>
		[ValueName("Deprecated"), Display(Name = "Deprecated", Description = "For a machine-readable list of deprecated characters. No characters will ever be removed from the standard, but the usage of deprecated characters is strongly discouraged.")]
		Deprecated = 0x00000008,
		/// <summary>Represents the Diacritic property.</summary>
		/// <remarks>Characters that linguistically modify the meaning of another character to which they apply. Some diacritics are not combining characters, and some combining characters are not diacritics.</remarks>
		[ValueName("Diacritic"), Display(Name = "Diacritic", Description = "Characters that linguistically modify the meaning of another character to which they apply. Some diacritics are not combining characters, and some combining characters are not diacritics.")]
		Diacritic = 0x00000010,
		/// <summary>Represents the Extender property.</summary>
		/// <remarks>Characters whose principal function is to extend the value or shape of a preceding alphabetic character. Typical of these are length and iteration marks.</remarks>
		[ValueName("Extender"), Display(Name = "Extender", Description = "Characters whose principal function is to extend the value or shape of a preceding alphabetic character. Typical of these are length and iteration marks.")]
		Extender = 0x00000020,
		/// <summary>Represents the Hex_Digit property.</summary>
		/// <remarks>Characters commonly used for the representation of hexadecimal numbers, plus their compatibility equivalents.</remarks>
		[ValueName("Hex_Digit"), Display(Name = "Hex_Digit", Description = "Characters commonly used for the representation of hexadecimal numbers, plus their compatibility equivalents.")]
		HexDigit = 0x00000040,
		/// <summary>Represents the Hyphen property.</summary>
		/// <remarks>Dashes which are used to mark connections between pieces of words, plus the Katakana middle dot. The Katakana middle dotfunctions like a hyphen, but is shaped like a dot rather than a dash.</remarks>
		[ValueName("Hyphen"), Display(Name = "Hyphen", Description = "Dashes which are used to mark connections between pieces of words, plus the Katakana middle dot. The Katakana middle dotfunctions like a hyphen, but is shaped like a dot rather than a dash.")]
		Hyphen = 0x00000080,
		/// <summary>Represents the Ideographic property.</summary>
		/// <remarks>Characters considered to be CJKV (Chinese, Japanese, Korean, and Vietnamese) ideographs. This property roughly defines the class of "Chinese characters" and does not include characters of other logographic scripts such as Cuneiform or Egyptian Hieroglyphs.</remarks>
		[ValueName("Ideographic"), Display(Name = "Ideographic", Description = "Characters considered to be CJKV (Chinese, Japanese, Korean, and Vietnamese) ideographs. This property roughly defines the class of \"Chinese characters\" and does not include characters of other logographic scripts such as Cuneiform or Egyptian Hieroglyphs.")]
		Ideographic = 0x00000100,
		/// <summary>Represents the IDS_Binary_Operator property.</summary>
		/// <remarks>Used in Ideographic Description Sequences.</remarks>
		[ValueName("IDS_Binary_Operator"), Display(Name = "IDS_Binary_Operator", Description = "Used in Ideographic Description Sequences.")]
		IdsBinaryOperator = 0x00000200,
		/// <summary>Represents the IDS_Trinary_Operator property.</summary>
		/// <remarks>Used in Ideographic Description Sequences.</remarks>
		[ValueName("IDS_Trinary_Operator"), Display(Name = "IDS_Trinary_Operator", Description = "Used in Ideographic Description Sequences.")]
		IdsTrinaryOperator = 0x00000400,
		/// <summary>Represents the Join_Control property.</summary>
		/// <remarks>Format control characters which have specific functions for control of cursive joining and ligation.</remarks>
		[ValueName("Join_Control"), Display(Name = "Join_Control", Description = "Format control characters which have specific functions for control of cursive joining and ligation.")]
		JoinControl = 0x00000800,
		/// <summary>Represents the Logical_Order_Exception property.</summary>
		/// <remarks>A small number of spacing vowel letters occurring in certain Southeast Asian scripts such as Thai and Lao, which use a visual order display model. These letters are stored in text ahead of syllable-initial consonants, and require special handling for processes such as searching and sorting.</remarks>
		[ValueName("Logical_Order_Exception"), Display(Name = "Logical_Order_Exception", Description = "A small number of spacing vowel letters occurring in certain Southeast Asian scripts such as Thai and Lao, which use a visual order display model. These letters are stored in text ahead of syllable-initial consonants, and require special handling for processes such as searching and sorting.")]
		LogicalOrderException = 0x00001000,
		/// <summary>Represents the Noncharacter_Code_Point property.</summary>
		/// <remarks>Code points permanently reserved for internal use.</remarks>
		[ValueName("Noncharacter_Code_Point"), Display(Name = "Noncharacter_Code_Point", Description = "Code points permanently reserved for internal use.")]
		NonCharacterCodePoint = 0x00002000,
		/// <summary>Represents the Other_Alphabetic property.</summary>
		/// <remarks>Used in deriving the Alphabetic property.</remarks>
		[ValueName("Other_Alphabetic"), Display(Name = "Other_Alphabetic", Description = "Used in deriving the Alphabetic property.")]
		OtherAlphabetic = 0x00004000,
		/// <summary>Represents the Other_Default_Ignorable_Code_Point property.</summary>
		/// <remarks>Used in deriving the Default_Ignorable_Code_Point property.</remarks>
		[ValueName("Other_Default_Ignorable_Code_Point"), Display(Name = "Other_Default_Ignorable_Code_Point", Description = "Used in deriving the Default_Ignorable_Code_Point property.")]
		OtherDefaultIgnorableCodePoint = 0x00008000,
		/// <summary>Represents the Other_Grapheme_Extend property.</summary>
		/// <remarks>Used in deriving  the Grapheme_Extend property.</remarks>
		[ValueName("Other_Grapheme_Extend"), Display(Name = "Other_Grapheme_Extend", Description = "Used in deriving  the Grapheme_Extend property.")]
		OtherGraphemeExtend = 0x00010000,
		/// <summary>Represents the Other_ID_Continue property.</summary>
		/// <remarks>Used to maintain backward compatibility of ID_Continue.</remarks>
		[ValueName("Other_ID_Continue"), Display(Name = "Other_ID_Continue", Description = "Used to maintain backward compatibility of ID_Continue.")]
		OtherIdContinue = 0x00020000,
		/// <summary>Represents the Other_ID_Start property.</summary>
		/// <remarks>Used to maintain backward compatibility of ID_Start.</remarks>
		[ValueName("Other_ID_Start"), Display(Name = "Other_ID_Start", Description = "Used to maintain backward compatibility of ID_Start.")]
		OtherIdStart = 0x00040000,
		/// <summary>Represents the Other_Lowercase property.</summary>
		/// <remarks>Used in deriving the Lowercase property.</remarks>
		[ValueName("Other_Lowercase"), Display(Name = "Other_Lowercase", Description = "Used in deriving the Lowercase property.")]
		OtherLowercase = 0x00080000,
		/// <summary>Represents the Other_Math property.</summary>
		/// <remarks>Used in deriving the Math property.</remarks>
		[ValueName("Other_Math"), Display(Name = "Other_Math", Description = "Used in deriving the Math property.")]
		OtherMath = 0x00100000,
		/// <summary>Represents the Other_Uppercase property.</summary>
		/// <remarks>Used in deriving the Uppercase property.</remarks>
		[ValueName("Other_Uppercase"), Display(Name = "Other_Uppercase", Description = "Used in deriving the Uppercase property.")]
		OtherUppercase = 0x00200000,
		/// <summary>Represents the Pattern_Syntax property.</summary>
		/// <remarks>Used for pattern syntax as described in Unicode Standard Annex #31, "Unicode Identifier and Pattern Syntax" [UAX31].</remarks>
		[ValueName("Pattern_Syntax"), Display(Name = "Pattern_Syntax", Description = "Used for pattern syntax as described in Unicode Standard Annex #31, \"Unicode Identifier and Pattern Syntax\" [UAX31].")]
		PatternSyntax = 0x00400000,
		/// <summary>Represents the Pattern_White_Space property.</summary>
		[ValueName("Pattern_White_Space"), Display(Name = "Pattern_White_Space")]
		PatternWhiteSpace = 0x00800000,
		/// <summary>Represents the Quotation_Mark property.</summary>
		/// <remarks>Punctuation characters that function as quotation marks.</remarks>
		[ValueName("Quotation_Mark"), Display(Name = "Quotation_Mark", Description = "Punctuation characters that function as quotation marks.")]
		QuotationMark = 0x01000000,
		/// <summary>Represents the Radical property.</summary>
		/// <remarks>Used in Ideographic Description Sequences.</remarks>
		[ValueName("Radical"), Display(Name = "Radical", Description = "Used in Ideographic Description Sequences.")]
		Radical = 0x02000000,
		/// <summary>Represents the Soft_Dotted property.</summary>
		/// <remarks>Characters with a "soft dot", like i or j. An accent placed on these characters causes the dot to disappear. An explicit dot abovecan be added where required, such as in Lithuanian.</remarks>
		[ValueName("Soft_Dotted"), Display(Name = "Soft_Dotted", Description = "Characters with a \"soft dot\", like i or j. An accent placed on these characters causes the dot to disappear. An explicit dot abovecan be added where required, such as in Lithuanian.")]
		SoftDotted = 0x04000000,
		/// <summary>Represents the STerm property.</summary>
		/// <remarks>Sentence Terminal. Used in Unicode Standard Annex #29, "Unicode Text Segmentation" [UAX29].</remarks>
		[ValueName("STerm"), Display(Name = "STerm", Description = "Sentence Terminal. Used in Unicode Standard Annex #29, \"Unicode Text Segmentation\" [UAX29].")]
		SequenceTerminal = 0x08000000,
		/// <summary>Represents the Terminal_Punctuation property.</summary>
		/// <remarks>Punctuation characters that generally mark the end of textual units.</remarks>
		[ValueName("Terminal_Punctuation"), Display(Name = "Terminal_Punctuation", Description = "Punctuation characters that generally mark the end of textual units.")]
		TerminalPunctuation = 0x10000000,
		/// <summary>Represents the Unified_Ideograph property.</summary>
		/// <remarks>A property which specifies the exact set of Unified CJK Ideographs in the standard. This set excludes CJK Compatibility Ideographs (which have canonical decompositions to Unified CJK Ideographs), as well as characters from the CJK Symbols and Punctuation block. The property is used in the definition of Ideographic Description Sequences.</remarks>
		[ValueName("Unified_Ideograph"), Display(Name = "Unified_Ideograph", Description = "A property which specifies the exact set of Unified CJK Ideographs in the standard. This set excludes CJK Compatibility Ideographs (which have canonical decompositions to Unified CJK Ideographs), as well as characters from the CJK Symbols and Punctuation block. The property is used in the definition of Ideographic Description Sequences.")]
		UnifiedIdeograph = 0x20000000,
		/// <summary>Represents the Variation_Selector property.</summary>
		/// <remarks>Indicates characters that are Variation Selectors. For details on the behavior of these characters, seeStandardizedVariants.html, Section 23.4, Variation Selectors in [Unicode], and Unicode Technical Standard #37, "Unicode Ideographic Variation Database" [UTS37].</remarks>
		[ValueName("Variation_Selector"), Display(Name = "Variation_Selector", Description = "Indicates characters that are Variation Selectors. For details on the behavior of these characters, seeStandardizedVariants.html, Section 23.4, Variation Selectors in [Unicode], and Unicode Technical Standard #37, \"Unicode Ideographic Variation Database\" [UTS37].")]
		VariationSelector = 0x40000000,
		/// <summary>Represents the White_Space property.</summary>
		/// <remarks>Spaces, separator characters and other control characters which should be treated by programming languages as "white space" for the purpose of parsing elements. See also Line_Break, Grapheme_Cluster_Break, Sentence_Break, and Word_Break, which classify space characters and related controls somewhat differently for particular text segmentation contexts.</remarks>
		[ValueName("White_Space"), Display(Name = "White_Space", Description = "Spaces, separator characters and other control characters which should be treated by programming languages as \"white space\" for the purpose of parsing elements. See also Line_Break, Grapheme_Cluster_Break, Sentence_Break, and Word_Break, which classify space characters and related controls somewhat differently for particular text segmentation contexts.")]
		WhiteSpace = ~0x7FFFFFFF,
	}
}
