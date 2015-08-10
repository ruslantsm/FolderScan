using System.Collections.Generic;
using WebUI.MVC.Models;

namespace WebUI.MVC.ViewModels
{
	public class WordsViewModel
	{
		public List<WordInfoModel> Words { get; private set; }

		public int PageSize { get; private set; }

		public int PageNumber { get; private set; }

		public int TotalCount { get; private set; }

		public WordsViewModel(List<WordInfoModel> words, int pageSize, int pageNumber, int total)
		{
			Words = words;
			PageSize = pageSize;
			PageNumber = pageNumber;
			TotalCount = total;
		}
	}
}