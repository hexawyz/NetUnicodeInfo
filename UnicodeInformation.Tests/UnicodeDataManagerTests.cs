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

				var fileName = Path.Combine(directoryName, "UCD.zip");

				if (!File.Exists(fileName))
				{
					new WebClient().DownloadFile("http://www.unicode.org/Public/UCD/latest/ucd/UCD.zip", fileName);
					ZipFile.ExtractToDirectory(fileName, directoryName);
				}
			}
		}

		[TestMethod]
		public async Task BuildDataAsync()
		{
			var source = new FileUcdSource(UcdDirectoryName);

			var data = (await UnicodeDataManager.BuildDataAsync(source)).ToUnicodeData();

			Assert.AreEqual((int)'\t', data.Get('\t').CodePoint);
		}

		[TestMethod]
		public async Task BuildAndWriteDataAsync()
		{
			var source = new FileUcdSource(UcdDirectoryName);

			var data = await UnicodeDataManager.BuildDataAsync(source);

			using (var stream = new DeflateStream(File.Create("ucd.dat"), CompressionLevel.Optimal, false))
			{
				data.WriteToStream(stream);
			}

			using (var stream = new DeflateStream(File.OpenRead("ucd.dat"), CompressionMode.Decompress, false))
			{
				var readData = UnicodeInfo.FromStream(stream);

				Assert.AreEqual((int)'\t', data.Get('\t').CodePointRange.FirstCodePoint);
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
