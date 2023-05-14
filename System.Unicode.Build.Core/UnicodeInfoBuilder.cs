using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace System.Unicode.Build.Core
{
	public class UnicodeInfoBuilder
	{
		public const int CjkRadicalCount = 214; // The number of radicals (214) shouldn't change in the near future…

		private readonly Version _unicodeVersion;
		private UnicodeCharacterDataBuilder[] _ucdEntries = new UnicodeCharacterDataBuilder[10000];
		private int _ucdEntryCount;
		private UnihanCharacterDataBuilder[] _unihanEntries = new UnihanCharacterDataBuilder[10000];
		private int _unihanEntryCount;
		private readonly List<UnicodeBlock> _blockEntries = new List<UnicodeBlock>(100);
		private readonly CjkRadicalData[] _cjkRadicals = new CjkRadicalData[CjkRadicalCount];

		public UnicodeInfoBuilder(Version unicodeVersion) => _unicodeVersion = unicodeVersion;

		private int FindUcdCodePoint(int codePoint)
		{
			if (_ucdEntryCount == 0) return -1;

			int minIndex = 0;
			int maxIndex = _ucdEntryCount - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int δ = _ucdEntries[index].CodePointRange.CompareCodePoint(codePoint);

				if (δ == 0) return index;
				else if (δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		// Returns the index at which a new range can be insertes, or at which a pre-existing range overlaps.
		private int FindUcdInsertionPoint(int startCodePoint, int endCodePoint)
		{
			int minIndex;
			int maxIndex;

			if (_ucdEntryCount == 0 || _ucdEntries[maxIndex = _ucdEntryCount - 1].CodePointRange.LastCodePoint < startCodePoint) return _ucdEntryCount;
			else if (endCodePoint < _ucdEntries[minIndex = 0].CodePointRange.FirstCodePoint) return 0;
			else if (_ucdEntryCount == 1) return -1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int δ = _ucdEntries[index].CodePointRange.CompareCodePoint(startCodePoint);

				if (δ == 0) return index;
				else if (δ < 0) maxIndex = index;
				else minIndex = index;
			} while (maxIndex - minIndex > 1);

			return maxIndex;
		}

		private int FindUnihanCodePoint(int codePoint)
		{
			if (_unihanEntryCount == 0) return -1;

			int minIndex = 0;
			int maxIndex = _unihanEntryCount - 1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int δ = codePoint - _unihanEntries[index].CodePoint;

				if (δ == 0) return index;
				else if (δ < 0) maxIndex = index - 1;
				else minIndex = index + 1;
			} while (minIndex <= maxIndex);

			return -1;
		}

		private int FindUnihanInsertionPoint(int codePoint)
		{
			int minIndex;
			int maxIndex;

			if (_unihanEntryCount == 0 || _unihanEntries[maxIndex = _unihanEntryCount - 1].CodePoint < codePoint) return _unihanEntryCount;
			else if (codePoint < _unihanEntries[minIndex = 0].CodePoint) return 0;
			else if (_unihanEntryCount == 1) return -1;

			do
			{
				int index = (minIndex + maxIndex) >> 1;

				int δ = codePoint - _unihanEntries[index].CodePoint;

				if (δ == 0) return -1;
				else if (δ < 0) maxIndex = index;
				else minIndex = index;
			} while (maxIndex - minIndex > 1);

			if (_unihanEntries[maxIndex].CodePoint < codePoint) return -1;
			else return maxIndex;
		}

		// Returns true if the range has been inserted as a whole; false if multiple empty ranges have been created.
		// NB: We could split the provided range into multiple ones, but only the caller knows how to update the ranegs that were already there…
		public bool Insert(UnicodeCharacterDataBuilder data)
		{
			int insertionPoint = FindUcdInsertionPoint(data.CodePointRange.FirstCodePoint, data.CodePointRange.LastCodePoint);

			if (insertionPoint < 0) throw new InvalidOperationException("The specified range overlaps with pre-existing ranges.");

			int currentCodePoint = data.CodePointRange.FirstCodePoint;

			// Validate that the returned index properly indicates where the range starts.
			Debug.Assert(insertionPoint >= _ucdEntryCount || _ucdEntries[insertionPoint].CodePointRange.FirstCodePoint > currentCodePoint || _ucdEntries[insertionPoint].CodePointRange.Contains(currentCodePoint));

			// Either insert the specified data as a whole, or successively insert bits of empty data.
			while (currentCodePoint <= data.CodePointRange.LastCodePoint)
			{
				int lastCodePoint = data.CodePointRange.LastCodePoint;

				// If we insert at the position of an already existing range,
				// check how many code points we can span, or if we must skip the range.
				if (insertionPoint < _ucdEntryCount)
				{
					int firstCodePointAtInsertionPoint = _ucdEntries[insertionPoint].CodePointRange.FirstCodePoint;

					// Skip a range that has already been inserted.
					if (currentCodePoint >= firstCodePointAtInsertionPoint)
					{
						currentCodePoint = _ucdEntries[insertionPoint].CodePointRange.LastCodePoint + 1;
						insertionPoint++;
						continue;
					}

					// We can only insert a range that stops before the next one.
					lastCodePoint = Math.Min(lastCodePoint, firstCodePointAtInsertionPoint - 1);
				}

				if (_ucdEntryCount == _ucdEntries.Length)
				{
					Array.Resize(ref _ucdEntries, _ucdEntries.Length << 1);
				}

				if (insertionPoint < _ucdEntryCount)
				{
					Array.Copy(_ucdEntries, insertionPoint, _ucdEntries, insertionPoint + 1, _ucdEntryCount - insertionPoint);
				}

				++_ucdEntryCount;

				if (currentCodePoint == data.CodePointRange.FirstCodePoint && lastCodePoint == data.CodePointRange.LastCodePoint)
				{
					_ucdEntries[insertionPoint] = data;
					return true;
				}
				else
				{
					_ucdEntries[insertionPoint] = new UnicodeCharacterDataBuilder(new UnicodeCodePointRange(currentCodePoint, lastCodePoint));
					// We should be able to skip to after the next data, but that would make the code more complex.
					// Let's just waste the next iteration of the loop instead.
					insertionPoint++;
					currentCodePoint = lastCodePoint + 1;
				}
			}

			return false;
		}

		private void Insert(UnihanCharacterDataBuilder data)
		{
			int insertionPoint = FindUnihanInsertionPoint(data.CodePoint);

			if (insertionPoint < 0) throw new InvalidOperationException("The specified range overlaps with pre-existing ranges.");

			if (_unihanEntryCount == _unihanEntries.Length)
			{
				Array.Resize(ref _unihanEntries, _unihanEntries.Length << 1);
			}

			if (insertionPoint < _unihanEntryCount)
			{
				Array.Copy(_unihanEntries, insertionPoint, _unihanEntries, insertionPoint + 1, _unihanEntryCount - insertionPoint);
			}

			_unihanEntries[insertionPoint] = data;
			++_unihanEntryCount;
		}

		public UnicodeCharacterDataBuilder GetUcd(int codePoint)
		{
			int index = FindUcdCodePoint(codePoint);

			return index >= 0 ? _ucdEntries[index] : null;
		}

		public UnihanCharacterDataBuilder GetUnihan(int codePoint)
		{
			int index = FindUnihanCodePoint(codePoint);

			if (index >= 0)
			{
				return _unihanEntries[index];
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
			var (firstIndex, lastIndex) = FindCodePointRange(codePointRange);

			if (firstIndex < 0 || lastIndex < 0)
			{
				if (Insert(new UnicodeCharacterDataBuilder(codePointRange) { ContributoryProperties = property }))
				{
					return;
				}

				(firstIndex, lastIndex) = FindCodePointRange(codePointRange);
			}

			if (firstIndex < 0
				|| lastIndex < 0
				|| _ucdEntries[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| _ucdEntries[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException("Unable to find code point for setting contributory property.");
			}

			int i = firstIndex;

			while (true)
			{
				_ucdEntries[i].ContributoryProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		public void SetProperties(CoreProperties property, UnicodeCodePointRange codePointRange)
		{
			var (firstIndex, lastIndex) = FindCodePointRange(codePointRange);

			if (firstIndex < 0 || lastIndex < 0)
			{
				if (Insert(new UnicodeCharacterDataBuilder(codePointRange) { CoreProperties = property }))
				{
					return;
				}

				(firstIndex, lastIndex) = FindCodePointRange(codePointRange);
			}

			if (firstIndex < 0
				|| lastIndex < 0
				|| _ucdEntries[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| _ucdEntries[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException("Unable to find code point for setting core property.");
			}

			int i = firstIndex;

			while (true)
			{
				_ucdEntries[i].CoreProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		public void SetProperties(EmojiProperties property, UnicodeCodePointRange codePointRange)
		{
			var (firstIndex, lastIndex) = FindCodePointRange(codePointRange);

			if (firstIndex < 0 || lastIndex < 0)
			{
				if (Insert(new UnicodeCharacterDataBuilder(codePointRange) { EmojiProperties = property }))
				{
					return;
				}

				(firstIndex, lastIndex) = FindCodePointRange(codePointRange);
			}

			if (firstIndex < 0
				|| lastIndex < 0
				|| _ucdEntries[firstIndex].CodePointRange.FirstCodePoint < codePointRange.FirstCodePoint
				|| _ucdEntries[lastIndex].CodePointRange.LastCodePoint > codePointRange.LastCodePoint)
			{
				throw new InvalidOperationException("Unable to find code point for setting emoji property.");
			}

			int i = firstIndex;

			while (true)
			{
				_ucdEntries[i].EmojiProperties |= property;

				if (i == lastIndex) break;

				++i;
			}
		}

		private (int FirstIndex, int LastIndex) FindCodePointRange(UnicodeCodePointRange codePointRange)
			=> (FindUcdCodePoint(codePointRange.FirstCodePoint), FindUcdCodePoint(codePointRange.LastCodePoint));

		public void SetRadicalInfo(int radicalIndex, CjkRadicalData data)
		{
			if (radicalIndex < 1 || radicalIndex > CjkRadicalCount) throw new ArgumentOutOfRangeException(nameof(radicalIndex));

			_cjkRadicals[radicalIndex - 1] = data;
		}

		public CjkRadicalData GetRadicalInfo(int radicalIndex)
		{
			if (radicalIndex < 1 || radicalIndex > CjkRadicalCount) throw new ArgumentOutOfRangeException(nameof(radicalIndex));

			return _cjkRadicals[radicalIndex - 1];
		}

		public void AddBlockEntry(UnicodeBlock block) => _blockEntries.Add(block);

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
				writer.Write(new byte[] { (byte)'U', (byte)'C', (byte)'D', 2 });
				writer.Write(checked((ushort)_unicodeVersion.Major));
				writer.Write(checked((byte)_unicodeVersion.Minor));
				writer.Write(checked((byte)_unicodeVersion.Build));
				writer.WriteCodePoint(_ucdEntryCount);
				for (int i = 0; i < _ucdEntryCount; ++i)
				{
					_ucdEntries[i].WriteToFile(writer);
				}
				if (_blockEntries.Count > ushort.MaxValue) throw new InvalidOperationException("There are too many block entries. The file format needs to be upgraded.");
				writer.Write((ushort)_blockEntries.Count);
				for (int i = 0; i < _blockEntries.Count; ++i)
				{
					WriteUnicodeBlockToFile(writer, _blockEntries[i]);
				}
				writer.Write((byte)CjkRadicalCount);
				for (int i = 0; i < _cjkRadicals.Length; ++i)
				{
					var radical = _cjkRadicals[i];

					writer.Write((ushort)(radical.HasSimplifiedForm ? 0x8000 | radical.TraditionalRadicalCodePoint : radical.TraditionalRadicalCodePoint));
					writer.Write((ushort)radical.TraditionalCharacterCodePoint);

					if (radical.HasSimplifiedForm)
					{
						writer.Write((ushort)radical.SimplifiedRadicalCodePoint);
						writer.Write((ushort)radical.SimplifiedCharacterCodePoint);
					}
				}
				writer.WriteCodePoint(_unihanEntryCount);
				for (int i = 0; i < _unihanEntryCount; ++i)
				{
					_unihanEntries[i].WriteToFile(writer);
				}

				writer.Flush();
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
