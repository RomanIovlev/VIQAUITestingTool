using System;
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
            set { _url = GetUrlValue(value, Site); }
            get { return _url; }
        }

        public static string GetUrlValue(string url, VISite site)
        {
            return (string.IsNullOrEmpty(url))
                    ? site.Domain
                    : (url.Contains("http://"))
                        ? url
                        : site.Domain + url.TrimStart('/');
        }

        public string Title { get; set; }
        public bool IsHomePage { get; set; }

        public PageCheckType UrlCheckType { get; set; }
        public PageCheckType TitleCheckType { get; set; }

        private void SetEmptyPage()
        {
            if (IsSiteSet)
                Url = "";
            else
                _url = "";
            Title = "";
            UrlCheckType = PageCheckType.NoCheck;
            TitleCheckType = PageCheckType.NoCheck;
            IsHomePage = false;
        }

        public VIPage() 
        {
            DefaultNameFunc = () => string.Format("Page with Title: '{0}', Url: '{1}'", Title ?? "", Url ?? "");
            SetEmptyPage();
        }
        
        public void FillFromPageAttribute(PageAttribute pageAttr)
        {
            if (pageAttr == null) return;
            Url = pageAttr.Url;
            Title = pageAttr.Title;
            UrlCheckType = pageAttr.UrlCheckType;
            TitleCheckType = pageAttr.TitleCheckType;
            IsHomePage = pageAttr.IsHomePage;
        }

        public void UpdatePageAttribute(PageAttribute pageAttr)
        {
            if (pageAttr == null) return;
            if (!string.IsNullOrEmpty(pageAttr.Url))
                Url = pageAttr.Url;
            if (!string.IsNullOrEmpty(pageAttr.Title))
                Title = pageAttr.Title;
            if (pageAttr.IsCheckUrlSetManual)
                UrlCheckType = pageAttr.UrlCheckType;
            if (pageAttr.IsCheckTitleSetManual)
                TitleCheckType = pageAttr.TitleCheckType;
            if (pageAttr.IsHomePage)
                IsHomePage = true;
        }
        
        public VIPage(string name, string url = null, string title = null, VISite site = null)
        {
            DefaultNameFunc = () => string.Format("Page with Title: '{0}', Url: '{1}'", Title ?? "", Url ?? "");
            if (!string.IsNullOrEmpty(name))
                Name = name;
            else
            {
                name = NameAttribute.GetName(this);
                if (!string.IsNullOrEmpty(name))
                    Name = name;
            }
            if (site != null)
                Site = site;
            var pageAttr = PageAttribute.Handler(this);
            if (pageAttr != null)
                FillFromPageAttribute(pageAttr);
            else
                SetEmptyPage();
            if (!string.IsNullOrEmpty(url)) 
                Url = url;
            if (!string.IsNullOrEmpty(title))
                Title = title;
        }

        public void Open()
        {
            Site.Navigate.OpenPage(this);
            VerifyPage(true);
        }

        public bool VerifyPage(bool throwError = false)
        {
            return CheckUrl(UrlCheckType, throwError) && CheckTitle(TitleCheckType, throwError);
        }

        public bool CheckUrl(PageCheckType checkType, bool throwError = false)
        {
            if (checkType == PageCheckType.NoCheck) return true;
            if (string.IsNullOrEmpty(Url))
            {
                VISite.Alerting.ThrowError(string.Format("Page '{0}' url is empty. Please set Url for this page", Name));
                return false;
            }
            VISite.Logger.Event(string.Format("Check page '{0}' url {1} '{2}'", Name, checkType == PageCheckType.Equal ? "equal to " : "contains", Url));
            var timer = new Timer();
            while (!((checkType == PageCheckType.Equal) ? WebDriver.Url == Url : WebDriver.Url.Contains(Url)))
                if (timer.TimeoutPassed(Site.WebDriverTimeouts.WaitPageToLoadInSec*1000))
                {
                    var errorMsg = string.Format("Failed to check page url'{0}'." + 
                        "Actual: '{1}'".FromNewLine() + 
                        "Expected: '{2}'".FromNewLine() +
                        "CheckType: " + checkType, Name, WebDriver.Url, Url);
                    if (throwError)
                        throw VISite.Alerting.ThrowError(errorMsg);
                    VISite.Logger.Error(errorMsg);
                    return false;
                }
            return true;
        }

        public bool CheckTitle(PageCheckType checkType, bool throwError = false)
        {
            if (checkType == PageCheckType.NoCheck) return true;
            if (string.IsNullOrEmpty(Title))
            {
                VISite.Alerting.ThrowError(string.Format("Page '{0}' title is empty. Please set Title for this page", Name));
                return false;
            }
            VISite.Logger.Event(string.Format("Check page '{0}' title {1} '{2}'", Name, checkType == PageCheckType.Equal ? "equal to " : "contains", Title));
            var timer = new Timer();
            while (!((checkType == PageCheckType.Equal) ? WebDriver.Title == Title : WebDriver.Title.Contains(Title)))
                if (timer.TimeoutPassed(Site.WebDriverTimeouts.WaitPageToLoadInSec * 1000))
                {
                    var errorMsg = string.Format("Failed to check page title '{0}'." +
                        "Actual: '{1}'".FromNewLine() +
                        "Expected: '{2}'".FromNewLine() +
                        "CheckType: " + checkType, Name, WebDriver.Title, Title);
                    if (throwError)
                        throw VISite.Alerting.ThrowError(errorMsg);
                    VISite.Logger.Error(errorMsg);
                    return false;
                }
            return true;
        }

        public Navigation Navigate { get { return Site.Navigate; } } 

        public IWebDriver WebDriver { get { return Site.WebDriver; } }

        public static void Init(VISite site)
        {
            site.PagesFields.ForEach(
                page =>
                {
                    var instance = (VIPage)Activator.CreateInstance(page.FieldType);
                    instance.Site = site;

                    var pageAttr = PageAttribute.Handler(instance);
                    if (pageAttr == null)
                        instance.SetEmptyPage();
                    else
                        instance.FillFromPageAttribute(pageAttr);

                    pageAttr = PageAttribute.Handler(page);
                    if (pageAttr != null)
                        instance.UpdatePageAttribute(pageAttr);

                    page.SetValue(site, instance);

                    instance.InitSubElements();
                });
        }
    }
}
