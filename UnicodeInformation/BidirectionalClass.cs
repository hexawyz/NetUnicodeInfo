using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public enum BidirectionalClass : byte
	{
		[ValueName("L"), ValueName("Left_To_Right"), Display(Name = "Left_To_Right", Description = "any strong left-to-right character")]
		LeftToRight,
		[ValueName("R"), ValueName("Right_To_Left"), Display(Name = "Right_To_Left", Description = "any strong right-to-left (non-Arabic-type) character")]
		RightToLeft,
		[ValueName("AL"), ValueName("Arabic_Letter"), Display(Name = "Arabic_Letter", Description = "any strong right-to-left (Arabic-type) character")]
		ArabicLetter,
		[ValueName("EN"), ValueName("European_Number"), Display(Name = "European_Number", Description = "any ASCII digit or Eastern Arabic-Indic digit")]
		EuropeanNumber,
		[ValueName("ES"), ValueName("European_Separator"), Display(Name = "European_Separator", Description = "plus and minus signs")]
		EuropeanSeparator,
		[ValueName("ET"), ValueName("European_Terminator"), Display(Name = "European_Terminator", Description = "a terminator in a numeric format context, includes currency signs")]
		EuropeanTerminator,
		[ValueName("AN"), ValueName("Arabic_Number"), Display(Name = "Arabic_Number", Description = "any Arabic-Indic digit")]
		ArabicNumber,
		[ValueName("CS"), ValueName("Common_Separator"), Display(Name = "Common_Separator", Description = "commas, colons, and slashes")]
		CommonSeparator,
		[ValueName("NSM"), ValueName("Nonspacing_Mark"), Display(Name = "Nonspacing_Mark", Description = "any nonspacing mark")]
		NonspacingMark,
		[ValueName("BN"), ValueName("Boundary_Neutral"), Display(Name = "Boundary_Neutral", Description = "most format characters, control codes, or noncharacters")]
		BoundaryNeutral,
		[ValueName("B"), ValueName("Paragraph_Separator"), Display(Name = "Paragraph_Separator", Description = "various newline characters")]
		ParagraphSeparator,
		[ValueName("S"), ValueName("Segment_Separator"), Display(Name = "Segment_Separator", Description = "various segment-related control codes")]
		SegmentSeparator,
		[ValueName("WS"), ValueName("White_Space"), Display(Name = "White_Space", Description = "spaces")]
		WhiteSpace,
		[ValueName("ON"), ValueName("Other_Neutral"), Display(Name = "Other_Neutral", Description = "most other symbols and punctuation marks")]
		OtherNeutral,
		[ValueName("LRE"), ValueName("Left_To_Right_Embedding"), Display(Name = "Left_To_Right_Embedding", Description = "U+202A: the LR embedding control")]
		LeftToRightEmbedding,
		[ValueName("LRO"), ValueName("Left_To_Right_Override"), Display(Name = "Left_To_Right_Override", Description = "U+202D: the LR override control")]
		LeftToRightOverride,
		[ValueName("RLE"), ValueName("Right_To_Left_Embedding"), Display(Name = "Right_To_Left_Embedding", Description = "U+202B: the RL embedding control")]
		RightToLeftEmbedding,
		[ValueName("RLO"), ValueName("Right_To_Left_Override"), Display(Name = "Right_To_Left_Override", Description = "U+202E: the RL override control")]
		RightToLeftOverride,
		[ValueName("PDF"), ValueName("Pop_Directional_Format"), Display(Name = "Pop_Directional_Format", Description = "U+202C: terminates an embedding or override control")]
		PopDirectionalFormat,
		[ValueName("LRI"), ValueName("Left_To_Right_Isolate"), Display(Name = "Left_To_Right_Isolate", Description = "U+2066: the LR isolate control")]
		LeftToRightIsolate,
		[ValueName("RLI"), ValueName("Right_To_Left_Isolate"), Display(Name = "Right_To_Left_Isolate", Description = "U+2067: the RL isolate control")]
		RightToLeftIsolate,
		[ValueName("FSI"), ValueName("First_Strong_Isolate"), Display(Name = "First_Strong_Isolate", Description = "U+2068: the first strong isolate control")]
		FirstStrongIsolate,
		[ValueName("PDI"), ValueName("Pop_Directional_Isolate"), Display(Name = "Pop_Directional_Isolate", Description = "U+2069: terminates an isolate control")]
		PopDirectionalIsolate,
	}
}
