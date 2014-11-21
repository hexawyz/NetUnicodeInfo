namespace System.Unicode
{
	public enum UnicodeNameAliasKind : byte
	{
		[ValueName("correction")]
		Correction = 1,
		[ValueName("control")]
		Control = 2,
		[ValueName("alternate")]
		Alternate = 3,
		[ValueName("figment")]
		Figment = 4,
		[ValueName("abbreviation")]
		Abbreviation = 5
	}
}
