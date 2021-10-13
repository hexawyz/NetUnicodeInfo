using System.Collections;
using System.Collections.Generic;

namespace System.Unicode
{
	/// <summary>Supports a standard iteration of code points in a <see cref="string"/>.</summary>
	public struct CodePointEnumerator : IEnumerator<int>
	{
		private readonly string _text;
		private int _current;
		private int _index;

		/// <summary>Initializes a new instance of the <see cref="PermissiveCodePointEnumerator"/> struct.</summary>
		/// <param name="text">The text whose code point should be enumerated.</param>
		/// <exception cref="ArgumentNullException"><paramref cref="_text"/> is <see langword="null"/>.</exception>
		public CodePointEnumerator(string text)
		{
			_text = text ?? throw new ArgumentNullException(nameof(text));
			_current = 0;
			_index = -1;
		}

		/// <summary>Gets the element in the collection at the current position of the enumerator..</summary>
		/// <value>The element in the collection at the current position of the enumerator.</value>
		public int Current => _current;

		object IEnumerator.Current => _current;

		void IDisposable.Dispose() { }

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the collection.</returns>
		public bool MoveNext()
		{
			if (_index < _text.Length && (_index += _current > 0xFFFF ? 2 : 1) < _text.Length)
			{
				_current = char.ConvertToUtf32(_text, _index);
				return true;
			}
			else
			{
				_current = 0;
				return false;
			}
		}

		void IEnumerator.Reset() => (_current, _index) = (0, -1);
	}
}
