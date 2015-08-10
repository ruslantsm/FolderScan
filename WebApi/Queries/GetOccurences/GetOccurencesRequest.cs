using Common.Queries;

namespace WebApi.Queries.GetOccurences
{
	public class GetOccurencesRequest : IQueryRequest
	{
		public string WordName { get; private set; }
		
		public GetOccurencesRequest(string word)
		{
			WordName = word;
		}
	}
}