using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public sealed class UnicodeData
	{
		private readonly Version unicodeVersion;
		private readonly UnicodeCharacterData[] characterData;

		public static async Task<UnicodeData> FromStreamAsync(Stream stream)
		{
			var buffer = new byte[4096];

			if (await stream.ReadAsync(buffer, 0, 6).ConfigureAwait(false) != 6)
				throw new EndOfStreamException();

			if (buffer[0] != (byte)'U'
				|| buffer[1] != (byte)'C'
				|| buffer[2] != (byte)'D')
				throw new InvalidDataException();

			int formatVersion = buffer[3];

			if (formatVersion != 1) throw new InvalidDataException();

			var unicodeVersion = new Version(buffer[4], buffer[6]);

			return new UnicodeData(unicodeVersion, null);
		}

		internal UnicodeData(Version unicodeVersion, UnicodeCharacterData[] characterData)
		{
			this.unicodeVersion = unicodeVersion;
			this.characterData = characterData;
        }

		public Version UnicodeVersion { get { return unicodeVersion; } }

		public UnicodeCharacterData GetUnicodeData(int codePoint)
		{
			return default(UnicodeCharacterData);
		}

		private void LoadData(Stream stream)
		{
		}
	}
}
