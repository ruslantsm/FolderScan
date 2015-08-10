using System.Collections.Generic;

namespace WordEntryDal.Models
{
	public class WEntryModel
	{
		public string Name { get; set; }

		public IEnumerable<Occurence> Occurences { get; set; }

		public int Count { get; set; }
	}
}
