using System.Collections.Generic;

namespace ScanningLib
{
	public interface ILineProcessor
	{
		IEnumerable<WordItem> Process(string line);
	}
}
