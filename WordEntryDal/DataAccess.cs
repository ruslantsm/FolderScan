using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordEntryDal.Models;
using WordEntryDal.Repositories;
using File = WordEntryDal.Models.File;

namespace WordEntryDal
{
	public class DataAccess : IDataAccess
	{
		private readonly IFileRepository _fileRepository;
		private readonly IWordRepository _wordRepository;
		private readonly IWordEntryRepository _wordEntryRepository;

		public DataAccess(IFileRepository fileRepository, IWordRepository wordRepository,
			IWordEntryRepository wordEntryRepository)
		{
			_fileRepository = fileRepository;
			_wordRepository = wordRepository;
			_wordEntryRepository = wordEntryRepository;
		}

		public void AddWordItems(string filePath, string wordName, IEnumerable<WordModel> models)
		{
			var fileName = Path.GetFileName(filePath);
			
			var file = _fileRepository.GetFile(fileName) ?? _fileRepository.CreateFile(fileName, filePath);
			
			var word = _wordRepository.GetOrCreateWord(wordName);

			_wordEntryRepository.CreateEntry(models.Select(x => new WEntry
												{
													FileId = file.Id,
													WordId = word.Id,
													WIndex = x.Index,
													WLine = x.LineNum
												}));
		}

		public IDictionary<string, Word> SaveWords(IEnumerable<string> words)
		{
			return _wordRepository.SaveWords(words);
		}

		public File GetFile(string filePath)
		{
			var fileName = Path.GetFileName(filePath);
			var file = _fileRepository.GetFile(fileName) ?? _fileRepository.CreateFile(fileName, filePath);

			return file;
		}

		public void AddWordEntries(List<WEntry> toSaveItems)
		{
			_wordEntryRepository.CreateEntry(toSaveItems);
		}
	}
}
