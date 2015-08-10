using Common.Queries;
using WordEntryDal.Repositories;

namespace WebApi.Queries.GetWords
{
	public class GetWordsQuery : IQuery<GetWordsResponse, GetWordsRequest>
	{
		private readonly IWordRepository _wordRepository;

		public GetWordsQuery(IWordRepository wordRepository)
		{
			_wordRepository = wordRepository;
		}

		public GetWordsResponse Handle(GetWordsRequest request)
		{
			var words = _wordRepository.GetWords();

			return new GetWordsResponse(words);
		}
	}
}