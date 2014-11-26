using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct PermissiveCodePointEnumerable : IEnumerable<int>
	{
		private readonly string text;

		public PermissiveCodePointEnumerable(string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			this.text = text;
		}

		public string Text { get { return text; } }

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
