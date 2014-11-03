using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Unicode.Builder;

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

			Assert.AreEqual((int)'\t', data.GetUnicodeData('\t').CodePointRange.FirstCodePoint);
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
				var readData = UnicodeData.FromStream(stream);

				Assert.AreEqual((int)'\t', data.Get('\t').CodePointRange.FirstCodePoint);
			}
        }
	}
}
