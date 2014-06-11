using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;
using VIQA.SiteClasses;

namespace CommonTestsWithComplexElements.Tests
{    
    [TestFixture]
    public class TablesExamplesTests
    {
        [SetUp]
        public void Init() { }

        [TearDown]
        public void TestCleanup() { }

        private VIPage W3CPageTable = new VIPage { Url = "http://www.w3schools.com/html/html_tables.asp" };

        [Test]
        public void TableColNamesRowIndexTest()
        {
            W3CPageTable.Open();
            ITable table = new Table
            {
                Context = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"), 
                RowIndex = new List<int> { 2, 3, 4, 5 }
            };
            Assert.AreEqual(table.ColumnNames.Count, 3);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.GetVIElementXY(1, 2).Value, "Eve");
            var AllElements = table.GetAllElements();
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
            W3CPageTable.Open();
            ITable table = new Table
            {
                Context = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"), 
                ColumnNames = new List<string> {"Lastname", "Points"},
                HeadingsType = TableHeadingType.RowsAndColumns,
                ColumnIndex = new List<int> { 2, 3 },
                RowIndex = new List<int> { 2, 3, 4, 5 }
            };
            Assert.AreEqual(table.ColumnNames.Count, 2);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.GetVIElementXY(1, 2).Value, "Jackson");
            var AllElements = table.GetAllElements();
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
            W3CPageTable.Open();
            ITable table = new Table
            {
                Context = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
                HeadingsType = TableHeadingType.NoHeadings,
                RowIndex = new List<int> { 2, 3, 4, 5 }
            };
            Assert.AreEqual(table.ColumnNames.Count, 3);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.GetVIElementXY(1, 2).Value, "Eve");
            var AllElements = table.GetAllElements();
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
            W3CPageTable.Open();
            ITable table = new Table
            {
                Context = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
                HeadingsType = TableHeadingType.RowsOnly,
                ColumnIndex = new List<int> { 2, 3 },
                RowIndex = new List<int> { 2, 3, 4, 5 }
            };
            Assert.AreEqual(table.ColumnNames.Count, 2);
            Assert.AreEqual(table.RowNames.Count, 4);
            Assert.AreEqual(table.GetVIElementXY(1, 2).Value, "Jackson");
            var AllElements = table.GetAllElements();
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


