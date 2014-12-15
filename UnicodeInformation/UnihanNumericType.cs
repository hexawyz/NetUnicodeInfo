using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Represents the different numeric types from the Unihan database.</summary>
	public enum UnihanNumericType : byte
	{
		/// <summary>Indicates that there is no Unihan numeric property defined for the code point.</summary>
		None = 0,
		/// <summary>Indicates that the propery kPrimaryNumeric is defined for this code point.</summary>
		/// <remarks>The kPrimaryNumeric property is used for ideographs wich are standard numerals.</remarks>
		[ValueName("kPrimaryNumeric")]
		Primary = 1,
		/// <summary>Indicates that the propery kAccountingNumeric is defined for this code point.</summary>
		/// <remarks>The kAccountingNumeric property is used for ideographs used as accounting numerals.</remarks>
		[ValueName("kAccountingNumeric")]
		Accounting = 2,
		/// <summary>Indicates that the propery kOtherNumeric is defined for this code point.</summary>
		/// <remarks>The kOtherNumeric property is used for ideographs wich are used as numerals in non common contexts.</remarks>
		[ValueName("kOtherNumeric")]
		Other = 3,
	}
}
