using System.Collections.Generic;

namespace WordEntryDal.Models
{
	public class WordEntryEntity
	{
		public string Name { get; set; }

		public IEnumerable<FullOccurence> Occurences { get; set; }
	}
}
