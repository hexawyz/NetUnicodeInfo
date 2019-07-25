using System.IO;
using System.Threading.Tasks;

namespace System.Unicode.Build.Core
{
	public interface IDataSource : IDisposable
	{
		ValueTask<Stream> OpenDataFileAsync(string fileName);
	}
}
