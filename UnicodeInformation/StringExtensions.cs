using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public static class StringExtensions
	{
		public static CodePointEnumerable AsCodePointEnumerable(this string s)
		{
			return new CodePointEnumerable(s);
		}
	}
}
