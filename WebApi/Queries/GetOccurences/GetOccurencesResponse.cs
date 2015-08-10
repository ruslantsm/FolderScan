using Common.Queries;
using System.Collections.Generic;
using WordEntryDal.Models;

namespace WebApi.Queries.GetOccurences
{
	public class GetOccurencesResponse : IQueryResponse
    {
		public IEnumerable<Occurence> Occurences { get; private set; }


		public GetOccurencesResponse(IEnumerable<Occurence> occurences)
		{
			Occurences = occurences;
		}
    }
}