using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Allows enumeration of the code points contained in an encapsulated string, even when this one contains lone surrogates.</summary>
	/// <remarks>
	/// This enumerable will allow enumeration of UTF-16 strings containing lone surrogates.
	/// For a more conformant enumeration of code points, please use <see cref="CodePointEnumerable"/> instead.
	/// </remarks>
	public struct PermissiveCodePointEnumerable : IEnumerable<int>
	{
		private readonly string text;

		/// <summary>Initializes a new instance of the struct <see cref="PermissiveCodePointEnumerable"/>.</summary>
		/// <param name="text">The string whose code points must be enumerated.</param>
		public PermissiveCodePointEnumerable(string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			this.text = text;
		}

		/// <summary>Gets the text whose code points are being enumerated.</summary>
		public string Text { get { return text; } }

		/// <summary>Gets an enumerator which can be used to enumerate the code points in the text.</summary>
		/// <returns></returns>
		public PermissiveCodePointEnumerator GetEnumerator()
		{
			return new PermissiveCodePointEnumerator(text);
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
