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
		private readonly Stream stream;
		private readonly byte[] byteBuffer;
		private int index;
		private int length;
		private readonly char fieldSeparator;
		private bool hasField = false;
		private readonly bool leaveOpen;

		public UnicodeDataFileReader(Stream stream, char fieldSeparator)
			: this(stream, fieldSeparator, false)
		{
		}

		public UnicodeDataFileReader(Stream stream, char fieldSeparator, bool leaveOpen)
		{
			this.stream = stream;
			this.fieldSeparator = fieldSeparator;
			this.byteBuffer = new byte[8192];
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
			Completed:;
			return hasField;
		}

		private string ReadFieldInternal(bool trim)
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

			using (var buffer = Utf8Buffer.Get())
			{
				int startOffset;
				int endOffset;

				do
				{
					startOffset = index;
					endOffset = -1;

					while (index < length)
					{
						byte b = byteBuffer[index];

						if (IsNewLineOrComment(b))	 // NB: Do not advance to the next byte when end of line has been reached.
						{
							endOffset = index;
							hasField = false;
							break;
						}
						else if (b == fieldSeparator)
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

				return trim ? buffer.ToTrimmedString() : buffer.ToString();
			}
		}

		/// <summary>Reads the next data field.</summary>
		/// <remarks>This method will return <see langword="null"/> on end of line.</remarks>
		/// <returns>The text value of the read field, if available; <see langword="null"/> otherwise.</returns>
		public string ReadField()
		{
			return ReadFieldInternal(false);
		}

		/// <summary>Reads the next data field as a trimmed value.</summary>
		/// <remarks>This method will return <see langword="null"/> on end of line.</remarks>
		/// <returns>The trimmed text value of the read field, if available; <see langword="null"/> otherwise.</returns>
		public string ReadTrimmedField()
		{
			return ReadFieldInternal(true);
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

					if (IsNewLineOrComment(b))   // NB: Do not advance to the next byte when end of line has been reached.
					{
						hasField = false;
						return true;
					}
					else
					{
						++index;

						if (b == fieldSeparator)
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
