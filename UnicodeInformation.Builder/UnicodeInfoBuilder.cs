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
		public const int CjkRadicalCount = 214;	// The number of radicals (214) shouldn't change in the near future…

		private readonly Version unicodeVersion;
		private UnicodeCharacterDataBuilder[] ucdEntries = new UnicodeCharacterDataBuilder[10000];
		private int ucdEntryCount;
		private UnihanCharacterDataBuilder[] unihanEntries = new UnihanCharacterDataBuilder[10000];
		private int unihanEntryCount;
		private readonly List<UnicodeBlock> blockEntries = new List<UnicodeBlock>(100);
		private readonly CjkRadicalData[] cjkRadicals = new CjkRadicalData[CjkRadicalCount];

		public UnicodeInfoBuilder(Version unicodeVersion)
		{
			this.unicodeVersion = unicodeVersion;
		}

		private int FindUcdCodePoint(int codePoint)
		{
			if (ucdEntryCount == 0) return -1;

			int minIndex = 0;
			int maxIndex = ucdEntryCount - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = ucdEntries[index].CodePointRange.CompareCodePoint(codePoint);

				if (Δ == 0) return index;
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		private int FindUcdInsertionPoint(int startCodePoint, int endCodePoint)
		{
			int minIndex;
			int maxIndex;

			if (ucdEntryCount == 0 || ucdEntries[maxIndex = ucdEntryCount - 1].CodePointRange.LastCodePoint < startCodePoint) return ucdEntryCount;
			else if (endCodePoint < ucdEntries[minIndex = 0].CodePointRange.FirstCodePoint) return 0;
			else if (ucdEntryCount == 1) return -1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = ucdEntries[index].CodePointRange.CompareCodePoint(startCodePoint);

				if (Δ == 0) return -1;
				else if (Δ < 0) maxIndex = index;
				else minIndex = index;
			} while (maxIndex - minIndex > 1);

			if (ucdEntries[maxIndex].CodePointRange.FirstCodePoint < endCodePoint) return -1;
			else return maxIndex;
		}

		private int FindUnihanCodePoint(int codePoint)
		{
			if (unihanEntryCount == 0) return -1;

			int minIndex = 0;
			int maxIndex = unihanEntryCount - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = codePoint - unihanEntries[index].CodePoint;

				if (Δ == 0) return index;
				else if (Δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		private int FindUnihanInsertionPoint(int codePoint)
		{
			int minIndex;
			int maxIndex;

			if (unihanEntryCount == 0 || unihanEntries[maxIndex = unihanEntryCount - 1].CodePoint < codePoint) return unihanEntryCount;
			else if (codePoint < unihanEntries[minIndex = 0].CodePoint) return 0;
			else if (unihanEntryCount == 1) return -1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int Δ = codePoint - unihanEntries[index].CodePoint;

				if (Δ == 0) return -1;
				else if (Δ < 0) maxIndex = index;
				else minIndex = index;
			} while (maxIndex - minIndex > 1);

			if (unihanEntries[maxIndex].CodePoint < codePoint) return -1;
			else return maxIndex;
		}

		public void Insert(UnicodeCharacterDataBuilder data)
		{
			int insertionPoint = FindUcdInsertionPoint(data.CodePointRange.FirstCodePoint, data.CodePointRange.LastCodePoint);

			if (insertionPoint < 0) throw new InvalidOperationException("The specified range overlaps with pre-existing ranges.");

			if (ucdEntryCount == ucdEntries.Length)
			{
				Array.Resize(ref ucdEntries, ucdEntries.Length << 1);
			}

			if (insertionPoint < ucdEntryCount)
			{
				Array.Copy(ucdEntries, insertionPoint, ucdEntries, insertionPoint + 1, ucdEntryCount - insertionPoint);
			}

			ucdEntries[insertionPoint] = data;
			++ucdEntryCount;
		}

		private void Insert(UnihanCharacterDataBuilder data)
		{
			int insertionPoint = FindUnihanInsertionPoint(data.CodePoint);

			if (insertionPoint < 0) throw new InvalidOperationException("The specified range overlaps with pre-existing ranges.");

			if (unihanEntryCount == unihanEntries.Length)
			{
				Array.Resize(ref unihanEntries, unihanEntries.Length << 1);
			}

			if (insertionPoint < unihanEntryCount)
			{
				Array.Copy(unihanEntries, insertionPoint, unihanEntries, insertionPoint + 1, unihanEntryCount - insertionPoint);
			}

			unihanEntries[insertionPoint] = data;
			++unihanEntryCount;
		}

		public UnicodeCharacterDataBuilder GetUcd(int codePoint)
		{
			int index = FindUcdCodePoint(codePoint);

			return index >= 0 ? ucdEntries[index] : null;
		}

		public UnihanCharacterDataBuilder GetUnihan(int codePoint)
		{
			int index = FindUnihanCodePoint(codePoint);

			if (index >= 0)
			{
				return unihanEntries[index];
			}
			else
			{
				var data = new UnihanCharacterDataBuilder(codePoint);

				Insert(data);

				return data;
			}
		}

		public void SetProperties(ContributoryProperties property, UnicodeCodePointRange codePointRange)
		{
			int firstIndex = FindUcdCodePoint(codePointRange.FirstCodePoint);
			int lastIndex = FindUcdCodePoint(codePointRange.LastCodePoint);

			if (firstIndex < 0 && lastIndex < 0)
			{
				Insert(new UnicodeCharacterDataBuilder(codePointRange) { ContributoryProperties = property });
				return;
			}

			if (firstIndex < 0
				|| lastIndex < 0
				|| ucdEntries[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| ucdEntries[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException();
			}

			int i = firstIndex;

			while (true)
			{
				ucdEntries[i].ContributoryProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		public void SetProperties(CoreProperties property, UnicodeCodePointRange codePointRange)
		{
			int firstIndex = FindUcdCodePoint(codePointRange.FirstCodePoint);
			int lastIndex = FindUcdCodePoint(codePointRange.LastCodePoint);

			if (firstIndex < 0 && lastIndex < 0)
			{
				Insert(new UnicodeCharacterDataBuilder(codePointRange) { CoreProperties = property });
				return;
			}

			if (firstIndex < 0
				|| lastIndex < 0
				|| ucdEntries[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| ucdEntries[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException();
			}

			int i = firstIndex;

			while (true)
			{
				ucdEntries[i].CoreProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		public void SetRadicalInfo(int radicalIndex, CjkRadicalData data)
		{
			if (radicalIndex < 1 || radicalIndex > CjkRadicalCount) throw new ArgumentOutOfRangeException(nameof(radicalIndex));

			cjkRadicals[radicalIndex - 1] = data;
		}

		public CjkRadicalData GetRadicalInfo(int radicalIndex)
		{
			if (radicalIndex < 1 || radicalIndex > CjkRadicalCount) throw new ArgumentOutOfRangeException(nameof(radicalIndex));

			return cjkRadicals[radicalIndex - 1];
		}

		public void AddBlockEntry(UnicodeBlock block)
		{
			blockEntries.Add(block);
		}

		//public UnicodeInfo ToUnicodeData()
		//{
		//	var finalUnicodeData = new UnicodeCharacterData[ucdEntryCount];

		//	for (int i = 0; i < finalUnicodeData.Length; ++i)
		//		finalUnicodeData[i] = ucdEntries[i].ToCharacterData();

		//	var finalUnihanData = new UnihanCharacterData[unihanEntryCount];

		//	for (int i = 0; i < finalUnihanData.Length; ++i)
		//		finalUnihanData[i] = unihanEntries[i].ToCharacterData();

		//	return new UnicodeInfo(unicodeVersion, finalUnicodeData, finalUnihanData, blockEntries.ToArray());
		//}

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
				writer.Write(new byte[] { (byte)'U', (byte)'C', (byte)'D', 1 });
				writer.Write((ushort)7); // Hardcode Unicode 7.0
				writer.Write((byte)0);
				writer.WriteCodePoint(ucdEntryCount);
				for (int i = 0; i < ucdEntryCount; ++i)
				{
					ucdEntries[i].WriteToFile(writer);
				}
				if (blockEntries.Count > 255) throw new InvalidOperationException("There are too many block entries. The file format needs to be upgraded.");
				writer.Write((byte)blockEntries.Count);
				for (int i = 0; i < blockEntries.Count; ++i)
				{
					WriteUnicodeBlockToFile(writer, blockEntries[i]);
				}
				writer.Write((byte)CjkRadicalCount);
				for (int i = 0; i < cjkRadicals.Length; ++i)
				{
					var radical = cjkRadicals[i];

					writer.Write((ushort)(radical.HasSimplifiedForm ? 0x8000 | radical.TraditionalRadicalCodePoint : radical.TraditionalRadicalCodePoint));
					writer.Write((ushort)radical.TraditionalCharacterCodePoint);

					if (radical.HasSimplifiedForm)
					{
						writer.Write((ushort)radical.SimplifiedRadicalCodePoint);
						writer.Write((ushort)radical.SimplifiedCharacterCodePoint);
					}
				}
				writer.WriteCodePoint(unihanEntryCount);
				for (int i = 0; i < unihanEntryCount; ++i)
				{
					unihanEntries[i].WriteToFile(writer);
				}
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
