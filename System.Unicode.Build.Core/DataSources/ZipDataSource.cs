using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace System.Unicode.Build.Core.DataSources
{
	public sealed class ZipDataSource : IDataSource
	{
		private readonly ZipArchive _archive;

		public ZipDataSource(Stream stream) => _archive = new ZipArchive(stream, ZipArchiveMode.Read, false);

		public void Dispose() => _archive.Dispose();

		public ValueTask<Stream> OpenDataFileAsync(string fileName)
		{
			var entry = _archive.Entries.Where(e => e.FullName == fileName).FirstOrDefault();

			if (entry == null) throw new FileNotFoundException();

			return new ValueTask<Stream>(entry.Open());
		}
	}
}
