namespace System.Unicode
{
	/// <summary>Represents the fields available for an UCD entry.</summary>
	/// <remarks>Not all the enumeration member directly map to a field.</remarks>
	[Flags]
	internal enum UcdFields : ushort
	{
		// Not really a field, just here to indicate that the entry is a range
		CodePointRange = 0b_0000_0000_0000_0001,

		Name = 0b_0000_0000_0000_0010, // Will stand in for official name as well as related names.
		Category = 0b_0000_0000_0000_0100,
		CanonicalCombiningClass = 0b_0000_0000_0000_1000,
		BidirectionalClass = 0b_0000_0000_0001_0000,
		DecompositionMapping = 0b_0000_0000_0010_0000,

		// NumericType / NumericValue : Not exactly a bit mask here… More like [0…3] << 6
		NumericDecimal = 0b_0000_0000_0100_0000,
		NumericDigit = 0b_0000_0000_1000_0000,
		NumericNumeric = 0b_0000_0000_1100_0000,

		// This is a yes/no field, so obviously, no extra storage is required for this one…
		BidirectionalMirrored = 0b_0000_0001_0000_0000,

		OldName = 0b_0000_0010_0000_0000,
		SimpleUpperCaseMapping = 0b_0000_0100_0000_0000,
		SimpleLowerCaseMapping = 0b_0000_1000_0000_0000,
		SimpleTitleCaseMapping = 0b_0001_0000_0000_0000,

		ContributoryProperties = 0b_0010_0000_0000_0000,
		CorePropertiesAndEmojiProperties = 0b_0100_0000_0000_0000,

		CrossRerefences = 0b_1000_0000_0000_0000,
	}
}
