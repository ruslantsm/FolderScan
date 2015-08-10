using Common.Queries;

namespace WebUI.MVC.Queries.GetWords
{
	public class GetWordsRequest : IQueryRequest
	{
		public int PageSize { get; private set; }

		public int PageNumber { get; private set; }

		public GetWordsRequest(int pageSize, int pageNumber)
		{
			PageSize = pageSize;
			PageNumber = pageNumber;
		}
	}
}