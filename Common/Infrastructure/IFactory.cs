namespace Common.Infrastructure
{
	public interface IFactory
	{
		T GetInstance<T>();
	}
}