using System.Collections.Generic;

namespace ScanningLib
{
	public interface IFileReader
	{
		IEnumerable<string> ReadByLine(string path);
	}
}
