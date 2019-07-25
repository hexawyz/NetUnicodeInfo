using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Unicode.Build.Core.DataSourceProvider;

namespace System.Unicode.Build.Core
{
	public static class UnicodeDatabaseGenerator
	{
		public static async ValueTask GenerateDatabase(HttpClient httpClient, string baseDirectory, string outputFilePath, bool? shouldDownloadFiles, bool? shouldSaveFiles, bool? shouldExtractFiles)
		{
			UnicodeInfoBuilder data;

			baseDirectory = string.IsNullOrWhiteSpace(baseDirectory) ?
				Environment.CurrentDirectory :
				Path.GetFullPath(baseDirectory);

			using (var ucdSource = await GetDataSourceAsync(httpClient, UnicodeCharacterDataUri, baseDirectory, UcdDataSourceName, UcdRequiredFiles, true, shouldDownloadFiles, shouldSaveFiles, shouldExtractFiles))
			using (var unihanSource = await GetDataSourceAsync(httpClient, UnicodeCharacterDataUri, baseDirectory, UnihanDataSourceName, UnihanRequiredFiles, true, shouldDownloadFiles, shouldSaveFiles, shouldExtractFiles))
			using (var emojiSource = await GetDataSourceAsync(httpClient, EmojiDataUri, baseDirectory, EmojiDataSourceName, EmojiRequiredFiles, false, shouldDownloadFiles, shouldSaveFiles, shouldExtractFiles))
			{
				data = await UnicodeDataProcessor.BuildDataAsync(ucdSource, unihanSource, emojiSource);
			}

			// This part is actually highly susceptible to framework version. Different frameworks give a different results.
			// In order to consistently produce the same result, the framework executing this code must be fixed.
			using (var stream = new DeflateStream(File.Create(outputFilePath), CompressionLevel.Optimal, false))
				data.WriteToStream(stream);
		}
	}
}
