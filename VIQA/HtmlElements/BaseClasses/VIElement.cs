using System;
using System.Collections.Generic;
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
    public class VIElement : VIElementsSet, IVIElement
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
        private IWebElement _webElement;
        public IWebElement WebElement { set
        {
            DropCache(); _webElement = value; }
            get { return _webElement; }
        }

        private int _defaultWaitTimeoutInSec { get { return Timeouts.WaitWebElementInSec; } }
        
        #region Temp Settings
        public void SetWaitTimeout(int waitTimeoutInSec) { _waitTimeoutInSec = waitTimeoutInSec; }
        private int? _waitTimeoutInSec = null;
        private string _templateId;
        public string TemplateId
        {
            set { DropCache(); _templateId = value; }
            private get { return _templateId;  }}


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

        public bool WithPageLoadAction { get; set; }
        protected static bool NextActionNeedWaitPageToLoad;
        protected static Action PreviousClickAction = null;
        
        protected virtual string _typeName { get { return "Element type undefined"; } }
        public string FullName { get { return Name ?? _typeName + " with undefiened Name";} }

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
                WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                var elements = WebDriver.FindElements(Locator);
                WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Site.WebDriverTimeouts.WaitWebElementInSec));
                return elements.Count != 0 && CheckWebElementIsUnique(elements).Displayed;
            }
        }

        public int CashDropTime { get; set; }

        private void IsClearCashNeeded()
        {
            if (Site.UseCache) {
                if (CashDropTime != Site.CashDropTimes) 
                    DropCache();
                return;
            }
            _webElement = null;
        }

        private void DropCache()
        {
            CashDropTime = Site.CashDropTimes;
            _webElement = null;
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
            //ClearTempSettings();
            return CheckWebElementIsUnique(FoundElements);
        }

        public IVIElement GetVIElement()
        {
            WebElement = GetWebElement();
            return this;
        }

        public VIElement()
        {
            WithPageLoadAction = false;
            DefaultNameFunc = () => _typeName + " with by selector " + Locator;
        }

        public VIElement(string name)
        {
            WithPageLoadAction = false;
            Name = name;
        }

        public VIElement(string name, string cssSelector) : this(name) { Locator = By.CssSelector(cssSelector); }
        public VIElement(string name, By byLocator) : this(name) { Locator = byLocator; }
        public VIElement(By byLocator) : this() { Locator = byLocator; }
        public VIElement(string name, IWebElement webElement) : this(name) { WebElement = webElement; }
        public VIElement(IWebElement webElement) : this("", webElement) { }

        public static void Init(VISite site) { _defaultSite = site; }

        public string DefaultLogMessage(string text)
        {
            return text + string.Format(" (Name: '{0}', Type: '{1}', Locator: '{2}')", FullName, _typeName, Locator);
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

        public static Dictionary<Type, Type> InterfaceTypeMap = new Dictionary<Type, Type>
        {
            { typeof(IButton), typeof(Button) },
            { typeof(ICheckbox), typeof(Checkbox) },
            { typeof(ICheckList), typeof(CheckList) },
            { typeof(ILink), typeof(Link) },
            { typeof(ITextField), typeof(TextField) },
            { typeof(ITextArea), typeof(TextArea) },
            { typeof(ILabeled), typeof(TextElement) },
            { typeof(IClickable), typeof(ClickableElement) },
        };

        public static Func<string, string, bool> DefaultCompareFunc = (a, e) => a == e;
    }
}
