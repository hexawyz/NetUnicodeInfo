using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public class HttpDataSource : IDataSource
	{
		private readonly HttpClient _httpClient;
		private readonly Uri _baseUri;

		public HttpDataSource(Uri baseUri, HttpClient httpClient)
		{
			_httpClient = httpClient ?? new HttpClient();
			_baseUri = baseUri;
		}

		public void Dispose() => _httpClient.Dispose();

		public Task<Stream> OpenDataFileAsync(string fileName) => _httpClient.GetStreamAsync(_baseUri + fileName);
	}
}
