using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Represents a Unicode block.</summary>
	public struct UnicodeBlock
	{
		/// <summary>The code point range of this block.</summary>
		public readonly UnicodeCodePointRange CodePointRange;
		/// <summary>The name of this block.</summary>
		public readonly string Name;

		internal UnicodeBlock(UnicodeCodePointRange codePointRange, string name)
		{
			this.CodePointRange = codePointRange;
			this.Name = name;
		}
	}
}
