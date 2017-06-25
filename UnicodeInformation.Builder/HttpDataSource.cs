using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public class HttpDataSource : IDataSource
	{
		private readonly HttpClient httpClient;
		private readonly Uri baseUri;
		
		public HttpDataSource(Uri baseUri, HttpClient httpClient)
		{
			this.httpClient = httpClient ?? new HttpClient();
			this.baseUri = baseUri;
		}

		public void Dispose()
		{
			httpClient.Dispose();
		}

		public Task<Stream> OpenDataFileAsync(string fileName)
		{
			return httpClient.GetStreamAsync(baseUri + fileName);
		}
	}
}
