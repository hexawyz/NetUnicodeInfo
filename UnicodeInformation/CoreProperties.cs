using System.ComponentModel.DataAnnotations;

namespace System.Unicode
{
	/// <summary>A bitmask of the various available core properties.</summary>
	/// <remarks>Core properties are normative, and derived from various properties as well as <see cref="ContributoryProperties"/>.</remarks>
	[Flags]
	public enum CoreProperties : int
	{
		// ⚠️ Be careful when adding new properties to the enum. Only up to 22 bits should be consumed.

		/// <summary>Represents the Lowercase property.</summary>
		[ValueName("Lowercase"), ValueName("Lower"), Display(Name = "Lowercase")]
		Lowercase = 0b_0000_0000_0000_0000_0000_0001,
		/// <summary>Represents the Uppercase property.</summary>
		[ValueName("Uppercase"), ValueName("Upper"), Display(Name = "Uppercase")]
		Uppercase = 0b_0000_0000_0000_0000_0000_0010,
		/// <summary>Represents the Cased property.</summary>
		[ValueName("Cased"), Display(Name = "Cased")]
		Cased = 0b_0000_0000_0000_0000_0000_0100,
		/// <summary>Represents the Case_Ignorable property.</summary>
		[ValueName("Case_Ignorable"), ValueName("CI"), Display(Name = "Case_Ignorable")]
		CaseIgnorable = 0b_0000_0000_0000_0000_0000_1000,
		/// <summary>Represents the Changes_When_Lowercased property.</summary>
		[ValueName("Changes_When_Lowercased"), ValueName("CWL"), Display(Name = "Changes_When_Lowercased")]
		ChangesWhenLowercased = 0b_0000_0000_0000_0000_0001_0000,
		/// <summary>Represents the Changes_When_Uppercased property.</summary>
		[ValueName("Changes_When_Uppercased"), ValueName("CWU"), Display(Name = "Changes_When_Uppercased")]
		ChangesWhenUppercased = 0b_0000_0000_0000_0000_0010_0000,
		/// <summary>Represents the Changes_When_Titlecased property.</summary>
		[ValueName("Changes_When_Titlecased"), ValueName("CWT"), Display(Name = "Changes_When_Titlecased")]
		ChangesWhenTitlecased = 0b_0000_0000_0000_0000_0100_0000,
		/// <summary>Represents the Changes_When_Casefolded property.</summary>
		[ValueName("Changes_When_Casefolded"), ValueName("CWCF"), Display(Name = "Changes_When_Casefolded")]
		ChangesWhenCasefolded = 0b_0000_0000_0000_0000_1000_0000,
		/// <summary>Represents the Changes_When_Casemapped property.</summary>
		[ValueName("Changes_When_Casemapped"), ValueName("CWCM"), Display(Name = "Changes_When_Casemapped")]
		ChangesWhenCasemapped = 0b_0000_0000_0000_0001_0000_0000,
		/// <summary>Represents the Alphabetic property.</summary>
		[ValueName("Alphabetic"), ValueName("Alpha"), Display(Name = "Alphabetic")]
		Alphabetic = 0b_0000_0000_0000_0010_0000_0000,
		/// <summary>Represents the Default_Ignorable_Code_Point property.</summary>
		[ValueName("Default_Ignorable_Code_Point"), ValueName("DI"), Display(Name = "Default_Ignorable_Code_Point")]
		DefaultIgnorableCodePoint = 0b_0000_0000_0000_0100_0000_0000,
		/// <summary>Represents the Grapheme_Base property.</summary>
		[ValueName("Grapheme_Base"), ValueName("Gr_Base"), Display(Name = "Grapheme_Base")]
		GraphemeBase = 0b_0000_0000_0000_1000_0000_0000,
		/// <summary>Represents the Grapheme_Extend property.</summary>
		[ValueName("Grapheme_Extend"), ValueName("Gr_Ext"), Display(Name = "Grapheme_Extend")]
		GraphemeExtend = 0b_0000_0000_0001_0000_0000_0000,
		/// <summary>Represents the Grapheme_Link property.</summary>
		[ValueName("Grapheme_Link"), ValueName("Gr_Link"), Display(Name = "Grapheme_Link")]
		GraphemeLink = 0b_0000_0000_0010_0000_0000_0000,
		/// <summary>Represents the Math property.</summary>
		[ValueName("Math"), Display(Name = "Math")]
		Math = 0b_0000_0000_0100_0000_0000_0000,
		/// <summary>Represents the ID_Start property.</summary>
		[ValueName("ID_Start"), ValueName("IDS"), Display(Name = "ID_Start")]
		IdentifierStart = 0b_0000_0000_1000_0000_0000_0000,
		/// <summary>Represents the ID_Continue property.</summary>
		[ValueName("ID_Continue"), ValueName("IDC"), Display(Name = "ID_Continue")]
		IdentifierContinue = 0b_0000_0001_0000_0000_0000_0000,
		/// <summary>Represents the XID_Start property.</summary>
		[ValueName("XID_Start"), ValueName("XIDS"), Display(Name = "XID_Start")]
		ExtendedIdentifierStart = 0b_0000_0010_0000_0000_0000_0000,
		/// <summary>Represents the XID_Continue property.</summary>
		[ValueName("XID_Continue"), ValueName("XIDC"), Display(Name = "XID_Continue")]
		ExtendedIdentifierContinue = 0b_0000_0100_0000_0000_0000_0000,
	}
}
