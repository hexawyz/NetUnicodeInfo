using System.IO;
using System.Text;

namespace System.Unicode.Builder
{
	internal static class BinaryWriterExtensions
	{
		public static void WriteUInt24(this BinaryWriter writer, int value)
		{
			if (value < 0 || value > 0xFFFFFF) throw new ArgumentOutOfRangeException(nameof(value));

			writer.Write((byte)value);
			writer.Write((byte)(value >> 8));
			writer.Write((byte)(value >> 16));
		}

		public static void WriteVariableUInt64(this BinaryWriter writer, ulong value)
		{
			byte b = (byte)(value & 0x7F);
			value >>= 7;

			while (value > 0)
			{
				writer.Write((byte)(b | 0x80));
				b = (byte)(value & 0x7F);
				value >>= 7;
			}
			writer.Write(b);
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
			if (value < 0 || value > 0x40407F) throw new ArgumentOutOfRangeException(nameof(value));

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

		/// <summary>Writes a character name alias.</summary>
		/// <remarks>We assume that character names will not exceed 64 bytes in length.</remarks>
		/// <param name="writer">The writer to use.</param>
		/// <param name="nameAlias">The name alias value to write.</param>
		public static void WriteNameAliasToFile(this BinaryWriter writer, UnicodeNameAlias nameAlias)
		{
			writer.Write(nameAlias.Name);
			writer.Write((byte)nameAlias.Kind);
		}

		/// <summary>Writes a character name, packing two information bits along with the length.</summary>
		/// <remarks>We assume that character names will not exceed 128 bytes in length.</remarks>
		/// <param name="writer">The writer to use.</param>
		/// <param name="name">The name to write.</param>
		public static void WriteNamePropertyToFile(this BinaryWriter writer, string name)
		{
			var bytes = Encoding.UTF8.GetBytes(name);
			if (bytes.Length > 128) throw new InvalidOperationException("Did not expect UTF-8 encoded name to be longer than 128 bytes.");
			writer.Write((byte)(name.Length - 1));   // The most significant bit will always be cleared, because it will be used for other cases.
			writer.Write(bytes);
		}

		/// <summary>Writes a 6 bits length packed with two extra bits.</summary>
		/// <remarks>The parameters have a restricted range, which must be respected.</remarks>
		/// <param name="writer">The writer used to perform the operation.</param>
		/// <param name="extraBits">The value of the two extra bits.</param>
		/// <param name="length">The length to write.</param>
		public static void WritePackedLength(this BinaryWriter writer, byte extraBits, int length)
		{
			if (extraBits > 3) throw new ArgumentOutOfRangeException(nameof(extraBits));
			if (length < 1 || length > 64) throw new ArgumentOutOfRangeException(nameof(length));

			writer.Write((byte)((extraBits << 6) | (length - 1)));
		}
	}
}
