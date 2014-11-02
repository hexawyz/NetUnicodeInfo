using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode.Builder
{
	public interface IUcdSource
	{
		Task<Stream> OpenDataFileAsync(string fileName);
	}
}
