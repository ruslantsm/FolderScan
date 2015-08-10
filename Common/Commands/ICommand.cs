namespace Common.Commands
{
	public interface ICommand<in TRequest> where TRequest : ICommandRequest
	{
		void Handle(TRequest request);
	}
}