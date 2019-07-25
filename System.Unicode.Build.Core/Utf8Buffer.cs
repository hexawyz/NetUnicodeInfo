using System.Collections.Concurrent;
using System.Text;

namespace System.Unicode.Build.Core
{
	public struct Utf8Buffer : IDisposable
	{
		private static readonly ConcurrentStack<byte[]> BufferStack = new ConcurrentStack<byte[]>();

		public static Utf8Buffer Get() => new Utf8Buffer(BufferStack.TryPop(out var buffer) ? buffer : new byte[100]);

		private byte[] _buffer;

		public int Length { get; private set; }

		private Utf8Buffer(byte[] buffer)
		{
			_buffer = buffer;
			Length = 0;
		}

		public void Dispose()
		{
			if (_buffer != null)
			{
				BufferStack.Push(_buffer);
				this = default;
			}
		}

		private void EnsureExtraCapacity(int count)
		{
			if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
			if (_buffer.Length < checked(Length + count))
				Array.Resize(ref _buffer, Math.Max(Length + count, _buffer.Length << 1));
		}

		public void Append(byte[] value, int startIndex, int count)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));
			if (startIndex >= value.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
			if (checked(count += startIndex) > value.Length) throw new ArgumentOutOfRangeException(nameof(count));

			EnsureExtraCapacity(value.Length);

			var buffer = _buffer;

			for (int i = startIndex; i < count; ++i)
			{
				buffer[Length++] = value[i];
			}
		}

		public override string ToString() => Length > 0 ? Encoding.UTF8.GetString(_buffer, 0, Length) : string.Empty;

		public string ToTrimmedString()
		{
			if (Length == 0) return string.Empty;

			var buffer = _buffer;
			int start = 0;
			int end = Length;

			while (buffer[start] == ' ') if (++start == Length) return string.Empty;
			while (buffer[--end] == ' ') ;

			return end > start ? Encoding.UTF8.GetString(buffer, start, end - start + 1) : string.Empty;
		}
	}
}
