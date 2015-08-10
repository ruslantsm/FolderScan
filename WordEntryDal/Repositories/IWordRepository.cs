using System.Collections.Generic;
using WordEntryDal.Models;

namespace WordEntryDal.Repositories
{
	public interface IWordRepository
	{
		Word GetOrCreateWord(string name);

		IEnumerable<WEntryModel> GetWords();

		IDictionary<string, Word> SaveWords(IEnumerable<string> words);

		IEnumerable<WordEntryEntity> GetWords(int pageSize, int pageNumber, out int total);
	}
}
