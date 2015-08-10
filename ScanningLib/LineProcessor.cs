using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScanningLib
{
	public class LineProcessor : ILineProcessor
	{
		private const string WORD_PATTERN = @"([^\W_\d]([^\W_\d]|[-'\d](?=[^\W_\d]))*[^\W_\d])";
		private Regex regex = new Regex(WORD_PATTERN, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		public IEnumerable<WordItem> Process(string line)
		{
			return from Group @group in regex.Matches(line)
					select new WordItem
					{
						Name = @group.Value.ToLower(),
						Index = @group.Index
					};
		}
	}
}
