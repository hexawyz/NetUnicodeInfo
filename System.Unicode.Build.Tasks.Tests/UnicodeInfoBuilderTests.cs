using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Unicode.Build.Core;
using Xunit;
using static System.Unicode.Build.Core.DataSourceProvider;

namespace System.Unicode.Build.Tests
{
	public class UnicodeInfoBuilderTests
	{
		private sealed class FileHttpResponseHandler : HttpMessageHandler
		{
			private readonly Dictionary<Uri, string> _registeredFiles = new Dictionary<Uri, string>();

			public void RegisterFile(Uri uri, string fileName)
				=> _registeredFiles.Add(uri, fileName);

			protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
			{
				if (_registeredFiles.TryGetValue(request.RequestUri, out string fileName))
				{
					return new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(await Task.Run(() => File.ReadAllBytes(fileName)).ConfigureAwait(false)) };
				}
				else
				{
					return new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request };
				}
			}
		}

		private const string HttpCacheDirectory = "_HttpCache";

		private static async Task RegisterAndDownloadFile(FileHttpResponseHandler handler, string httpCacheDirectory, Uri baseUri, string fileName)
		{
			var uri = new Uri(baseUri, fileName);
			string path = Path.Combine(httpCacheDirectory, fileName);

			if (!File.Exists(path))
			{
				using (var httpClient = new HttpClient())
				{
					var data = await httpClient.GetByteArrayAsync(uri).ConfigureAwait(false);

					File.WriteAllBytes(path, data);
				}
			}

			handler.RegisterFile(uri, path);
		}

		private readonly HttpClient _httpClient;

		public UnicodeInfoBuilderTests()
		{
			string httpCacheDirectory = Path.GetFullPath(HttpCacheDirectory);

			if (!Directory.Exists(httpCacheDirectory)) Directory.CreateDirectory(httpCacheDirectory);

			var handler = new FileHttpResponseHandler();

			string ucdArchiveName = UcdDataSourceName + ".zip";
			string unihanArchiveName = UnihanDataSourceName + ".zip";

			var ucdTask = RegisterAndDownloadFile(handler, httpCacheDirectory, UnicodeCharacterDataUri, ucdArchiveName);
			var unihanTask = RegisterAndDownloadFile(handler, httpCacheDirectory, UnicodeCharacterDataUri, unihanArchiveName);
			var emojiTasks = EmojiRequiredFiles.Select(f => RegisterAndDownloadFile(handler, httpCacheDirectory, EmojiDataUri, f)).ToArray();

			_httpClient = new HttpClient(handler);

			if (Directory.Exists(UcdDataSourceName)) Directory.Delete(UcdDataSourceName, true);
			if (File.Exists(ucdArchiveName)) File.Delete(ucdArchiveName);
			if (Directory.Exists(UnihanDataSourceName)) Directory.Delete(UnihanDataSourceName, true);
			if (File.Exists(unihanArchiveName)) File.Delete(unihanArchiveName);

			Task.WaitAll(ucdTask, unihanTask, Task.WhenAll(emojiTasks));
		}

		[Fact]
		public void DownloadUcdArchive()
		{
			using (var source = GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UcdDataSourceName, UcdRequiredFiles, true, true, false, false).Result)
			{
			}
		}

		[Fact]
		public void DownloadAndSaveUcdArchive()
		{
			using (var source = GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UcdDataSourceName, UcdRequiredFiles, true, true, true, false).Result)
			{
			}
		}

		[Fact]
		public void ExtractUcdArchive()
		{
			using (var source = GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UcdDataSourceName, UcdRequiredFiles, true, true, true, true).Result)
			{
			}
		}

		[Fact]
		public void DownloadUnihanArchive()
		{
			using (var source = GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UnihanDataSourceName, UnihanRequiredFiles, true, true, false, false).Result)
			{
			}
		}

		[Fact]
		public void DownloadAndSaveUnihanArchive()
		{
			using (var source = GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UnihanDataSourceName, UnihanRequiredFiles, true, true, true, false).Result)
			{
			}
		}

		[Fact]
		public void ExtractUnihanArchive()
		{
			using (var source = GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UnihanDataSourceName, UnihanRequiredFiles, true, true, true, true).Result)
			{
			}
		}

		[Fact]
		public async Task BuildDataAsync()
		{
			using (var ucdSource = await GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UcdDataSourceName, UcdRequiredFiles, true, null, null, null))
			using (var unihanSource = await GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UnihanDataSourceName, UnihanRequiredFiles, true, null, null, null))
			using (var emojiSource = await GetDataSourceAsync(_httpClient, EmojiDataUri, "", EmojiDataSourceName, EmojiRequiredFiles, false, null, null, null))
			{
				var data = await UnicodeDataProcessor.BuildDataAsync(ucdSource, unihanSource, emojiSource);

				Assert.Equal('\t', data.GetUcd('\t').CodePointRange.FirstCodePoint);
			}
		}

		[Fact]
		public async Task BuildAndWriteDataAsync()
		{
			using (var ucdSource = await GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UcdDataSourceName, UcdRequiredFiles, true, null, null, null))
			using (var unihanSource = await GetDataSourceAsync(_httpClient, UnicodeCharacterDataUri, "", UnihanDataSourceName, UnihanRequiredFiles, true, null, null, null))
			using (var emojiSource = await GetDataSourceAsync(_httpClient, EmojiDataUri, "", EmojiDataSourceName, EmojiRequiredFiles, false, null, null, null))
			{
				var data = await UnicodeDataProcessor.BuildDataAsync(ucdSource, unihanSource, emojiSource);

				using (var stream = new MemoryStream())
				{
					data.WriteToStream(stream);
				}
			}
		}

#if FIXME
		[Fact]
		public void CodePointEncodingTest()
		{
			using (var stream = new MemoryStream(4))
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				for (int i = 0; i <= 0x10FFFF; ++i)
				{
					writer.WriteCodePoint(i);
					writer.Flush();
					stream.Position = 0;
					Assert.Equal(i, UnicodeData.ReadCodePoint(reader));
					stream.Position = 0;
				}
			}
		}
#endif
	}
}
