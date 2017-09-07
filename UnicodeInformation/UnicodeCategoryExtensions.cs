using System.Globalization;

namespace System.Unicode
{
	/// <summary>Provides extensions to the <see cref="UnicodeCategory"/> type.</summary>
	public static class UnicodeCategoryExtensions
	{
		/// <summary>Gets the short name of the unicode category.</summary>
		/// <param name="category">The category whose short name should be retrieved.</param>
		/// <returns>The short name of the unicode category.</returns>
		public static string GetShortName(this UnicodeCategory category)
		{
			return UnicodeCategoryInfo.Get(category).ShortName;
		}

		/// <summary>Gets the long name of the unicode category.</summary>
		/// <param name="category">The category whose long name should be retrieved.</param>
		/// <returns>The long name of the unicode category.</returns>
		public static string GetLongName(this UnicodeCategory category)
		{
			return UnicodeCategoryInfo.Get(category).LongName;
		}
	}
}
