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
                        : site.Domain.TrimEnd('/') + "/"+ url.TrimStart('/');
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
            return CheckPageAttribute(checkType, throwError, "url", WebDriver.Url.TrimEnd('/'), Url.TrimEnd('/'));
        }
        
        public bool CheckTitle(PageCheckType checkType, bool throwError = false)
        {
            return CheckPageAttribute(checkType, throwError, "title", WebDriver.Title, Title);
        }
        private bool CheckPageAttribute(PageCheckType checkType, bool throwError, string checkWhat, string actual, string expected)
        {
            if (checkType == PageCheckType.NoCheck) return true;
            if (string.IsNullOrEmpty(expected))
            {
                VISite.Alerting.ThrowError(string.Format("Page '{0}' {1} is empty. Please set {1} for this page", Name, checkWhat));
                return false;
            }
            VISite.Logger.Event(string.Format("Check page '{0}' {1} {2} '{3}'", Name, checkWhat, checkType == PageCheckType.Equal ? "equal to " : "contains", expected));
            var result =
                // new Timer(Site.WebDriverTimeouts.WaitPageToLoadInSec, Site.WebDriverTimeouts.RetryActionInMsec).Wait(
                //() => 
                    (checkType == PageCheckType.Equal) 
                    ? actual == expected 
                    : actual.Contains(expected);

            if (result) return true;
            var errorMsg = string.Format("Failed to check page {0} '{1}'." +
                "Actual: '{2}'".FromNewLine() +
                "Expected: '{3}'".FromNewLine() +
                "CheckType: '{4}'", checkWhat, Name, actual, expected, checkType);
            if (throwError)
                throw VISite.Alerting.ThrowError(errorMsg);
            VISite.Logger.Error(errorMsg);
            return false;
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
