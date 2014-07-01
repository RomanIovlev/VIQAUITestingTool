using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;
using W3CSchools_Tests.Site;

namespace W3CSchools_Tests.Tests
{    
    [TestFixture]
    public class TablesExamplesTests
    {
        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }

        public static W3CSite W3CSite = new W3CSite();

        [Test]
        public void TableColNamesRowIndexTest()
        {
            W3CSite.W3CPageTable.Open();
            ITable table = new Table
            {
                Locator = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
                StartRowIndex = 2
            };
            Assert.AreEqual(table.ColumnNames.Count, 3);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.Cell(1, 2).Value, "Eve");
            var AllElements = table.Cells;
            Assert.AreEqual(AllElements.Count, 3);
            Assert.AreEqual(AllElements.Last().Value.Count, 4);
            Assert.AreEqual(table.Value, 
@"||X|Firstname|Lastname|Points|| 
||1||Jill|Smith|50|| 
||2||Eve|Jackson|94|| 
||3||John|Doe|80|| 
||4||Adam|Johnson|67||");
        }

        [Test]
        public void TableColNamesRowNamesTest()
        {
            var w3CPage = W3CSite.W3CPageTable;
            w3CPage.Open();
            var table = w3CPage.TableColNamesRowNames;
            Assert.AreEqual(table.ColumnNames.Count, 2);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.Cell(1, 2).Value, "Jackson");
            var AllElements = table.Cells;
            Assert.AreEqual(AllElements.Count, 2);
            Assert.AreEqual(AllElements.Last().Value.Count, 4);
            Assert.AreEqual(table.Value,
@"||X|Lastname|Points|| 
||Jill||Smith|50|| 
||Eve||Jackson|94|| 
||John||Doe|80|| 
||Adam||Johnson|67||");
        }

        [Test]
        public void TableColIndexRowIndexTest()
        {
            W3CSite.W3CPageTable.Open();
            ITable table = new Table
            {
                Locator = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
                HeadingsType = TableHeadingType.NoHeadings,
                StartRowIndex = 2
            };
            Assert.AreEqual(table.ColumnNames.Count, 3);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.Cell(1, 2).Value, "Eve");
            var AllElements = table.Cells;
            Assert.AreEqual(AllElements.Count, 3);
            Assert.AreEqual(AllElements.Last().Value.Count, 4);
            Assert.AreEqual(table.Value,
@"||X|1|2|3|| 
||1||Jill|Smith|50|| 
||2||Eve|Jackson|94|| 
||3||John|Doe|80|| 
||4||Adam|Johnson|67||");
        }

        [Test]
        public void TableColIndexRowNamesTest()
        {
            var w3cPage = W3CSite.W3CPageTable;
            w3cPage.Open();
            var table = w3cPage.TableColIndexRowNames;
            Assert.AreEqual(table.ColumnNames.Count, 2);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.Cell(1, 2).Value, "Jackson");
            var AllElements = table.Cells;
            Assert.AreEqual(AllElements.Count, 2);
            Assert.AreEqual(AllElements.Last().Value.Count, 4);
            Assert.AreEqual(table.Value,
@"||X|1|2|| 
||Jill||Smith|50|| 
||Eve||Jackson|94|| 
||John||Doe|80|| 
||Adam||Johnson|67||");
        }
        
    }
}


