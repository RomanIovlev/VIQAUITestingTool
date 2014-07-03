using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.Common;
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
            Assert.AreEqual(table.ColumnNames.Print(), "Firstname, Lastname, Points");
            Assert.AreEqual(table.RowNames.Print(), "1, 2, 3, 4");
            Assert.AreEqual(table.Cell(1, 2).Value, "Eve");
            Assert.AreEqual(table.Cell("Firstname", "2").Value, "Eve");

            var allCells = table.Cells;
            Assert.AreEqual(allCells.Count, 3);
            Assert.AreEqual(allCells.Last().Value.Count, 4);

            Assert.AreEqual(table.GetRow(2).Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.GetRow("2").Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.GetColumn(3).Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.GetColumn("Points").Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "John, Johnson");
            { var cell = table.FindFirstCellWithValue("Doe");
            Assert.AreEqual(cell.X + ":" + cell.Y, "2:3"); }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnName, "Lastname");
            Assert.AreEqual(table.FindCellInRow("3", "Doe").X, 2);
            Assert.AreEqual(table.FindCellInColumn(2, "Doe").RowName, "3");
            Assert.AreEqual(table.FindCellInColumn("Lastname", "Doe").Y, 3);

            Assert.AreEqual(table.FindRowByColumnValue(2, "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("Lastname", "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("3", "Doe").Print(), "Smith, Jackson, Doe, Johnson");

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

            Assert.AreEqual(table.ColumnNames.Print(), "Lastname, Points");
            Assert.AreEqual(table.RowNames.Print(), "Jill, Eve, John, Adam");
            Assert.AreEqual(table.Cell(1, 2).Value, "Jackson");
            Assert.AreEqual(table.Cell("Lastname", "Eve").Value, "Jackson");

            var allCells = table.Cells;
            Assert.AreEqual(allCells.Count, 2);
            Assert.AreEqual(allCells.Last().Value.Count, 4);

            Assert.AreEqual(table.GetRow(2).Print(), "Jackson, 94");
            Assert.AreEqual(table.GetRow("Eve").Print(), "Jackson, 94");
            Assert.AreEqual(table.GetColumn(2).Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.GetColumn("Points").Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "Johnson");
            { var cell = table.FindFirstCellWithValue("Doe");
            Assert.AreEqual(cell.X + ":" + cell.Y, "1:3"); }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnName, "Lastname");
            Assert.AreEqual(table.FindCellInRow("John", "Doe").X, 1);
            Assert.AreEqual(table.FindCellInColumn(1, "Doe").RowName, "John");
            Assert.AreEqual(table.FindCellInColumn("Lastname", "Doe").Y, 3);

            Assert.AreEqual(table.FindRowByColumnValue(1, "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("Lastname", "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("John", "Doe").Print(), "Smith, Jackson, Doe, Johnson");

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
            Assert.AreEqual(table.ColumnNames.Print(), "1, 2, 3");
            Assert.AreEqual(table.RowNames.Print(), "1, 2, 3, 4");
            Assert.AreEqual(table.Cell(1, 2).Value, "Eve");
            Assert.AreEqual(table.Cell("1", "2").Value, "Eve");

            var allCells = table.Cells;
            Assert.AreEqual(allCells.Count, 3);
            Assert.AreEqual(allCells.Last().Value.Count, 4);

            Assert.AreEqual(table.GetRow(2).Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.GetRow("2").Print(), "Eve, Jackson, 94");
            Assert.AreEqual(table.GetColumn(3).Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.GetColumn("3").Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "John, Johnson");
            {
                var cell = table.FindFirstCellWithValue("Doe");
                Assert.AreEqual(cell.X + ":" + cell.Y, "2:3");
            }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnName, "2");
            Assert.AreEqual(table.FindCellInRow("3", "Doe").X, 2);
            Assert.AreEqual(table.FindCellInColumn(2, "Doe").RowName, "3");
            Assert.AreEqual(table.FindCellInColumn("2", "Doe").Y, 3);

            Assert.AreEqual(table.FindRowByColumnValue(2, "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("2", "Doe").Print(), "John, Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("3", "Doe").Print(), "Smith, Jackson, Doe, Johnson");

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

            Assert.AreEqual(table.ColumnNames.Print(), "1, 2");
            Assert.AreEqual(table.RowNames.Print(), "Jill, Eve, John, Adam");
            Assert.AreEqual(table.Cell(1, 2).Value, "Jackson");
            Assert.AreEqual(table.Cell("1", "Eve").Value, "Jackson");

            var allCells = table.Cells;
            Assert.AreEqual(allCells.Count, 2);
            Assert.AreEqual(allCells.Last().Value.Count, 4);

            Assert.AreEqual(table.GetRow(2).Print(), "Jackson, 94");
            Assert.AreEqual(table.GetRow("Eve").Print(), "Jackson, 94");
            Assert.AreEqual(table.GetColumn(2).Print(), "50, 94, 80, 67");
            Assert.AreEqual(table.GetColumn("2").Print(), "50, 94, 80, 67");

            Assert.AreEqual(table.FindCellsWithValue("Doe").Count, 1);
            Assert.AreEqual(table.FindCellsWithValue(new Regex("John")).Print(), "Johnson");
            {
                var cell = table.FindFirstCellWithValue("Doe");
                Assert.AreEqual(cell.X + ":" + cell.Y, "1:3");
            }

            Assert.AreEqual(table.FindCellInRow(3, "Doe").ColumnName, "1");
            Assert.AreEqual(table.FindCellInRow("John", "Doe").X, 1);
            Assert.AreEqual(table.FindCellInColumn(1, "Doe").RowName, "John");
            Assert.AreEqual(table.FindCellInColumn("1", "Doe").Y, 3);

            Assert.AreEqual(table.FindRowByColumnValue(1, "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindRowByColumnValue("1", "Doe").Print(), "Doe, 80");
            Assert.AreEqual(table.FindColumnByRowValue(3, "Doe").Print(), "Smith, Jackson, Doe, Johnson");
            Assert.AreEqual(table.FindColumnByRowValue("John", "Doe").Print(), "Smith, Jackson, Doe, Johnson");

            Assert.AreEqual(table.Value,
@"||X|1|2|| 
||Jill||Smith|50|| 
||Eve||Jackson|94|| 
||John||Doe|80|| 
||Adam||Johnson|67||");
        }
        
    }
}


