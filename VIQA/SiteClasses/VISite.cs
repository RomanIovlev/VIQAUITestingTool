using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using VIQA.Common;
using VIQA.Common.Interfaces;
using VIQA.HAttributes;
using VIQA.HtmlElements;

namespace VIQA.SiteClasses
{
    public class VISite : Named
    {
        public static ILogger Logger;
        public static IAlerting Alerting;

        public IWebDriverTimeouts WebDriverTimeouts {
            get { return SiteSettings.WebDriverTimeouts ?? new DefaultWebDriverTimeouts(); }
            set { SiteSettings.WebDriverTimeouts = value; }
        }

        public readonly SiteSettings SiteSettings;
        
        private string _domain;
        public string Domain { 
            set
            {
                _domain = (string.IsNullOrEmpty(value))
                    ? "/"
                    : value.TrimEnd('/') + "/";
            } get { return _domain; } 
        }

        public Func<IWebDriver> WebDriverFunc;
        public BrowserType UseBrowser { set { WebDriverFunc = GetDriver(value); } }

        private IWebDriver _webDriver;

        public IWebDriver WebDriver
        {
            get { return _webDriver ?? RunWebDriver(); }
        }

        public IWebDriver RunWebDriver()
        {
            _webDriver = WebDriverFunc();
            RunWebDrivers.Add(_webDriver);
            _webDriver.Manage().Window.Maximize();
            _webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(WebDriverTimeouts.WaitWebElementInSec));
            return _webDriver;
        }

        public VISite(BrowserType browserType = BrowserType.Firefox, bool isMain = true) : this(GetDriver(browserType), isMain) { }

        public VISite(Func<IWebDriver> webDriver, bool isMain = true)
        {
            Name = NameAttribute.GetName(this);
            WebDriverFunc = webDriver;
            Logger = Logger ?? new DefaultLogger();
            Alerting = Alerting ?? new DefaultAllert();
            Navigate = new Navigation(this);
            var site = SiteAttribute.Get(this);
            SiteSettings = new SiteSettings();
            if (site != null)
            {
                if (site.Domain != null)
                    Domain = site.Domain;
                if (isMain)
                    isMain = site.IsMain;
                if (site.UseCache)
                    SiteSettings.UseCache = site.UseCache;
            }
            if (!isMain) return;
            VIElementsSet.DefaultSite = this;
            VIPage.Init(this);
        }

        public IEnumerable<FieldInfo> PagesFields {  get  { 
            return GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(_ => typeof(VIPage).IsAssignableFrom(_.FieldType)).ToList();
        } }

        public List<VIPage> Pages { get { return PagesFields.Select(pageField => (VIPage)pageField.GetValue(this)).ToList(); } }

        public void CheckPage(string name)
        {
            VIPage viPage = null;
            if (Pages.Any(page => String.Equals(page.Name, name, StringComparison.CurrentCultureIgnoreCase)))
                viPage = Pages.First(page => String.Equals(page.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (PagesFields.Any(field => String.Equals(field.Name, name, StringComparison.CurrentCultureIgnoreCase)))
                viPage = ((VIPage)PagesFields.First(field => String.Equals(field.Name, name, StringComparison.CurrentCultureIgnoreCase)).GetValue(this));
            if (viPage != null)
                viPage.CheckPage();
            else
                Alerting.ThrowError("Can't check page '" + name + "'. Site have no pages with this name.");
        }

        public VIPage HomePage
        {
            get
            {
                var homePagesCount = Pages.Count(page => page.IsHomePage);
                if (homePagesCount == 1)
                    return Pages.First(page => page.IsHomePage);
                throw Alerting.ThrowError((homePagesCount == 0)
                    ? "Site have no HomePage. Please specify one VIPage as HomePage using attribute IsHomePage"
                    : "Site have more than one HomePage. Please specify only one HomePage. Current HomePages: " + 
                        Pages.Where(page => page.IsHomePage).Select(page => page.Name));
            }
        }

        public Navigation Navigate;
        
        public void Dispose()
        {
            if (WebDriver == null)
                return;
            try
            {
                WebDriver.Quit();
                WebDriver.Dispose();
            }
            catch { }
        }

        public static void DisposeAll()
        {
            RunWebDrivers.ForEach(_ => _.Dispose());
        }

        private static readonly List<IWebDriver> RunWebDrivers = new List<IWebDriver>(); 

        private static Func<IWebDriver> GetDriver(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Firefox:
                    return () => new FirefoxDriver();
                case BrowserType.Chrome:
                    return () => new ChromeDriver("..\\..\\Drivers");
                case BrowserType.IE:
                    return () => new InternetExplorerDriver("..\\..\\Drivers");
                default:
                    return () => new FirefoxDriver();
            }

        }

        #region Kill WebDrivers Windows

        public static void KillAllRunWebDrivers()
        {
            foreach (var proc in Process.GetProcessesByName("chromedriver"))
                KillProcessTree(proc.Id);
            foreach (var proc in from proc in Process.GetProcessesByName("firefox") 
                let cmd = GetProcessCommandLine(proc.Id) where cmd.EndsWith("-foreground") select proc)
                    proc.Kill();

            foreach (var proc in Process.GetProcessesByName("IEDriverServer"))
                KillProcessTree(proc.Id);

        }

        private static string GetProcessCommandLine(int pid)
        {
            using (var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ProcessID=" + pid))
            {
                using (var moc = searcher.Get())
                {
                    foreach (var mo in moc)
                        return mo["CommandLine"].ToString();
                }
            }
            throw new ArgumentException("pid");
        }

        private static void KillProcessTree(int pid)
        {
            using (var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid))
            {
                using (var moc = searcher.Get())
                {
                    foreach (var mo in moc)
                    
                        KillProcessTree(Convert.ToInt32(mo["ProcessID"]));
                    try
                    {
                        var proc = Process.GetProcessById(pid);
                        proc.Kill();
                    }
                    catch (ArgumentException)
                    {
                        // Process already exited
                    }
                }
            }
        }
        #endregion

    }

    public enum BrowserType
    {
        Firefox,
        IE,
        Chrome
    }
}
