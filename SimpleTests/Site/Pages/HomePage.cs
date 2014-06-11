using SimpleTests.Site.Sections;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace SimpleTests.Site.Pages
{
    [Name("HomePage"), Page(Title = "Яндекс.Маркет", Url = "http://market.yandex.ru/", CheckUrl = PageCheckType.Equal, CheckTitle = PageCheckType.Contains, IsHomePage = true)]
    public class HomePage : VIPage
    {
        [Name("Search section")]
        [Locate(ByClassName = "b-head-search")]
        public SearchSection SearchSection;

    }
}
