using System.Collections.Generic;
using WordEntryDal.Models;

namespace WordEntryDal.Repositories
{
	public interface IWordEntryRepository
	{
		void CreateEntry(IEnumerable<WEntry> models);

		IEnumerable<WordEntry> GetEntries(string word);
	}
}
