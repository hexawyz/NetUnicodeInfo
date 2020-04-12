using System.Linq;
using Xunit;

namespace System.Unicode.Tests
{
	public sealed class UnihanCharacterDataTests
	{
		private static readonly UnicodeBlock[] Blocks = UnicodeInfo.GetBlocks();

		[Theory]
		[InlineData("CJK Unified Ideographs")]
		[InlineData("CJK Unified Ideographs Extension A")]
		[InlineData("CJK Unified Ideographs Extension B")]
		[InlineData("CJK Unified Ideographs Extension C")]
		[InlineData("CJK Unified Ideographs Extension D")]
		[InlineData("CJK Unified Ideographs Extension E")]
		[InlineData("CJK Unified Ideographs Extension F")]
		[InlineData("CJK Unified Ideographs Extension G")]
		[InlineData("CJK Compatibility Ideographs")]
		[InlineData("CJK Compatibility Ideographs Supplement")]
		public void CodePointPackingShouldRoundTrip(string blockName)
		{
			var block = Blocks.Single(b => b.Name == blockName);

			foreach (int codePoint in block.CodePointRange)
			{
				Assert.Equal(codePoint, UnihanCharacterData.UnpackCodePoint(UnihanCharacterData.PackCodePoint(codePoint)));
			}
		}
	}
}
