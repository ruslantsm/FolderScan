using System.Collections.Generic;
using System.Linq;
using WordEntryDal.Models;

using File = WordEntryDal.Models.File;

namespace WordEntryDal.Repositories
{
	public class FileRepository : IFileRepository
	{
		private readonly IDBContextFactory _contextFactory;

		public FileRepository(IDBContextFactory contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public File GetFile(string name)
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				return ctx.Files.FirstOrDefault(x => x.Name == name);
			}
		}

		public File CreateFile(string fileName, string path)
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				var file = ctx.Files.Add(new File
					{
						Name = fileName,
						Path = path
					});

				ctx.SaveChanges();
				
				return file;
			}
		}

		public IEnumerable<FEntry> GetFiles()
		{
			using (var ctx = _contextFactory.CreateContext())
			{
				var result = from file in ctx.Files
					select new FEntry
					{
						Id	= file.Id, 
						Name = file.Name, 
						Path = file.Path, 
						Words = file.WordEntries.Count
					};

				return result.ToList();
			}
		}
	}
}
