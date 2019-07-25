using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace System.Unicode.Build.Core.DataSources
{
	internal class InMemoryDataSource : IDataSource
	{
		private readonly Dictionary<string, byte[]> _files;

		public InMemoryDataSource(Dictionary<string, byte[]> files) => _files = files;

		public void Dispose() { }

		public ValueTask<Stream> OpenDataFileAsync(string fileName)
			=> new ValueTask<Stream>(new MemoryStream(_files[fileName], false));
	}
}
