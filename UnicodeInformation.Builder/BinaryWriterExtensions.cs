using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	internal static class BinaryWriterExtensions
	{
		public static void WriteUInt24(this BinaryWriter writer, int value)
		{
			if (value < 0 || value > 0xFFFFFF) throw new ArgumentOutOfRangeException("value");

			writer.Write((byte)(value));
			writer.Write((byte)(value >> 8));
			writer.Write((byte)(value >> 16));
		}

		/// <summary>Writes code point in a custom, but compact encoding.</summary>
		/// <remarks>
		/// Unlike UTF-8, this encoding will consume at most 3 bytes.
		/// It could ideally store values between 0x0 and 0x40409F, but this range is useless at the moment.
		/// </remarks>
		/// <param name="writer">The binary writer to use.</param>
		/// <param name="value">The value to write</param>
		public static void WriteCodePoint(this BinaryWriter writer, int value)
		{
			if (value < 0 || value > 0x40407F) throw new ArgumentOutOfRangeException("value");

			if (value < 0xA0) writer.Write((byte)value);
			else if (value < 0x20A0)
			{
				value -= 0xA0;
				writer.Write((byte)((byte)(value >> 8) | 0xA0));
				writer.Write((byte)value);
			}
			else if (value < 0x40A0)
			{
				value -= 0x20A0;
				writer.Write((byte)((byte)(value >> 8) | 0xC0));
				writer.Write((byte)value);
			}
			else
			{
				value -= 0x40A0;
				writer.Write((byte)((byte)(value >> 16) | 0xE0));
				writer.Write((byte)(value >> 8));
				writer.Write((byte)value);
			}
		}
	}
}
