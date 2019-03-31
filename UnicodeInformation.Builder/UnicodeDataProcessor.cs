using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	internal class UnicodeDataProcessor
	{
		public const string ReadMeFileName = "ReadMe.txt";
		public const string UnicodeDataFileName = "UnicodeData.txt";
		public const string PropListFileName = "PropList.txt";
		public const string DerivedCorePropertiesFileName = "DerivedCoreProperties.txt";
		public const string CjkRadicalsFileName = "CJKRadicals.txt";
		public const string NameAliasesFileName = "NameAliases.txt";
		public const string NamesListFileName = "NamesList.txt";
		public const string BlocksFileName = "Blocks.txt";
		public const string UnihanReadingsFileName = "Unihan_Readings.txt";
		public const string UnihanVariantsFileName = "Unihan_Variants.txt";
		public const string UnihanNumericValuesFileName = "Unihan_NumericValues.txt";
		public const string UnihanIrgSourcesFileName = "Unihan_IRGSources.txt";
		public const string EmojiDataFileName = "emoji-data.txt";

		private static string ParseSimpleCaseMapping(string mapping)
		{
			if (string.IsNullOrEmpty(mapping)) return null;

			return char.ConvertFromUtf32(int.Parse(mapping, NumberStyles.HexNumber));
		}

		private static string NullIfEmpty(string s)
		{
			return string.IsNullOrEmpty(s) ? null : s;
		}

		public static async Task<UnicodeInfoBuilder> BuildDataAsync(IDataSource ucdSource, IDataSource unihanSource, IDataSource emojiSource)
		{
			var builder = new UnicodeInfoBuilder(await ReadUnicodeVersionAsync(ucdSource).ConfigureAwait(false));

			await ProcessUnicodeDataFile(ucdSource, builder).ConfigureAwait(false);
			await ProcessPropListFile(ucdSource, builder).ConfigureAwait(false);
			await ProcessDerivedCorePropertiesFile(ucdSource, builder).ConfigureAwait(false);
			await ProcessEmojiDataFile(emojiSource, builder).ConfigureAwait(false);
			await ProcessCjkRadicalsFile(ucdSource, builder).ConfigureAwait(false);
			await ProcessNameAliasesFile(ucdSource, builder).ConfigureAwait(false);
			await ProcessNamesListFile(ucdSource, builder).ConfigureAwait(false);
			await ProcessBlocksFile(ucdSource, builder).ConfigureAwait(false);
			await ProcessUnihanReadings(unihanSource, builder).ConfigureAwait(false);
			await ProcessUnihanVariants(unihanSource, builder).ConfigureAwait(false);
			await ProcessUnihanNumericValues(unihanSource, builder).ConfigureAwait(false);
			await ProcessUnihanIrgSources(unihanSource, builder).ConfigureAwait(false);

			return builder;
		}

		private static async Task<Version> ReadUnicodeVersionAsync(IDataSource ucdSource)
		{
			using (var reader = new StreamReader(await ucdSource.OpenDataFileAsync(ReadMeFileName).ConfigureAwait(false), Encoding.UTF8, true))
			{
				string text = await reader.ReadToEndAsync().ConfigureAwait(false);

				var match = Regex.Match(text, @"Version\s+(?<Major>[1-9][0-9]*)\.(?<Minor>[0-9]+)\.(?<Update>[0-9]+)\s+of\s+the\s+Unicode\s+Standard\.");
				if (!match.Success)
				{
					match = Regex.Match(text, @"Unicode\s+(?<Major>[1-9][0-9]*)\.(?<Minor>[0-9]+)\.(?<Update>[0-9]+)\.");
				}

				if (!match.Success) throw new InvalidDataException("Could not determine the version of the Unicode standard defined in the files.");

				return new Version(int.Parse(match.Groups["Major"].Value), int.Parse(match.Groups["Minor"].Value), int.Parse(match.Groups["Update"].Value));
			}
		}

		private static async Task ProcessUnicodeDataFile(IDataSource ucdSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnicodeDataFileReader(await ucdSource.OpenDataFileAsync(UnicodeDataFileName).ConfigureAwait(false), ';'))
			{
				int rangeStartCodePoint = -1;

				while (reader.MoveToNextLine())
				{
					var codePoint = new UnicodeCodePointRange(int.Parse(reader.ReadField(), NumberStyles.HexNumber));

					string name = reader.ReadField();

					if (!string.IsNullOrEmpty(name) && name[0] == '<' && name[name.Length - 1] == '>')
					{
						if (name.EndsWith(", First>", StringComparison.OrdinalIgnoreCase))
						{
							if (rangeStartCodePoint >= 0) throw new InvalidDataException("Invalid range data in UnicodeData.txt.");

							rangeStartCodePoint = codePoint.FirstCodePoint;

							continue;
						}
						else if (name.EndsWith(", Last>", StringComparison.OrdinalIgnoreCase))
						{
							if (rangeStartCodePoint < 0) throw new InvalidDataException("Invalid range data in UnicodeData.txt.");

							codePoint = new UnicodeCodePointRange(rangeStartCodePoint, codePoint.LastCodePoint);

							name = name.Substring(1, name.Length - 8).ToUpperInvariant(); // Upper-case the name in order to respect unicode naming scheme. (Spec says all names are uppercase ASCII)

							rangeStartCodePoint = -1;
						}
						else if (name == "<control>") // Ignore the name of the property for these code points, as it should really be empty by the spec.
						{
							// For control characters, we can derive a character label in of the form <control-NNNN>, which is not the character name.
							name = null;
						}
						else
						{
							throw new InvalidDataException("Unexpected code point name tag: " + name + ".");
						}
					}
					else if (rangeStartCodePoint >= 0)
					{
						throw new InvalidDataException("Invalid range data in UnicodeData.txt.");
					}

					// NB: Fields 10 and 11 are deemed obsolete. Field 11 should always be empty, and will be ignored here.
					var characterData = new UnicodeCharacterDataBuilder(codePoint)
					{
						Name = NullIfEmpty(name),
						Category = UnicodeCategoryInfo.FromShortName(reader.ReadField()).Category,
						CanonicalCombiningClass = (CanonicalCombiningClass)byte.Parse(reader.ReadField()),
					};

					if (EnumHelper<BidirectionalClass>.TryGetNamedValue(reader.ReadField(), out var bidirectionalClass))
					{
						characterData.BidirectionalClass = bidirectionalClass;
					}
					else
					{
						throw new InvalidDataException(string.Format("Missing Bidi_Class property for code point(s) {0}.", codePoint));
					}

					characterData.CharacterDecompositionMapping = CharacterDecompositionMapping.Parse(NullIfEmpty(reader.ReadField()));

					string numericDecimalField = NullIfEmpty(reader.ReadField());
					string numericDigitField = NullIfEmpty(reader.ReadField());
					string numericNumericField = NullIfEmpty(reader.ReadField());

					characterData.BidirectionalMirrored = reader.ReadField() == "Y";
					characterData.OldName = NullIfEmpty(reader.ReadField());
					reader.SkipField();
					characterData.SimpleUpperCaseMapping = ParseSimpleCaseMapping(reader.ReadField());
					characterData.SimpleLowerCaseMapping = ParseSimpleCaseMapping(reader.ReadField());
					characterData.SimpleTitleCaseMapping = ParseSimpleCaseMapping(reader.ReadField());

					// Handle Numeric_Type & Numeric_Value:
					// If field 6 is set, fields 7 and 8 should have the same value, and Numeric_Type is Decimal.
					// If field 6 is not set but field 7 is set, field 8 should be set and have the same value. Then, the type is Digit.
					// If field 6 and 7 are not set, but field 8 is set, then Numeric_Type is Numeric.
					if (numericNumericField != null)
					{
						characterData.NumericValue = UnicodeRationalNumber.Parse(numericNumericField);

						if (numericDigitField != null)
						{
							if (numericDigitField != numericNumericField)
							{
								throw new InvalidDataException("Invalid value for field 7 of code point " + characterData.CodePointRange.ToString() + ".");
							}

							if (numericDecimalField != null)
							{
								if (numericDecimalField != numericDigitField)
								{
									throw new InvalidDataException("Invalid value for field 6 of code point " + characterData.CodePointRange.ToString() + ".");
								}
								characterData.NumericType = UnicodeNumericType.Decimal;
							}
							else
							{
								characterData.NumericType = UnicodeNumericType.Digit;
							}
						}
						else
						{
							characterData.NumericType = UnicodeNumericType.Numeric;
						}
					}

					builder.Insert(characterData);
				}
			}
		}

		private static async Task ProcessPropListFile(IDataSource ucdSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnicodeDataFileReader(await ucdSource.OpenDataFileAsync(PropListFileName).ConfigureAwait(false), ';'))
			{
				while (reader.MoveToNextLine())
				{
					var range = UnicodeCodePointRange.Parse(reader.ReadTrimmedField());
					if (EnumHelper<ContributoryProperties>.TryGetNamedValue(reader.ReadTrimmedField(), out var property))
					{
						builder.SetProperties(property, range);
					}
				}
			}
		}

		private static async Task ProcessDerivedCorePropertiesFile(IDataSource ucdSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnicodeDataFileReader(await ucdSource.OpenDataFileAsync(DerivedCorePropertiesFileName).ConfigureAwait(false), ';'))
			{
				while (reader.MoveToNextLine())
				{
					var range = UnicodeCodePointRange.Parse(reader.ReadTrimmedField());
					if (EnumHelper<CoreProperties>.TryGetNamedValue(reader.ReadTrimmedField(), out var property))
					{
						builder.SetProperties(property, range);
					}
				}
			}
		}

		private static async Task ProcessEmojiDataFile(IDataSource emojiSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnicodeDataFileReader(await emojiSource.OpenDataFileAsync(EmojiDataFileName).ConfigureAwait(false), ';'))
			{
				while (reader.MoveToNextLine())
				{
					var range = UnicodeCodePointRange.Parse(reader.ReadTrimmedField());
					if (EnumHelper<EmojiProperties>.TryGetNamedValue(reader.ReadTrimmedField(), out var property))
					{
						builder.SetProperties(property, range);
					}
				}
			}
		}

		private static async Task ProcessCjkRadicalsFile(IDataSource ucdSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnicodeDataFileReader(await ucdSource.OpenDataFileAsync(CjkRadicalsFileName).ConfigureAwait(false), ';'))
			{
				int lastReadRadical = 0;

				while (reader.MoveToNextLine())
				{
					string radicalIndexText = reader.ReadField();
					bool isSimplified = radicalIndexText[radicalIndexText.Length - 1] == '\'';
					int radicalIndex = int.Parse(isSimplified ? radicalIndexText.Substring(0, radicalIndexText.Length - 1) : radicalIndexText);

					if (isSimplified ? radicalIndex != lastReadRadical : lastReadRadical + 1 != (lastReadRadical = radicalIndex))
						throw new InvalidDataException("Did not expect radical number " + radicalIndexText + ".");

					char radicalCodePoint = checked((char)int.Parse(reader.ReadTrimmedField(), NumberStyles.HexNumber));
					char characterCodePoint = checked((char)int.Parse(reader.ReadTrimmedField(), NumberStyles.HexNumber));

					if (!isSimplified && (radicalCodePoint & 0x8000) != 0)
						throw new InvalidOperationException("Did not expect the radical code point to be higher than U+8000 for radical " + radicalIndex.ToString() + ".");

					if (isSimplified)
					{
						builder.SetRadicalInfo(radicalIndex, UpdateRadicalData(builder.GetRadicalInfo(radicalIndex), radicalCodePoint, characterCodePoint));
					}
					else
					{
						builder.SetRadicalInfo(radicalIndex, new CjkRadicalData(radicalCodePoint, characterCodePoint));
					}
				}

				if (lastReadRadical != UnicodeInfoBuilder.CjkRadicalCount)
					throw new InvalidOperationException("There was not enough data for the 214 CJK radicals.");
			}
		}

		private static CjkRadicalData UpdateRadicalData(CjkRadicalData traditionalData, char simplifiedRadicalCodePoint, char simplifiedCharacterCodePoint)
			=> new CjkRadicalData
			(
				traditionalData.TraditionalRadicalCodePoint,
				traditionalData.TraditionalCharacterCodePoint,
				simplifiedRadicalCodePoint,
				simplifiedCharacterCodePoint
			);

		private static async Task ProcessNameAliasesFile(IDataSource ucdSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnicodeDataFileReader(await ucdSource.OpenDataFileAsync(NameAliasesFileName).ConfigureAwait(false), ';'))
			{
				while (reader.MoveToNextLine())
				{
					var ucd = builder.GetUcd(int.Parse(reader.ReadField(), NumberStyles.HexNumber));

					string name = reader.ReadField();
					string kindName = reader.ReadField();

					if (!EnumHelper<UnicodeNameAliasKind>.TryGetNamedValue(kindName, out var kind))
						throw new InvalidDataException("Unrecognized name alias: " + kindName + ".3");

					ucd.NameAliases.Add(new UnicodeNameAlias(name, kind));
				}
			}
		}

		private static async Task ProcessNamesListFile(IDataSource ucdSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new StreamReader(await ucdSource.OpenDataFileAsync(NamesListFileName).ConfigureAwait(false), Encoding.UTF8, false))
			{
				string line;
				var characterData = null as UnicodeCharacterDataBuilder;

				while ((line = reader.ReadLine()) != null)
				{
					if (line.Length == 0)
					{
						characterData = null;
						continue;
					}

					if (characterData != null && line.Length > 3 && line[0] == '\t')
					{
						if (line[1] == 'x')
						{
							// We should get at least 7 characters for a valid line: <tab> "x" <space> [0-9A-Z][0-9A-Z][0-9A-Z][0-9A-Z]
							if (line.Length < 7)
							{
								characterData = null;
								continue;
							}
							if (line[2] != ' ') throw new InvalidDataException();

							int length;

							if (line[3].IsHexDigit())
							{
								length = line.IndexOf(' ', 3);
								if (length < 0) length = line.Length;
								length -= 3;

								characterData.CrossRerefences.Add(int.Parse(line.Substring(3, length), NumberStyles.HexNumber));
							}
							else if (line[3] == '(')
							{
								bool hasBrackets = line[4] == '<';
								int codePointOffset = line.IndexOf(hasBrackets ? "> - " : "- ", 4);

								if (codePointOffset < 0) throw new InvalidDataException();
								codePointOffset += hasBrackets ? 4 : 2;

								length = line.IndexOf(')', codePointOffset);
								if (length < 0) throw new InvalidDataException();
								length -= codePointOffset;

								characterData.CrossRerefences.Add(int.Parse(line.Substring(codePointOffset, length), NumberStyles.HexNumber));
							}
							else throw new InvalidDataException();
						}
						continue;
					}

					if (line[0].IsHexDigit())
					{
						int codePoint = int.Parse(line.Substring(0, line.IndexOf('\t')), NumberStyles.HexNumber);
						// This may return null, but for now, we will ignore code points that are not defined in UnicodeData.txt.
						characterData = builder.GetUcd(codePoint);
						// There should be no NamesList.txt entries for code points defined in a range.
						if (characterData != null && !characterData.CodePointRange.IsSingleCodePoint)
						{
							// The only exception to this rule will be when we added the "Noncharacter_Code_Point" property to a few ranges, and we will ignore those.
							if ((characterData.ContributoryProperties & ContributoryProperties.NonCharacterCodePoint) != 0)
								characterData = null;
							else
								throw new InvalidDataException("Did not expect an NamesList.txt entry for U+" + codePoint.ToString("X4") + ".");
						}
						continue;
					}

					switch (line[0])
					{
					case '@':
					case ';':
					case '\t':
						characterData = null;
						break;
					default:
						throw new InvalidDataException("Unrecognized data in NamesList.txt.");
					}
				}
			}
		}

		private static async Task ProcessBlocksFile(IDataSource ucdSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnicodeDataFileReader(await ucdSource.OpenDataFileAsync(BlocksFileName).ConfigureAwait(false), ';'))
			{
				while (reader.MoveToNextLine())
				{
					builder.AddBlockEntry(new UnicodeBlock(UnicodeCodePointRange.Parse(reader.ReadField()), reader.ReadTrimmedField()));
				}
			}
		}

		private static async Task ProcessUnihanReadings(IDataSource unihanDataSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnihanDataFileReader(await unihanDataSource.OpenDataFileAsync(UnihanReadingsFileName).ConfigureAwait(false)))
			{
				while (reader.Read())
				{
					// This statement is used to skip unhandled properties entirely.
					switch (reader.PropertyName)
					{
					case UnihanProperty.kDefinition:
					case UnihanProperty.kMandarin:
					case UnihanProperty.kCantonese:
					case UnihanProperty.kJapaneseKun:
					case UnihanProperty.kJapaneseOn:
					case UnihanProperty.kKorean:
					case UnihanProperty.kHangul:
					case UnihanProperty.kVietnamese:
						break;
					default:
						// Ignore unhandled properties for now.
						continue;
					}

					// This entry will only be created if there is meaningful data.
					var entry = builder.GetUnihan(reader.CodePoint);

					switch (reader.PropertyName)
					{
					case UnihanProperty.kDefinition:
						entry.Definition = reader.PropertyValue;
						break;
					case UnihanProperty.kMandarin:
						entry.MandarinReading = reader.PropertyValue;
						break;
					case UnihanProperty.kCantonese:
						entry.CantoneseReading = reader.PropertyValue;
						break;
					case UnihanProperty.kJapaneseKun:
						entry.JapaneseKunReading = reader.PropertyValue;
						break;
					case UnihanProperty.kJapaneseOn:
						entry.JapaneseOnReading = reader.PropertyValue;
						break;
					case UnihanProperty.kKorean:
						entry.KoreanReading = reader.PropertyValue;
						break;
					case UnihanProperty.kHangul:
						entry.HangulReading = reader.PropertyValue;
						break;
					case UnihanProperty.kVietnamese:
						entry.VietnameseReading = reader.PropertyValue;
						break;
					default:
						throw new InvalidOperationException();
					}
				}
			}
		}

		private static async Task ProcessUnihanVariants(IDataSource unihanDataSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnihanDataFileReader(await unihanDataSource.OpenDataFileAsync(UnihanVariantsFileName).ConfigureAwait(false)))
			{
				while (reader.Read())
				{
					// This statement is used to skip unhandled properties entirely.
					switch (reader.PropertyName)
					{
					case UnihanProperty.kSimplifiedVariant:
					case UnihanProperty.kTraditionalVariant:
						break;
					default:
						// Ignore unhandled properties for now.
						continue;
					}

					var entry = builder.GetUnihan(reader.CodePoint);

					switch (reader.PropertyName)
					{
					case UnihanProperty.kSimplifiedVariant:
						entry.SimplifiedVariant = char.ConvertFromUtf32(HexCodePoint.ParsePrefixed(reader.PropertyValue));
						break;
					case UnihanProperty.kTraditionalVariant:
						entry.TraditionalVariant = char.ConvertFromUtf32(HexCodePoint.ParsePrefixed(reader.PropertyValue));
						break;
					default:
						throw new InvalidOperationException();
					}
				}
			}
		}

		private static async Task ProcessUnihanNumericValues(IDataSource unihanDataSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnihanDataFileReader(await unihanDataSource.OpenDataFileAsync(UnihanNumericValuesFileName).ConfigureAwait(false)))
			{
				while (reader.Read())
				{
					var entry = builder.GetUnihan(reader.CodePoint);

					switch (reader.PropertyName)
					{
					case UnihanProperty.kAccountingNumeric:
						entry.NumericType = UnihanNumericType.Accounting;
						break;
					case UnihanProperty.kOtherNumeric:
						entry.NumericType = UnihanNumericType.Other;
						break;
					case UnihanProperty.kPrimaryNumeric:
						entry.NumericType = UnihanNumericType.Primary;
						break;
					default:
						throw new InvalidDataException("Unrecognized property name: " + reader.PropertyName + ".");
					}

					entry.NumericValue = long.Parse(reader.PropertyValue);
				}
			}
		}

		private static async Task ProcessUnihanIrgSources(IDataSource unihanDataSource, UnicodeInfoBuilder builder)
		{
			using (var reader = new UnihanDataFileReader(await unihanDataSource.OpenDataFileAsync(UnihanIrgSourcesFileName).ConfigureAwait(false)))
			{
				while (reader.Read())
				{
					switch (reader.PropertyName)
					{
					case UnihanProperty.kRSUnicode:
						var entry = builder.GetUnihan(reader.CodePoint);
						var values = reader.PropertyValue.Split(' ');

						foreach (string value in values)
						{
							bool isSimplified = false;
							int index;

							for (int i = 0; i < value.Length; ++i)
							{
								switch (value[i])
								{
								case '\'':
									isSimplified = true;
									goto case '.';
								case '.':
									index = i;
									goto SeparatorFound;
								}
							}
							throw new InvalidDataException("Failed to decode value for kRSUnicode / Unicode_Radical_Stroke.");

						SeparatorFound:;
							entry.UnicodeRadicalStrokeCounts.Add(new UnicodeRadicalStrokeCount(byte.Parse(value.Substring(0, index), NumberStyles.None), sbyte.Parse(value.Substring(index + (isSimplified ? 2 : 1)), NumberStyles.AllowLeadingSign), isSimplified));
						}
						break;
					default:
						// Ignore unhandled properties for now.
						break;
					}
				}
			}
		}
	}
}
