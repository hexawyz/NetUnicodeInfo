using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>A bitmask of the various available core properties.</summary>
	/// <remarks>Core properties are normative, and derived from various properties as well as <see cref="ContributoryProperties"/>.</remarks>
	[Flags]
	public enum CoreProperties : int
	{
		// NB: Be careful when adding new properties to the enum, as EmojiProperties will be stored in the 4 upper bits out of 24.

		/// <summary>Represents the Lowercase property.</summary>
		[ValueName("Lowercase"), ValueName("Lower"), Display(Name = "Lowercase")]
		Lowercase = 0x00000001,
		/// <summary>Represents the Uppercase property.</summary>
		[ValueName("Uppercase"), ValueName("Upper"), Display(Name = "Uppercase")]
		Uppercase = 0x00000002,
		/// <summary>Represents the Cased property.</summary>
		[ValueName("Cased"), Display(Name = "Cased")]
		Cased = 0x00000004,
		/// <summary>Represents the Case_Ignorable property.</summary>
		[ValueName("Case_Ignorable"), ValueName("CI"), Display(Name = "Case_Ignorable")]
		CaseIgnorable = 0x00000008,
		/// <summary>Represents the Changes_When_Lowercased property.</summary>
		[ValueName("Changes_When_Lowercased"), ValueName("CWL"), Display(Name = "Changes_When_Lowercased")]
		ChangesWhenLowercased = 0x00000010,
		/// <summary>Represents the Changes_When_Uppercased property.</summary>
		[ValueName("Changes_When_Uppercased"), ValueName("CWU"), Display(Name = "Changes_When_Uppercased")]
		ChangesWhenUppercased = 0x00000020,
		/// <summary>Represents the Changes_When_Titlecased property.</summary>
		[ValueName("Changes_When_Titlecased"), ValueName("CWT"), Display(Name = "Changes_When_Titlecased")]
		ChangesWhenTitlecased = 0x00000040,
		/// <summary>Represents the Changes_When_Casefolded property.</summary>
		[ValueName("Changes_When_Casefolded"), ValueName("CWCF"), Display(Name = "Changes_When_Casefolded")]
		ChangesWhenCasefolded = 0x00000080,
		/// <summary>Represents the Changes_When_Casemapped property.</summary>
		[ValueName("Changes_When_Casemapped"), ValueName("CWCM"), Display(Name = "Changes_When_Casemapped")]
		ChangesWhenCasemapped = 0x00000100,
		/// <summary>Represents the Alphabetic property.</summary>
		[ValueName("Alphabetic"), ValueName("Alpha"), Display(Name = "Alphabetic")]
		Alphabetic = 0x00000200,
		/// <summary>Represents the Default_Ignorable_Code_Point property.</summary>
		[ValueName("Default_Ignorable_Code_Point"), ValueName("DI"), Display(Name = "Default_Ignorable_Code_Point")]
		DefaultIgnorableCodePoint = 0x00000400,
		/// <summary>Represents the Grapheme_Base property.</summary>
		[ValueName("Grapheme_Base"), ValueName("Gr_Base"), Display(Name = "Grapheme_Base")]
		GraphemeBase = 0x00000800,
		/// <summary>Represents the Grapheme_Extend property.</summary>
		[ValueName("Grapheme_Extend"), ValueName("Gr_Ext"), Display(Name = "Grapheme_Extend")]
		GraphemeExtend = 0x00001000,
		/// <summary>Represents the Grapheme_Link property.</summary>
		[ValueName("Grapheme_Link"), ValueName("Gr_Link"), Display(Name = "Grapheme_Link")]
		GraphemeLink = 0x00002000,
		/// <summary>Represents the Math property.</summary>
		[ValueName("Math"), Display(Name = "Math")]
		Math = 0x00004000,
		/// <summary>Represents the ID_Start property.</summary>
		[ValueName("ID_Start"), ValueName("IDS"), Display(Name = "ID_Start")]
		IdentifierStart = 0x00008000,
		/// <summary>Represents the ID_Continue property.</summary>
		[ValueName("ID_Continue"), ValueName("IDC"), Display(Name = "ID_Continue")]
		IdentifierContinue = 0x00010000,
		/// <summary>Represents the XID_Start property.</summary>
		[ValueName("XID_Start"), ValueName("XIDS"), Display(Name = "XID_Start")]
		ExtendedIdentifierStart = 0x00020000,
		/// <summary>Represents the XID_Continue property.</summary>
		[ValueName("XID_Continue"), ValueName("XIDC"), Display(Name = "XID_Continue")]
		ExtendedIdentifierContinue = 0x00040000,
	}
}
