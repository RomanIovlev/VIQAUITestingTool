using VIQA.HAttributes;
using VIQA.SiteClasses;
using VITestsProject.Site.Sections;

namespace VITestsProject.Site.Pages
{
    [Page(Title = "Яндекс.Маркет", Url = "http://market.yandex.ru/")]
    public class HomePage : VIPage
    {
        [Name("Search section")]
        [Locate(ByClassName = "b-head-search")]
        public static SearchSection SearchSection;

    }
}
