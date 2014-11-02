using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public class UnicodeDataFileReader : IDisposable
	{
		private struct AsciiCharBuffer
		{
			private readonly UnicodeDataFileReader reader;
			private int length;

			public AsciiCharBuffer(UnicodeDataFileReader reader)
			{
				this.reader = reader;
				this.length = 0;
			}

			public int Length { get { return length; } }

			private void EnsureExtraCapacity(int count)
			{
				if (count < 0) throw new ArgumentOutOfRangeException("requiredExtraCapacity");
				if (reader.charBuffer.Length < checked(length + count))
					Array.Resize(ref reader.charBuffer, Math.Max(length + count, reader.charBuffer.Length << 1));
			}

			public void Append(byte[] value, int startIndex, int count)
			{
				if (value == null) throw new ArgumentNullException("value");
				if (startIndex >= value.Length) throw new ArgumentOutOfRangeException("startIndex");
				if (checked(count += startIndex) > value.Length) throw new ArgumentOutOfRangeException("count");

				EnsureExtraCapacity(value.Length);

				var buffer = reader.charBuffer;

				for (int i = startIndex; i < count; ++i)
				{
					byte b = value[i];

					if (b > 127) throw new InvalidDataException(string.Format("Unexpected character value: {0}.", b));

					buffer[length++] = (char)b;
				}
			}

			public override string ToString()
			{
				return length > 0 ? new string(reader.charBuffer, 0, length) : string.Empty;
			}
		}

		private readonly Stream stream;
		private readonly byte[] byteBuffer;
		private char[] charBuffer;
		private int index;
		private int length;
		private bool hasField = false;
		private readonly bool leaveOpen;

		public UnicodeDataFileReader(Stream stream)
			: this(stream, false)
		{
		}

		public UnicodeDataFileReader(Stream stream, bool leaveOpen)
		{
			this.stream = stream;
			this.byteBuffer = new byte[8192];
			this.charBuffer = new char[100];
			this.leaveOpen = leaveOpen;
		}

		public void Dispose()
		{
			if (!leaveOpen) stream.Dispose();
		}

		private bool RefillBuffer()
		{
			// Evilish line of code. 😈
			return (length = stream.Read(byteBuffer, 0, byteBuffer.Length)) != (index = 0);
		}

		private static bool IsNewLineOrComment(byte b)
		{
			return b == '\n' || b == '#';
		}

		/// <summary>Moves the stream to the next valid data row.</summary>
		/// <returns><see langword="true"/> if data is available; <see langword="false"/> otherwise.</returns>
		public bool MoveToNextLine()
		{
			if (length == 0)
			{
				if (RefillBuffer())
				{
					if (!IsNewLineOrComment(byteBuffer[index]))
					{
						hasField = true;
						goto Completed;
					}
                }
				else
				{
					return false;
				}
			}

			do
			{
				while (index < length)
				{
					if (byteBuffer[index++] == '\n')
					{
						if (index < length && !IsNewLineOrComment(byteBuffer[index]))
						{
							hasField = true;
							goto Completed;
						}
					}
				}
			} while (RefillBuffer());

			hasField = false;
		Completed: ;
			return hasField;
        }

		/// <summary>Reads the next data field.</summary>
		/// <remarks>This method will return <see langword="null"/> on end of line.</remarks>
		/// <returns>The text value of the read field, if available; <see langword="null"/> otherwise.</returns>
		public string ReadField()
		{
			if (length == 0) throw new InvalidOperationException();

			if (!hasField) return null;
			else if (index >= length) RefillBuffer();

			// If the current character is a new line or a comment, we are at the end of a line.
			if (IsNewLineOrComment(byteBuffer[index]))
			{
				if (hasField)
				{
					hasField = false;
					return string.Empty;
				}
				else
				{
					return null;
				}
			}

			var buffer = new AsciiCharBuffer(this);
			int startOffset;
			int endOffset;

			do
			{
				startOffset = index;
				endOffset = -1;

				while (index < length)
				{
					byte b = byteBuffer[index];

					if (IsNewLineOrComment(b)) // NB: Do not advance to the next byte when end of line has been reached.
					{
						endOffset = index;
						hasField = false;
						break;
					}
					else if(b == ';')
					{
						endOffset = index++;
						break;
					}
					else
					{
						++index;
					}
				}

				if (endOffset >= 0)
				{
					buffer.Append(byteBuffer, startOffset, endOffset - startOffset);
					break;
				}
				else if (index > startOffset)
				{
					buffer.Append(byteBuffer, startOffset, index - startOffset);
				}
			} while (RefillBuffer());

			return buffer.ToString();
		}

		/// <summary>Skips the next data field.</summary>
		/// <remarks>This method will return <see langword="false"/> on end of line.</remarks>
		/// <returns><see langword="true"/> if a field was skipped; <see langword="false"/> otherwise.</returns>
		public bool SkipField()
		{
			if (length == 0) throw new InvalidOperationException();

			if (!hasField) return false;
			else if (index >= length) RefillBuffer();

			// If the current character is a new line or a comment, we are at the end of a line.
			if (IsNewLineOrComment(byteBuffer[index]))
			{
				hasField = false;
				return false;
			}

			do
			{
				while (index < length)
				{
					byte b = byteBuffer[index];

					if (IsNewLineOrComment(b)) // NB: Do not advance to the next byte when end of line has been reached.
					{
						hasField = false;
						return true;
					}
					else
					{
						++index;

						if (b == ';')
						{
							return true;
						}
					}
				}
			} while (RefillBuffer());

			return true;
		}
	}
}
