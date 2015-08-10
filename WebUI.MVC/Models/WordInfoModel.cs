using System.Collections.Generic;

namespace WebUI.MVC.Models
{
	public class WordInfoModel
	{
		public string Name { get; set; }

		public List<OccurenceModel> Occurences { get; set; }

		public List<WordFileInfoModel> FilesInfo { get; set; }
	}
}