
namespace WebUI.MVC.Models
{
	public class OccurenceModel
	{
		public int FileId { get; set; }
		public string FileName { get; set; }
		public int Line { get; set; }
		public int Index { get; set; }

		public OccurenceModel(int fileId, string fileName, int line, int index)
		{
			FileId = fileId;
			FileName = fileName;
			Line = line;
			Index = index;
		}
	}
}