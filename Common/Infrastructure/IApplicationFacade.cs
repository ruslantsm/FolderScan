using Common.Commands;
using Common.Queries;

namespace Common.Infrastructure
{
	public interface IApplicationFacade
	{
		TResponse Query<TResponse, TRequest>(TRequest request)
			where TResponse : IQueryResponse
			where TRequest : IQueryRequest;

		void Command<TRequest>(TRequest request) where TRequest : ICommandRequest;
	}
}