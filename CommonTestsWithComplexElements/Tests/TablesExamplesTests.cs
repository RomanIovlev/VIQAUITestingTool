using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.HAttributes;
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

        [Page(Url = "http://www.w3schools.com/html/html_tables.asp")] 

        public class W3CSite : VISite
        {
            [Page(Url = "http://www.w3schools.com/html/html_tables.asp")]
            public W3CPageTable W3CPageTable;
        }

        public class W3CPageTable : VIPage
        {
            public ITable TableColNamesRowNames = new Table
            {
                Context = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
                ColumnNames = new List<string> { "Lastname", "Points" },
                HeadingsType = TableHeadingType.RowsAndColumns,
                ColumnIndex = new List<int> { 2, 3 },
                RowIndex = new List<int> { 2, 3, 4, 5 }
            };

            public ITable TableColIndexRowNames {
                get { return new Table
                        {
                            Context = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
                            HeadingsType = TableHeadingType.RowsOnly,
                            ColumnIndex = new List<int> {2, 3},
                            RowIndex = new List<int> {2, 3, 4, 5}
            }; } }

            [Locator(ByPartialLinkText = "W3Schools Home")]
            public ILink W3SchoolsHome;
        }

        private VIPage W3CPageFrames = new VIPage { Url = "http://www.w3schools.com/html/html_iframe.asp" };


        [Test, Ignore]
        public void FramesTest()
        {
            var page = new W3CSite().W3CPageTable;
            W3CPageFrames.Open();
            page.WebDriver.SwitchTo().Frame(page.WebDriver.FindElement(By.TagName("iframe")));
            page.W3SchoolsHome.Click();
        }

        [Test]
        public void TableColNamesRowIndexTest()
        {
            new W3CSite().W3CPageTable.Open();
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
            var w3CPage = new W3CSite().W3CPageTable;
            w3CPage.Open();
            var table = w3CPage.TableColNamesRowNames;
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
            new W3CSite().W3CPageTable.Open();
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
            var w3cPage = new W3CSite().W3CPageTable;
            w3cPage.Open();
            var table = w3cPage.TableColIndexRowNames;
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


