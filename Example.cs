using System;
using System.Text;
using System.Unicode;

namespace Example
{
	internal static class Program
	{
		private static void Main()
		{
			Console.OutputEncoding = Encoding.Unicode;
			PrintCodePointInfo('A');
			PrintCodePointInfo('∞');
			PrintCodePointInfo(0x1F600);
		}

		private static void PrintCodePointInfo(int codePoint)
		{
			var charInfo = UnicodeInfo.GetCharInfo(codePoint);
			Console.WriteLine(UnicodeInfo.GetDisplayText(charInfo));
			Console.WriteLine("U+" + codePoint.ToString("X4"));
			Console.WriteLine(charInfo.Name ?? charInfo.OldName);
			Console.WriteLine(charInfo.Category);
		}
	}
}
