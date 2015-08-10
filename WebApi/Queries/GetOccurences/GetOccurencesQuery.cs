using Common.Queries;
using System.Linq;
using WordEntryDal.Models;
using WordEntryDal.Repositories;

namespace WebApi.Queries.GetOccurences
{
	public class GetOccurencesQuery : IQuery<GetOccurencesResponse, GetOccurencesRequest>
	{
		private readonly IWordEntryRepository _wordEntryRepository;

		public GetOccurencesQuery(IWordEntryRepository wordEntryRepository)
		{
			_wordEntryRepository = wordEntryRepository;
		}

		public GetOccurencesResponse Handle(GetOccurencesRequest request)
		{
			var entries = _wordEntryRepository.GetEntries(request.WordName);
			var occurences = entries.Select(x => new Occurence
								{
									FId = x.FileId,
									Index = x.WordIndex,
									Line = x.WordLine
								});

			return new GetOccurencesResponse(occurences);
		}
	}
}