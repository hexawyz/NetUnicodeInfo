using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace System.Unicode
{
	/// <summary>A bitmask of the various available emoji properties.</summary>
	/// <remarks>Emoji properties are not formally part of UCD, but .</remarks>
	[Flags]
	public enum EmojiProperties : byte
	{
		// NB: These values will be stored in as the most significant bits of CoreProperties.
		// CoreProperties are stored using 18 bits out of 24 available bits, leaving at most 6 bits to use. 4 of those will be used by emoji properties.

		/// <summary>Represents the Emoji property.</summary>
		[ValueName("Emoji"), Display(Name = "Emoji")]
		Emoji = 0x1,
		/// <summary>Represents the Emoji property.</summary>
		[ValueName("Emoji_Presentation"), Display(Name = "Emoji_Presentation")]
		EmojiPresentation = 0x2,
		/// <summary>Represents the Emoji property.</summary>
		[ValueName("Emoji_Modifier_Base"), Display(Name = "Emoji_Modifier_Base")]
		EmojiModifierBase = 0x4,
		/// <summary>Represents the Emoji property.</summary>
		[ValueName("Emoji_Component"), Display(Name = "Emoji_Component")]
		EmojiComponent = 0x8,
	}
}
