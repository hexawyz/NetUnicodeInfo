using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public static class CharExtensions
	{
		public static bool IsHexDigit(this char c)
		{
			return c >= '0' && c <= 'f' && (c <= '9' || c <= 'F' && c >= 'A' || c >= 'a');
		}
	}
}
