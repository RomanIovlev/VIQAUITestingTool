using VIQA.Common;
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
            Columns = new Columns<TextElement> { Headers = new [] { "Lastname", "Points" }, StartIndex = 2, HaveHeaders = true },
            Rows = new Rows<TextElement> { StartIndex = 2, HaveHeaders = true },
        };

        [Locator(ByXPath = "//*[text()='HTML Table Example:']//..//table[1]")]
        public ITable TableColIndexRowNames = new Table
        {
            Columns = new Columns<TextElement> { Headers = new[] { "1", "2" }, StartIndex = 2, HaveHeaders = false },
            Rows = new Rows<TextElement> { StartIndex = 2, HaveHeaders = true },
        };
        
        [Locator(ByPartialLinkText = "W3Schools Home")]
        public ILink W3SchoolsHome;
    }
}
