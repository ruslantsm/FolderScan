using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScanningLib;

namespace UnitTests
{
	[TestClass]
	public class LineProcessorTests
	 {
		 private string testLatinLine = "The facts relating this \"apparition\" (various log-books)";
		 private string testRusLine = "– Мы не можем больше оставаться здесь!";
		 private string testWithoutWordsLine = "12f3 \\45 \" \", !! 856w";

		 [TestMethod]
		 public void ParseLatinLine_Test()
		 {
			 // Arrange
			 var processor = new LineProcessor();
			 var expectedResult = GetExpectedLatinResult();

			 // Act
			 var actualResult = processor.Process(testLatinLine);

			 // Assert
			 AssertWordItemsCollectionsEquality(expectedResult, actualResult);
		 }

		 [TestMethod]
		 public void ParseRusLine_Test()
		 {
			 // Arrange
			 var processor = new LineProcessor();
			 var expectedResult = GetExpectedRusResult();

			 // Act
			 var actualResult = processor.Process(testRusLine);

			 // Assert
			 AssertWordItemsCollectionsEquality(expectedResult, actualResult);
		 }


		 [TestMethod]
		 public void ParseLineWithoutWords_Test()
		 {
			 // Arrange
			 var processor = new LineProcessor();
			 var expectedResult = new List<WordItem>();

			 // Act
			 var actualResult = processor.Process(testWithoutWordsLine);

			 // Assert
			 AssertWordItemsCollectionsEquality(expectedResult, actualResult);
		 }

		 private void AssertWordItemsCollectionsEquality(IEnumerable<WordItem> expected, IEnumerable<WordItem> actual)
		 {
			 var sortedExpected = expected.OrderBy(x => x.Index).ToList();
			 var sortedActual = actual.OrderBy(x => x.Index).ToList();

			 Assert.AreEqual(sortedExpected.Count, sortedActual.Count);

			 for (int i = 0; i < sortedExpected.Count; i++)
			 {
				 AssertWordItemEquality(sortedExpected[i], sortedActual[i]);
			 }
		 }

		 private void AssertWordItemEquality(WordItem expected, WordItem actual)
		 {
			 Assert.AreEqual(expected.Name, actual.Name);
			 Assert.AreEqual(expected.LineNum, actual.LineNum);
			 Assert.AreEqual(expected.Index, actual.Index);
		 }

		 private IEnumerable<WordItem> GetExpectedLatinResult()
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

		 private IEnumerable<WordItem> GetExpectedRusResult()
		 {
			 return new List<WordItem>()
			 {
				 new WordItem { Name  =  "мы", Index = 2},
				 new WordItem { Name  =  "не", Index = 5},
				 new WordItem { Name  =  "можем", Index = 8},
				 new WordItem { Name  =  "больше", Index = 14},
				 new WordItem { Name  =  "оставаться", Index = 21},
				 new WordItem { Name  =  "здесь", Index = 32}
			 };
		 }
	}
}
