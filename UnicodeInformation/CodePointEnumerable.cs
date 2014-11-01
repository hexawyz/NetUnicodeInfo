using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public struct CodePointEnumerable : IEnumerable<int>
	{
		private readonly string text;

		public CodePointEnumerable(string text)
		{
			this.text = text;
		}

		public string Text { get { return text; } }

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
