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
				using (var reader = new StreamReader(await httpClient.GetStreamAsync(UnicodeCharacterDataUri + UnicodeDataFileName).ConfigureAwait(false), Encoding.UTF8, false))
				{
					string line;

					while (!string.IsNullOrEmpty((line = await reader.ReadLineAsync().ConfigureAwait(false))))
					{
						var fields = line.Split(';');

						if (fields.Length < 15) throw new InvalidDataException();

						// NB: Fields 10 and 11 are deemed obsolete. Field 11 should always be empty, and will be ignored here.
						var characterData = new UnicodeCharacterDataBuilder(int.Parse(fields[0], NumberStyles.HexNumber))
						{
							Name = fields[1],
							Category = UnicodeCategoryInfo.FromShortName(fields[2]).Category,
							CanonicalCombiningClass = (CanonicalCombiningClass)byte.Parse(fields[3]),
							BidirectionalClass = fields[4],
							DecompositionType = fields[5],
							BidirectionalMirrored = fields[9] == "Y",
							OldName = fields[10],
							SimpleUpperCaseMapping = fields[12],
							SimpleLowerCaseMapping = fields[13],
							SimpleTitleCaseMapping = fields[14]
						};

						// Handle Numeric_Type & Numeric_Value:
						// If field 6 is set, fields 7 and 8 should have the same value, and Numeric_Type is Decimal.
						// If field 6 is not set but field 7 is set, field 8 should be set and have the same value. Then, the type is Digit.
						// If field 6 and 7 are not set, but field 8 is set, then Numeric_Type is Numeric.
						if (!string.IsNullOrEmpty(fields[8]))
						{
							characterData.NumericValue = UnicodeRationalNumber.Parse(fields[8]);

							if (!string.IsNullOrEmpty(fields[7]))
							{
								if (fields[7] != fields[8])
								{
									throw new InvalidDataException("Invalid value for field 7 of character U+" + characterData.CodePoint.ToString("X4") + ".");
								}

								if (!string.IsNullOrEmpty(fields[6]))
								{
									if (fields[6] != fields[7])
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

				//using (var reader = new StreamReader(await httpClient.GetStreamAsync(UnicodeCharacterDataUri + PropListFileName)))
				//{
				//}
			}

			var finalData = new UnicodeCharacterData[characterDataBuilders.Count];

			for (int i = 0; i < finalData.Length; ++i)
				finalData[i] = characterDataBuilders[i].ToCharacterData();

			return new UnicodeData(new Version(7, 0), finalData);
		}
	}
}
