using System;
using OpenQA.Selenium;
using VIQA.HAttributes;
using VIQA.SiteClasses;
using VITestsProject._4_TelerikItems.Site.Pages;

namespace VITestsProject._4_TelerikItems.Site
{
    [Site(Domain = "http://demos.telerik.com//")]
    public class TelerikSite : VISite
    {
        [Page(Title = "ComboBox page", Url = "http://demos.telerik.com/kendo-ui/combobox/index")]
        public ComboBoxPage ComboBoxPage;

        public TelerikSite(BrowserType browser = BrowserType.Chrome) : base(browser, isMain: true) { }
        public TelerikSite(Func<IWebDriver> browserFunc) : base(browserFunc, isMain: true) { }
    }
}