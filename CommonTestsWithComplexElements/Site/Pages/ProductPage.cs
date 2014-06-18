using CommonTestsWithComplexElements.Site.Sections;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace CommonTestsWithComplexElements.Site.Pages
{
    [Page(Title = "выбор по параметрам", Url = "http://market.yandex.ru/guru.xml")]
    public class ProductPage : VIPage
    {
        [Name("Filter section")]
        [Locate(ByXPath = "//*[@class='b-gurufilters']")]
        public FilterSection FilterSection;
    }
}
