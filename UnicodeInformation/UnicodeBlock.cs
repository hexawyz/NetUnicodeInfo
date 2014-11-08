using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeBlock
	{
		public readonly UnicodeCharacterRange CodePointRange;
		public readonly string Name;

		internal UnicodeBlock(UnicodeCharacterRange codePointRange, string name)
		{
			this.CodePointRange = codePointRange;
			this.Name = name;
		}
    }
}
