using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Supports a standard iteration of code points in a <see cref="string"/>.</summary>
	public struct CodePointEnumerator : IEnumerator<int>
	{
		private readonly string text;
		private int current;
		private int index;

		/// <summary>Initializes a new instance of the <see cref="PermissiveCodePointEnumerator"/> struct.</summary>
		/// <param name="text">The text whose code point should be enumerated.</param>
		/// <exception cref="ArgumentNullException"><paramref cref="text"/> is <see langword="null"/>.</exception>
		public CodePointEnumerator(string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			this.text = text;
			this.current = 0;
			this.index = -1;
		}

		/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
		/// <value>The element in the collection at the current position of the enumerator.</value>
		public int Current { get { return current; } }

		object IEnumerator.Current { get { return current; } }

		void IDisposable.Dispose() { }

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the collection.</returns>
		public bool MoveNext()
		{
			if (index < text.Length && (index += current > 0xFFFF ? 2 : 1) < text.Length)
			{
				current = char.ConvertToUtf32(text, index);
				return true;
			}
			else
			{
				current = 0;
				return false;
			}
		}

		void IEnumerator.Reset()
		{
			current = 0;
			index = -1;
		}
	}
}
