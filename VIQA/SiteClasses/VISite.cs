using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
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
        public BrowserType BrowserType;
        private IWebDriverTimeouts _webDriverTimeouts;
        public static ILogger Logger;
        public static IAlerting Alerting;

        public IWebDriverTimeouts WebDriverTimeouts {
            get { return _webDriverTimeouts ?? new DefaultWebDriverTimeouts(); }
            set { _webDriverTimeouts = value; }
        }

        public int CashDropTimes = -1;
        public bool UseCache = true;

        public string WindowHandle;
        private string _domain;
        public string Domain { 
            set
            {
                _domain = (string.IsNullOrEmpty(value))
                    ? "/"
                    : value.TrimEnd('/') + "/";
            } get { return _domain; } 
        }

        public readonly Func<IWebDriver> WebDriverFunc;
        private IWebDriver _webDriver;

        public IWebDriver WebDriver
        {
            get { return _webDriver ?? RunWebDriver(); }
        }

        public IWebDriver RunWebDriver()
        {
            _webDriver = WebDriverFunc.Invoke();
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
            var site = SiteAttribute.Get(this);
            if (site != null)
            {
                if (site.Domain != null)
                    Domain = site.Domain;
                if (isMain)
                    isMain = site.IsMain;
                if (site.UseCache)
                    UseCache = site.UseCache;
            }
            if (!isMain) return;
            ;
            VIElement.Init(this);
            VIPage.Init(this);
        }
        
        public void OpenPage(string uri)
        {
            new VIPage(Name, uri, site: this).Open();
        }

        public void OpenHomePage()
        {
            OpenPage("");
        }
        
        
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
