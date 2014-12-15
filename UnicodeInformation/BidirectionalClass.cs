using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Represents possible values for the Bidi_Class unicode property.</summary>
	public enum BidirectionalClass : byte
	{
		/// <summary>Represents the value Left_To_Right.</summary>
		/// <remarks>Any strong left-to-right character.</remarks>
		[ValueName("L"), ValueName("Left_To_Right"), Display(Name = "Left_To_Right", Description = "Any strong left-to-right character.")]
		LeftToRight,
		/// <summary>Represents the value Right_To_Left.</summary>
		/// <remarks>Any strong right-to-left (non-Arabic-type) character.</remarks>
		[ValueName("R"), ValueName("Right_To_Left"), Display(Name = "Right_To_Left", Description = "Any strong right-to-left (non-Arabic-type) character.")]
		RightToLeft,
		/// <summary>Represents the value Arabic_Letter.</summary>
		/// <remarks>Any strong right-to-left (Arabic-type) character.</remarks>
		[ValueName("AL"), ValueName("Arabic_Letter"), Display(Name = "Arabic_Letter", Description = "Any strong right-to-left (Arabic-type) character.")]
		ArabicLetter,
		/// <summary>Represents the value European_Number.</summary>
		/// <remarks>Any ASCII digit or Eastern Arabic-Indic digit.</remarks>
		[ValueName("EN"), ValueName("European_Number"), Display(Name = "European_Number", Description = "Any ASCII digit or Eastern Arabic-Indic digit.")]
		EuropeanNumber,
		/// <summary>Represents the value European_Separator.</summary>
		/// <remarks>Plus and minus signs.</remarks>
		[ValueName("ES"), ValueName("European_Separator"), Display(Name = "European_Separator", Description = "Plus and minus signs.")]
		EuropeanSeparator,
		/// <summary>Represents the value European_Terminator.</summary>
		/// <remarks>A terminator in a numeric format context, includes currency signs.</remarks>
		[ValueName("ET"), ValueName("European_Terminator"), Display(Name = "European_Terminator", Description = "A terminator in a numeric format context, includes currency signs.")]
		EuropeanTerminator,
		/// <summary>Represents the value Arabic_Number.</summary>
		/// <remarks>Any Arabic-Indic digit.</remarks>
		[ValueName("AN"), ValueName("Arabic_Number"), Display(Name = "Arabic_Number", Description = "Any Arabic-Indic digit.")]
		ArabicNumber,
		/// <summary>Represents the value Common_Separator.</summary>
		/// <remarks>Commas, colons, and slashes.</remarks>
		[ValueName("CS"), ValueName("Common_Separator"), Display(Name = "Common_Separator", Description = "Commas, colons, and slashes.")]
		CommonSeparator,
		/// <summary>Represents the value Nonspacing_Mark.</summary>
		/// <remarks>Any nonspacing mark.</remarks>
		[ValueName("NSM"), ValueName("Nonspacing_Mark"), Display(Name = "Nonspacing_Mark", Description = "Any nonspacing mark.")]
		NonSpacingMark,
		/// <summary>Represents the value Boundary_Neutral.</summary>
		/// <remarks>Most format characters, control codes, or noncharacters.</remarks>
		[ValueName("BN"), ValueName("Boundary_Neutral"), Display(Name = "Boundary_Neutral", Description = "Most format characters, control codes, or noncharacters.")]
		BoundaryNeutral,
		/// <summary>Represents the value Paragraph_Separator.</summary>
		/// <remarks>Various newline characters.</remarks>
		[ValueName("B"), ValueName("Paragraph_Separator"), Display(Name = "Paragraph_Separator", Description = "Various newline characters.")]
		ParagraphSeparator,
		/// <summary>Represents the value Segment_Separator.</summary>
		/// <remarks>Various segment-related control codes.</remarks>
		[ValueName("S"), ValueName("Segment_Separator"), Display(Name = "Segment_Separator", Description = "Various segment-related control codes.")]
		SegmentSeparator,
		/// <summary>Represents the value White_Space.</summary>
		/// <remarks>Spaces.</remarks>
		[ValueName("WS"), ValueName("White_Space"), Display(Name = "White_Space", Description = "Spaces.")]
		WhiteSpace,
		/// <summary>Represents the value Other_Neutral.</summary>
		/// <remarks>Most other symbols and punctuation marks.</remarks>
		[ValueName("ON"), ValueName("Other_Neutral"), Display(Name = "Other_Neutral", Description = "Most other symbols and punctuation marks.")]
		OtherNeutral,
		/// <summary>Represents the value Left_To_Right_Embedding.</summary>
		/// <remarks>U+202A: the LR embedding control.</remarks>
		[ValueName("LRE"), ValueName("Left_To_Right_Embedding"), Display(Name = "Left_To_Right_Embedding", Description = "U+202A: the LR embedding control.")]
		LeftToRightEmbedding,
		/// <summary>Represents the value Left_To_Right_Override.</summary>
		/// <remarks>U+202D: the LR override control.</remarks>
		[ValueName("LRO"), ValueName("Left_To_Right_Override"), Display(Name = "Left_To_Right_Override", Description = "U+202D: the LR override control.")]
		LeftToRightOverride,
		/// <summary>Represents the value Right_To_Left_Embedding.</summary>
		/// <remarks>U+202B: the RL embedding control.</remarks>
		[ValueName("RLE"), ValueName("Right_To_Left_Embedding"), Display(Name = "Right_To_Left_Embedding", Description = "U+202B: the RL embedding control.")]
		RightToLeftEmbedding,
		/// <summary>Represents the value Right_To_Left_Override.</summary>
		/// <remarks>U+202E: the RL override control.</remarks>
		[ValueName("RLO"), ValueName("Right_To_Left_Override"), Display(Name = "Right_To_Left_Override", Description = "U+202E: the RL override control.")]
		RightToLeftOverride,
		/// <summary>Represents the value Pop_Directional_Format.</summary>
		/// <remarks>U+202C: terminates an embedding or override control.</remarks>
		[ValueName("PDF"), ValueName("Pop_Directional_Format"), Display(Name = "Pop_Directional_Format", Description = "U+202C: terminates an embedding or override control.")]
		PopDirectionalFormat,
		/// <summary>Represents the value Left_To_Right_Isolate.</summary>
		/// <remarks>U+2066: the LR isolate control.</remarks>
		[ValueName("LRI"), ValueName("Left_To_Right_Isolate"), Display(Name = "Left_To_Right_Isolate", Description = "U+2066: the LR isolate control.")]
		LeftToRightIsolate,
		/// <summary>Represents the value Right_To_Left_Isolate.</summary>
		/// <remarks>U+2067: the RL isolate control.</remarks>
		[ValueName("RLI"), ValueName("Right_To_Left_Isolate"), Display(Name = "Right_To_Left_Isolate", Description = "U+2067: the RL isolate control.")]
		RightToLeftIsolate,
		/// <summary>Represents the value First_Strong_Isolate.</summary>
		/// <remarks>U+2068: the first strong isolate control.</remarks>
		[ValueName("FSI"), ValueName("First_Strong_Isolate"), Display(Name = "First_Strong_Isolate", Description = "U+2068: the first strong isolate control.")]
		FirstStrongIsolate,
		/// <summary>Represents the value Pop_Directional_Isolate.</summary>
		/// <remarks>U+2069: terminates an isolate control.</remarks>
		[ValueName("PDI"), ValueName("Pop_Directional_Isolate"), Display(Name = "Pop_Directional_Isolate", Description = "U+2069: terminates an isolate control.")]
		PopDirectionalIsolate,
	}
}
