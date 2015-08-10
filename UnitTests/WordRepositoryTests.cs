using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WordEntryDal.Models;
using WordEntryDal.Repositories;

namespace UnitTests
{
	[TestClass]
	public class WordRepositoryTests
	{
		private IDBContextFactory _dbContextFactory;
		private FolderScanEntities _dbContext;
		private IQueryable<Word> _words;
		private IDbSet<Word> _wordsSet;

		[TestMethod]
		public void ShouldReturnProperPage_Test()
		{
			// Arrange
			ArrangeMocks();
			int pageSize = 2;
			int pageNumber = 2;
			
			var expectedWords = GetExpectedWords();

			var wordsRopository = new WordRepository(_dbContextFactory);

			// Act
			int total = 0;
			var actualWords = wordsRopository.GetWords(pageSize, pageNumber, out total);

			// Assert
			_dbContextFactory.VerifyAllExpectations();
			AsserWordsEquality(expectedWords, actualWords.ToList());
		}

		[TestMethod]
		public void TotalItems_Test()
		{
			// Arrange
			ArrangeMocks();

			var expectedTotal = GetTestWords().Count();

			var wordsRopository = new WordRepository(_dbContextFactory);

			// Act
			int total = 0;
			var actualWords = wordsRopository.GetWords(1, 1, out total);

			// Assert
			_dbContextFactory.VerifyAllExpectations();
			Assert.AreEqual(expectedTotal, total);
		}


		private void AsserWordsEquality(List<WordEntryEntity> expectedWords, List<WordEntryEntity> actualWords)
		{
			Assert.AreEqual(expectedWords.Count, actualWords.Count);
			var expected = expectedWords.OrderBy(x => x.Name).ToList();
			var actual = actualWords.OrderBy(x => x.Name).ToList();
			for (int i = 0; i < expected.Count; i++)
			{
				AsserWordEquality(expected[i], actual[i]);
			}
		}

		private void AsserWordEquality(WordEntryEntity expected, WordEntryEntity actual)
		{
			Assert.AreEqual(expected.Name, actual.Name);
			var expectedOccurences = expected.Occurences.OrderBy(x => x.Line).ThenBy(x => x.Index).ToList();
			var actualOccurences = actual.Occurences.OrderBy(x => x.Line).ThenBy(x => x.Index).ToList();

			Assert.AreEqual(expectedOccurences.Count, actualOccurences.Count);
			for (int i = 0; i < expectedOccurences.Count; i++)
			{
				AsserOccurenceEquality(expectedOccurences[i], actualOccurences[i]);
			}
		}

		private void AsserOccurenceEquality(FullOccurence expected, FullOccurence actual)
		{
			Assert.AreEqual(expected.FileId, actual.FileId);
			Assert.AreEqual(expected.FileName, actual.FileName);
			Assert.AreEqual(expected.Index, actual.Index);
			Assert.AreEqual(expected.Line, actual.Line);
		}

		private void ArrangeMocks()
		{
			_dbContext = MockRepository.GenerateMock<FolderScanEntities>();
			_wordsSet = MockRepository.GenerateMock<IDbSet<Word>, IQueryable>();

			_dbContextFactory = MockRepository.GenerateMock<IDBContextFactory>();
			_dbContextFactory.Expect(x => x.CreateContext()).Return(_dbContext);

			_words = GetTestWords();

			_wordsSet.Stub(m => m.Provider).Return(_words.Provider);
			_wordsSet.Stub(m => m.Expression).Return(_words.Expression);
			_wordsSet.Stub(m => m.ElementType).Return(_words.ElementType);
			_wordsSet.Stub(m => m.GetEnumerator()).Return(_words.GetEnumerator());

			_dbContext.Stub(x => x.Words).PropertyBehavior();
			_dbContext.Words = _wordsSet;
		}

		private List<WordEntryEntity> GetExpectedWords()
		{
			var words = GetTestWords().ToList();
			return new List<WordEntryEntity>()
			{
				new WordEntryEntity
				{
					Name = words[2].Name,
					Occurences = new List<FullOccurence>()
					{
						new FullOccurence()
						{
							FileId = 1,
							FileName = "test.txt",
							Line = 10,
							Index = 15
						},
						new FullOccurence()
						{
							FileId = 1,
							FileName = "test.txt",
							Line = 11,
							Index = 17
						}
					}
				},
				new WordEntryEntity
				{
					Name = words[3].Name,
					Occurences = new List<FullOccurence>()
					{
						new FullOccurence()
						{
							FileId = 1,
							FileName = "test.txt",
							Line = 10,
							Index = 15
						},
						new FullOccurence()
						{
							FileId = 1,
							FileName = "test.txt",
							Line = 11,
							Index = 17
						}
					}
				}
			};
		}

		private IQueryable<Word> GetTestWords()
		{
			return new List<Word>
			{
				new Word()
				{
					Id = 1,
					Name = "Word1",
					WordEntries = GetTestWordEntries(1, 1)
				},
				new Word()
				{
					Id = 2,
					Name = "Word2",
					WordEntries = GetTestWordEntries(2, 1)
				},
				new Word()
				{
					Id = 3,
					Name = "Word3",
					WordEntries = GetTestWordEntries(3, 1)
				},
				new Word()
				{
					Id = 4,
					Name = "Word4",
					WordEntries = GetTestWordEntries(4, 1)
				}
			}.AsQueryable();
		}

		private List<WordEntry> GetTestWordEntries(int wordId, int fileId)
		{
			return new List<WordEntry>()
			{
				new WordEntry
				{
					Id = 1,
					FileId = fileId,
					WordId = wordId,
					WordLine = 10,
					WordIndex = 15,
					File = new File { Id = fileId, Name = "test.txt" }
				},
				new WordEntry
				{
					Id = 2,
					FileId = fileId,
					WordId = wordId,
					WordLine = 11,
					WordIndex = 17,
					File = new File { Id = fileId, Name = "test.txt" }
				},
			};
		}
	}
}
