using System.Collections.Generic;
using WordEntryDal.Models;

namespace WordEntryDal
{
	public interface IDataAccess
	{
		void AddWordItems(string filePath, string wordName, IEnumerable<WordModel> models);

		IDictionary<string, Word> SaveWords(IEnumerable<string> words);

		File GetFile(string filePath);

		void AddWordEntries(List<WEntry> toSaveItems);
	}
}
