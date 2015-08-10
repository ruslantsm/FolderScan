using System.Collections.Generic;
using System.Text;

namespace ScanningLib
{
	public class FileReader : IFileReader
	{
		public IEnumerable<string> ReadByLine(string path)
		{
			using (var file = new System.IO.StreamReader(path, Encoding.GetEncoding(1251)))
			{
				string line;
				while ((line = file.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}
	}
}
