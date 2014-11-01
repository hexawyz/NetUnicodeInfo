using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation
{
	public class UnicodeDataManager
	{
		public static readonly Uri UnicodeCharacterDataUri = new Uri("http://www.unicode.org/Public/UCD/latest/ucd/", UriKind.Absolute);
		public const string UnicodeDataFileName = "UnicodeData.txt";
		public const string PropListFileName = "PropList.txt";

		public static async Task<UnicodeData> DownloadAndBuildDataAsync()
		{
			var characterDataBuilders = new List<UnicodeCharacterDataBuilder>();

			using (var httpClient = new HttpClient())
			{
				using (var reader = new UnicodeDataFileReader(await httpClient.GetStreamAsync(UnicodeCharacterDataUri + UnicodeDataFileName).ConfigureAwait(false)))
				{
					while (reader.MoveToNextLine())
					{
						// NB: Fields 10 and 11 are deemed obsolete. Field 11 should always be empty, and will be ignored here.
						var characterData = new UnicodeCharacterDataBuilder(int.Parse(reader.ReadField(), NumberStyles.HexNumber))
						{
							Name = reader.ReadField(),
							Category = UnicodeCategoryInfo.FromShortName(reader.ReadField()).Category,
							CanonicalCombiningClass = (CanonicalCombiningClass)byte.Parse(reader.ReadField()),
							BidirectionalClass = reader.ReadField(),
							DecompositionType = reader.ReadField()
						};

						string numericDecimalField = reader.ReadField();
						string numericDigitField = reader.ReadField();
						string numericNumericField = reader.ReadField();

						characterData.BidirectionalMirrored = reader.ReadField() == "Y";
						characterData.OldName = reader.ReadField();
						reader.SkipField();
						characterData.SimpleUpperCaseMapping = reader.ReadField();
						characterData.SimpleLowerCaseMapping = reader.ReadField();
						characterData.SimpleTitleCaseMapping = reader.ReadField();

						// Handle Numeric_Type & Numeric_Value:
						// If field 6 is set, fields 7 and 8 should have the same value, and Numeric_Type is Decimal.
						// If field 6 is not set but field 7 is set, field 8 should be set and have the same value. Then, the type is Digit.
						// If field 6 and 7 are not set, but field 8 is set, then Numeric_Type is Numeric.
						if (!string.IsNullOrEmpty(numericNumericField))
						{
							characterData.NumericValue = UnicodeRationalNumber.Parse(numericNumericField);

							if (!string.IsNullOrEmpty(numericDigitField))
							{
								if (numericDigitField != numericNumericField)
								{
									throw new InvalidDataException("Invalid value for field 7 of character U+" + characterData.CodePoint.ToString("X4") + ".");
								}

								if (!string.IsNullOrEmpty(numericDecimalField))
								{
									if (numericDecimalField != numericDigitField)
									{
										throw new InvalidDataException("Invalid value for field 6 of character U+" + characterData.CodePoint.ToString("X4") + ".");
									}
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

						characterDataBuilders.Add(characterData);
					}
				}
				/*
				using (var reader = new UnicodeDataFileReader(await httpClient.GetStreamAsync(UnicodeCharacterDataUri + PropListFileName).ConfigureAwait(false)))
				{
					while (reader.MoveToNextLine())
					{
						ContributoryProperties property;

						var range = UnicodeCharacterRange.Parse(reader.ReadField().TrimEnd());
						if (EnumHelper<ContributoryProperties>.TryGetNamedValue(reader.ReadField().Trim(), out property))
						{
						}
					}
                }
				*/
			}

			var finalData = new UnicodeCharacterData[characterDataBuilders.Count];

			for (int i = 0; i < finalData.Length; ++i)
				finalData[i] = characterDataBuilders[i].ToCharacterData();

			return new UnicodeData(new Version(7, 0), finalData);
		}
	}
}
