using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebUI.MVC.Models;
using WebUI.MVC.Queries.GetWords;
using WordEntryDal.Models;
using WordEntryDal.Repositories;

namespace UnitTests
{
	[TestClass]
	public class GetWordsQueryTests
	{
		private IWordRepository _wordRepository;

		[TestInitialize]
		public void Initialize()
		{
			_wordRepository = MockRepository.GenerateMock<IWordRepository>();
		}

		[TestMethod]
		public void Should_CreateProperResponse_Test()
		{
			// Arrange
			var request = CreateGetWordsRequest();
			var total = 3;
			var expectedWordInfoModels = GetExpectedWordInfoModels();

			_wordRepository.Expect(x => x.GetWords(Arg<int>.Is.Equal(request.PageSize),
				Arg<int>.Is.Equal(request.PageNumber), out Arg<int>.Out(total).Dummy))
				.Return(GetExpectedWordEntryEntity());

			var query = new GetWordsQuery(_wordRepository);

			// Act
			var response = query.Handle(request);

			// Assert
			Assert.IsNotNull(response);
			Assert.AreEqual(total, response.Total);
			AsserWordInfoModelsEquality(expectedWordInfoModels, response.Words);
		}

		private void AsserWordInfoModelsEquality(List<WordInfoModel> expectedWordInfoModels, List<WordInfoModel> actualWordInfoModels)
		{
			Assert.AreEqual(expectedWordInfoModels.Count, actualWordInfoModels.Count);
			var expected = expectedWordInfoModels.OrderBy(x => x.Name).ToList();
			var actual = actualWordInfoModels.OrderBy(x => x.Name).ToList();

			for (int i = 0; i < expected.Count; i++)
				AssertWordInfoModelEquality(expected[i], actual[i]);
		}

		private void AssertWordInfoModelEquality(WordInfoModel expectedModel, WordInfoModel actualModel)
		{
			Assert.AreEqual(expectedModel.Name, actualModel.Name);
			Assert.AreEqual(expectedModel.FilesInfo.Count, actualModel.FilesInfo.Count);

			var expectedFiModel = expectedModel.FilesInfo.OrderBy(x => x.FileId).ToList();
			var actualFiModel = actualModel.FilesInfo.OrderBy(x => x.FileId).ToList();

			for (int i = 0; i < expectedFiModel.Count; i++)
				AssertFileInfoModelEquality(expectedFiModel[i], actualFiModel[i]);

			var expectedOccurences = expectedModel.Occurences.OrderBy(x => x.Line).ThenBy(x => x.Index).ToList();
			var actualOccurences = actualModel.Occurences.OrderBy(x => x.Line).ThenBy(x => x.Index).ToList();

			for (int i = 0; i < expectedOccurences.Count; i++)
				AssertOccurencesEquality(expectedOccurences[i], actualOccurences[i]);
		}

		private void AssertOccurencesEquality(OccurenceModel expected, OccurenceModel actual)
		{
			Assert.AreEqual(expected.FileId, actual.FileId);
			Assert.AreEqual(expected.FileName, actual.FileName);
			Assert.AreEqual(expected.Index, actual.Index);
			Assert.AreEqual(expected.Line, actual.Line);
		}

		private void AssertFileInfoModelEquality(WordFileInfoModel expected, WordFileInfoModel actual)
		{
			Assert.AreEqual(expected.Count, actual.Count);
			Assert.AreEqual(expected.FileId, actual.FileId);
			Assert.AreEqual(expected.FileName, actual.FileName);
		}

		private List<WordInfoModel> GetExpectedWordInfoModels()
		{
			return new List<WordInfoModel>()
			{
				new WordInfoModel()
				{
					FilesInfo = GetExpectedFileInfoModels(),
					Name = "test",
					Occurences = GetExpectedOccurenceModels()
				}
			};
		}

		private List<OccurenceModel> GetExpectedOccurenceModels()
		{
			return new List<OccurenceModel>()
			{
				new OccurenceModel(1, "test1.txt", 42, 27),
				new OccurenceModel(1, "test1.txt", 125, 29),
				new OccurenceModel(2, "test2.txt", 125, 11),
			};
		}

		private List<WordFileInfoModel> GetExpectedFileInfoModels()
		{
			return new List<WordFileInfoModel>()
			{
				new WordFileInfoModel(1, "test1.txt", 2),
				new WordFileInfoModel(2, "test2.txt", 1),
			};
		}

		private GetWordsRequest CreateGetWordsRequest()
		{
			return new GetWordsRequest(15, 1);
		}

		private IEnumerable<WordEntryEntity> GetExpectedWordEntryEntity()
		{
			return new List<WordEntryEntity>()
			{
				new WordEntryEntity()
				{
					Name = "test",
					Occurences = GetExpectedOccurences()
				}
			};
		}

		private IEnumerable<FullOccurence> GetExpectedOccurences()
		{
			return new List<FullOccurence>()
			{
				new FullOccurence()
				{
					FileId = 1,
					FileName = "test1.txt",
					Index = 27,
					Line = 42
				},
				new FullOccurence()
				{
					FileId = 1,
					FileName = "test1.txt",
					Index = 29,
					Line = 125
				},
				new FullOccurence()
				{
					FileId = 2,
					FileName = "test2.txt",
					Index = 11,
					Line = 125
				}
			};
		}
	}
}
