using System.Threading;
using System.Threading.Tasks;

namespace ScanningLib
{
	public interface IScanningService
	{
		Task AddScanningFile(string filePath, CancellationToken cancellationToken);
	}
}
