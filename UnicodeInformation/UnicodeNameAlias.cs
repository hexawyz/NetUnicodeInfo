namespace System.Unicode
{
	public struct UnicodeNameAlias
	{
		internal static readonly UnicodeNameAlias[] EmptyArray = new UnicodeNameAlias[0];

		public string Name { get; }
		public UnicodeNameAliasKind Kind { get; }

		internal UnicodeNameAlias(string name, UnicodeNameAliasKind kind)
		{
			Name = name;
			Kind = kind;
		}
	}
}
