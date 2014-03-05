using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HAttributes;

namespace VIQA.SiteClasses
{
    public class VIPage : VISection
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

        public bool CheckUrl { get; set; }

        public bool CheckTitle { get; set; }

        private new By RootLocator;

        public VIPage() { }

        public new VIPage InitFromAttr(FieldInfo pageField, VISection parentSection)
        {
            var page = (VIPage) base.InitFromAttr(pageField, parentSection);
            page.DefaultNameFunc = () => string.Format("Page with Title: '{0}', Url: '{1}'", Title ?? "", Url ?? "");
            var pageAttr = pageField.GetCustomAttribute<PageAttribute>(false);

            if (pageAttr == null)
            {
                page.Url = "";
                page.Title = "";
                page.CheckUrl = false;
                page.CheckTitle = false;
            }
            else
            {
                page.Url = pageAttr.Url;
                page.Title = pageAttr.Title;
                page.CheckUrl = pageAttr.CheckUrl;
                page.CheckTitle = pageAttr.CheckTitle;
            }
            page.FillMetaFromClass();
            return page;
        }

        public VIPage(string name, string url = null, string title = null, bool checkUrl = true, bool checkTitle = true, VISite site = null)
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
            CheckUrl = checkUrl;
            CheckTitle = checkTitle;
            
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
                if (CheckUrl && !pageAttr.CheckUrl)
                    CheckUrl = pageAttr.CheckUrl;
                if (CheckTitle && !pageAttr.CheckTitle)
                    CheckTitle = pageAttr.CheckTitle;
            }
            
        }

        public void Open()
        {
            Site.Logger.Event("Open page: " + Url);
            Site.WebDriver.Navigate().GoToUrl(Url);
            Site.WindowHandle = WebDriver.WindowHandles.First();
            if (CheckUrl) DoUrlCheck();
            if (CheckTitle) DoTitleCheck();
        }

        public void DoUrlCheck()
        {
            if (string.IsNullOrEmpty(Url)) return;
            var timer = new Timer();
            while (!(WebDriver.Url.Contains(Url) || timer.TimeoutPassed(Site.WebDriverTimeouts.WaitPageToLoadInSec * 1000))) { }
        }
        
        public void DoTitleCheck()
        {
            if (string.IsNullOrEmpty(Title)) return;
            var timer = new Timer();
            while (!(WebDriver.Title.Contains(Title) || timer.TimeoutPassed(Site.WebDriverTimeouts.WaitPageToLoadInSec * 1000))) { }
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
                ((VIPage) Activator.CreateInstance(page.FieldType))
                    .InitFromAttr(page, null)));
        }
    }
}
