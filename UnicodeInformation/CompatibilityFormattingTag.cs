using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Provides information on the kind of compatibility decomposition provided.</summary>
	/// <remarks>The default value of <see cref="CompatibilityFormattingTag.Canonical"/> indicates canonical decomposition of the code point.</remarks>
	public enum CompatibilityFormattingTag : byte
	{
		/// <summary>Canonical form.</summary>
		Canonical = 0,
		/// <summary>Font variant (for example, a blackletter form).</summary>
		[ValueName("font"), Display(Name = "font", Description = "Font variant (for example, a blackletter form).")]
		Font,
		/// <summary>No-break version of a space or hyphen.</summary>
		[ValueName("noBreak"), Display(Name = "noBreak", Description = "No-break version of a space or hyphen.")]
		NoBreak,
		/// <summary>Initial presentation form (Arabic).</summary>
		[ValueName("initial"), Display(Name = "initial", Description = "Initial presentation form (Arabic).")]
		Initial,
		/// <summary>Medial presentation form (Arabic).</summary>
		[ValueName("medial"), Display(Name = "medial", Description = "Medial presentation form (Arabic).")]
		Medial,
		/// <summary>Final presentation form (Arabic).</summary>
		[ValueName("final"), Display(Name = "final", Description = "Final presentation form (Arabic).")]
		Final,
		/// <summary>Isolated presentation form (Arabic).</summary>
		[ValueName("isolated"), Display(Name = "isolated", Description = "Isolated presentation form (Arabic).")]
		Isolated,
		/// <summary>Encircled form.</summary>
		[ValueName("circle"), Display(Name = "circle", Description = "Encircled form.")]
		Circle,
		/// <summary>Superscript form.</summary>
		[ValueName("super"), Display(Name = "super", Description = "Superscript form.")]
		Super,
		/// <summary>Subscript form.</summary>
		[ValueName("sub"), Display(Name = "sub", Description = "Subscript form.")]
		Sub,
		/// <summary>Vertical layout presentation form.</summary>
		[ValueName("vertical"), Display(Name = "vertical", Description = "Vertical layout presentation form.")]
		Vertical,
		/// <summary>Wide (or zenkaku) compatibility character.</summary>
		[ValueName("wide"), Display(Name = "wide", Description = "Wide (or zenkaku) compatibility character.")]
		Wide,
		/// <summary>Narrow (or hankaku) compatibility character.</summary>
		[ValueName("narrow"), Display(Name = "narrow", Description = "Narrow (or hankaku) compatibility character.")]
		Narrow,
		/// <summary>Small variant form (CNS compatibility).</summary>
		[ValueName("small"), Display(Name = "small", Description = "Small variant form (CNS compatibility).")]
		Small,
		/// <summary>CJK squared font variant.</summary>
		[ValueName("square"), Display(Name = "square", Description = "CJK squared font variant.")]
		Square,
		/// <summary>Vulgar fraction form.</summary>
		[ValueName("fraction"), Display(Name = "fraction", Description = "Vulgar fraction form.")]
		Fraction,
		/// <summary>Otherwise unspecified compatibility character.</summary>
		[ValueName("compat"), Display(Name = "compat", Description = "Otherwise unspecified compatibility character.")]
		Compat,
	}
}
