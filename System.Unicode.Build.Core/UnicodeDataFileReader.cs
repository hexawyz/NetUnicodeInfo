using System.IO;

namespace System.Unicode.Build.Core
{
	public class UnicodeDataFileReader : IDisposable
	{
		private readonly Stream _stream;
		private readonly byte[] _byteBuffer;
		private int _index;
		private int _length;
		private readonly char _fieldSeparator;
		private bool _hasField = false;
		private readonly bool _leaveOpen;

		public UnicodeDataFileReader(Stream stream, char fieldSeparator)
			: this(stream, fieldSeparator, false)
		{
		}

		public UnicodeDataFileReader(Stream stream, char fieldSeparator, bool leaveOpen)
		{
			_stream = stream;
			_fieldSeparator = fieldSeparator;
			_byteBuffer = new byte[8192];
			_leaveOpen = leaveOpen;
		}

		public void Dispose()
		{
			if (!_leaveOpen) _stream.Dispose();
		}

		private bool RefillBuffer()
			// Evilish line of code. 😈
			=> (_length = _stream.Read(_byteBuffer, 0, _byteBuffer.Length)) != (_index = 0);

		private static bool IsNewLineOrComment(byte b)
			=> b == '\n' || b == '#';

		/// <summary>Moves the stream to the next valid data row.</summary>
		/// <returns><see langword="true"/> if data is available; <see langword="false"/> otherwise.</returns>
		public bool MoveToNextLine()
		{
			if (_length == 0)
			{
				if (RefillBuffer())
				{
					if (!IsNewLineOrComment(_byteBuffer[_index]))
					{
						_hasField = true;
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
				while (_index < _length)
				{
					if (_byteBuffer[_index++] == '\n')
					{
						if ((_index < _length || RefillBuffer()) && !IsNewLineOrComment(_byteBuffer[_index]))
						{
							_hasField = true;
							goto Completed;
						}
					}
				}
			} while (RefillBuffer());

			_hasField = false;
		Completed:;
			return _hasField;
		}

		private string ReadFieldInternal(bool trim)
		{
			if (_length == 0) throw new InvalidOperationException();

			if (!_hasField) return null;
			else if (_index >= _length) RefillBuffer();

			// If the current character is a new line or a comment, we are at the end of a line.
			if (IsNewLineOrComment(_byteBuffer[_index]))
			{
				if (_hasField)
				{
					_hasField = false;
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
					startOffset = _index;
					endOffset = -1;

					while (_index < _length)
					{
						byte b = _byteBuffer[_index];

						if (IsNewLineOrComment(b))   // NB: Do not advance to the next byte when end of line has been reached.
						{
							endOffset = _index;
							_hasField = false;
							break;
						}
						else if (b == _fieldSeparator)
						{
							endOffset = _index++;
							break;
						}
						else
						{
							++_index;
						}
					}

					if (endOffset >= 0)
					{
						buffer.Append(_byteBuffer, startOffset, endOffset - startOffset);
						break;
					}
					else if (_index > startOffset)
					{
						buffer.Append(_byteBuffer, startOffset, _index - startOffset);
					}
				} while (RefillBuffer());

				return trim ? buffer.ToTrimmedString() : buffer.ToString();
			}
		}

		/// <summary>Reads the next data field.</summary>
		/// <remarks>This method will return <see langword="null"/> on end of line.</remarks>
		/// <returns>The text value of the read field, if available; <see langword="null"/> otherwise.</returns>
		public string ReadField() => ReadFieldInternal(false);

		/// <summary>Reads the next data field as a trimmed value.</summary>
		/// <remarks>This method will return <see langword="null"/> on end of line.</remarks>
		/// <returns>The trimmed text value of the read field, if available; <see langword="null"/> otherwise.</returns>
		public string ReadTrimmedField() => ReadFieldInternal(true);

		/// <summary>Skips the next data field.</summary>
		/// <remarks>This method will return <see langword="false"/> on end of line.</remarks>
		/// <returns><see langword="true"/> if a field was skipped; <see langword="false"/> otherwise.</returns>
		public bool SkipField()
		{
			if (_length == 0) throw new InvalidOperationException();

			if (!_hasField) return false;
			else if (_index >= _length) RefillBuffer();

			// If the current character is a new line or a comment, we are at the end of a line.
			if (IsNewLineOrComment(_byteBuffer[_index]))
			{
				_hasField = false;
				return false;
			}

			do
			{
				while (_index < _length)
				{
					byte b = _byteBuffer[_index];

					if (IsNewLineOrComment(b))   // NB: Do not advance to the next byte when end of line has been reached.
					{
						_hasField = false;
						return true;
					}
					else
					{
						++_index;

						if (b == _fieldSeparator)
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
