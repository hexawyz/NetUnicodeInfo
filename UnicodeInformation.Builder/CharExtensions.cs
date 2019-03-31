namespace System.Unicode.Builder
{
	public static class CharExtensions
	{
		public static bool IsHexDigit(this char c)
			=> c >= '0' && c <= 'f' && (c <= '9' || c <= 'F' && c >= 'A' || c >= 'a');
	}
}
