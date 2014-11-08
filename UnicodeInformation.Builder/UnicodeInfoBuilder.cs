using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	internal class UnicodeInfoBuilder
	{
		private readonly Version unicodeVersion;
		private UnicodeCharacterDataBuilder[] entries = new UnicodeCharacterDataBuilder[10000];
		private int entryCount;
		private List<UnicodeBlock> blockEntries = new List<UnicodeBlock>(100);

		public UnicodeInfoBuilder(Version unicodeVersion)
		{
			this.unicodeVersion = unicodeVersion;
		}

		private int FindCodePoint(int codePoint)
		{
			if (entryCount == 0) return -1;

			int minIndex = 0;
			int maxIndex = entryCount - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = entries[index].CodePointRange.CompareCodePoint(codePoint);

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

			if (entryCount == 0 || entries[maxIndex = entryCount - 1].CodePointRange.LastCodePoint < startCodePoint) return entryCount;
			else if (endCodePoint < entries[minIndex = 0].CodePointRange.FirstCodePoint) return 0;
			else if (entryCount == 1) return -1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = entries[index].CodePointRange.CompareCodePoint(startCodePoint);

				if (Δ == 0) return -1;
				else if (Δ < 0) maxIndex = index;
				else minIndex = index;
			} while (maxIndex - minIndex > 1);

			if (entries[maxIndex].CodePointRange.FirstCodePoint < endCodePoint) return -1;
			else return maxIndex;
		}

		public void Insert(UnicodeCharacterDataBuilder data)
		{
			int insertionPoint = FindInsertionPoint(data.CodePointRange.FirstCodePoint, data.CodePointRange.LastCodePoint);

			if (insertionPoint < 0) throw new InvalidOperationException("The specified range overlaps with pre-existing ranges.");

			if (insertionPoint >= entries.Length)
			{
				Array.Resize(ref entries, entries.Length << 1);
			}

			if (insertionPoint < entryCount)
			{
				Array.Copy(entries, insertionPoint, entries, insertionPoint + 1, entryCount - insertionPoint);
			}

			entries[insertionPoint] = data;
			++entryCount;
		}

		public UnicodeCharacterDataBuilder Get(int codePoint)
		{
			int index = FindCodePoint(codePoint);

			return index >= 0 ? entries[index] : null;
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
				|| entries[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| entries[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException();
			}

			int i = firstIndex;

			while (true)
			{
				entries[i].ContributoryProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		public void SetProperties(CoreProperties property, UnicodeCharacterRange codePointRange)
		{
			int firstIndex = FindCodePoint(codePointRange.FirstCodePoint);
			int lastIndex = FindCodePoint(codePointRange.LastCodePoint);

			if (firstIndex < 0 && lastIndex < 0)
			{
				Insert(new UnicodeCharacterDataBuilder(codePointRange) { CoreProperties = property });
				return;
			}

			if (firstIndex < 0
				|| lastIndex < 0
				|| entries[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| entries[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException();
			}

			int i = firstIndex;

			while (true)
			{
				entries[i].CoreProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		public void AddBlockEntry(UnicodeBlock block)
		{
			blockEntries.Add(block);
        }

		public UnicodeInfo ToUnicodeData()
		{
			var finalData = new UnicodeCharacterData[entryCount];

			for (int i = 0; i < finalData.Length; ++i)
				finalData[i] = entries[i].ToCharacterData();

			return new UnicodeInfo(unicodeVersion, finalData, blockEntries.ToArray());
		}

		private void WriteUnicodeBlockToFile(BinaryWriter writer, UnicodeBlock block)
		{
			writer.WriteCodePoint(block.CodePointRange.FirstCodePoint);
			writer.WriteCodePoint(block.CodePointRange.LastCodePoint);
			writer.Write(block.Name);
		}

		public void WriteToStream(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write(new byte[] { (byte)'U', (byte)'C', (byte)'D', 0 });
				writer.Write((ushort)7);
				writer.Write((byte)0);
				writer.WriteCodePoint(entryCount);
				for (int i = 0; i < entryCount; ++i)
				{
					entries[i].WriteToFile(writer);
				}
				if (blockEntries.Count > 255) throw new InvalidOperationException("There are too many block entries. The file format needs to be upgraded.");
				writer.Write((byte)blockEntries.Count);
				for (int i = 0; i < blockEntries.Count; ++i)
					WriteUnicodeBlockToFile(writer, blockEntries[i]);
			}
		}

		public void WriteToFile(string fileName)
		{
			using (var stream = File.Create(fileName))
			{
				WriteToStream(stream);
			}
		}
	}
}
