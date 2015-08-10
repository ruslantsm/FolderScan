using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using WordEntryDal;
using WordEntryDal.Models;

namespace ScanningLib
{
	public class ScanningService : IScanningService
	{
		private readonly ILineProcessor _lineProcessor;
		private readonly IDataAccess _dataAccess;
		private readonly IFileReader _fileReader;

		public ScanningService(ILineProcessor lineProcessor, IDataAccess dataAccess, IFileReader fileReader)
		{
			_lineProcessor = lineProcessor;
			_dataAccess = dataAccess;
			_fileReader = fileReader;
			
			Mapper.CreateMap<WordItem, WordModel>();
		}

		public Task AddScanningFile(string filePath, CancellationToken cancellationToken)
		{
			return Task.Run(() => ParseFile(filePath), cancellationToken);
		}

		private void ParseFile(string filePath)
		{
			var words = new Dictionary<string, List<WordItem>>();

			int lineNumber = 1;
			foreach (var line in _fileReader.ReadByLine(filePath))
			{
				if (!string.IsNullOrWhiteSpace(line))
				{
					var items = _lineProcessor.Process(line);
					var num = lineNumber;
					items.Each(x => { x.LineNum = num; AddItem(x, words); });
				}

				lineNumber++;
			}

			SaveItems(filePath, words);
		}

		private void SaveItems(string filePath, Dictionary<string, List<WordItem>> words)
		{
			var file = _dataAccess.GetFile(filePath);
			var dbWords = _dataAccess.SaveWords(words.Keys);

			var toSaveItems = new List<WEntry>();
			foreach (var key in words.Keys)
			{
				var items = words[key];
				Word word; 
				dbWords.TryGetValue(key, out word);
				if (word == null)
				{
					_dataAccess.AddWordItems(filePath, key.ToLower(), items.Select(Mapper.Map<WordModel>));
				}
				else
				{
					SaveEntries(items.Select(x => new WEntry
											{
												FileId = file.Id,
												WordId = word.Id,
												WLine = x.LineNum,
												WIndex = x.Index
											}), ref toSaveItems);
				}
			}

			_dataAccess.AddWordEntries(toSaveItems);
		}

		private void SaveEntries(IEnumerable<WEntry> items, ref List<WEntry> toSaveItems)
		{
			toSaveItems.AddRange(items);
			if (toSaveItems.Count > 10000)
			{
				_dataAccess.AddWordEntries(toSaveItems);
				toSaveItems = new List<WEntry>();
			}
		}


		private void AddItem(WordItem item, Dictionary<string, List<WordItem>> words)
		{
			List<WordItem> items;
			if (words.TryGetValue(item.Name, out items))
				items.Add(item);
			else
				words.Add(item.Name, new List<WordItem> { item });
		}
	}
}
