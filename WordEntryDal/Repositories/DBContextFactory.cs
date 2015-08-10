using WordEntryDal.Models;

namespace WordEntryDal.Repositories
{
	public class DBContextFactory : IDBContextFactory
	{
		public FolderScanEntities CreateContext()
		{
			return new FolderScanEntities();
		}
	}
}
