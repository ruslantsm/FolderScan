using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ScanningLib;

namespace FolderScan.Model
{
	public class DataService : IDataService
	{
		private readonly IScanningService _scanningService;

		private CancellationTokenSource _cancellationTokenSource;

		public DataService(IScanningService scanningService)
		{
			_scanningService = scanningService;
		}

		public async Task Scan(string folder)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = _cancellationTokenSource.Token;

			var scanTask = Task.Run(() =>
			{
				var files = GetFiles(folder);

				var workers = files.Where(File.Exists).Select(file => _scanningService.AddScanningFile(file, cancellationToken)).ToArray();

				Task.WaitAll(workers, cancellationToken);

			}, cancellationToken);

			await scanTask;
		}

		public void Cancel()
		{
			if (_cancellationTokenSource != null)
				_cancellationTokenSource.Cancel();
		}

		private IEnumerable<string> GetFiles(string folder)
		{
			if (Directory.Exists(folder))
			{
				return Directory.GetFiles(folder, "*.txt");
			}
			
			throw new InvalidOperationException("Directory does not exist: " + folder);
		}
	}
}