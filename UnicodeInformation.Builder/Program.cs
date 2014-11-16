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
		public const string UnihanDirectoryName = "Unihan";
		public const string UnihanArchiveName = "Unihan.zip";
		public const string directoryName = "UCD";
		public const string UcdArchiveName = "UCD.zip";

		public static readonly string[] ucdRequiredFiles = new[]
		{
			"UnicodeData.txt",
			"PropList.txt",
			"DerivedCoreProperties.txt",
			"Blocks.txt",
			"Jamo.txt",
		};

		public static readonly string[] unihanRequiredFiles = new[]
		{
			"Unihan_NumericValues.txt",
			"Unihan_Readings.txt",
			"Unihan_Variants.txt",
		};

		private static byte[] DownloadDataArchive(string archiveName)
		{
			using (var httpClient = new HttpClient())
			{
				return httpClient.GetByteArrayAsync(HttpDataSource.UnicodeCharacterDataUri + archiveName).Result;
			}
		}

		internal static IDataSource GetDataSource(string archiveName, string directoryName, string[] requiredFiles, bool? shouldDownload, bool? shouldSaveArchive, bool? shouldExtract)
		{
			string baseDirectory = Environment.CurrentDirectory;
			string dataDirectory = Path.Combine(baseDirectory, Program.directoryName);
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
				var dataArchiveData = DownloadDataArchive(archiveName);

				if (shouldSaveArchive == true)
				{
					var stream = File.Open(dataArchiveFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
					try
					{
						stream.Write(dataArchiveData, 0, dataArchiveData.Length);
						dataArchiveData = null;	// Release the reference now, since we won't need it anymore.

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
					catch
					{
						stream.Dispose();
						throw;
					}
				}
				else
				{
					return new ZipDataSource(new MemoryStream(dataArchiveData));
				}
			}

			throw new InvalidOperationException();
		}

		private static void Main(string[] args)
		{
			UnicodeInfoBuilder data;

			using (var ucdSource = GetDataSource(UcdArchiveName, directoryName, ucdRequiredFiles, null, null, null))
			using (var unihanSource = GetDataSource(UnihanArchiveName, UnihanDirectoryName, unihanRequiredFiles, null, null, null))
			{
				data = UnicodeDataProcessor.BuildDataAsync(ucdSource, unihanSource).Result;
			}

			using (var stream = new DeflateStream(File.Create("ucd.dat"), CompressionLevel.Optimal, false))
				data.WriteToStream(stream);
		}
	}
}
