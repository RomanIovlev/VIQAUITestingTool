using VIQA.HAttributes;
using VIQA.SiteClasses;
using W3CSchools_Tests.Site.Pages;

namespace W3CSchools_Tests.Site
{
    [Site(Domain = "http://www.w3schools.com/", DemoMode = false)]
    [DemoSettings(FrameColor = "blue")]
    public class W3CSite : VISite
    {
        [Page(Url = "http://www.w3schools.com/html/html_tables.asp")]
        public W3CTablePage W3CPageTable;

        [Page(Url = "http://www.w3schools.com/html/html_iframe.asp")]
        public W3CFramePage W3CPageFrame;
    }
}
