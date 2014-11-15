using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	[Flags]
	internal enum UnihanFields : ushort
	{
		// NumericType / NumericValue : Not exactly a bit mask here…
		PrimaryNumeric = 1,
		AccountingNumeric = 2,
		OtherNumeric = 3,

		Definition = 4,
		MandarinReading = 8,
		CantoneseReading = 16,
		JapaneseKunReading = 32,
		JapaneseOnReading = 64,
		KoreanReading = 128,
		HangulReading = 256,
		VietnameseReading = 512,

		SimplifiedVariant = 1024,
		TraditionalVariant = 2048,
	}
}
