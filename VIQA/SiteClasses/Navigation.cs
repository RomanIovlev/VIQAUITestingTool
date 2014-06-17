using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using OpenQA.Selenium;

namespace VIQA.SiteClasses
{
    public class Navigation
    {
        private readonly VISite _site;
        private IWebDriver WebDriver { get { return _site.WebDriver; } }

        public Navigation(VISite site)
        {
            PagesHistory = new List<VIPage>();
            _currentPageNum = -1; 
            _site = site;
        }

        public readonly List<VIPage> PagesHistory;
        private int _currentPageNum;
        public VIPage CurrentPage { get { return PagesHistory[_currentPageNum]; } }
        public string WindowHandle;
        
        public void OpenPage(string uri)
        {
            OpenPage(new VIPage("Page with url " + VIPage.GetUrlValue(uri, _site), uri, site: _site));
        }

        public void OpenPage(VIPage page)
        {
            VISite.Logger.Event("Open page: " + page.Url);
            WebDriver.Navigate().GoToUrl(page.Url);
            WindowHandle = WebDriver.CurrentWindowHandle;
            _site.SiteSettings.CashDropTimes++;
            PagesHistory.Add(page);
            _currentPageNum++;
        }

        public void GoBack()
        {
            VISite.Logger.Event("GoBack to previous page");
            _site.WebDriver.Navigate().Back();
            _currentPageNum--;
        }
        public void GoForward()
        {
            VISite.Logger.Event("GoForward to next page");
            _site.WebDriver.Navigate().Forward();
            _currentPageNum++;
        }

        public void RefreshPage()
        {
            VISite.Logger.Event("Refresh current page");
            _site.WebDriver.Navigate().Refresh();
        }
    }
}
