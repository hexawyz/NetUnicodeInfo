using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct CodePointEnumerator : IEnumerator<int>
	{
		private readonly string text;
		private int current;
		private int index;

		public CodePointEnumerator(string text)
		{
			if (text == null) throw new ArgumentNullException("text");

			this.text = text;
			this.current = 0;
			this.index = -1;
		}

		public int Current { get { return current; } }

		object IEnumerator.Current { get { return current; } }

		void IDisposable.Dispose() { }

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
