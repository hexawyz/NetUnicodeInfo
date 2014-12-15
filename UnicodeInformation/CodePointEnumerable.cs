using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Allows enumeration of the code points contained in an encapsulated string.</summary>
	/// <remarks>
	/// This enumerable will only allow enumeration of valid UTF-16 strings.
	/// For incomplete or invalid UTF-16 strings, please use <see cref="PermissiveCodePointEnumerable"/> instead.
	/// </remarks>
	public struct CodePointEnumerable : IEnumerable<int>
	{
		private readonly string text;

		/// <summary>Initializes a new instance of the struct <see cref="CodePointEnumerable"/>.</summary>
		/// <param name="text">The string whose code points must be enumerated.</param>
		public CodePointEnumerable(string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			this.text = text;
		}

		/// <summary>Gets the text whose code points are being enumerated.</summary>
		public string Text { get { return text; } }

		/// <summary>Gets an enumerator which can be used to enumerate the code points in the text.</summary>
		public CodePointEnumerator GetEnumerator()
		{
			return new CodePointEnumerator(text);
		}

		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
