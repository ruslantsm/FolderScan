using Ninject;

namespace Common.Infrastructure
{
	public class NinjectFactory : IFactory
	{
		private readonly IKernel _kernel;

		public NinjectFactory(IKernel kernel)
		{
			_kernel = kernel;
		}

		public T GetInstance<T>()
		{
			return _kernel.Get<T>();
		}
	}
}