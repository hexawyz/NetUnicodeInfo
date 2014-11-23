using System;
using System.Globalization;
using System.Unicode;
using System.Windows.Data;

namespace UnicodeCharacterInspector
{
	internal sealed class Utf32ToNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null ? UnicodeInfo.GetName((int)value) : null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
