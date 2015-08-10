using Common.Queries;
using System.Linq;
using WebUI.MVC.Models;
using WordEntryDal.Repositories;

namespace WebUI.MVC.Queries.GetWords
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
			int total;
			var words = _wordRepository.GetWords(request.PageSize, request.PageNumber, out total).ToList();

			var result = from word in words
						 let fsi = (from fi in word.Occurences
								    group fi by fi.FileName into gr
								    select new WordFileInfoModel(word.Occurences.First(x => x.FileName == gr.Key).FileId, 
																	gr.Key, gr.Count())).ToList()
						 let occ = (from oc in word.Occurences
									select new OccurenceModel(oc.FileId, oc.FileName, oc.Line, oc.Index)).ToList()
						 select new WordInfoModel
						 {
							 Name = word.Name,
							 Occurences = occ,
							 FilesInfo = fsi
						 };

			return new GetWordsResponse(result.ToList(), total);
		}
	}
}