namespace System.Unicode
{
	partial struct UnihanCharacterData
	{
		// This method densely packs code points by predicted order of importance (it may be wrong)
		// Its purpose is to avoid skipping numbers so that file encoding can be more efficient.
		internal static int PackCodePoint(int codePoint)
		{
			if (codePoint >= 0x3400)
			{
				// 3400..4DBF; CJK Unified Ideographs Extension A
				if (codePoint < 0x4DC0) return codePoint + 0x01E00;
				else if (codePoint >= 0x4E00)
				{
					// 4E00..9FFF; CJK Unified Ideographs
					if (codePoint < 0xA000) return codePoint - 0x04E00;
					else if (codePoint >= 0xF900)
					{
						// F900..FAFF; CJK Compatibility Ideographs
						if (codePoint < 0xFB00) return codePoint + 0x071E0;
						else if (codePoint >= 0x20000)
						{
							// 20000..2A6DF; CJK Unified Ideographs Extension B
							if (codePoint < 0x2A6E0) return codePoint - 0x19440;
							else if (codePoint >= 0x2A700)
							{
								// 2A700..2B73F; CJK Unified Ideographs Extension C
								// 2B740..2B81F; CJK Unified Ideographs Extension D
								// 2B820..2CEAF; CJK Unified Ideographs Extension E
								// 2CEB0..2EBEF; CJK Unified Ideographs Extension F
								if (codePoint < 0x2EBF0) return codePoint - 0x19460;
								else if (codePoint >= 0x2F800)
								{
									// 2F800..2FA1F; CJK Compatibility Ideographs Supplement
									if (codePoint < 0x2FA20) return codePoint - 0x18B20;
									else if (codePoint >= 0x30000)
									{
										// 30000..3134F; CJK Unified Ideographs Extension G
										if (codePoint < 0x31350) return codePoint - 0x1A870;
									}
								}
							}
						}
					}
				}
			}

			throw new ArgumentOutOfRangeException(nameof(codePoint));
		}

		// Reverses the packing done by the PackCodePoint method.
		internal static int UnpackCodePoint(int packedCodePoint)
		{
			if (packedCodePoint >= 0)
			{
				// 4E00..9FFF; CJK Unified Ideographs
				if (packedCodePoint < 0x05200) return packedCodePoint + 0x4E00;
				// 3400..4DBF; CJK Unified Ideographs Extension A
				else if (packedCodePoint < 0x06BC0) return packedCodePoint - 0x1E00;
				// 20000..2A6DF; CJK Unified Ideographs Extension B
				else if (packedCodePoint < 0x112A0) return packedCodePoint + 0x19440;
				// 2A700..2B73F; CJK Unified Ideographs Extension C
				// 2B740..2B81F; CJK Unified Ideographs Extension D
				// 2B820..2CEAF; CJK Unified Ideographs Extension E
				// 2CEB0..2EBEF; CJK Unified Ideographs Extension F
				else if (packedCodePoint < 0x15790) return packedCodePoint + 0x19460;
				// 30000..3134F; CJK Unified Ideographs Extension G
				else if (packedCodePoint < 0x16AE0) return packedCodePoint + 0x1A870;
				// F900..FAFF; CJK Compatibility Ideographs
				else if (packedCodePoint < 0x16CE0) return packedCodePoint - 0x71E0;
				// 2F800..2FA1F; CJK Compatibility Ideographs Supplement
				else if (packedCodePoint < 0x16F00) return packedCodePoint + 0x18B20;
			}
			throw new ArgumentOutOfRangeException(nameof(packedCodePoint));
		}
	}
}

