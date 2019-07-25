using System.Net.Http;
using System.Threading.Tasks;
using System.Unicode.Build.Core;

namespace System.Unicode.Build.DatabaseGenerator
{
	internal static class Program
	{
		private static async Task Main(string[] args)
		{
			Console.WriteLine("Called Once !!!");
			// The sole purpose of this program is to consistently generate the database using .NET Core 2.2.
			using (var httpClient = new HttpClient())
			{
				await UnicodeDatabaseGenerator.GenerateDatabase(httpClient, args[0], args[1], null, null, null);
			}
		}
	}
}
