using System.Diagnostics;

namespace System.Unicode
{
	/// <summary>Represents a Unicode block.</summary>
	[DebuggerDisplay("[{CodePointRange.ToString(),nq}] {Name,nq}")]
	public struct UnicodeBlock
	{
		/// <summary>The code point range of this block.</summary>
		public readonly UnicodeCodePointRange CodePointRange;
		/// <summary>The name of this block.</summary>
		public readonly string Name;

		internal UnicodeBlock(UnicodeCodePointRange codePointRange, string name)
		{
			CodePointRange = codePointRange;
			Name = name;
		}
	}
}
