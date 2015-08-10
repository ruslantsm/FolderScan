using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScanningLib;
using WordEntryDal;
using Rhino.Mocks;
using WordEntryDal.Models;
using File = WordEntryDal.Models.File;

namespace UnitTests
{
	[TestClass]
	public class ScanningServiceTests
	{
		private const string _testLine = "The facts relating this \"apparition\" (various log-books)";
		private readonly IEnumerable<string> _lines = new List<string>()
		{
			_testLine, "", _testLine
		};

		private const string _testPath = @"D:\tmp\test.txt";
		private CancellationToken _token = new CancellationToken();

		private ILineProcessor _lineProcessor;
		private IDataAccess _dataAccess;
		private IFileReader _fileReader;


		[TestInitialize]
		public void Initialize()
		{
			_lineProcessor = MockRepository.GenerateMock<ILineProcessor>();
			_dataAccess = MockRepository.GenerateMock<IDataAccess>();
			_fileReader = MockRepository.GenerateMock<IFileReader>();
		}

		[TestMethod]
		public void ReadProperlyFile_Test()
		{
			// Arrange
			ArrangeMocks();

			_fileReader = MockRepository.GenerateMock<IFileReader>();
			var service = new ScanningService(_lineProcessor, _dataAccess, _fileReader);

			var actualPath = string.Empty;
			_fileReader.Expect(x => x.ReadByLine(Arg<string>.Is.Anything)).Return(_lines).WhenCalled(x =>
															{
																actualPath = (string)x.Arguments[0];
															});

			// Act
			var actualResult = service.AddScanningFile(_testPath, _token);
			actualResult.Wait(_token);

			// Assert
			_fileReader.VerifyAllExpectations();
			Assert.AreEqual(_testPath, actualPath);
		}


		[TestMethod]
		public void ProcessProperlyLines_Test()
		{
			// Arrange
			ArrangeMocks();

			_lineProcessor = MockRepository.GenerateMock<ILineProcessor>();
			var service = new ScanningService(_lineProcessor, _dataAccess, _fileReader);

			var actualLines = new List<string>();
			_lineProcessor.Expect(x => x.Process(Arg<string>.Is.Anything))
				.Repeat.Twice().WhenCalled(x => actualLines.Add((string)x.Arguments[0]))
				.Return(GetLineProcessorWorkResult());

			// Act
			var actualResult = service.AddScanningFile(_testPath, _token);
			actualResult.Wait(_token);

			// Assert
			_lineProcessor.VerifyAllExpectations();

			Assert.AreEqual(2, actualLines.Count);
			actualLines.ForEach(x => Assert.AreEqual(_testLine, x));
		}

		[TestMethod]
		public void CreateProperlyFileInDb_Test()
		{
			// Arrange
			ArrangeMocks();
			_dataAccess = MockRepository.GenerateMock<IDataAccess>();
			var service = new ScanningService(_lineProcessor, _dataAccess, _fileReader);

			var actualCreatedFilePath = string.Empty;
			_dataAccess.Expect(x => x.GetFile(Arg<string>.Is.Anything)).WhenCalled(x =>
									{
										actualCreatedFilePath = (string) x.Arguments[0];
									})
									.Return(GetTestFileEntity(_testPath));

			_dataAccess.Expect(x => x.SaveWords(Arg<IEnumerable<string>>.Is.Anything)).Return(GetSaveWordsResult());
			_dataAccess.Expect(x => x.AddWordEntries(Arg<List<WEntry>>.Is.Anything));

			// Act
			var actualResult = service.AddScanningFile(_testPath, _token);
			actualResult.Wait(_token);

			// Assert
			_dataAccess.VerifyAllExpectations();
			Assert.AreEqual(_testPath, actualCreatedFilePath);
		}

		[TestMethod]
		public void SaveProperlyWords_Test()
		{
			// Arrange
			ArrangeMocks();
			_dataAccess = MockRepository.GenerateMock<IDataAccess>();
			var service = new ScanningService(_lineProcessor, _dataAccess, _fileReader);

			var expectedWordList = GetExpectedWordsList();

			_dataAccess.Expect(x => x.GetFile(Arg<string>.Is.Anything))
							.Return(GetTestFileEntity(_testPath));

			var actualWordList = Enumerable.Empty<string>();
			_dataAccess.Expect(x => x.SaveWords(Arg<IEnumerable<string>>.Is.Anything)).WhenCalled(x =>
								{
									actualWordList = (IEnumerable<string>)x.Arguments[0];
								})
								.Return(GetSaveWordsResult());

			_dataAccess.Expect(x => x.AddWordEntries(Arg<List<WEntry>>.Is.Anything));

			// Act
			var actualResult = service.AddScanningFile(_testPath, _token);
			actualResult.Wait(_token);

			// Assert
			_dataAccess.VerifyAllExpectations();
			AssertWordLisdEquality(expectedWordList, actualWordList);
		}

		[TestMethod]
		public void SaveProperlyWordEntries_Test()
		{
			// Arrange
			ArrangeMocks();
			_dataAccess = MockRepository.GenerateMock<IDataAccess>();
			var service = new ScanningService(_lineProcessor, _dataAccess, _fileReader);

			var expectedWordEntries = GetExpectedWordEntries();

			_dataAccess.Expect(x => x.GetFile(Arg<string>.Is.Anything))
								.Return(GetTestFileEntity(_testPath));
			_dataAccess.Expect(x => x.SaveWords(Arg<IEnumerable<string>>.Is.Anything))
								.Return(GetSaveWordsResult());

			var actualWordEntries = new List<WEntry>();
			_dataAccess.Expect(x => x.AddWordEntries(Arg<List<WEntry>>.Is.Anything)).WhenCalled(x =>
								{
									actualWordEntries = (List<WEntry>)x.Arguments[0];
								});

			// Act
			var actualResult = service.AddScanningFile(_testPath, _token);
			actualResult.Wait(_token);

			// Assert
			_dataAccess.VerifyAllExpectations();

			AssertWordEntriesEquality(expectedWordEntries, actualWordEntries);
		}

		private void AssertWordEntriesEquality(List<WEntry> expectedWordEntries, List<WEntry> actualWordEntries)
		{
			Assert.AreEqual(expectedWordEntries.Count, actualWordEntries.Count);
			var expectedOrdered = expectedWordEntries.OrderBy(x => x.WLine).ThenBy(x => x.WIndex).ToList();
			var actualOrdered = actualWordEntries.OrderBy(x => x.WLine).ThenBy(x => x.WIndex).ToList();

			for (int i = 0; i < expectedOrdered.Count; i++)
			{
				AssertWEntryEquality(expectedOrdered[i], actualOrdered[i]);
			}
		}

		private void AssertWEntryEquality(WEntry expected, WEntry actual)
		{
			Assert.AreEqual(expected.FileId, actual.FileId);
			Assert.AreEqual(expected.WIndex, actual.WIndex);
			Assert.AreEqual(expected.WLine, actual.WLine);
			Assert.AreEqual(expected.WordId, actual.WordId);
		}

		private void ArrangeMocks()
		{
			_fileReader.Expect(x => x.ReadByLine(Arg<string>.Is.Equal(_testPath))).Return(_lines);

			_lineProcessor.Expect(x => x.Process(Arg<string>.Is.Equal(_testLine)))
				.Return(GetLineProcessorWorkResult()).Repeat.Once();
			_lineProcessor.Expect(x => x.Process(Arg<string>.Is.Equal(_testLine)))
				.Return(GetLineProcessorWorkResult()).Repeat.Once();

			_dataAccess.Expect(x => x.GetFile(Arg<string>.Is.Equal(_testPath)))
				.Return(GetTestFileEntity(_testPath));

			var actualWordList = Enumerable.Empty<string>();
			_dataAccess.Expect(x => x.SaveWords(Arg<IEnumerable<string>>.Is.Anything)).WhenCalled(x =>
			{
				actualWordList = (IEnumerable<string>)x.Arguments[0];
			})
								.Return(GetSaveWordsResult());

			var actualWordEntries = new List<WEntry>();
			_dataAccess.Expect(x => x.AddWordEntries(Arg<List<WEntry>>.Is.Anything)).WhenCalled(x =>
			{
				actualWordEntries = (List<WEntry>)x.Arguments[0];
			});
		}

		private void AssertWordLisdEquality(IEnumerable<string> expectedList, IEnumerable<string> actualList)
		{
			var expected = expectedList.ToList();
			var actual = actualList.ToList();

			Assert.AreEqual(expected.Count, actual.Count);
			for (int i = 0; i < expected.Count; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		private File GetTestFileEntity(string path)
		{
			return new File
			{
				Id = 1,
				Name = Path.GetFileName(path),
				Path = path
			};
		}

		private IDictionary<string, Word> GetSaveWordsResult()
		{
			var names = GetLineProcessorWorkResult().Select(x => x.Name).ToList();
			return names.ToDictionary(x => x, y => new Word
										{
											Id = 1,
											Name = y,
										});
		}

		private IEnumerable<string> GetExpectedWordsList()
		{
			return GetLineProcessorWorkResult().Select(x => x.Name).ToList();
		}

		private IEnumerable<WordItem> GetLineProcessorWorkResult()
		{
			return new List<WordItem>()
			 {
				 new WordItem { Name  =  "the", Index = 0},
				 new WordItem { Name  =  "facts", Index = 4},
				 new WordItem { Name  =  "relating", Index = 10},
				 new WordItem { Name  =  "this", Index = 19},
				 new WordItem { Name  =  "apparition", Index = 25},
				 new WordItem { Name  =  "various", Index = 38},
				 new WordItem { Name  =  "log-books", Index = 46}
			 };
		}

		private List<WEntry> GetExpectedWordEntries()
		{
			var list = new List<WEntry>();
			list.AddRange(
				GetLineProcessorWorkResult().Select(x =>
					new WEntry()
					{
						FileId = 1,
						WordId = 1,
						WIndex = x.Index,
						WLine = 1
					}
				));
			list.AddRange(
				GetLineProcessorWorkResult().Select(x =>
					new WEntry()
					{
						FileId = 1,
						WordId = 1,
						WIndex = x.Index,
						WLine = 3
					}
				));

			return list;
		}
	}
}
