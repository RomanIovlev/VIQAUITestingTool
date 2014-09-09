using OpenQA.Selenium.Support.PageObjects;
using VIQA.HAttributes;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;
using W3CSchools_Tests.Site.Sections;

namespace W3CSchools_Tests.Site.Pages
{

    public class W3CFramePage : VIPage
    {
        [Locator(ByXPath = "//iframe[@src='default.asp']")]
        public ExampleFrame Frame;

        [FindsBy(How = How.LinkText, Using = "Start learning HTML now!")]
        [Frame(ByXPath = "//iframe[@src='default.asp']")]
        public ILink StartLearningLink;
    }
}
