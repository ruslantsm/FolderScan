using System.Data.Entity;

namespace WordEntryDal.Repositories
{
	public static class DbContextExt
	{
		public static void OffTracking(this DbContext ctx)
		{
			ctx.Configuration.AutoDetectChangesEnabled = false;
			ctx.Configuration.ValidateOnSaveEnabled = false;
		}
	}
}
