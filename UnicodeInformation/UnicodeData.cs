using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
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

		private UnicodeCharacterData FindCodePoint(int codePoint)
		{
			int minIndex = 0;
			int maxIndex = characterData.Length - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = characterData[index].CodePointRange.CompareCodePoint(codePoint);

				if (Δ == 0) return characterData[index];
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return null;
		}

		public UnicodeCharacterData GetUnicodeData(int codePoint)
		{
			return FindCodePoint(codePoint);
		}

		private void LoadData(Stream stream)
		{
		}
	}
}
