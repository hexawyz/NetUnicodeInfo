using System.ComponentModel.DataAnnotations;

namespace System.Unicode
{
	/// <summary>A bitmask of the various available emoji properties.</summary>
	/// <remarks>Emoji properties are not formally part of UCD, but .</remarks>
	[Flags]
	public enum EmojiProperties : byte
	{
		// ⚠️ Only 6 bits can be used here at the moment. Refactoring of the encoding is required to use 8 or more bits.
		// Reason: EmojiProperties does not have its own bit in UcdFields.

		/// <summary>Represents the Emoji property.</summary>
		[ValueName("Emoji"), Display(Name = "Emoji")]
		Emoji = 0b_00_0001,
		/// <summary>Represents the Emoji_Presentation property.</summary>
		[ValueName("Emoji_Presentation"), ValueName("EPres"), Display(Name = "Emoji_Presentation")]
		EmojiPresentation = 0b_00_0010,
		/// <summary>Represents the Emoji_Modifier property.</summary>
		[ValueName("Emoji_Modifier"), ValueName("EMod"), Display(Name = "Emoji_Modifier")]
		EmojiModifier = 0b_01_0000,
		/// <summary>Represents the Emoji_Modifier_Base property.</summary>
		[ValueName("Emoji_Modifier_Base"), ValueName("EBase"), Display(Name = "Emoji_Modifier_Base")]
		EmojiModifierBase = 0b_00_0100,
		/// <summary>Represents the Emoji_Component property.</summary>
		[ValueName("Emoji_Component"), ValueName("EComp"), Display(Name = "Emoji_Component")]
		EmojiComponent = 0b_00_1000,
		/// <summary>Represents the Extended_Pictographic property.</summary>
		[ValueName("Extended_Pictographic"), ValueName("ExtPict"), Display(Name = "Extended_Pictographic")]
		ExtendedPictographic = 0b_10_0000,
	}
}
