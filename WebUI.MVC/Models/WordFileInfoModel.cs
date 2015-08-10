
namespace WebUI.MVC.Models
{
	public class WordFileInfoModel
	{
		public int FileId { get; set; }

		public string FileName { get; private set; }

		public int Count { get; private set; }

		public WordFileInfoModel(int fileId, string fileName, int count)
		{
			FileId = fileId;
			FileName = fileName;
			Count = count;
		}
	}
}