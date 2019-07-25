using System.IO;

namespace System.Unicode.Build.Core
{
	public sealed class UnihanDataFileReader : IDisposable
	{
		private readonly UnicodeDataFileReader _reader;

		public int CodePoint { get; private set; }

		public string PropertyName { get; private set; }

		public string PropertyValue { get; private set; }

		public UnihanDataFileReader(Stream stream)
			: this(stream, false)
		{
		}

		public UnihanDataFileReader(Stream stream, bool leaveOpen) => _reader = new UnicodeDataFileReader(stream, '\t', leaveOpen);

		public void Dispose() => _reader.Dispose();

		public bool Read()
		{
			bool result;

			if (result = _reader.MoveToNextLine())
			{
				CodePoint = HexCodePoint.ParsePrefixed(_reader.ReadField());
				PropertyName = _reader.ReadField();
				PropertyValue = _reader.ReadField();
			}
			else
			{
				CodePoint = 0;
				PropertyName = null;
				PropertyValue = null;
			}

			return result;
		}
	}
}
