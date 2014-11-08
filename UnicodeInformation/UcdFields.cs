using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Represents the fields available for an UCD entry.</summary>
	/// <remarks>Not all the enumeration member directly map to a field.</remarks>
	[Flags]
	internal enum UcdFields : ushort
	{
		// Not really a field, just here to indicate that the entry is a range
        CodePointRange = 1,
		
		Name = 2,
		Category = 4,
		CanonicalCombiningClass = 8,
		BidirectionalClass = 16,
		DecompositionMapping = 32,

		// NumericType / NumericValue : Not exactly a bit mask here… More like [0…3] << 6
		NumericDecimal = 64,
		NumericDigit = 128,
        NumericNumeric = 192,

		// This is a yes/no field, so obviously, no extra storage is required for this one…
		BidirectionalMirrored = 256,

		OldName = 512,
		SimpleUpperCaseMapping = 1024,
		SimpleLowerCaseMapping = 2048,
		SimpleTitleCaseMapping = 4096,

		ContributoryProperties = 8192,
		CoreProperties = 16384,
	}
}
