using WordEntryDal.Models;

namespace WordEntryDal.Repositories
{
	public interface IDBContextFactory
	{
		FolderScanEntities CreateContext();
	}
}
