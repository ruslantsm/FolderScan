using Common.Queries;
using System.Collections.Generic;
using WordEntryDal.Models;

namespace WebApi.Queries.GetWords
{
	public class GetWordsResponse : IQueryResponse
    {
        public IEnumerable<WEntryModel> Words { get; private set; }


		public GetWordsResponse(IEnumerable<WEntryModel> words)
		{
			Words = words;
		}
    }
}