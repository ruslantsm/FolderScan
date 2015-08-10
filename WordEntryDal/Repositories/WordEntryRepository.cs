using System.Collections.Generic;
using System.Linq;
using WordEntryDal.Extension;
using WordEntryDal.Models;

namespace WordEntryDal.Repositories
{
	public class WordEntryRepository : IWordEntryRepository
	{
		private readonly IDBContextFactory _contextFactory;

		public WordEntryRepository(IDBContextFactory contextFactory)
		{
			_contextFactory = contextFactory;
		}

		/// <summary>
		/// Bulk insert for better performance
		/// </summary>
		/// <param name="models">All entries for particular word</param>
		public void CreateEntry(IEnumerable<WEntry> models)
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				ctx.ExecuteTableValueProcedure<WEntry>(models, "InsertEntries", "@entries", "WEntry");
			}
		}

		public IEnumerable<WordEntry> GetEntries(string word)
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				ctx.OffTracking();
				return ctx.Words.First(x => x.Name == word).WordEntries;
			}
		}
	}
}
