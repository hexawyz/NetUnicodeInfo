using System.Diagnostics;
using System.Linq;

namespace System.Unicode
{
	/// <summary>Represents a name alias for an Unicode code point.</summary>
	[DebuggerDisplay("{DisplayText,nq}")]
	public struct UnicodeNameAlias
	{
		internal static readonly UnicodeNameAlias[] EmptyArray = new UnicodeNameAlias[0];

		/// <summary>Gets the alias name.</summary>
		/// <value>The name.</value>
		public string Name { get; }
		/// <summary>Gets the kind of alias.</summary>
		/// <value>The kind of alias.</value>
		public UnicodeNameAliasKind Kind { get; }

		private string DisplayText => (Kind != 0 ? "<" + EnumHelper<UnicodeNameAliasKind>.GetValueNames(Kind).FirstOrDefault() + "> " : string.Empty) + Name;

		internal UnicodeNameAlias(string name, UnicodeNameAliasKind kind)
		{
			Name = name;
			Kind = kind;
		}
	}
}
