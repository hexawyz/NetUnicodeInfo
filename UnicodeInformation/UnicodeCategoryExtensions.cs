using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Globalization
{
	public static class UnicodeCategoryExtensions
	{
		public static string GetShortName(this UnicodeCategory category)
		{
			return UnicodeInformation.UnicodeCategoryInfo.Get(category).ShortName;
		}

		public static string GetLongName(this UnicodeCategory category)
		{
			return UnicodeInformation.UnicodeCategoryInfo.Get(category).LongName;
		}
	}
}
