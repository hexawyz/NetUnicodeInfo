using System;
using System.Unicode;
using System.Collections.Generic;
using Xunit;
using System.Collections;
using System.Linq;

namespace UnicodeInformation.Tests
{
	public class CodePointEnumerableTests
	{
		public static readonly TheoryData<int[], string> EnumerationTestData = new TheoryData<int[], string>
		{
			{ new int[0], "" },
			{ new int[] { 0x0041,0x1F600, 0x00E9 }, "\u0041\U0001F600\u00E9" },
		};

		[Theory]
		[MemberData(nameof(EnumerationTestData))]
		public void EnumerationShouldHaveExpectedResults(int[] expectedCharacters, string text)
		{
			var enumerable = text.AsCodePointEnumerable();

			// Test C# foreach enumeration
			{
				int i = 0;
				foreach (int codePoint in enumerable)
				{
					Assert.Equal(expectedCharacters[i++], codePoint);
				}
				Assert.Equal(expectedCharacters.Length, i);
			}

			// Test generic enumerable
			Assert.Equal(expectedCharacters, from codePoint in enumerable select codePoint);

			// Test legacy enumeration
			{
				// We could use Enumerable.Cast<>, but we can't guarantee that the LINQ implementation we use wouldn't be smart and cast IEnumerable back to IEnumerable<int>
				var legacyEnumerator = ((IEnumerable)enumerable).GetEnumerator();

				int index = 0;

				while (legacyEnumerator.MoveNext())
				{
					Assert.True(index < expectedCharacters.Length);
					Assert.Equal(expectedCharacters[index++], Assert.IsType<int>(legacyEnumerator.Current));
				}

				Assert.Equal(expectedCharacters.Length, index);
			}
		}

		[Fact]
		public void NullArgumentShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => { foreach (int codePoint in (null as string).AsCodePointEnumerable()) { } });
		}

		public static readonly TheoryData<XUnitSerializableString> EnumerationFailureTestData = new TheoryData<XUnitSerializableString>
		{
			"\uDA00",
			"\uDCD0",
			"\uDCD0\uDA00",
			"\u0041\uDA00",
			"\u0041\uDCD0",
			"\uDA00\u0041",
			"\uDCD0\u0041",
			"\uDA00\u0041\uDCD0\u0041",
			"\u0041\uDA00\u0041\uDCD0\u0041",
		};

		[Theory]
		[MemberData(nameof(EnumerationFailureTestData))]
		public void EnumerationOfInvalidUtf16StringsShouldThrowArgumentException(XUnitSerializableString text)
		{
			Assert.Throws<ArgumentException>(() => { foreach (int codePoint in ((string)text).AsCodePointEnumerable()) { } });
		}
	}
}
