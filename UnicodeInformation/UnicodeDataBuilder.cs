using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public class UnicodeDataBuilder
	{
		private readonly Version unicodeVersion;
		private UnicodeCharacterDataBuilder[] characterDataBuilders = new UnicodeCharacterDataBuilder[10000];
		private int characterCount;

		public UnicodeDataBuilder(Version unicodeVersion)
		{
			this.unicodeVersion = unicodeVersion;
		}

		private int FindCodePoint(int codePoint)
		{
			if (characterCount == 0) return -1;

			int minIndex = 0;
			int maxIndex = characterCount - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = characterDataBuilders[index].CodePointRange.CompareCodePoint(codePoint);

				if (Δ == 0) return index;
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
            } while (minIndex <= maxIndex);

			return -1;
		}

		private int FindInsertionPoint(int startCodePoint, int endCodePoint)
		{
			int minIndex;
			int maxIndex;

			if (characterCount == 0 || characterDataBuilders[maxIndex = characterCount - 1].CodePointRange.LastCodePoint < startCodePoint) return characterCount;
			else if (endCodePoint < characterDataBuilders[minIndex = 0].CodePointRange.FirstCodePoint) return 0;
			else if (characterCount == 1) return -1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = characterDataBuilders[index].CodePointRange.CompareCodePoint(startCodePoint);

				if (Δ == 0) return -1;
				else if (Δ < 0) maxIndex = index;
				else minIndex = index;
			} while (maxIndex - minIndex > 1);

			if (characterDataBuilders[maxIndex].CodePointRange.FirstCodePoint < endCodePoint) return -1;
			else return maxIndex;
		}

		public void Insert(UnicodeCharacterDataBuilder data)
		{
			int insertionPoint = FindInsertionPoint(data.CodePointRange.FirstCodePoint, data.CodePointRange.LastCodePoint);

			if (insertionPoint < 0) throw new InvalidOperationException("The specified range overlaps with pre-existing ranges.");

			if (insertionPoint >= characterDataBuilders.Length)
			{
				Array.Resize(ref characterDataBuilders, characterDataBuilders.Length << 1);
			}

			if (insertionPoint < characterCount)
			{
				Array.Copy(characterDataBuilders, insertionPoint, characterDataBuilders, insertionPoint + 1, characterCount - insertionPoint);
			}

			characterDataBuilders[insertionPoint] = data;
			++characterCount;
		}

		public UnicodeCharacterDataBuilder Get(int codePoint)
		{
			int index = FindCodePoint(codePoint);

			return index >= 0 ? characterDataBuilders[index] : null;
		}

		public void SetProperties(ContributoryProperties property, UnicodeCharacterRange codePointRange)
		{
			int firstIndex = FindCodePoint(codePointRange.FirstCodePoint);
			int lastIndex = FindCodePoint(codePointRange.LastCodePoint);

			if (firstIndex < 0 && lastIndex < 0)
			{
				Insert(new UnicodeCharacterDataBuilder(codePointRange) { ContributoryProperties = property });
				return;
			}

			if (firstIndex < 0
				|| lastIndex < 0
				|| characterDataBuilders[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| characterDataBuilders[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException();
			}

			int i = firstIndex;

			while (true)
			{
				characterDataBuilders[i].ContributoryProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		public UnicodeData ToUnicodeData()
		{
			var finalData = new UnicodeCharacterData[characterCount];

			for (int i = 0; i < finalData.Length; ++i)
				finalData[i] = characterDataBuilders[i].ToCharacterData();

			return new UnicodeData(unicodeVersion, finalData);
		}
	}
}
