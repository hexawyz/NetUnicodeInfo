namespace System.Unicode
{
	/// <summary>Contains extension methods applicable to the <see cref="string"/> type.</summary>
	public static class StringExtensions
	{
		/// <summary>Encapsulates the string in an object which can be used to enumerate code points.</summary>
		/// <remarks>
		/// The enumerable returned by this method enumerates code points in a strict manner.
		/// If the string contains lone surrogates, the enumeration will throw.
		/// </remarks>
		/// <param name="s">The string to encapsulate.</param>
		/// <returns>An enumerable object, which can be used to enumerate code points in the string.</returns>
		public static CodePointEnumerable AsCodePointEnumerable(this string s) => new(s);

		/// <summary>Encapsulates the string in an object which can be used to enumerate code points in a permissive way.</summary>
		/// <remarks>
		/// The enumerable returned by this method is permissive, regarding the code points represented.
		/// It allows invalid sequences, such as lone surrogates, the enumeration will handle those gracefully.
		/// </remarks>
		/// <param name="s">The string to encapsulate.</param>
		/// <returns>An enumerable object, which can be used to enumerate code points in the string.</returns>
		public static PermissiveCodePointEnumerable AsPermissiveCodePointEnumerable(this string s) => new(s);
	}
}
