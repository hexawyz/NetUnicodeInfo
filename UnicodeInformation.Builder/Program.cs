using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	internal class Program
	{
		public static readonly Uri UnicodeCharacterDataUri = new Uri("http://www.unicode.org/Public/UCD/latest/ucd/", UriKind.Absolute);
		public static readonly Uri EmojiDataUri = new Uri("http://www.unicode.org/Public/emoji/latest/", UriKind.Absolute);

		public const string UnihanDirectoryName = "Unihan";
		public const string UnihanArchiveName = "Unihan.zip";
		public const string UcdDirectoryName = "UCD";
		public const string UcdArchiveName = "UCD.zip";

		public static readonly string[] ucdRequiredFiles = new[]
		{
			"UnicodeData.txt",
			"PropList.txt",
			"DerivedCoreProperties.txt",
			"CJKRadicals.txt",
			//"Jamo.txt", // Not used right now, as the hangul syllable algorithm implementation takes care of this.
			"NameAliases.txt",
			"NamesList.txt",
			"Blocks.txt",
		};

		public static readonly string[] unihanRequiredFiles = new[]
		{
			"Unihan_NumericValues.txt",
			"Unihan_Readings.txt",
			"Unihan_Variants.txt",
			"Unihan_IRGSources.txt",
		};

		public static readonly string[] emojiRequiredFiles = new[]
		{
			"emoji-data.txt",
			"emoji-sequences.txt",
			"emoji-variation-sequences.txt",
			"emoji-zwj-sequences.txt",
		};

		private static HttpClient httpClient;

		// The only purpose of this is for tests…
		internal static void SetHttpMessageHandler(HttpMessageHandler handler)
		{
			httpClient = new HttpClient(handler ?? new HttpClientHandler());
		}

		internal static HttpClient HttpClient { get { return httpClient ?? (httpClient = new HttpClient()); } }

		private static Task<byte[]> DownloadDataArchiveAsync(string archiveName)
			=> HttpClient.GetByteArrayAsync(UnicodeCharacterDataUri + archiveName);

		internal static async Task<IDataSource> GetDataSourceAsync(string archiveName, string directoryName, string[] requiredFiles, bool? shouldDownload, bool? shouldSaveArchive, bool? shouldExtract)
		{
			string baseDirectory = Directory.GetCurrentDirectory();
			string dataDirectory = Path.Combine(baseDirectory, UcdDirectoryName);
			string dataArchiveFileName = Path.Combine(baseDirectory, archiveName);

			if (shouldDownload != true)
			{
				bool hasValidDirectory = Directory.Exists(dataDirectory);

				if (hasValidDirectory)
				{
					foreach (string requiredFile in requiredFiles)
					{
						if (!File.Exists(Path.Combine(dataDirectory, requiredFile)))
						{
							hasValidDirectory = false;
							break;
						}
					}
				}

				if (hasValidDirectory)
				{
					return new FileDataSource(dataDirectory);
				}

				if (File.Exists(dataArchiveFileName))
				{
					if (shouldExtract == true)
					{
						ZipFile.ExtractToDirectory(dataArchiveFileName, dataDirectory);
						return new FileDataSource(dataDirectory);
					}
					else
					{
						return new ZipDataSource(File.OpenRead(dataArchiveFileName));
					}
				}
			}

			if (shouldDownload != false)
			{
				var dataArchiveData = await DownloadDataArchiveAsync(archiveName).ConfigureAwait(false);

				if (shouldSaveArchive == true)
				{
					using (var stream = File.Open(dataArchiveFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
					{
						await stream.WriteAsync(dataArchiveData, 0, dataArchiveData.Length).ConfigureAwait(false);
						dataArchiveData = null; // Release the reference now, since we won't need it anymore.

						if (shouldExtract == true)
						{
							using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, false))
							{
								archive.ExtractToDirectory(dataDirectory);

								return new FileDataSource(dataDirectory);
							}
						}
						else
						{
							return new ZipDataSource(stream);
						}
					}
				}
				else
				{
					return new ZipDataSource(new MemoryStream(dataArchiveData));
				}
			}

			throw new InvalidOperationException();
		}

		private static async Task MainAsync(string[] args)
		{
			UnicodeInfoBuilder data;

			using (var ucdSource = await GetDataSourceAsync(UcdArchiveName, UcdDirectoryName, ucdRequiredFiles, null, null, null))
			using (var unihanSource = await GetDataSourceAsync(UnihanArchiveName, UnihanDirectoryName, unihanRequiredFiles, null, null, null))
			using (var emojiSource = new HttpDataSource(EmojiDataUri, HttpClient))
			{
				data = await UnicodeDataProcessor.BuildDataAsync(ucdSource, unihanSource, emojiSource);
			}

			using (var stream = new DeflateStream(File.Create("ucd.dat"), CompressionLevel.Optimal, false))
				data.WriteToStream(stream);
		}

		private static void Main(string[] args) => MainAsync(args).Wait();
	}
}
