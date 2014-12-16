using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Represents the value of the Numeric_Type property.</summary>
	public enum UnicodeNumericType : byte
	{
		/// <summary>The code point has no numeric value.</summary>
		None = 0,
		/// <summary>The code point represents a decimal digit which is part of a contiguous ascending range of characters from 0 to 9, and can be used in a decimal radix positional numeral system.</summary>
		Decimal = 1,
		/// <summary>The code point represents a digit between 0 and 9 and requires special handling.</summary>
		Digit = 2,
		/// <summary>The code point represents another kind of numeric value.</summary>
		Numeric = 3
	}
}
