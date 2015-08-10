using System.Collections.Generic;
using Common.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebUI.MVC.Controllers;
using Rhino.Mocks;
using WebUI.MVC.Models;
using WebUI.MVC.Queries.GetWords;
using WebUI.MVC.ViewModels;

namespace UnitTests
{
	[TestClass]
	public class HomeControllerTests
	{
		private IApplicationFacade _facade;

		[TestInitialize]
		public void Initialize()
		{
			_facade = MockRepository.GenerateMock<IApplicationFacade>();
		}

		[TestMethod]
		public void Index_ShouldReturnDefaultView_Test()
		{
			// Arrange
			var controller = new HomeController(_facade);

			GetWordsRequest actualRequest = null;
			_facade.Expect(x => x.Query<GetWordsResponse, GetWordsRequest>(Arg<GetWordsRequest>.Is.Anything))
							.Return(new GetWordsResponse(new List<WordInfoModel>(), 0));

			// Act
			var result = controller.Index(null, null);

			// Assert
			Assert.AreEqual("", result.ViewName);
		}

		[TestMethod]
		public void Index_Should_CreateRequestWithDefaultPage_Test()
		{
			// Arrange
			var controller = new HomeController(_facade);

			GetWordsRequest actualRequest = null;
			_facade.Expect(x => x.Query<GetWordsResponse, GetWordsRequest>(Arg<GetWordsRequest>.Is.Anything)).WhenCalled(x =>
							{
								actualRequest = (GetWordsRequest) x.Arguments[0];
							})
							.Return(new GetWordsResponse(new List<WordInfoModel>(), 0));

			// Act
			controller.Index(null, null);

			// Assert
			_facade.VerifyAllExpectations();
			Assert.IsNotNull(actualRequest);
			Assert.AreEqual(1 , actualRequest.PageNumber);
			Assert.AreEqual(15, actualRequest.PageSize);
		}

		[TestMethod]
		public void Index_Should_CreateRequestWithProperlyPageNumber_Test()
		{
			// Arrange
			var controller = new HomeController(_facade);
			var expectedPageNumber = 42;

			GetWordsRequest actualRequest = null;
			_facade.Expect(x => x.Query<GetWordsResponse, GetWordsRequest>(Arg<GetWordsRequest>.Is.Anything)).WhenCalled(x =>
							{
								actualRequest = (GetWordsRequest)x.Arguments[0];
							})
							.Return(new GetWordsResponse(new List<WordInfoModel>(), 0));

			// Act
			controller.Index(null, expectedPageNumber);

			// Assert
			_facade.VerifyAllExpectations();
			Assert.IsNotNull(actualRequest);
			Assert.AreEqual(expectedPageNumber, actualRequest.PageNumber);
		}

		[TestMethod]
		public void Index_Should_ReturnProperlyWordsViewModel_Test()
		{
			// Arrange
			var controller = new HomeController(_facade);
			var expectedPageNumber = 42;
			var expectedTotal = 150;
			var expectedPageSize = 15;
			var expectedWordsInfo = GetExpectedWordsInfo();

			_facade.Expect(x => x.Query<GetWordsResponse, GetWordsRequest>(Arg<GetWordsRequest>.Is.Anything))
							.Return(new GetWordsResponse(expectedWordsInfo, expectedTotal));

			// Act
			var result = controller.Index(null, expectedPageNumber);
			var actualWordsViewModel = (WordsViewModel)result.ViewData.Model;

			// Assert
			_facade.VerifyAllExpectations();
			
			Assert.AreEqual(expectedPageNumber, actualWordsViewModel.PageNumber);
			Assert.AreEqual(expectedPageSize, actualWordsViewModel.PageSize);
			Assert.AreEqual(expectedTotal, actualWordsViewModel.TotalCount);
			Assert.IsTrue(ReferenceEquals(expectedWordsInfo, actualWordsViewModel.Words));
		}

		private List<WordInfoModel> GetExpectedWordsInfo()
		{
			return new List<WordInfoModel>()
			{
				new WordInfoModel
				{
					Name = "test1",
					FilesInfo = GetExpectedFileInfoModels(),
					Occurences = GetExpectedOccurenceModels(),
				}
			};
		}

		private List<OccurenceModel> GetExpectedOccurenceModels()
		{
			return new List<OccurenceModel>()
			{
				new OccurenceModel(1, "test_file1.txt", 1, 0),
				new OccurenceModel(1, "test_file1.txt", 10, 6),
				new OccurenceModel(2, "test_file2.txt", 1, 1),
				new OccurenceModel(2, "test_file2.txt", 5, 4),
				new OccurenceModel(2, "test_file2.txt", 11, 7)
			};
		}

		private List<WordFileInfoModel> GetExpectedFileInfoModels()
		{
			return new List<WordFileInfoModel>()
			{
				new WordFileInfoModel(1, "test_file1.txt", 2),
				new WordFileInfoModel(2, "test_file2.txt", 3)
			};
		}
	}
}
