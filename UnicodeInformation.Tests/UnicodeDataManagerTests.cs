using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnicodeInformation.Tests
{
	[TestClass]
	public class UnicodeDataManagerTests
	{
		[TestMethod]
		public async Task DownloadAndBuildDataAsync()
		{
			await UnicodeDataManager.DownloadAndBuildDataAsync();
		}
	}
}
