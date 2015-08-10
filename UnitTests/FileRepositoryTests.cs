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
	public class FileRepositoryTests
	{
		private IDBContextFactory _dbContextFactory;
		private FolderScanEntities _dbContext;
		private IQueryable<File> _files;
		private IDbSet<File> _filesSet;

		[TestMethod]
		public void Should_ReturnProperlyFile_Test()
		{
			// Arrange
			ArrangeMocks();
			var testFileName = "test.txt";
			var expectedFile = GetTestFiles().First(x => x.Name == testFileName);

			var fileRopository = new FileRepository(_dbContextFactory);

			// Act
			var actualFile = fileRopository.GetFile(testFileName);

			// Assert
			_dbContextFactory.VerifyAllExpectations();
			Assert.IsNotNull(actualFile);
			Assert.AreEqual(expectedFile.Id, actualFile.Id);
		}

		[TestMethod]
		public void Should_CreateProperlyFile_Test()
		{
			// Arrange
			ArrangeMocks();
			var testFileName = "test.txt";
			var expectedFile = GetTestFiles().First(x => x.Name == testFileName);

			File actualAddedFile = null;
			_filesSet.Stub(m => m.Add(Arg<File>.Is.Anything)).WhenCalled(x =>
						{
							actualAddedFile = (File)x.Arguments[0];
						}).Return(expectedFile);

			var fileRopository = new FileRepository(_dbContextFactory);

			// Act
			fileRopository.CreateFile(expectedFile.Name, expectedFile.Path);

			// Assert
			_dbContextFactory.VerifyAllExpectations();
			Assert.AreEqual(expectedFile.Name, actualAddedFile.Name);
			Assert.AreEqual(expectedFile.Path, actualAddedFile.Path);
		}

		[TestMethod]
		public void Should_ReturnAllFiles_Test()
		{
			// Arrange
			ArrangeMocks();
			var expectedFiles = GetExpectedFiles();

			var fileRopository = new FileRepository(_dbContextFactory);

			// Act
			var actualFiles = fileRopository.GetFiles();

			// Assert
			_dbContextFactory.VerifyAllExpectations();
			AsserFilesEquality(expectedFiles, actualFiles.ToList());
		}

		private void AsserFilesEquality(List<FEntry> expectedFiles, List<FEntry> actualFiles)
		{
			Assert.AreEqual(expectedFiles.Count, actualFiles.Count);
			var expected = expectedFiles.OrderBy(x => x.Id).ToList();
			var actual = actualFiles.OrderBy(x => x.Id).ToList();
			for (int i = 0; i < expected.Count; i++)
				AssertFileEquality(expected[i], actual[i]);
		}

		private void AssertFileEquality(FEntry expected, FEntry actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.Path, actual.Path);
			Assert.AreEqual(expected.Words, actual.Words);
		}

		private IQueryable<File> GetTestFiles()
		{
			return new List<File>
			{
				new File() { Id = 1, Name = "test.txt", Path = "D:\\temp\test.txt"},
				new File() { Id = 2, Name = "test2.txt", Path = "D:\\temp\test2.txt"}
			}.AsQueryable();
		}

		private List<FEntry> GetExpectedFiles()
		{
			return GetTestFiles().Select(x => new FEntry
			{
				Id = x.Id,
				Name = x.Name,
				Path = x.Path,
				Words = 0,
			}).ToList();
		}

		private void ArrangeMocks()
		{
			_dbContext = MockRepository.GenerateMock<FolderScanEntities>();
			_filesSet = MockRepository.GenerateMock<IDbSet<File>, IQueryable>();

			_dbContextFactory = MockRepository.GenerateMock<IDBContextFactory>();
			_dbContextFactory.Expect(x => x.CreateContext()).Return(_dbContext);

			_files = GetTestFiles();

			_filesSet.Stub(m => m.Provider).Return(_files.Provider);
			_filesSet.Stub(m => m.Expression).Return(_files.Expression);
			_filesSet.Stub(m => m.ElementType).Return(_files.ElementType);
			_filesSet.Stub(m => m.GetEnumerator()).Return(_files.GetEnumerator());
			
			_dbContext.Stub(x => x.Files).PropertyBehavior();
			_dbContext.Files = _filesSet;
		}
	}
}
