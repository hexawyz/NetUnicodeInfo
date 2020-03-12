using System.Buffers.Binary;

namespace System.Unicode
{
	internal ref struct SpanReader
	{
		private ReadOnlySpan<byte> _span;
		private int _position;

		public int Position => _position;

		public SpanReader(ReadOnlySpan<byte> span)
		{
			_span = span;
			_position = 0;
		}

		public SpanReader(ReadOnlySpan<byte> span, int start)
		{
			_span = span.Slice(start);
			_position = start;
		}

		public void Skip(int count)
		{
			_span = _span.Slice(count);
			_position += count;
		}

		public byte ReadByte()
		{
			byte result = _span[0];
			Skip(sizeof(byte));
			return result;
		}

		public ushort ReadUInt16LittleEndian()
		{
			ushort result = BinaryPrimitives.ReadUInt16LittleEndian(_span);
			Skip(sizeof(ushort));
			return result;
		}

		public int ReadUInt24LittleEndian()
		{
			int result = BinaryPrimitives.ReadUInt16LittleEndian(_span) | (_span[2] << 16);
			Skip(sizeof(ushort) + sizeof(byte));
			return result;
		}

		public int ReadInt32LittleEndian()
		{
			int result = BinaryPrimitives.ReadInt32LittleEndian(_span);
			Skip(sizeof(int));
			return result;
		}

		public long ReadInt64LittleEndian()
		{
			long result = BinaryPrimitives.ReadInt64LittleEndian(_span);
			Skip(sizeof(long));
			return result;
		}

		public ushort ReadVariableUInt16LittleEndian()
		{
			byte b = ReadByte();
			ushort value = unchecked((ushort)(b & 0x7F));

			if (unchecked((sbyte)b) < 0)
			{
				b = ReadByte();
				value |= unchecked((ushort)((b & 0x7F) << 7));

				if (unchecked((sbyte)b) < 0)
				{
					b = ReadByte();
					value |= unchecked((ushort)((b & 0x03) << 14));
				}
			}

			return value;
		}

		public int ReadVariableUInt24LittleEndian()
		{
			byte b = ReadByte();
			uint value = unchecked((uint)(b & 0x7F));

			if (unchecked((sbyte)b) < 0)
			{
				b = ReadByte();
				value |= ((uint)b & 0x7F) << 7;

				if (unchecked((sbyte)b) < 0)
				{
					b = ReadByte();
					value |= ((uint)b & 0x7F) << 14;

					if (unchecked((sbyte)b) < 0)
					{
						b = ReadByte();
						value |= ((uint)b & 0x07) << 21;
					}
				}
			}

			return unchecked((int)value);
		}

		public uint ReadVariableUInt32LittleEndian()
		{
			byte b = ReadByte();
			uint value = (uint)b & 0x7F;

			if (unchecked((sbyte)b) < 0)
			{
				b = ReadByte();
				value |= ((uint)b & 0x7F) << 7;

				if (unchecked((sbyte)b) < 0)
				{
					b = ReadByte();
					value |= ((uint)b & 0x7F) << 14;

					if (unchecked((sbyte)b) < 0)
					{
						b = ReadByte();
						value |= ((uint)b & 0x7F) << 21;

						if (unchecked((sbyte)b) < 0)
						{
							b = ReadByte();
							value |= ((uint)b & 0x0F) << 28;
						}
					}
				}
			}

			return value;
		}

		public int ReadCodePoint()
		{
			byte b = ReadByte();

			if (b < 0xA0) return b;
			else if (b < 0xC0)
			{
				return 0xA0 + (((b & 0x1F) << 8) | ReadByte());
			}
			else if (b < 0xE0)
			{
				return 0x20A0 + (((b & 0x1F) << 8) | ReadByte());
			}
			else
			{
				return 0x40A0 + (((((b & 0x1F) << 8) | ReadByte()) << 8) | ReadByte());
			}
		}
	}
}
