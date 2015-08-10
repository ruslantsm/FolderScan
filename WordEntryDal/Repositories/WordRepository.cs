using System.Collections.Generic;
using System.Linq;
using WordEntryDal.Extension;
using WordEntryDal.Models;

namespace WordEntryDal.Repositories
{
	public class WordRepository : IWordRepository
	{
		private readonly IDBContextFactory _contextFactory;

		public WordRepository(IDBContextFactory contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public Word GetOrCreateWord(string name)
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				ctx.OffTracking();
				var word = ctx.Words.FirstOrDefault(x => x.Name == name);
				if (word == null)
				{
					word = ctx.Words.Add(new Word { Name = name });
					ctx.SaveChanges();
				}

				return word;
			}
		}

		public IEnumerable<WordEntryEntity> GetWords(int pageSize, int pageNumber, out int total)
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				ctx.OffTracking();
				var query = (from word in ctx.Words
							 select new WordEntryEntity
							 {
								 Name = word.Name,
								 Occurences = word.WordEntries.Select(x => new FullOccurence
								 {
									 FileId = x.FileId,
									 FileName = x.File.Name,
									 Index = x.WordIndex,
									 Line = x.WordLine
								 })
							 }).AsQueryable();

				var skip = pageNumber > 1 ? (pageNumber - 1) * pageSize : 0;
				total = query.Count();

				return query.OrderBy(x => x.Name).Skip(skip).Take(pageSize).ToList();
			}
		}

		public IEnumerable<WEntryModel> GetWords()
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				ctx.OffTracking();
				var result = from word in ctx.Words
							 select new WEntryModel
							 {
								 Name = word.Name,
								 Count = word.WordEntries.Count
							 };

				return result.ToList();
			}
		}

		public IDictionary<string, Word> SaveWords(IEnumerable<string> words)
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				ctx.OffTracking();
				var result = ctx.ExecuteTableValueProcedure<WordNameModel>(words.Select(x => new WordNameModel { Name = x }), "InsertWords", "@entries", "WordEntry");

				return ctx.Words.ToDictionary(x => x.Name, y => y);
			}
		}
	}
}
