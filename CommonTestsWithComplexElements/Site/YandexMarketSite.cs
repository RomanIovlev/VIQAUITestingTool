using CommonTestsWithComplexElements.Site.Pages;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace CommonTestsWithComplexElements.Site
{
    [Site(Domain = "http://market.yandex.ru/")]
    public class YandexMarketSite : VISite
    {
        [Page(Title = "Яндекс.Маркет", Url = "http://market.yandex.ru/")]
        public HomePage HomePage;

        [Page(Title = "Выбор по параметрам - Яндекс.Маркет", Url = "http://market.yandex.ru/guru.xml")]
        public ProductPage ProductPage;
        
    }
}
