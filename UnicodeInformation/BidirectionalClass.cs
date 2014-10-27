using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public enum BidirectionalClass : byte
	{
		[Display(Name = "Left_To_Right", Description = "any strong left-to-right character")]
		LeftToRight,
		[Display(Name = "Right_To_Left", Description = "any strong right-to-left (non-Arabic-type) character")]
		RightToLeft,
		[Display(Name = "Arabic_Letter", Description = "any strong right-to-left (Arabic-type) character")]
		ArabicLetter,
		[Display(Name = "European_Number", Description = "any ASCII digit or Eastern Arabic-Indic digit")]
		EuropeanNumber,
		[Display(Name = "European_Separator", Description = "plus and minus signs")]
		EuropeanSeparator,
		[Display(Name = "European_Terminator", Description = "a terminator in a numeric format context, includes currency signs")]
		EuropeanTerminator,
		[Display(Name = "Arabic_Number", Description = "any Arabic-Indic digit")]
		ArabicNumber,
		[Display(Name = "Common_Separator", Description = "commas, colons, and slashes")]
		CommonSeparator,
		[Display(Name = "Nonspacing_Mark", Description = "any nonspacing mark")]
		NonspacingMark,
		[Display(Name = "Boundary_Neutral", Description = "most format characters, control codes, or noncharacters")]
		BoundaryNeutral,
		[Display(Name = "Paragraph_Separator", Description = "various newline characters")]
		ParagraphSeparator,
		[Display(Name = "Segment_Separator", Description = "various segment-related control codes")]
		SegmentSeparator,
		[Display(Name = "White_Space", Description = "spaces")]
		WhiteSpace,
		[Display(Name = "Other_Neutral", Description = "most other symbols and punctuation marks")]
		OtherNeutral,
		[Display(Name = "Left_To_Right_Embedding", Description = "U+202A: the LR embedding control")]
		LeftToRightEmbedding,
		[Display(Name = "Left_To_Right_Override", Description = "U+202D: the LR override control")]
		LeftToRightOverride,
		[Display(Name = "Right_To_Left_Embedding", Description = "U+202B: the RL embedding control")]
		RightToLeftEmbedding,
		[Display(Name = "Right_To_Left_Override", Description = "U+202E: the RL override control")]
		RightToLeftOverride,
		[Display(Name = "Pop_Directional_Format", Description = "U+202C: terminates an embedding or override control")]
		PopDirectionalFormat,
		[Display(Name = "Left_To_Right_Isolate", Description = "U+2066: the LR isolate control")]
		LeftToRightIsolate,
		[Display(Name = "Right_To_Left_Isolate", Description = "U+2067: the RL isolate control")]
		RightToLeftIsolate,
		[Display(Name = "First_Strong_Isolate", Description = "U+2068: the first strong isolate control")]
		FirstStrongIsolate,
		[Display(Name = "Pop_Directional_Isolate", Description = "U+2069: terminates an isolate control")]
		PopDirectionalIsolate,
	}
}
