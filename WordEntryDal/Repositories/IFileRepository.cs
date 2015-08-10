using System.Collections.Generic;
using WordEntryDal.Models;
using File = WordEntryDal.Models.File;

namespace WordEntryDal.Repositories
{
	public interface IFileRepository
	{
		File GetFile(string name);

		File CreateFile(string fileName, string path);

		IEnumerable<FEntry> GetFiles();
	}
}
