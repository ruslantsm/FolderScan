using System;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using FolderScan.Model;
using GalaSoft.MvvmLight.Command;

namespace FolderScan.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IDataService _dataService;

		const string FOLDER_PATH_PROP_NAME = "FolderPath";
		const string IS_SELECTED_PROP_NAME = "IsSelectEnabled";
		const string IS_SCAN_ENABLED_PRP_NAME = "IsScanEnabled";
		const string IS_CANCEL_ENABLED_PROP_NAME = "IsCancelEnabled";
		const string BUSY_INDICATOR_PROP_NAME = "BusyIndicator";

		private string _folderPath = string.Empty;
		private bool _scanning = false;

		public MainViewModel(IDataService dataService)
		{
			_dataService = dataService;

			OpenFolderDialogCommand = new RelayCommand(OpenFolderDialog);
			ScanCommand = new RelayCommand(Scan);
			CancelCommand = new RelayCommand(Cancel);
		}

		public string FolderPath
		{
			get { return _folderPath; }
			set
			{
				if (_folderPath == value)
				{
					return;
				}

				_folderPath = value;
				RaisePropertyChanged(FOLDER_PATH_PROP_NAME);
				RaisePropertyChanged(IS_SCAN_ENABLED_PRP_NAME);
			}
		}

		public bool IsSelectEnabled
		{
			get { return !_scanning; }
		}

		public bool IsScanEnabled
		{
			get { return !string.IsNullOrEmpty(_folderPath) && !_scanning; }
		}

		public bool IsCancelEnabled
		{
			get { return _scanning; }
		}

		public Visibility BusyIndicator
		{
			get { return _scanning ? Visibility.Visible : Visibility.Hidden; }
		}

		public RelayCommand OpenFolderDialogCommand { get; set; }

		public void OpenFolderDialog()
		{
			var dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				FolderPath = dialog.SelectedPath;
			}
		}

		public RelayCommand ScanCommand { get; set; }

		public async void Scan()
		{
			var start = DateTime.Now;
			_scanning = true;
			NotifyScan();

			try
			{
				await _dataService.Scan(_folderPath);

				var stop = DateTime.Now;
				var dif = stop - start;

				_scanning = false;
				NotifyScan();

				System.Windows.MessageBox.Show("Operation complete!\r\n\r\n Duration: " + dif.ToString("g"));
			}
			catch (OperationCanceledException)
			{
				System.Windows.MessageBox.Show("Operation canceled!");
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Something went wrong. Error: " + ex.Message);
			}

			_scanning = false;
			NotifyScan();
		}

		private void NotifyScan()
		{
			RaisePropertyChanged(IS_SCAN_ENABLED_PRP_NAME);
			RaisePropertyChanged(IS_CANCEL_ENABLED_PROP_NAME);
			RaisePropertyChanged(IS_SELECTED_PROP_NAME);
			RaisePropertyChanged(BUSY_INDICATOR_PROP_NAME);
		}

		public RelayCommand CancelCommand { get; set; }

		public void Cancel()
		{
			_dataService.Cancel();
		}
	}
}