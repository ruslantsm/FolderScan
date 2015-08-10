using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using FolderScan.Model;
using ScanningLib;
using WordEntryDal;
using WordEntryDal.Repositories;

namespace FolderScan.ViewModel
{
	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class ViewModelLocator
	{
		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			if (ViewModelBase.IsInDesignModeStatic)
			{
				
			}
			else
			{
				RegisterDependencies();
			}

			SimpleIoc.Default.Register<MainViewModel>();
		}

		private static void RegisterDependencies()
		{
			SimpleIoc.Default.Register<ILineProcessor, LineProcessor>();
			SimpleIoc.Default.Register<IScanningService, ScanningService>();
			SimpleIoc.Default.Register<IDataService, DataService>();

			SimpleIoc.Default.Register<IFileReader, FileReader>();
			SimpleIoc.Default.Register<IDataAccess, DataAccess>();
			SimpleIoc.Default.Register<IDBContextFactory, DBContextFactory>();
			SimpleIoc.Default.Register<IFileRepository, FileRepository>();
			SimpleIoc.Default.Register<IWordRepository, WordRepository>();
			SimpleIoc.Default.Register<IWordEntryRepository, WordEntryRepository>();
		}

		/// <summary>
		/// Gets the Main property.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This non-static member is needed for data binding purposes.")]
		public MainViewModel Main
		{
			get
			{
				return ServiceLocator.Current.GetInstance<MainViewModel>();
			}
		}

		/// <summary>
		/// Cleans up all the resources.
		/// </summary>
		public static void Cleanup()
		{
		}
	}
}