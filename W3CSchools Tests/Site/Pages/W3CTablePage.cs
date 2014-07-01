using System.Collections.Generic;
using VIQA.HAttributes;
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
            StartColumnIndex = 2,
            StartRowIndex = 2
        };

        [Locator(ByXPath = "//*[text()='HTML Table Example:']//..//table[1]")]
        public ITable TableColIndexRowNames = new Table
        {
            HeadingsType = TableHeadingType.RowsOnly,
            ColumnNames = new List<string> { "1", "2" },
            StartColumnIndex = 2,
            StartRowIndex = 2
        };
        
        [Locator(ByPartialLinkText = "W3Schools Home")]
        public ILink W3SchoolsHome;
    }
}
