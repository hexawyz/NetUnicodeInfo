using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public enum CompatibilityFormattingTag : byte
	{
		Canonical = 0,
		[ValueName("font"), Display(Name = "font", Description = "Font variant (for example, a blackletter form)")]
		Font,
		[ValueName("noBreak"), Display(Name = "noBreak", Description = "No-break version of a space or hyphen")]
		NoBreak,
		[ValueName("initial"), Display(Name = "initial", Description = "Initial presentation form (Arabic)")]
		Initial,
		[ValueName("medial"), Display(Name = "medial", Description = "Medial presentation form (Arabic)")]
		Medial,
		[ValueName("final"), Display(Name = "final", Description = "Final presentation form (Arabic)")]
		Final,
		[ValueName("isolated"), Display(Name = "isolated", Description = "Isolated presentation form (Arabic)")]
		Isolated,
		[ValueName("circle"), Display(Name = "circle", Description = "Encircled form")]
		Circle,
		[ValueName("super"), Display(Name = "super", Description = "Superscript form")]
		Super,
		[ValueName("sub"), Display(Name = "sub", Description = "Subscript form")]
		Sub,
		[ValueName("vertical"), Display(Name = "vertical", Description = "Vertical layout presentation form")]
		Vertical,
		[ValueName("wide"), Display(Name = "wide", Description = "Wide (or zenkaku) compatibility character")]
		Wide,
		[ValueName("narrow"), Display(Name = "narrow", Description = "Narrow (or hankaku) compatibility character")]
		Narrow,
		[ValueName("small"), Display(Name = "small", Description = "Small variant form (CNS compatibility)")]
		Small,
		[ValueName("square"), Display(Name = "square", Description = "CJK squared font variant")]
		Square,
		[ValueName("fraction"), Display(Name = "fraction", Description = "Vulgar fraction form")]
		Fraction,
		[ValueName("compat"), Display(Name = "compat", Description = "Otherwise unspecified compatibility character")]
		Compat,
	}
}
