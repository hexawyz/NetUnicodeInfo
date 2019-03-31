using System.IO;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public interface IDataSource : IDisposable
	{
		Task<Stream> OpenDataFileAsync(string fileName);
	}
}
