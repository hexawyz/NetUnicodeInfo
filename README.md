# .NET Unicode Information Library

[![Build Status](https://dev.azure.com/goldencrystal/UnicodeInformation/_apis/build/status/GoldenCrystal.NetUnicodeInfo?branchName=master)](https://dev.azure.com/goldencrystal/UnicodeInformation/_build/latest?definitionId=1&branchName=master)

## Summary

This project consists of a library that provides access to some of the data contained in the Unicode Character Database.

## Version of Unicode supported

Unicode 12.1
Emoji 12.0

## Breaking changes from versions 1.x to 2.x

UnicodeRadicalStrokeCount.StrokeCount is now of type System.SByte instead of type System.Byte.

## Using the library

### Reference the NuGet package

Grab the latest version of the package on NuGet: https://www.nuget.org/packages/UnicodeInformation/.
Once the library is installed in your project, you will find everything you need in the System.Unicode namespace.

### Basic information

Everything provided by the library will be under the namespace `System.Unicode`.
XML documentation should be complete enough so that you can navigate the API without getting lost.

In its current state, the project is written in C# 7.3, compilable by [Roslyn](http://roslyn.codeplex.com/), and targets both .NET Standard 2.0 and .NET Standard 1.1.
The library UnicodeInformation includes a (large) subset of the official [Unicode Character Database](http://www.unicode.org/Public/UCD/latest/) stored in a custom file format.

### Example usage

The following program will display informations on a few characters:

```csharp
using System;
using System.Text;
using System.Unicode;

namespace Example
{
	internal static class Program
	{
		private static void Main()
		{
			Console.OutputEncoding = Encoding.Unicode;
			PrintCodePointInfo('A');
			PrintCodePointInfo('∞');
			PrintCodePointInfo(0x1F600);
		}

		private static void PrintCodePointInfo(int codePoint)
		{
			var charInfo = UnicodeInfo.GetCharInfo(codePoint);
			Console.WriteLine(UnicodeInfo.GetDisplayText(charInfo));
			Console.WriteLine("U+" + codePoint.ToString("X4"));
			Console.WriteLine(charInfo.Name ?? charInfo.OldName);
			Console.WriteLine(charInfo.Category);
		}
	}
}
```

Explanations:

* `UnicodeInfo.GetCharInfo(int)` returns a structure `UnicodeCharInfo` that provides access to various bit of information associated with the specified code point.
* `UnicodeInfo.GetDisplayText(UnicodeCharInfo)` is a helper method that computes a display text for the specified code point.
  Since some code points are not designed to be displayed in a standalone fashion, this will try to make the specified character more displayable.
  The algorithm used to provide a display text is quite simplistic, and will only affect very specific code points. (e.g. Control Characters)
  For most code points, this will simply return the direct string representation.
* `UnicodeCharInfo.Name` returns the name of the code point as specified by the Unicode standard.
  Please note that some characters will, by design, not have any name assigned to them in the standard. (e.g. control characters)
  Those characters, however may have alternate names assigned to them, that you can use as fallbacks. (e.g. `UnicodeCharInfo.OldName`)
* `UnicodeCharInfo.OldName` returns the name of the character as defined in Unicode 1.0, when applicable and different from the current name.
* `UnicodeCharInfo.Category` returns the category assigned to the specified code point.


### Included Properties

#### From UCD
* Name
* General_Category
* Canonical_Combining_Class
* Bidi_Class
* Decomposition_Type
* Decomposition_Mapping
* Numeric_Type (See also kAccountingNumeric/kOtherNumeric/kPrimaryNumeric. Those will set Numeric_Type to Numeric.)
* Numeric_Value
* Bidi_Mirrored
* Unicode_1_Name
* Simple_Uppercase_Maping
* Simple_Lowercase_Mapping
* Simple_Titlecase_Mapping
* Name_Alias
* Block
* ASCII_Hex_Digit
* Bidi_Control
* Dash
* Deprecated
* Diacritic
* Extender
* Hex_Digit
* Hyphen
* Ideographic
* IDS_Binary_Operator
* IDS_Trinary_Operator
* Join_Control
* Logical_Order_Exception
* Noncharacter_Code_Point
* Other_Alphabetic
* Other_Default_Ignorable_Code_Point
* Other_Grapheme_Extend
* Other_ID_Continue
* Other_ID_Start
* Other_Lowercase
* Other_Math
* Other_Uppercase
* Pattern_Syntax
* Pattern_White_Space
* Quotation_Mark
* Radical
* Soft_Dotted
* STerm
* Terminal_Punctuation
* Unified_Ideograph
* Variation_Selector
* White_Space
* Lowercase
* Uppercase
* Cased
* Case_Ignorable
* Changes_When_Lowercased
* Changes_When_Uppercased
* Changes_When_Titlecased
* Changes_When_Casefolded
* Changes_When_Casemapped
* Alphabetic
* Default_Ignorable_Code_Point
* Grapheme_Base
* Grapheme_Extend
* Grapheme_Link
* Math
* ID_Start
* ID_Continue
* XID_Start
* XID_Continue
* Unicode_Radical_Stroke (This is actually kRSUnicode from the Unihan database)
* Code point cross references extracted from NamesList.txt

NB: The UCD property ISO_Comment will never be included since this one is empty in all new Unicode versions.

#### From Unicode Emoji

* Emoji
* Emoji_Presentation
* Emoji_Modifier
* Emoji_Modifier_Base
* Emoji_Component
* Extended_Pictographic

#### From Unihan
* kAccountingNumeric
* kOtherNumeric
* kPrimaryNumeric
* kRSUnicode
* kDefinition
* kMandarin
* kCantonese
* kJapaneseKun
* kJapaneseOn
* kKorean
* kHangul
* kVietnamese
* kSimplifiedVariant
* kTraditionalVariant

### Regenerating  the data
The project UnicodeInformation.Builder takes cares of generating a file named ucd.dat. This file contains Unicode data compressed by .NET's deflate algorithm, and should be included in UnicodeInformation.dll at compilation.
