using System;
using OpenQA.Selenium;
using VIQA.HAttributes;
using VIQA.SiteClasses;
using Parallel.VITestsProject.Site.Pages;

namespace Parallel.VITestsProject.Site
{
    [Site(Domain = "http://market.yandex.ru/")]
    public class YandexMarketSite : VISite
    {
        [Page(Title = "Яндекс.Маркет", Url = "http://market.yandex.ru/")]
        public HomePage HomePage;

        [Page(Title = "Выбор по параметрам - Яндекс.Маркет", Url = "http://market.yandex.ru/guru.xml")]
        public ProductPage ProductPage;

        public YandexMarketSite(BrowserType browser) : base(browser, isMain: true) { }
        public YandexMarketSite(Func<IWebDriver> browserFunc) : base(browserFunc, isMain: true) { }

    }
}
