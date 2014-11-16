using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public sealed class UnihanDataFileReader : IDisposable
	{
		private readonly UnicodeDataFileReader reader;
		private int codePoint;
		private string propertyName;
		private string propertyValue;

		public UnihanDataFileReader(Stream stream)
			: this(stream, false)
		{
		}

		public UnihanDataFileReader(Stream stream, bool leaveOpen)
		{
			reader = new UnicodeDataFileReader(stream, '\t', leaveOpen);
		}

		public void Dispose()
		{
			reader.Dispose();
		}

		public bool Read()
		{
			bool result;

			if (result = reader.MoveToNextLine())
			{
				codePoint = HexCodePoint.ParsePrefixed(reader.ReadField());
				propertyName = reader.ReadField();
				propertyValue = reader.ReadField();
			}
			else
			{
				codePoint = 0;
				propertyName = null;
				propertyValue = null;
			}

			return result;
		}

		public int CodePoint { get { return codePoint; } }

		public string PropertyName { get { return propertyName; } }

		public string PropertyValue { get { return propertyValue; } }
	}
}
