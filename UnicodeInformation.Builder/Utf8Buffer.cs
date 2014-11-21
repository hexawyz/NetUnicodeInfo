using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public struct Utf8Buffer : IDisposable
	{
		private static readonly ConcurrentStack<byte[]> bufferStack = new ConcurrentStack<byte[]>();

		public static Utf8Buffer Get()
		{
			byte[] buffer;

			return new Utf8Buffer(bufferStack.TryPop(out buffer) ? buffer : new byte[100]);
		}

		private byte[] buffer;
		private int length;

		private Utf8Buffer(byte[] buffer)
		{
			this.buffer = buffer;
			this.length = 0;
		}

		public void Dispose()
		{
			if (buffer != null)
			{
				bufferStack.Push(buffer);
				this = default(Utf8Buffer);
			}
		}

		public int Length { get { return length; } }

		private void EnsureExtraCapacity(int count)
		{
			if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
			if (buffer.Length < checked(length + count))
				Array.Resize(ref buffer, Math.Max(length + count, buffer.Length << 1));
		}

		public void Append(byte[] value, int startIndex, int count)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));
			if (startIndex >= value.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
			if (checked(count += startIndex) > value.Length) throw new ArgumentOutOfRangeException(nameof(count));

			EnsureExtraCapacity(value.Length);

			var buffer = this.buffer;

			for (int i = startIndex; i < count; ++i)
			{
				buffer[length++] = value[i];
			}
		}

		public override string ToString()
		{
			return length > 0 ? Encoding.UTF8.GetString(buffer, 0, length) : string.Empty;
		}

		public string ToTrimmedString()
		{
			if (length == 0) return string.Empty;

			var buffer = this.buffer;
			int start = 0;
			int end = length;

			while (buffer[start] == ' ') if (++start == length) return string.Empty;
			while (buffer[--end] == ' ') ;

			return end > start ? Encoding.UTF8.GetString(buffer, start, end - start + 1) : string.Empty;
		}
	}
}
