using System.IO;
using System.Net.Http;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Unicode.Build.Core;
using Microsoft.Build.Framework;

namespace System.Unicode.Build.Tasks
{
	[RunInMTA]
	public sealed class GenerateUnicodeDatabase : AsyncTask
	{
		public GenerateUnicodeDatabase()
		{
		}

		public GenerateUnicodeDatabase(ResourceManager taskResources) : base(taskResources)
		{
		}

		public GenerateUnicodeDatabase(ResourceManager taskResources, string helpKeywordPrefix) : base(taskResources, helpKeywordPrefix)
		{
		}

		[Required]
		public string DatabasePath { get; set; }

		public string IntermediateDirectory { get; set; }

		public bool? ShouldDownloadFiles { get; set; }

		public bool? ShouldSaveFiles { get; set; }

		public bool? ShouldExtractFiles { get; set; }

		protected override async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
		{
			using (var httpClient = new HttpClient())
			{
				string baseDirectory = IntermediateDirectory;

				baseDirectory = string.IsNullOrWhiteSpace(baseDirectory) ?
					Environment.CurrentDirectory :
					Path.GetFullPath(baseDirectory);

				await UnicodeDatabaseGenerator.GenerateDatabase(httpClient, baseDirectory, DatabasePath, ShouldDownloadFiles, ShouldSaveFiles, ShouldExtractFiles);

				return true;
			}
		}
	}
}
