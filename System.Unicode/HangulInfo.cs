using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Unicode
{
	internal static partial class HangulInfo
	{
		[StructLayout(LayoutKind.Sequential)]
		private readonly struct StringInfo
		{
			public readonly byte StringOffset;
			private readonly byte _nextStringOffset;
			public byte StringLength => (byte)(_nextStringOffset - StringOffset);
		}

		// Constants defined on page 144 of the Unicode 7.0 Standard (3.12)
		private const ushort SBase = 0xAC00;
		//private const ushort LBase = 0x1100;
		//private const ushort VBase = 0x1161;
		//private const ushort TBase = 0x11A7;
		private const int LCount = 19;
		private const int VCount = 21;
		private const int TCount = 28;
		private const int NCount = VCount * TCount;
		private const int SCount = LCount * NCount;

		// Algorithm to compute indices defined on page 150 of the Unicode 7.0 Standard (3.12)
		private static int ReadCodePointData(int codePoint, out StringInfo l, out StringInfo v, out StringInfo t)
		{
			int sIndex = codePoint - SBase;

			if (sIndex < 0 || sIndex >= SCount) throw new ArgumentOutOfRangeException(nameof(codePoint));

			l = Unsafe.ReadUnaligned<StringInfo>(ref Unsafe.AsRef(JamoLTable[sIndex / NCount * Unsafe.SizeOf<StringInfo>()]));
			v = Unsafe.ReadUnaligned<StringInfo>(ref Unsafe.AsRef(JamoLTable[sIndex % NCount / TCount * Unsafe.SizeOf<StringInfo>()]));
			t = Unsafe.ReadUnaligned<StringInfo>(ref Unsafe.AsRef(JamoLTable[sIndex % TCount * Unsafe.SizeOf<StringInfo>()]));

			return PrefixLength + l.StringLength + v.StringLength + t.StringLength;
		}

		private static void Write(Span<byte> buffer, StringInfo l, StringInfo v, StringInfo t)
		{
			var stringData = StringData;

			stringData.Slice(0, PrefixLength).CopyTo(buffer);
			buffer = buffer.Slice(PrefixLength);
			stringData.Slice(l.StringOffset, l.StringLength).CopyTo(buffer);
			buffer = buffer.Slice(l.StringLength);
			stringData.Slice(v.StringOffset, v.StringLength).CopyTo(buffer);
			buffer = buffer.Slice(v.StringLength);
			stringData.Slice(t.StringOffset, t.StringLength).CopyTo(buffer);
		}

		internal static bool TryWriteHangulName(Span<byte> buffer, int codePoint, out int byteCount)
		{
			int length = ReadCodePointData(codePoint, out var l, out var v, out var t);

			byteCount = length;

			if (buffer.Length < length) return false;

			Write(buffer, l, v, t);

			return true;
		}

		internal static int GetHangulNameLength(char codePoint)
			=> ReadCodePointData(codePoint, out _, out _, out _);

		internal static ReadOnlyMemory<byte> GetHangulName(char codePoint)
		{
			var buffer = new byte[ReadCodePointData(codePoint, out var l, out var v, out var t)].AsMemory();

			Write(buffer.Span, l, v, t);

			return buffer;
		}

		internal static bool IsHangul(int codePoint)
			=> codePoint >= SBase && codePoint < SBase + SCount;
	}
}
