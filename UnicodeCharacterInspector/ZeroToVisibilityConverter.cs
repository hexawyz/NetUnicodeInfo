using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UnicodeCharacterInspector
{
	internal sealed class ZeroToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null && System.Convert.ToInt32(value) != 0 ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
