using System;
using System.Globalization;
using System.Unicode;
using System.Windows.Data;

namespace UnicodeCharacterInspector
{
	internal sealed class RadicalStrokeCountToCharConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return null;

			var radicalStrokeCount = (UnicodeRadicalStrokeCount)value;
			var radical = UnicodeInfo.GetCjkRadicalInfo(radicalStrokeCount.Radical);

			return radicalStrokeCount.IsSimplified ? radical.TraditionalRadicalCodePoint : radical.SimplifiedRadicalCodePoint;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
