using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;
using VIQA.SiteClasses;

namespace W3CSchools_Tests.Site.Pages
{
    public class W3CTablePage : VIPage
    {
        [Locator(ByXPath = "//*[text()='HTML Table Example:']//..//table[1]")]
        public ITable TableColNamesRowNames = new Table
        {
            ColumnNames = new List<string> { "Lastname", "Points" },
            HeadingsType = TableHeadingType.RowsAndColumns,
            ColumnIndex = new List<int> { 2, 3 },
            RowIndex = new List<int> { 2, 3, 4, 5 }
        };

        [Locator(ByXPath = "//*[text()='HTML Table Example:']//..//table[1]")]
        public ITable TableColIndexRowNames = new Table
        {
            HeadingsType = TableHeadingType.RowsOnly,
            ColumnIndex = new List<int> { 2, 3 },
            RowIndex = new List<int> { 2, 3, 4, 5 }
        };

        public class MyCell : TextElement
        {
            [Locator(ByTagName = "a")] public ILink Link;
            [Locator(ByTagName = "span")] public IText Text;
        }

        [Locator(ByXPath = "//table[@role='grid']")]
        public ITable<ClickableText> AlexTable = new Table<ClickableText>(cellLocatorTemplate: By.XPath("td[{0}]"))
        {
            CellTemplate = () => new ClickableText(By.XPath("td[{0}]//"), By.XPath("td[{0}]"))
        };

        [Locator(ByPartialLinkText = "W3Schools Home")]
        public ILink W3SchoolsHome;
    }
}
