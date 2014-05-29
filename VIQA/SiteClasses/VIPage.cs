using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HAttributes;
using VIQA.HtmlElements;

namespace VIQA.SiteClasses
{
    public class VIPage : VIElementsSet
    {
        private string _url;
        public string Url
        {
            set
            {
                _url = (string.IsNullOrEmpty(value))
                    ? Site.Domain
                    : (value.Contains("http://"))
                        ? value
                        : Site.Domain + value.TrimStart('/');
            }
            get { return _url; }
        }

        public string Title { get; set; }

        public bool IsUrlCheckNeeded { get; set; }

        public bool IsTitleCheckNeeded { get; set; }

        public VIPage() 
        {
            DefaultNameFunc = () => string.Format("Page with Title: '{0}', Url: '{1}'", Title ?? "", Url ?? "");
            var pageAttr = GetType().GetCustomAttribute<PageAttribute>(false);

            if (pageAttr == null)
            {
                Url = "";
                Title = "";
                IsUrlCheckNeeded = false;
                IsTitleCheckNeeded = false;
            }
            else
            {
                Url = pageAttr.Url;
                Title = pageAttr.Title;
                IsUrlCheckNeeded = (!string.IsNullOrEmpty(Url)) && pageAttr.CheckUrl;
                IsTitleCheckNeeded = (!string.IsNullOrEmpty(Title)) && pageAttr.CheckTitle;
            }
            FillMetaFromClass();         
        }
        
        public VIPage(string name, string url = null, string title = null, bool isUrlCheckNeeded = true, bool isTitleCheckNeeded = true, VISite site = null)
        {
            DefaultNameFunc = () => string.Format("Page with Title: '{0}', Url: '{1}'", Title ?? "", Url ?? "");
            if (name != null)
                Name = name;
            else
            {
                var nameAttr = GetType().GetCustomAttribute<NameAttribute>(false);
                if (nameAttr != null)
                    Name = nameAttr.Name;
            }
            if (site != null)
                Site = site;
            Url = url ?? "";
            Title = title ?? "";
            IsUrlCheckNeeded = isUrlCheckNeeded;
            IsTitleCheckNeeded = isTitleCheckNeeded;
            
            FillMetaFromClass();
        }

        private void FillMetaFromClass()
        {           
            var pageAttr = GetType().GetCustomAttribute<PageAttribute>(false);

            if (pageAttr != null)
            {
                if (Url == null)
                    Url = pageAttr.Url;
                if (Url == null)
                    Title = pageAttr.Title;
                if (IsUrlCheckNeeded && !pageAttr.CheckUrl)
                    IsUrlCheckNeeded = pageAttr.CheckUrl;
                if (IsTitleCheckNeeded && !pageAttr.CheckTitle)
                    IsTitleCheckNeeded = pageAttr.CheckTitle;
            }
            
        }

        public void Open()
        {
            VISite.Logger.Event("Open page: " + Url);
            Site.WebDriver.Navigate().GoToUrl(Url);
            Site.WindowHandle = WebDriver.WindowHandles.First();
            Site.CashDropTimes ++;
            if (IsUrlCheckNeeded) CheckUrl();
            if (IsTitleCheckNeeded) CheckTitle();
        }

        public bool CheckUrl()
        {
            VISite.Logger.Event(string.Format("Check url {0} for page '{1}'", Url, Name));
            if (string.IsNullOrEmpty(Url))
            {
                VISite.Alerting.ThrowError(string.Format("Can't check url {0} for page '{1}'. Please set Expected value", Url, Name));
                return false;
            }
            var timer = new Timer();
            while (!(WebDriver.Url.Contains(Url)))
                if (timer.TimeoutPassed(Site.WebDriverTimeouts.WaitPageToLoadInSec*1000))
                {
                    VISite.Alerting.ThrowError(string.Format("Can't url {0} for page '{1}'", Url, Name));
                    return false;
                }
            return true;
        }
        
        public bool CheckTitle()
        {
            VISite.Logger.Event(string.Format("Check title {0} for page '{1}'", Title, Name));
            if (string.IsNullOrEmpty(Title))
            {
                VISite.Alerting.ThrowError(string.Format("Can't check title {0} for page '{1}'. Please set Expected value", Title, Name));
                return false;
            }
            var timer = new Timer();
            while (!(WebDriver.Title.Contains(Title)))
                if (timer.TimeoutPassed(Site.WebDriverTimeouts.WaitPageToLoadInSec * 1000))
                {
                    VISite.Alerting.ThrowError(string.Format("Can't title {0} for page '{1}'", Title, Name));
                    return false;
                }
            return true;
        }

        public IWebDriver WebDriver { get { return Site.WebDriver; } }
        private VISite _site;
        private static VISite _defaultSite { set; get; }
        public VISite Site { set { _site = value; } get { return _site ?? _defaultSite; } }

        public static void Init(VISite site)
        {
            _defaultSite = site;
            var fields = _defaultSite.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(_ => typeof(VIPage).IsAssignableFrom(_.FieldType));
            fields.ForEach(page => page.SetValue(site, 
                Activator.CreateInstance(page.FieldType)));
        }
    }
}
