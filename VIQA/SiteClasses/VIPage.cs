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

        public bool IsUrlCheckContainsNeeded { get; set; }
        public bool IsTitleCheckContainsNeeded { get; set; }
        public bool IsUrlCheckEqualNeeded { get; set; }
        public bool IsTitleCheckEqualNeeded { get; set; }

        public VIPage() 
        {
            DefaultNameFunc = () => string.Format("Page with Title: '{0}', Url: '{1}'", Title ?? "", Url ?? "");
            var pageAttr = GetType().GetCustomAttribute<PageAttribute>(false);

            if (pageAttr == null)
            {
                Url = "";
                Title = "";
                IsUrlCheckContainsNeeded = false;
                IsTitleCheckContainsNeeded = false;
                IsUrlCheckEqualNeeded = false;
                IsTitleCheckEqualNeeded = false;
            }
            else FillFromPageAttribute(pageAttr);
            FillMetaFromClass();         
        }

        public VIPage(PageAttribute pageAttributeField)
        {
            DefaultNameFunc = () => string.Format("Page with Title: '{0}', Url: '{1}'", Title ?? "", Url ?? "");
            var pageAttrClass = GetType().GetCustomAttribute<PageAttribute>(false);

            if (pageAttrClass == null && pageAttributeField == null)
            {
                Url = "";
                Title = "";
                IsUrlCheckContainsNeeded = false;
                IsTitleCheckContainsNeeded = false;
                IsUrlCheckEqualNeeded = false;
                IsTitleCheckEqualNeeded = false;
            }
            else 
                if (pageAttributeField ==null)
                    FillFromPageAttribute(pageAttrClass);
                else
                    FillFromPageAttribute(pageAttributeField);
            FillMetaFromClass();
        }
        public void FillFromPageAttribute(PageAttribute pageAttr)
        {
            Url = pageAttr.Url;
            Title = pageAttr.Title;
            IsUrlCheckContainsNeeded = (!string.IsNullOrEmpty(Url)) && pageAttr.IsUrlCheckContainsNeeded;
            IsTitleCheckContainsNeeded = (!string.IsNullOrEmpty(Title)) && pageAttr.IsTitleCheckContainsNeeded;
        }
        
        public VIPage(string name, string url = null, string title = null, VISite site = null)
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
                IsUrlCheckContainsNeeded = false;
                IsTitleCheckContainsNeeded = false;
                IsUrlCheckEqualNeeded = false;
                IsTitleCheckEqualNeeded = false;
                if (IsUrlCheckContainsNeeded && !pageAttr.IsUrlCheckContainsNeeded)
                    IsUrlCheckContainsNeeded = pageAttr.IsUrlCheckContainsNeeded;
                if (IsTitleCheckContainsNeeded && !pageAttr.IsTitleCheckContainsNeeded)
                    IsTitleCheckContainsNeeded = pageAttr.IsTitleCheckContainsNeeded;
                if (IsUrlCheckEqualNeeded && !pageAttr.IsUrlCheckEqualNeeded)
                    IsUrlCheckEqualNeeded = pageAttr.IsUrlCheckEqualNeeded;
                if (IsTitleCheckEqualNeeded && !pageAttr.IsTitleCheckEqualNeeded)
                    IsTitleCheckEqualNeeded = pageAttr.IsTitleCheckEqualNeeded;
            }
        }

        public void Open()
        {
            VISite.Logger.Event("Open page: " + Url);
            Site.WebDriver.Navigate().GoToUrl(Url);
            Site.WindowHandle = WebDriver.WindowHandles.First();
            Site.CashDropTimes++;
            if (IsUrlCheckEqualNeeded) 
                CheckUrl(true);
            else if (IsUrlCheckContainsNeeded)
                CheckUrl(false);
            if (IsTitleCheckEqualNeeded)
                CheckTitle(true);
            else if (IsTitleCheckContainsNeeded)
                CheckUrl(false);
        }

        public bool CheckUrl(bool checkEqual)
        {
            VISite.Logger.Event(string.Format("Check url {0} for page '{1}'", Url, Name));
            if (string.IsNullOrEmpty(Url))
            {
                VISite.Alerting.ThrowError(string.Format("Can't check url {0} for page '{1}'. Please set Expected value", Url, Name));
                return false;
            }
            var timer = new Timer();
            while (!((checkEqual) ? WebDriver.Url == Url : WebDriver.Url.Contains(Url)))
                if (timer.TimeoutPassed(Site.WebDriverTimeouts.WaitPageToLoadInSec*1000))
                {
                    VISite.Alerting.ThrowError(string.Format("Can't check url {0} for page '{1}'", Url, Name));
                    return false;
                }
            return true;
        }

        public bool CheckTitle(bool checkEqual)
        {
            VISite.Logger.Event(string.Format("Check title {0} for page '{1}'", Title, Name));
            if (string.IsNullOrEmpty(Title))
            {
                VISite.Alerting.ThrowError(string.Format("Can't check title {0} for page '{1}'. Please set Expected value", Title, Name));
                return false;
            }
            var timer = new Timer();
            while (!((checkEqual) ? WebDriver.Title == Title : WebDriver.Title.Contains(Title)))
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
            fields.ForEach(
                page =>
                {
                    var pageAttribute = PageAttribute.Handler(page);
                    var instance = (VIPage)Activator.CreateInstance(page.FieldType);
                    if (pageAttribute != null) 
                        instance.FillFromPageAttribute(pageAttribute);
                    page.SetValue(site, instance);
                    instance.InitSubElements();
                });
        }
    }
}
