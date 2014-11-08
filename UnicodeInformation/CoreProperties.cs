using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	[Flags]
	public enum CoreProperties : int
	{
		[ValueName("Lowercase"), ValueName("Lower"), Display(Name = "Lowercase")]
		Lowercase = 0x00000001,
		[ValueName("Uppercase"), ValueName("Upper"), Display(Name = "Uppercase")]
		Uppercase = 0x00000002,
		[ValueName("Cased"), Display(Name = "Cased")]
		Cased = 0x00000004,
		[ValueName("Case_Ignorable"), ValueName("CI"), Display(Name = "Case_Ignorable")]
		CaseIgnorable = 0x00000008,
		[ValueName("Changes_When_Lowercased"), ValueName("CWL"), Display(Name = "Changes_When_Lowercased")]
		ChangesWhenLowercased = 0x00000010,
		[ValueName("Changes_When_Uppercased"), ValueName("CWU"), Display(Name = "Changes_When_Uppercased")]
		ChangesWhenUppercased = 0x00000020,
		[ValueName("Changes_When_Titlecased"), ValueName("CWT"), Display(Name = "Changes_When_Titlecased")]
		ChangesWhenTitlecased = 0x00000040,
		[ValueName("Changes_When_Casefolded"), ValueName("CWCF"), Display(Name = "Changes_When_Casefolded")]
		ChangesWhenCasefolded = 0x00000080,
		[ValueName("Changes_When_Casemapped"), ValueName("CWCM"), Display(Name = "Changes_When_Casemapped")]
		ChangesWhenCasemapped = 0x00000100,
		[ValueName("Alphabetic"), ValueName("Alpha"), Display(Name = "Alphabetic")]
		Alphabetic = 0x00000200,
		[ValueName("Default_Ignorable_Code_Point"), ValueName("DI"), Display(Name = "Default_Ignorable_Code_Point")]
		DefaultIgnorableCodePoint = 0x00000400,
		[ValueName("Grapheme_Base"), ValueName("Gr_Base"), Display(Name = "Grapheme_Base")]
		GraphemeBase = 0x00000800,
		[ValueName("Grapheme_Extend"), ValueName("Gr_Ext"), Display(Name = "Grapheme_Extend")]
		GraphemeExtend = 0x00001000,
		[ValueName("Grapheme_Link"), ValueName("Gr_Link"), Display(Name = "Grapheme_Link")]
		GraphemeLink = 0x00002000,
		[ValueName("Math"), Display(Name = "Math")]
		Math = 0x00004000,
		[ValueName("ID_Start"), ValueName("IDS"), Display(Name = "ID_Start")]
		IdentifierStart = 0x00008000,
		[ValueName("ID_Continue"), ValueName("IDC"), Display(Name = "ID_Continue")]
		IdentifierContinue = 0x00010000,
		[ValueName("XID_Start"), ValueName("XIDS"), Display(Name = "XID_Start")]
		ExtendedIdentifierStart = 0x00020000,
		[ValueName("XID_Continue"), ValueName("XIDC"), Display(Name = "XID_Continue")]
		ExtendedIdentifierContinue = 0x00040000,
	}
}
