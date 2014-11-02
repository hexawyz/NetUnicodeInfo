using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public static class UnicodeCategoryExtensions
	{
		public static string GetShortName(this UnicodeCategory category)
		{
			return UnicodeCategoryInfo.Get(category).ShortName;
		}

		public static string GetLongName(this UnicodeCategory category)
		{
			return UnicodeCategoryInfo.Get(category).LongName;
		}
	}
}
