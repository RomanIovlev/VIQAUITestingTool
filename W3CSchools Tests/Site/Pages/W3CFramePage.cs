using VIQA.HAttributes;
using VIQA.SiteClasses;
using W3CSchools_Tests.Site.Sections;

namespace W3CSchools_Tests.Site.Pages
{

    public class W3CFramePage : VIPage
    {
        [Locator(ByXPath = "//iframe[@src='default.asp']")]
        public ExampleFrame Frame;
    }
}
