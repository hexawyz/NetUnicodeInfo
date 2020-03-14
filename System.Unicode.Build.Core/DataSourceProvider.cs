using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Unicode.Build.Core.DataSources;

namespace System.Unicode.Build.Core
{
	public static class DataSourceProvider
	{
		public static readonly Uri UnicodeCharacterDataUri = new Uri("http://www.unicode.org/Public/UCD/latest/ucd/", UriKind.Absolute);
		public static readonly Uri EmojiDataUri = new Uri("http://www.unicode.org/Public/emoji/latest/", UriKind.Absolute);

		public const string UnihanDataSourceName = "Unihan";
		public const string UcdDataSourceName = "UCD";
		public const string EmojiDataSourceName = "Emoji";

		public static readonly string[] UcdRequiredFiles = new[]
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

		public static readonly string[] UnihanRequiredFiles = new[]
		{
			"Unihan_NumericValues.txt",
			"Unihan_Readings.txt",
			"Unihan_Variants.txt",
			"Unihan_IRGSources.txt",
		};

		public static readonly string[] EmojiRequiredFiles = new[]
		{
			"emoji-data.txt",
			"emoji-sequences.txt",
			"emoji-variation-sequences.txt",
			"emoji-zwj-sequences.txt",
		};

		private static Task<byte[]> DownloadDataFileAsync(HttpClient httpClient, Uri baseUri, string archiveName)
			=> httpClient.GetByteArrayAsync(new Uri(baseUri, archiveName));

		public static async Task<IDataSource> GetDataSourceAsync(HttpClient httpClient, Uri baseUri, string baseDirectory, string dataSourceName, string[] requiredFiles, bool useArchive, bool? shouldDownload, bool? shouldSaveFiles, bool? shouldExtract)
		{
			string dataDirectory = Path.GetFullPath(Path.Combine(baseDirectory, dataSourceName));
			string dataArchiveFileName = dataSourceName + ".zip";
			string dataArchivePath = dataDirectory + ".zip";

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

				if (useArchive && File.Exists(dataArchivePath))
				{
					if (shouldExtract == true)
					{
						ZipFile.ExtractToDirectory(dataArchivePath, dataDirectory);
						return new FileDataSource(dataDirectory);
					}
					else
					{
						return new ZipDataSource(File.OpenRead(dataArchivePath));
					}
				}
			}

			if (shouldDownload != false)
			{
				if (useArchive)
				{
					var dataArchiveData = await DownloadDataFileAsync(httpClient, baseUri, dataArchiveFileName).ConfigureAwait(false);

					if (shouldSaveFiles == true)
					{
						using (var stream = File.Open(dataArchivePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
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
				else
				{
					var downloadedFiles = await Task.WhenAll
					(
						Array.ConvertAll
						(
							requiredFiles,
							async requiredFile =>
							(
								Name: requiredFile,
								Data: await DownloadDataFileAsync(httpClient, baseUri, requiredFile).ConfigureAwait(false)
							)
						)
					).ConfigureAwait(false);

					if (shouldSaveFiles == true)
					{
						Directory.CreateDirectory(dataDirectory);

						await Task.WhenAll
						(
							Array.ConvertAll
							(
								downloadedFiles,
								//file => File.WriteAllBytesAsync(Path.Combine(dataDirectory, file.Name), file.Data)
								async file =>
								{
									using (var stream = File.Open(Path.Combine(dataDirectory, file.Name), FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
									{
										await stream.WriteAsync(file.Data, 0, file.Data.Length).ConfigureAwait(false);
									}
								}
							)
						).ConfigureAwait(false);

						return new FileDataSource(dataDirectory);
					}
					else
					{
						return new InMemoryDataSource(downloadedFiles.ToDictionary(f => f.Name, f => f.Data));
					}
				}
			}

			throw new InvalidOperationException();
		}
	}
}
