using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;
using Timer = VIQA.Common.Timer;

namespace VIQA.HtmlElements
{
    public class VIElement : VIElementsList, IVIElement
    {
        public ISearchContext SearchContext
        {
            get
            {
                if (Context == null)
                    return WebDriver;
                var elements = WebDriver.FindElements(Context);
                return (elements.Count == 1) ? elements.First() : null;
            }
        }

        public By Locator { 
            set { _locator = value; }
            get
            {
                return string.IsNullOrEmpty(TemplateId) 
                    ? _locator 
                    : _locator.GetByFunc().Invoke(string.Format(_locator.GetByLocator(), TemplateId));
            }
        }

        public IWebDriver WebDriver { get { return Site.WebDriver; } }
        private VISite _site;
        private static VISite _defaultSite { set; get; }
        public VISite Site { set { _site = value; } get { return _site ?? _defaultSite; } }
        public IWebDriverTimeouts Timeouts { get { return Site.WebDriverTimeouts; }}

        protected IWebElement WebElement;

        private int _defaultWaitTimeoutInSec { get { return Timeouts.WaitWebElementInSec; } }
        
        #region Temp Settings
        private int? _waitTimeoutInSec = null;
        public string TemplateId;

        private void ClearTempSettings()
        {
            TemplateId = null;
            _waitTimeoutInSec = _defaultWaitTimeoutInSec;
        }

        public T WaitTimeout<T>(int timeoutInSec) where T : VIElement
        {
            _waitTimeoutInSec = timeoutInSec;
            return (T)this;
        }

        #endregion


        protected int WaitTimeoutInSec
        {
            get
            {
                if (!NextActionNeedWaitPageToLoad) 
                    return _waitTimeoutInSec ?? _defaultWaitTimeoutInSec;
                NextActionNeedWaitPageToLoad = false;
                return Timeouts.WaitPageToLoadInSec;
            }
        }

        public bool WithPageLoadAction = false;
        protected static bool NextActionNeedWaitPageToLoad;
        protected static Action PreviousClickAction = null;
        
        protected virtual string _typeName { get { return "Element type undefined"; } }
        public string FullName { get { return (Name != null) ? (_typeName + " " + Name) : Name;} }

        private IWebElement CheckWebElementIsUnique(ReadOnlyCollection<IWebElement> webElements)
        {
            if (webElements.Count == 1)
                return WebElement = webElements.First();
            if (webElements.Any())
                throw VISite.Alerting.ThrowError(string.Format("Found {0} {1} elements but expected. Please specify locator", webElements.Count, FullName));
            throw VISite.Alerting.ThrowError((string.Format("Can't found element {0} by selector {1}. Please correct locator", FullName, Locator)));
        }

        private ReadOnlyCollection<IWebElement> FoundElements;
        private bool WaitWebElement(int timeout)
        {
            var firstTime = true;
            var timer = new Timer();
            do { FoundElements = (SearchContext != null)
                    ? SearchContext.FindElements(Locator)
                    : null;
                if (firstTime)
                    firstTime = false;
                else
                    Thread.Sleep(Timeouts.RetryActionInMsec);
            }
            while (FoundElements == null || (!FoundElements.Any() && !timer.TimeoutPassed(timeout)));
            return FoundElements.Any();
        }

        public bool IsPresent { get { return WebDriver.FindElements(Locator).Any(); } }

        public bool IsDisplayed
        {
            get
            {
                var elements = WebDriver.FindElements(Locator);
                return elements.Count != 0 && CheckWebElementIsUnique(elements).Displayed;
            }
        }

        public int CashDropTimes;

        private void IsClearCashNeeded()
        {
            if (Site.UseCache) {
                if (CashDropTimes == Site.CashDropTimes) return;
                CashDropTimes = Site.CashDropTimes;
            }
            WebElement = null;
        }

        public IWebElement GetWebElement()
        {
            IsClearCashNeeded();
            if (WebElement != null)
                return WebElement;
            var timeout = WaitTimeoutInSec * 1000;
            if (WithPageLoadAction)
                NextActionNeedWaitPageToLoad = true;
            if (!WaitWebElement(timeout))
            {
                if (PreviousClickAction != null)
                try
                {
                    PreviousClickAction.Invoke();
                    WaitWebElement(timeout);
                    VISite.Logger.Event("Used Click Previous action");
                } catch {WaitWebElement(0);}
            }
            ClearTempSettings();
            return CheckWebElementIsUnique(FoundElements);
        }

        public VIElement() { DefaultNameFunc = () => _typeName + " with by selector " + Locator; }
        public VIElement(string name) { Name = name; }
        public VIElement(string name, string cssSelector) : this(name) { Locator = By.CssSelector(cssSelector); }
        public VIElement(string name, By byLocator) : this(name) { Locator = byLocator; }
        public VIElement(By byLocator) : this() { Locator = byLocator; }
        public VIElement(string name, IWebElement webElement) : this(name) { WebElement = webElement; }
        public VIElement(IWebElement webElement) : this("", webElement) { }

        public static void Init(VISite site) { _defaultSite = site; }

        public string DefaultLogMessage(string text)
        {
            return text + string.Format(" (Name: {0}, Type: {1}, Locator: {2})", FullName, _typeName, Locator);
        }

        private Func<string, Func<Object>, Func<Object, string>, Object> _defaultViActionR
        {
            get
            {
                return (text, viAction, logResult) =>
                {
                    VISite.Logger.Event(DefaultLogMessage(text));
                    var result = viAction.Invoke();
                    if (logResult != null)
                        VISite.Logger.Event(logResult.Invoke(result));
                    return result;
                }; } }

        private Func<string, Func<Object>, Func<Object, string>, Object> _viActionR;
        public Func<string, Func<Object>, Func<Object, string>, Object> VIActionR
        {
            set { _viActionR = value; }
            get { return _viActionR ?? _defaultViActionR; }
        }
        
        protected T DoVIAction<T>(string logActionName, Func<T> viAction, Func<T, string> logResult = null)
        {
            return (T) VIActionR.Invoke(logActionName, () => viAction(), res => logResult((T)res));
        }

        //----
        public VIAction<Action<VIElement, string, Action>> DoViAction = new VIAction<Action<VIElement, string, Action>>(
            (viElement, text, viAction) =>
            {
                VISite.Logger.Event(viElement.DefaultLogMessage(text));
                viAction.Invoke();
            });
        
        protected void DoVIAction(string logActionName, Action viAction)
        {
            DoViAction.Action.Invoke(this, logActionName, viAction);
        }
    }
}
