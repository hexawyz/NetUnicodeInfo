using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public static class StringExtensions
	{
		public static CodePointEnumerable AsCodePointEnumerable(this string s)
		{
			return new CodePointEnumerable(s);
		}

		public static PermissiveCodePointEnumerable AsPermissiveCodePointEnumerable(this string s)
		{
			return new PermissiveCodePointEnumerable(s);
		}
	}
}
