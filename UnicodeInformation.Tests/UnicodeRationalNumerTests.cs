using System;
using System.Collections.Generic;
using System.Unicode;
using Xunit;

namespace UnicodeInformation.Tests
{
	public class UnicodeRationalNumerTests
	{
		[Fact]
		public void DefaultValueShouldBeDetectedAsSuch()
		{
			Assert.True(default(UnicodeRationalNumber).IsDefaultValue);
			Assert.Equal(string.Empty, default(UnicodeRationalNumber).ToString());
		}

		public static readonly TheoryData<long> Numerators = new TheoryData<long>
		{
			0,
			1,
			long.MaxValue,
			long.MinValue
		};

		[Theory]
		[MemberData(nameof(Numerators))]
		public void NumbersAndFractionOverOneShouldBeEqual(long numerator)
		{
			Assert.Equal(new UnicodeRationalNumber(numerator), new UnicodeRationalNumber(numerator, 1));
			Assert.Equal(new UnicodeRationalNumber(numerator).GetHashCode(), new UnicodeRationalNumber(numerator, 1).GetHashCode());
		}

		[Theory]
		[InlineData("1/10", "10/1")]
		[InlineData("2/10", "1/10")]
		[InlineData("1/20", "1/10")]
		[InlineData("2/2", "1/1")]
		[InlineData("2/1", "1/2")]
		public void DifferentRationalNumbersShouldNotBeDeterminedEqual(string number1, string number2)
		{
			Assert.NotEqual(UnicodeRationalNumber.Parse(number1), UnicodeRationalNumber.Parse(number2));
			Assert.NotEqual(UnicodeRationalNumber.Parse(number2), UnicodeRationalNumber.Parse(number1));
		}

		public static readonly TheoryData<string, long, byte> StringConversionTestData = new TheoryData<string, long, byte>
		{
			{ "0", 0, 1 },
			{ "1", 1, 1 },
			{ "1/100", 1, 100 },
			{ "-20/7", -20, 7 },
			{ "-5", -5, 1 },
			{ "-9223372036854775808", long.MinValue, 1 },
			{ "9223372036854775807", long.MaxValue, 1 },
			{ "9223372036854775807/255", long.MaxValue, byte.MaxValue },
		};

		[Theory]
		[MemberData(nameof(StringConversionTestData))]
		public void MethodToStringShouldReturnExpectedResult(string expectedText, long numerator, byte denominator)
			=> Assert.Equal(expectedText, new UnicodeRationalNumber(numerator, denominator).ToString());

		[Fact]
		public void ParsingNullValueShoudlFail()
			=> Assert.Throws<ArgumentNullException>(() => UnicodeRationalNumber.Parse(null));

		[Fact]
		public void ParsingEmptyValueShoudlFail()
			=> Assert.Throws<ArgumentException>(() => UnicodeRationalNumber.Parse(string.Empty));

		[Theory]
		[InlineData(0, "0")]
		[InlineData(0, "0/1")]
		[InlineData(1, "1")]
		[InlineData(1, "1/1")]
		[InlineData(long.MaxValue, "9223372036854775807")]
		[InlineData(long.MaxValue, "9223372036854775807/1")]
		[InlineData(long.MinValue, "-9223372036854775808")]
		[InlineData(long.MinValue, "-9223372036854775808/1")]
		public void ParsingCanReturnSimpleNumber(long expectedNumber, string text)
			=> Assert.Equal(new UnicodeRationalNumber(expectedNumber), UnicodeRationalNumber.Parse(text));

		public static readonly TheoryData<long, byte, string> FractionParsingTestData = new TheoryData<long, byte, string>
		{
			{ 0, 1, "0" },
			{ 0, 1, "0/1" },
			{ 1, 1, "1" },
			{ 1, 1, "1/1" },
			{ 1, 10, "1/10" },
			{ 1, 255, "1/255" },
			{ 3, 4, "3/4" },
			{ 6, 8, "6/8" },
			{ 1, 255, "1/255" },
			{ long.MaxValue, 1, "9223372036854775807" },
			{ long.MaxValue, 1, "9223372036854775807/1" },
			{ long.MinValue, 1, "-9223372036854775808" },
			{ long.MinValue, 1, "-9223372036854775808/1" },
			{ long.MaxValue, byte.MaxValue, "9223372036854775807/255" },
		};

		[Theory]
		[MemberData(nameof(FractionParsingTestData))]
		public void ParsingCanReturnFraction(long expectedNumerator, byte expectedDenominator, string text)
			=> Assert.Equal(new UnicodeRationalNumber(expectedNumerator, expectedDenominator), UnicodeRationalNumber.Parse(text));

		[Fact]
		public void EqualityComparisonAndHashCodeShouldWorkAsExpected()
		{
			var numbers = new[]
			{
				default,
				new UnicodeRationalNumber(0),
				new UnicodeRationalNumber(1),
				new UnicodeRationalNumber(1, 10),
				new UnicodeRationalNumber(1, 100),
				new UnicodeRationalNumber(10),
				new UnicodeRationalNumber(100),
				new UnicodeRationalNumber(1000),
				new UnicodeRationalNumber(1000000),
				new UnicodeRationalNumber(1000000000),
				new UnicodeRationalNumber(1000000000000),
			};

			var hashSet = new HashSet<UnicodeRationalNumber>();

			// Verify that all numbers are unique
			foreach (var number in numbers)
				Assert.True(hashSet.Add(number));

			// Verify that all numbers are already in the list
			foreach (var number in numbers)
				Assert.False(hashSet.Add(number));
		}
	}
}
