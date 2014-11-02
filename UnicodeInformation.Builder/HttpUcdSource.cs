using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public class HttpUcdSource : IUcdSource
	{
		public static readonly Uri UnicodeCharacterDataUri = new Uri("http://www.unicode.org/Public/UCD/latest/ucd/", UriKind.Absolute);

		private readonly HttpClient httpClient;
		private readonly Uri baseUri;

		public HttpUcdSource()
			: this(UnicodeCharacterDataUri)
		{
		}

		public HttpUcdSource(Uri baseUri)
		{
			this.httpClient = new HttpClient();
			this.baseUri = baseUri;
        }

		public Task<Stream> OpenDataFileAsync(string fileName)
		{
			return httpClient.GetStreamAsync(baseUri + fileName);
		}
	}
}
