using System.IO;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public sealed class FileDataSource : IDataSource
	{
		private readonly string _baseDirectory;

		public FileDataSource(string baseDirectory)
			=> _baseDirectory = Path.GetFullPath(baseDirectory);

		public void Dispose()
		{
		}

		public Task<Stream> OpenDataFileAsync(string fileName)
			=> Task.FromResult<Stream>(File.OpenRead(Path.Combine(_baseDirectory, fileName)));
	}
}
