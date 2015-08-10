using Common.Commands;
using Common.Queries;

namespace Common.Infrastructure
{
	public class ApplicationFacade : IApplicationFacade
	{
		private readonly IFactory _factory;

		public ApplicationFacade(IFactory factory)
		{
			_factory = factory;
		}

		public TResponse Query<TResponse, TRequest>(TRequest request)
			where TResponse : IQueryResponse
			where TRequest : IQueryRequest
		{
			var query = _factory.GetInstance<IQuery<TResponse, TRequest>>();
			return query.Handle(request);
		}

		public void Command<TRequest>(TRequest request)
			where TRequest : ICommandRequest
		{
			var command = _factory.GetInstance<ICommand<TRequest>>();
			command.Handle(request);
		}
	}
}