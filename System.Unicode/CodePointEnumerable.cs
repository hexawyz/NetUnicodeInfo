using System.Collections;
using System.Collections.Generic;

namespace System.Unicode
{
	/// <summary>Allows enumeration of the code points contained in an encapsulated string.</summary>
	/// <remarks>
	/// This enumerable will only allow enumeration of valid UTF-16 strings.
	/// For incomplete or invalid UTF-16 strings, please use <see cref="PermissiveCodePointEnumerable"/> instead.
	/// </remarks>
	public readonly struct CodePointEnumerable : IEnumerable<int>
	{
		/// <summary>Initializes a new instance of the struct <see cref="CodePointEnumerable"/>.</summary>
		/// <param name="text">The string whose code points must be enumerated.</param>
		public CodePointEnumerable(string text) => Text = text ?? throw new ArgumentNullException(nameof(text));

		/// <summary>Gets the text whose code points are being enumerated.</summary>
		public string Text { get; }

		/// <summary>Gets an enumerator which can be used to enumerate the code points in the text.</summary>
		public CodePointEnumerator GetEnumerator() => new CodePointEnumerator(Text);

		IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
