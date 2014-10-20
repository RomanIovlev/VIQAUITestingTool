using SimpleTests.Site.Pages;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace SimpleTests.Site
{
    [Site(Domain = "http://market.yandex.ru/")]
    public class YandexMarketSite : VISite
    {
        [Page(Title = "Яндекс.Маркет", Url = "http://market.yandex.ru", IsHomePage = true)]
        public HomePage HomePage;

        [Page(Title = "выбор по параметрам на Яндекс.Маркет", Url = "guru.xml")]
        public ProductPage ProductPage;

    }
}
