using System.IO;
using System.Threading.Tasks;

namespace System.Unicode.Build.Core.DataSources
{
	public sealed class FileDataSource : IDataSource
	{
		private readonly string _baseDirectory;

		public FileDataSource(string baseDirectory)
			=> _baseDirectory = Path.GetFullPath(baseDirectory);

		public void Dispose()
		{
		}

		public ValueTask<Stream> OpenDataFileAsync(string fileName)
			=> new ValueTask<Stream>(File.OpenRead(Path.Combine(_baseDirectory, fileName)));
	}
}
