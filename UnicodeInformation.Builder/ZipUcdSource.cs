using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public sealed class ZipUcdSource : IUcdSource
	{
		private readonly ZipArchive archive;

		public ZipUcdSource(Stream stream)
		{
			archive = new ZipArchive(stream, ZipArchiveMode.Read, false);
		}

		public void Dispose()
		{
			archive.Dispose();
		}

		public Task<Stream> OpenDataFileAsync(string fileName)
		{
			var entry = archive.Entries.Where(e => e.FullName == fileName).FirstOrDefault();

			if (entry == null) throw new FileNotFoundException();

			return Task.FromResult(entry.Open());
		}
	}
}
