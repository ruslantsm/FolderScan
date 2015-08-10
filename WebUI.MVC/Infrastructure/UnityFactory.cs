using Common.Infrastructure;
using Microsoft.Practices.Unity;

namespace WebUI.MVC.Infrastructure
{
	public class UnityFactory : IFactory
	{
		private readonly IUnityContainer _container;

		public UnityFactory(IUnityContainer container)
		{
			_container = container;
		}

		public T GetInstance<T>()
		{
			return _container.Resolve<T>();
		}
	}
}