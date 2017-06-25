using System;
using System.Globalization;
using System.Unicode;
using System.Windows.Data;
using System.Windows.Media;

namespace UnicodeCharacterInspector
{
	internal sealed class CodePointToFontFamilyConverter : IValueConverter
	{
		public FontFamily DefaultFontFamily { get; set; }

		public FontFamily EmojiFontFamily { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is int codePoint && UnicodeInfo.GetCharInfo(codePoint).EmojiProperties != 0 ?
				EmojiFontFamily :
				DefaultFontFamily;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotSupportedException();
	}
}
