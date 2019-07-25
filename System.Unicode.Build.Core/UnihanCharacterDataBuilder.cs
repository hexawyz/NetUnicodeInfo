using System.Collections.Generic;
using System.IO;

namespace System.Unicode.Build.Core
{
	public sealed class UnihanCharacterDataBuilder
	{
		public int CodePoint { get; }
		public UnihanNumericType NumericType { get; set; }
		public long NumericValue { get; set; }
		public string Definition { get; set; }
		public string MandarinReading { get; set; }
		public string CantoneseReading { get; set; }
		public string JapaneseKunReading { get; set; }
		public string JapaneseOnReading { get; set; }
		public string KoreanReading { get; set; }
		public string HangulReading { get; set; }
		public string VietnameseReading { get; set; }
		public string SimplifiedVariant { get; set; }
		public string TraditionalVariant { get; set; }
		public IList<UnicodeRadicalStrokeCount> UnicodeRadicalStrokeCounts => _unicodeRadicalStrokeCounts;

		private readonly List<UnicodeRadicalStrokeCount> _unicodeRadicalStrokeCounts = new List<UnicodeRadicalStrokeCount>();

		internal UnihanCharacterDataBuilder(int codePoint) => CodePoint = codePoint;

		internal UnihanCharacterData ToCharacterData()
			=> new UnihanCharacterData
			(
				CodePoint,
				NumericType,
				NumericValue,
				_unicodeRadicalStrokeCounts.ToArray(),
				Definition,
				MandarinReading,
				CantoneseReading,
				JapaneseKunReading,
				JapaneseOnReading,
				KoreanReading,
				HangulReading,
				VietnameseReading,
				SimplifiedVariant,
				TraditionalVariant
			);

		internal void WriteToFile(BinaryWriter writer)
		{
			UnihanFields fields = default;

			fields |= (UnihanFields)NumericType;
			// For now, we have enough bits to encode the length of the array in the field specifier, so we'll do that.
			// (NB: A quick analysis of the files revealed thare there are almost always exactly one Radical/Stroke count, and occasionally two, yet never more.)
			if (_unicodeRadicalStrokeCounts.Count > 0)
			{
				if (_unicodeRadicalStrokeCounts.Count == 1) fields |= UnihanFields.UnicodeRadicalStrokeCount;
				else if (_unicodeRadicalStrokeCounts.Count == 2) fields |= UnihanFields.UnicodeRadicalStrokeCountTwice;
				else fields |= UnihanFields.UnicodeRadicalStrokeCountMore;
			}
			if (Definition != null) fields |= UnihanFields.Definition;
			if (MandarinReading != null) fields |= UnihanFields.MandarinReading;
			if (CantoneseReading != null) fields |= UnihanFields.CantoneseReading;
			if (JapaneseKunReading != null) fields |= UnihanFields.JapaneseKunReading;
			if (JapaneseOnReading != null) fields |= UnihanFields.JapaneseOnReading;
			if (KoreanReading != null) fields |= UnihanFields.KoreanReading;
			if (HangulReading != null) fields |= UnihanFields.HangulReading;
			if (VietnameseReading != null) fields |= UnihanFields.VietnameseReading;
			if (SimplifiedVariant != null) fields |= UnihanFields.SimplifiedVariant;
			if (TraditionalVariant != null) fields |= UnihanFields.TraditionalVariant;

			writer.Write((ushort)fields);

			writer.WriteCodePoint(UnihanCharacterData.PackCodePoint(CodePoint));
			if ((fields & UnihanFields.OtherNumeric) != 0) writer.Write(NumericValue);

			if ((fields & UnihanFields.UnicodeRadicalStrokeCountMore) != 0)
			{
				if ((fields & (UnihanFields.UnicodeRadicalStrokeCountMore)) == UnihanFields.UnicodeRadicalStrokeCountMore)
					writer.Write(checked((byte)(_unicodeRadicalStrokeCounts.Count - 3)));

				foreach (var radicalStrokeCount in _unicodeRadicalStrokeCounts)
				{
					writer.Write(radicalStrokeCount.Radical);
					writer.Write(radicalStrokeCount.RawStrokeCount);
				}
			}

			if ((fields & UnihanFields.Definition) != 0) writer.Write(Definition);
			if ((fields & UnihanFields.MandarinReading) != 0) writer.Write(MandarinReading);
			if ((fields & UnihanFields.CantoneseReading) != 0) writer.Write(CantoneseReading);
			if ((fields & UnihanFields.JapaneseKunReading) != 0) writer.Write(JapaneseKunReading);
			if ((fields & UnihanFields.JapaneseOnReading) != 0) writer.Write(JapaneseOnReading);
			if ((fields & UnihanFields.KoreanReading) != 0) writer.Write(KoreanReading);
			if ((fields & UnihanFields.HangulReading) != 0) writer.Write(HangulReading);
			if ((fields & UnihanFields.VietnameseReading) != 0) writer.Write(VietnameseReading);
			if ((fields & UnihanFields.SimplifiedVariant) != 0) writer.Write(SimplifiedVariant);
			if ((fields & UnihanFields.TraditionalVariant) != 0) writer.Write(TraditionalVariant);
		}
	}
}
