using System.Collections;
using Xunit;

namespace System.Unicode.Tests
{
	public class UnicodeCodePointRangeTests
	{
		[Theory]
		[InlineData(0, 0x10FFFF)]
		public void MultiCodePointRangeShouldHaveExpectedResults(int firstCodePoint, int lastCodePoint)
		{
			var range = new UnicodeCodePointRange(firstCodePoint, lastCodePoint);

			Assert.Equal(firstCodePoint, range.FirstCodePoint);
			Assert.Equal(lastCodePoint, range.LastCodePoint);
			Assert.False(range.IsSingleCodePoint);
		}

		[Theory]
		[InlineData((int)'A')]
		[InlineData(0x0)]
		[InlineData(0x10FFFF)]
		public void SingleCodePointRangeShouldHaveExpectedResults(int codePoint)
		{
			var range = new UnicodeCodePointRange(codePoint);

			Assert.Equal(codePoint, range.FirstCodePoint);
			Assert.Equal(codePoint, range.LastCodePoint);
			Assert.True(range.IsSingleCodePoint);
		}

		[Theory]
		[InlineData(0, 0, "0000")]
		[InlineData(0x1, 0x30, "0001..0030")]
		[InlineData(0x41, 0x5A, "0041..005A")]
		[InlineData(0x0, 0xFFFF, "0000..FFFF")]
		[InlineData(0xFFFF, 0xFFFF, "FFFF")]
		[InlineData(0xFFFF, 0x10000, "FFFF..10000")]
		[InlineData(0x10000, 0x10000, "10000")]
		[InlineData(0, 0xF0000, "0000..F0000")]
		[InlineData(0xFFFFF, 0xFFFFF, "FFFFF")]
		[InlineData(0, 0xFFFFF, "0000..FFFFF")]
		[InlineData(0, 0x10FFFF, "0000..10FFFF")]
		[InlineData(0xFFFF, 0x10FFFF, "FFFF..10FFFF")]
		[InlineData(0x1FFFF, 0x10FFFF, "1FFFF..10FFFF")]
		[InlineData(0x10FFFE, 0x10FFFF, "10FFFE..10FFFF")]
		[InlineData(0x10FFFF, 0x10FFFF, "10FFFF")]
		public void ToStringShouldProduceExpectedResultForCodePoints(int firstCodePoint, int lastCodePoint, string expectedResult)
		{
			var range = new UnicodeCodePointRange(firstCodePoint, lastCodePoint);

			Assert.Equal(expectedResult, range.ToString());
		}

		[Theory]
		[InlineData((int)'A', "0041")]
		[InlineData(0x0, "0000")]
		[InlineData(0xFFFF, "FFFF")]
		[InlineData(0x10000, "10000")]
		[InlineData(0x1FFFF, "1FFFF")]
		[InlineData(0xFFFFF, "FFFFF")]
		[InlineData(0x10FFFF, "10FFFF")]
		public void ToStringShouldProduceExpectedResultForCodePoint(int codePoint, string expectedResult)
		{
			var range = new UnicodeCodePointRange(codePoint);

			Assert.Equal(expectedResult, range.ToString());
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0x110000)]
		[InlineData(int.MaxValue)]
		public void ConstructorShouldFailForInvalidCodePoint(int codePoint)
			=> Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(codePoint));

		[Theory]
		[InlineData(-1, 10)]
		[InlineData(10, 0x110000)]
		[InlineData(-1, 0x110000)]
		public void ConstructorShouldFailForInvalidCodePoints(int firstCodePoint, int lastCodePoint)
			=> Assert.Throws<ArgumentOutOfRangeException>(() => new UnicodeCodePointRange(firstCodePoint, lastCodePoint));

		[Theory]
		[InlineData(0xA3F, 0x105F)]
		public void EnumerationShouldHaveExpectedResults(int firstCodePoint, int lastCodePoint)
		{
			// Generic test
			{
				int i = firstCodePoint;

				foreach (int n in new UnicodeCodePointRange(firstCodePoint, lastCodePoint))
				{
					Assert.Equal(i++, n);
				}
			}

			// Nongeneric test
			{
				int i = firstCodePoint;

				var enumerator = (IEnumerator)new UnicodeCodePointRange(firstCodePoint, lastCodePoint).GetEnumerator();

				while (enumerator.MoveNext())
				{
					Assert.Equal(i++, enumerator.Current);
				}

				enumerator.Reset();

				Assert.True(enumerator.MoveNext());
				Assert.Equal(firstCodePoint, enumerator.Current);
			}
		}
	}
}
