using System.Threading.Tasks;

namespace FolderScan.Model
{
	public interface IDataService
	{
		Task Scan(string folder);

		void Cancel();
	}
}
