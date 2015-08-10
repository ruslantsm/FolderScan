using Common.Queries;
using System.Collections.Generic;
using WebUI.MVC.Models;

namespace WebUI.MVC.Queries.GetWords
{
	public class GetWordsResponse : IQueryResponse
    {
		public List<WordInfoModel> Words { get; private set; }

		public int Total { get; private set; }

		public GetWordsResponse(List<WordInfoModel> words, int total)
		{
			Words = words;
			Total = total;
		}
    }
}