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
		public const string UcdDirectoryName = "UCD";
		public const string UcdArchiveName = "UCD.zip";

		public static readonly string[] ucdRequiredFiles = new[]
		{
			"UnicodeData.txt",
			"PropList.txt",
			"Blocks.txt",
			"Jamo.txt"
		};

		private static byte[] DownloadUcdArchive()
		{
			using (var httpClient = new HttpClient())
			{
				return httpClient.GetByteArrayAsync(HttpUcdSource.UnicodeCharacterDataUri + UcdArchiveName).Result;
            }
		}

		internal static IUcdSource GetUcdSource(bool? shouldDownload, bool? shouldSaveArchive, bool? shouldExtract)
		{
			string baseDirectory = Environment.CurrentDirectory;
			string ucdDirectory = Path.Combine(baseDirectory, UcdDirectoryName);
			string ucdArchiveFileName = Path.Combine(baseDirectory, UcdArchiveName);

			if (shouldDownload != true)
			{
				bool hasValidDirectory = Directory.Exists(ucdDirectory);

				if (hasValidDirectory)
				{
					foreach (string ucdRequiredFile in ucdRequiredFiles)
					{
						if (!File.Exists(Path.Combine(ucdDirectory, ucdRequiredFile)))
						{
							hasValidDirectory = false;
							break;
						}
					}
				}

				if (hasValidDirectory)
				{
					return new FileUcdSource(ucdDirectory);
				}

				if (File.Exists(ucdArchiveFileName))
				{
					if (shouldExtract == true)
					{
						ZipFile.ExtractToDirectory(ucdArchiveFileName, ucdDirectory);
						return new FileUcdSource(ucdDirectory);
					}
					else
					{
						return new ZipUcdSource(File.OpenRead(ucdArchiveFileName));
					}
				}
            }

			if (shouldDownload != false)
			{
				var ucdArchiveData = DownloadUcdArchive();

				if (shouldSaveArchive == true)
				{
					var stream = File.Open(ucdArchiveFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
					try
					{
						stream.Write(ucdArchiveData, 0, ucdArchiveData.Length);
						ucdArchiveData = null; // Release the reference now, since we won't need it anymore.

						if (shouldExtract == true)
						{
							using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, false))
							{
								archive.ExtractToDirectory(ucdDirectory);

								return new FileUcdSource(ucdDirectory);
							}
						}
						else
						{
							return new ZipUcdSource(stream);
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
					return new ZipUcdSource(new MemoryStream(ucdArchiveData));
				}
			}

			throw new InvalidOperationException();
        }

		private static void Main(string[] args)
		{
			UnicodeInfoBuilder data;

			using (var ucdSource = GetUcdSource(null, null, null))
			{
				data = UnicodeDataProcessor.BuildDataAsync(ucdSource).Result;
			}

			using (var stream = new DeflateStream(File.Create("ucd.dat"), CompressionLevel.Optimal, false))
				data.WriteToStream(stream);
        }
	}
}
