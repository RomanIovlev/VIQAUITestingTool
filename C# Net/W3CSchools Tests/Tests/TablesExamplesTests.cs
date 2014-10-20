using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.Common.Pairs;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;
using VIQA.SiteClasses;
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
                Rows = new Rows<TextElement> { StartIndex = 2},
                Locator = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
            };
            Assert.AreEqual(table.Columns.Headers.Print(), "Firstname, Lastname, Points");
            Assert.AreEqual(table.Rows.Headers.Print(), "1, 2, 3, 4");
            Assert.AreEqual(table.Cell(1, 2).Value, "Eve");
            Assert.AreEqual(table.Cell("Firstname", "2").Value, "Eve");

            Assert.AreEqual(table.Columns.Count, 3);
            Assert.AreEqual(table.Rows.Count, 4);

            Assert.AreEqual(table.Rows[2].Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.Rows["2"].Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.Columns[3].Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.Columns["Points"].Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "John, Johnson");
            { var cell = table.FindFirstCellWithValue("Doe");
            Assert.AreEqual(cell.ColumnNum + ":" + cell.RowNum, "2:3"); }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnNum, 2);
            Assert.AreEqual(table.FindCellInRow("3", "Doe").ColumnNum, 2);
            Assert.AreEqual(table.FindCellInColumn(2, "Doe").RowNum, 3);
            Assert.AreEqual(table.FindCellInColumn("Lastname", "Doe").RowNum, 3);

            Assert.AreEqual(table.FindRowByColumnValue(2, "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("Lastname", "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("3", "Doe").Print(), "Smith, Jackson, Doe, Johnson");

            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "John, Johnson");
            Assert.AreEqual(table.FindCellsInColumn(2, new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInColumn("Lastname", new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInRow(3, new Regex("o")).Print(), "John, Doe");
            Assert.AreEqual(table.FindCellsInRow("3", new Regex("o")).Print(), "John, Doe");

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

            Assert.AreEqual(table.Columns.Headers.Print(), "Lastname, Points");
            Assert.AreEqual(table.Rows.Headers.Print(), "Jill, Eve, John, Adam");
            Assert.AreEqual(table.Cell(1, 2).Value, "Jackson");
            Assert.AreEqual(table.Cell("Lastname", "Eve").Value, "Jackson");

            Assert.AreEqual(table.Columns.Count, 2);
            Assert.AreEqual(table.Rows.Count, 4);

            Assert.AreEqual(table.Rows[2].Print(), "Jackson, 94");
            Assert.AreEqual(table.Rows["Eve"].Print(), "Jackson, 94");
            Assert.AreEqual(table.Columns[2].Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.Columns["Points"].Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            { var cell = table.FindFirstCellWithValue("Doe");
            Assert.AreEqual(cell.ColumnNum + ":" + cell.RowNum, "1:3"); }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnNum, 1);
            Assert.AreEqual(table.FindCellInRow("John", "Doe").ColumnNum, 1);
            Assert.AreEqual(table.FindCellInColumn(1, "Doe").RowNum, 3);
            Assert.AreEqual(table.FindCellInColumn("Lastname", "Doe").RowNum, 3);

            Assert.AreEqual(table.FindRowByColumnValue(1, "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("Lastname", "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("John", "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "Johnson");
            Assert.AreEqual(table.FindCellsInColumn(1, new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInColumn("Lastname", new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInRow(3, new Regex("o")).Print(), "Doe");
            Assert.AreEqual(table.FindCellsInRow("John", new Regex("o")).Print(), "Doe");

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
                Columns = new Columns<TextElement> { HaveHeaders = false },
                Rows = new Rows<TextElement> { StartIndex = 2, HaveHeaders = false },
                Locator = By.XPath("//*[text()='HTML Table Example:']//..//table[1]"),
            };
            Assert.AreEqual(table.Columns.Headers.Print(), "1, 2, 3");
            Assert.AreEqual(table.Rows.Headers.Print(), "1, 2, 3, 4");
            Assert.AreEqual(table.Cell(1, 2).Value, "Eve");
            Assert.AreEqual(table.Cell("1", "2").Value, "Eve");

            Assert.AreEqual(table.Columns.Count, 3);
            Assert.AreEqual(table.Rows.Count, 4);

            Assert.AreEqual(table.Rows[2].Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.Rows["2"].Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.Columns[3].Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.Columns["3"].Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "John, Johnson");
            {
                var cell = table.FindFirstCellWithValue("Doe");
                Assert.AreEqual(cell.ColumnNum + ":" + cell.RowNum, "2:3");
            }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnNum, 2);
            Assert.AreEqual(table.FindCellInRow("3", "Doe").ColumnNum, 2);
            Assert.AreEqual(table.FindCellInColumn(2, "Doe").RowNum, 3);
            Assert.AreEqual(table.FindCellInColumn("2", "Doe").RowNum, 3);

            Assert.AreEqual(table.FindRowByColumnValue(2, "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("2", "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("3", "Doe").Print(), "Smith, Jackson, Doe, Johnson");

            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "John, Johnson");
            Assert.AreEqual(table.FindCellsInColumn(2, new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInColumn("2", new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInRow(3, new Regex("o")).Print(), "John, Doe");
            Assert.AreEqual(table.FindCellsInRow("3", new Regex("o")).Print(), "John, Doe");

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

            Assert.AreEqual(table.Columns.Headers.Print(), "1, 2");
            Assert.AreEqual(table.Rows.Headers.Print(), "Jill, Eve, John, Adam");
            Assert.AreEqual(table.Cell(1, 2).Value, "Jackson");
            Assert.AreEqual(table.Cell("1", "Eve").Value, "Jackson");

            Assert.AreEqual(table.Columns.Count, 2);
            Assert.AreEqual(table.Rows.Count, 4);

            Assert.AreEqual(table.Rows[2].Print(), "Jackson, 94");
            Assert.AreEqual(table.Rows["Eve"].Print(), "Jackson, 94");
            Assert.AreEqual(table.Columns[2].Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.Columns["2"].Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "Johnson");
            {
                var cell = table.FindFirstCellWithValue("Doe");
                Assert.AreEqual(cell.ColumnNum + ":" + cell.RowNum, "1:3");
            }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnNum, 1);
            Assert.AreEqual(table.FindCellInRow("John", "Doe").ColumnNum, 1);
            Assert.AreEqual(table.FindCellInColumn(1, "Doe").RowNum, 3);
            Assert.AreEqual(table.FindCellInColumn("1", "Doe").RowNum, 3);

            Assert.AreEqual(table.FindRowByColumnValue(1, "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("1", "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("John", "Doe").Print(), "Smith, Jackson, Doe, Johnson");

            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "Johnson");
            Assert.AreEqual(table.FindCellsInColumn(1, new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInColumn("1", new Regex("son")).Print(), "Jackson, Johnson");
            Assert.AreEqual(table.FindCellsInRow(3, new Regex("o")).Print(), "Doe");
            Assert.AreEqual(table.FindCellsInRow("John", new Regex("o")).Print(), "Doe");

            Assert.AreEqual(table.Value,
@"||X|1|2|| 
||Jill||Smith|50|| 
||Eve||Jackson|94|| 
||John||Doe|80|| 
||Adam||Johnson|67||");
        }

        [Test]
        public void FooterTest()
        {
            var page = new VIPage { Url = "http://www.w3schools.com/tags/tag_tfoot.asp" };
            page.Open();
            new ClickableElement(By.XPath(".//a[contains(text(),'Try it Yourself')]")).Click();
            ITable table = new Table(By.XPath("//table")) { Frame = "iframeResult" };
            Assert.AreEqual(table.Footer.Print(), "Sum, $180");
        }
    }
}


