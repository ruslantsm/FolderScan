using Common.Infrastructure;
using Common.Queries;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using WebApi.Queries.GetFiles;
using WebApi.Queries.GetOccurences;
using WebApi.Queries.GetWords;
using WordEntryDal.Repositories;

namespace Ninject.Http
{
	/// <summary>
	/// Resolves Dependencies Using Ninject
	/// </summary>
	public class NinjectHttpResolver : IDependencyResolver, IDependencyScope
	{
		public IKernel Kernel { get; private set; }
		public NinjectHttpResolver(params NinjectModule[] modules)
		{
			Kernel = new StandardKernel(modules);
		}

		public NinjectHttpResolver(Assembly assembly)
		{
			Kernel = new StandardKernel();
			Kernel.Load(assembly);
		}

		public object GetService(Type serviceType)
		{
			return Kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return Kernel.GetAll(serviceType);
		}

		public void Dispose()
		{
			//Do Nothing
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}


	// List and Describe Necessary HttpModules
	// This class is optional if you already Have NinjectMvc
	public class NinjectHttpModules
	{
		//Return Lists of Modules in the Application
		public static NinjectModule[] Modules
		{
			get
			{
				return new[] { new MainModule() };
			}
		}

		//Main Module For Application
		public class MainModule : NinjectModule
		{
			public override void Load()
			{
				Kernel.Bind<IFactory>().To<NinjectFactory>();
				Kernel.Bind<IApplicationFacade>().To<ApplicationFacade>();
				Kernel.Bind<IFileRepository>().To<FileRepository>();
				Kernel.Bind<IWordRepository>().To<WordRepository>();
				Kernel.Bind<IWordEntryRepository>().To<WordEntryRepository>();
				Kernel.Bind<IDBContextFactory>().To<DBContextFactory>();

				Kernel.Bind<IQuery<GetFilesResponse, GetFilesRequest>>().To<GetFilesQuery>();
				Kernel.Bind<IQuery<GetWordsResponse, GetWordsRequest>>().To<GetWordsQuery>();
				Kernel.Bind<IQuery<GetOccurencesResponse, GetOccurencesRequest>>().To<GetOccurencesQuery>();
				
			}
		}
	}


	/// <summary>
	/// Its job is to Register Ninject Modules and Resolve Dependencies
	/// </summary>
	public class NinjectHttpContainer
	{
		private static NinjectHttpResolver _resolver;

		//Register Ninject Modules
		public static void RegisterModules(NinjectModule[] modules)
		{
			_resolver = new NinjectHttpResolver(modules);
			GlobalConfiguration.Configuration.DependencyResolver = _resolver;
		}

		public static void RegisterAssembly()
		{
			_resolver = new NinjectHttpResolver(Assembly.GetExecutingAssembly());
			//This is where the actual hookup to the Web API Pipeline is done.
			GlobalConfiguration.Configuration.DependencyResolver = _resolver;
		}

		//Manually Resolve Dependencies
		public static T Resolve<T>()
		{
			return _resolver.Kernel.Get<T>();
		}
	}
}