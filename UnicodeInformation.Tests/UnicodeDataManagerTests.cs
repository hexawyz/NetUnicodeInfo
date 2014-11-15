using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Unicode.Builder;
using System.Text;

namespace System.Unicode.Tests
{
	[TestClass]
	public class UnicodeDataManagerTests
	{
		private const string UcdDirectoryName = "UCD";

        [TestInitialize]
		public void Initialize()
		{
			var directoryName = Path.GetFullPath(UcdDirectoryName);

			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);

				string ucdFileName = Path.Combine(directoryName, "UCD.zip");
				string unihanFileName = Path.Combine(directoryName, "Unihan.zip");

				if (!File.Exists(ucdFileName))
				{
					new WebClient().DownloadFile("http://www.unicode.org/Public/UCD/latest/ucd/UCD.zip", ucdFileName);
					ZipFile.ExtractToDirectory(ucdFileName, directoryName);
				}

				if (!File.Exists(unihanFileName))
				{
					new WebClient().DownloadFile("http://www.unicode.org/Public/UCD/latest/ucd/Unihan.zip", unihanFileName);
					ZipFile.ExtractToDirectory(unihanFileName, directoryName);
				}
			}
		}

		[TestMethod]
		public async Task BuildDataAsync()
		{
			var source = new FileDataSource(UcdDirectoryName);

			var data = (await UnicodeDataProcessor.BuildDataAsync(source, source)).ToUnicodeData();

			Assert.AreEqual((int)'\t', data.GetCharInfo('\t').CodePoint);
		}

		[TestMethod]
		public async Task BuildAndWriteDataAsync()
		{
			var source = new FileDataSource(UcdDirectoryName);

			var data = await UnicodeDataProcessor.BuildDataAsync(source, source);

			using (var stream = new DeflateStream(File.Create("ucd.dat"), CompressionLevel.Optimal, false))
			{
				data.WriteToStream(stream);
			}

			using (var stream = new DeflateStream(File.OpenRead("ucd.dat"), CompressionMode.Decompress, false))
			{
				var readData = UnicodeInfo.FromStream(stream);

				Assert.AreEqual((int)'\t', data.GetUcd('\t').CodePointRange.FirstCodePoint);
			}
        }

#if DEBUG
		[TestMethod]
		public void TestCodePointEncoding()
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
					Assert.AreEqual(i, UnicodeInfo.ReadCodePoint(reader));
					stream.Position = 0;
				}
			}
		}
#endif
	}
}
