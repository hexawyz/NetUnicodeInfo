using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Unicode.Builder;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using Xunit;

namespace System.Unicode.Tests
{
	public class UnicodeInfoBuilderTests
	{
		private sealed class FileHttpResponseHandler : HttpMessageHandler
		{
			private readonly Dictionary<Uri, string> registeredFiles = new Dictionary<Uri, string>();

			public void RegisterFile(Uri uri, string fileName)
			{
				registeredFiles.Add(uri, fileName);
			}

			protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
			{
				string fileName;

				if (registeredFiles.TryGetValue(request.RequestUri, out fileName))
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
		
		public UnicodeInfoBuilderTests()
		{
			var httpCacheDirectory = Path.GetFullPath(HttpCacheDirectory);

            if (!Directory.Exists(httpCacheDirectory)) Directory.CreateDirectory(httpCacheDirectory);

			var handler = new FileHttpResponseHandler();

			var ucdTask = RegisterAndDownloadFile(handler, httpCacheDirectory, HttpDataSource.UnicodeCharacterDataUri, Program.UcdArchiveName);
			var unihanTask = RegisterAndDownloadFile(handler, httpCacheDirectory, HttpDataSource.UnicodeCharacterDataUri, Program.UnihanArchiveName);

			Program.SetHttpMessageHandler(handler);

			if (Directory.Exists(Program.UcdDirectoryName)) Directory.Delete(Program.UcdDirectoryName, true);
			if (File.Exists(Program.UcdArchiveName)) File.Delete(Program.UcdArchiveName);
			if (Directory.Exists(Program.UnihanDirectoryName)) Directory.Delete(Program.UnihanDirectoryName, true);
			if (File.Exists(Program.UnihanArchiveName)) File.Delete(Program.UnihanArchiveName);

			Task.WaitAll(ucdTask, unihanTask);
		}

		[Fact]
		public void DownloadUcdArchive()
		{
			using (var source = Program.GetDataSource(Program.UcdArchiveName, Program.UcdDirectoryName, Program.ucdRequiredFiles, true, false, false))
			{
			}
		}

		[Fact]
		public void DownloadAndSaveUcdArchive()
		{
			using (var source = Program.GetDataSource(Program.UcdArchiveName, Program.UcdDirectoryName, Program.ucdRequiredFiles, true, true, false))
			{
			}
		}

		[Fact]
		public void ExtractUcdArchive()
		{
			using (var source = Program.GetDataSource(Program.UcdArchiveName, Program.UcdDirectoryName, Program.ucdRequiredFiles, true, true, true))
			{
			}
		}

		[Fact]
		public void DownloadUnihanArchive()
		{
			using (var source = Program.GetDataSource(Program.UnihanArchiveName, Program.UnihanDirectoryName, Program.ucdRequiredFiles, true, false, false))
			{
			}
		}

		[Fact]
		public void DownloadAndSaveUnihanArchive()
		{
			using (var source = Program.GetDataSource(Program.UnihanArchiveName, Program.UnihanDirectoryName, Program.ucdRequiredFiles, true, true, false))
			{
			}
		}

		[Fact]
		public void ExtractUnihanArchive()
		{
			using (var source = Program.GetDataSource(Program.UnihanArchiveName, Program.UnihanDirectoryName, Program.ucdRequiredFiles, true, true, true))
			{
			}
		}

		[Fact]
		public async Task BuildDataAsync()
		{
			using (var ucdSource = Program.GetDataSource(Program.UcdArchiveName, Program.UcdDirectoryName, Program.ucdRequiredFiles, null, null, null))
			using (var unihanSource = Program.GetDataSource(Program.UnihanArchiveName, Program.UnihanDirectoryName, Program.ucdRequiredFiles, null, null, null))
			{
				var data = (await UnicodeDataProcessor.BuildDataAsync(ucdSource, unihanSource));

				Assert.Equal((int)'\t', data.GetUcd('\t').CodePointRange.FirstCodePoint);
			}
		}

		[Fact]
		public async Task BuildAndWriteDataAsync()
		{
			using (var ucdSource = Program.GetDataSource(Program.UcdArchiveName, Program.UcdDirectoryName, Program.ucdRequiredFiles, null, null, null))
			using (var unihanSource = Program.GetDataSource(Program.UnihanArchiveName, Program.UnihanDirectoryName, Program.ucdRequiredFiles, null, null, null))
			{
				var data = (await UnicodeDataProcessor.BuildDataAsync(ucdSource, unihanSource));

				//using (var stream = new DeflateStream(File.Create("ucd.dat"), CompressionLevel.Optimal, false))
				using (var stream = File.Create("ucd.dat"))
				{
					data.WriteToStream(stream);
				}
			}
		}

#if DEBUG
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
					Assert.Equal(i, UnicodeInfo.ReadCodePoint(reader));
					stream.Position = 0;
				}
			}
		}
#endif
	}
}
