using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.Common.Pairs;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;
using VIQA.SiteClasses;

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
                ISearchContext element = WebDriver.SwitchTo().DefaultContent();
                foreach (var locator in Context)
                {
                    var elements = element.FindElements(locator.Value2);
                    element = CheckWebElementIsUnique(elements);
                    if (locator.Value1 == ContextType.Locator) continue;
                    WebDriver.SwitchTo().Frame((IWebElement) element);
                    element = WebDriver;
                }
                return element;
            }
        }
        
        private string PrintLocator()
        {
            return string.Format("Context: '{0}'. Locator: '{1}'", 
                Context != null ? Context.ToPairs(el => el.Value1.ToString(), el => el.Value2).Print() : "null", 
                _locator != null ? _locator.ToString() : "null");
        }

        public By Locator { 
            set { _locator = value; }
            get
            {
                var locator = string.IsNullOrEmpty(TemplateId)
                    ? _locator
                    : _locator.FillByTemplate(TemplateId);
                if (locator != null)
                    return locator;
                throw VISite.Alerting.ThrowError(DefaultLogMessage("LocatorAttribute cannot be null"));
            }
        }

        protected Timer Timer { get { return new Timer(WaitTimeoutInSec * 1000, Site.WebDriverTimeouts.RetryActionInMsec); } }

        public bool HaveLocator() { return _locator != null; }

        public IWebDriver WebDriver { get { return Site.WebDriver; } }
        public IWebDriverTimeouts Timeouts { get { return Site.WebDriverTimeouts; }}
        private IWebElement _webElement;
        private IWebElement WebElement { set
        {
            DropCache(); _webElement = value; }
            get { return _webElement; }
        }

        private int DefaultWaitTimeoutInSec { get { return Timeouts.WaitWebElementInSec; } }
        
        #region Temp Settings
        public void SetWaitTimeout(int waitTimeoutInSec) { _waitTimeoutInSec = waitTimeoutInSec; }
        private int? _waitTimeoutInSec = null;
        private string _templateId;
        public string TemplateId
        {
            set { DropCache(); _templateId = value; }
            private get { return _templateId;  }}


        #endregion

        private int WaitTimeoutInSec
        {
            get
            {
                if (OpenPageName != null)
                {
                    if (OpenPageName != "")
                        Site.CheckPage(OpenPageName);
                    OpenPageName = null;
                    return Timeouts.WaitPageToLoadInSec;
                }
                return _waitTimeoutInSec ?? DefaultWaitTimeoutInSec;
            }
        }
        
        protected virtual string _typeName { get { return "Element type undefined"; } }
        public string FullName { get { return Name ?? _typeName + " with undefiened Name";} }

        private IWebElement CheckWebElementIsUnique(ReadOnlyCollection<IWebElement> webElements)
        {
            if (webElements.Count == 1)
                return webElements.First();
            if (webElements.Any())
                throw VISite.Alerting.ThrowError(LotOfFindElementsMessage(webElements));
            throw VISite.Alerting.ThrowError(CantFindElementMessage);
        }
        
        
        private string LotOfFindElementsMessage(ReadOnlyCollection<IWebElement> webElements) {
            return string.Format("Find {0} elements '{1}' but expected. Please correct locator '{2}'", webElements.Count, FullName, PrintLocator()); } 
        private string CantFindElementMessage { get {
            return string.Format("Can't find element '{0}' by selector '{1}'. Please correct locator", FullName, PrintLocator()); } }
        
        public ReadOnlyCollection<IWebElement> SearchElements(By locator = null)
        {
            try { return SearchContext.FindElements(locator ?? Locator); }
            catch { throw VISite.Alerting.ThrowError(CantFindElementMessage);}
        }

        private Timer GetTimer(double timeoutInSec, int retryTimeoutInMSec)
        {
            if (timeoutInSec < 0)
                return Timer;
            return new Timer(timeoutInSec, ((retryTimeoutInMSec >= 0) ? retryTimeoutInMSec : 100));
        }

        public bool WaitElementState(Func<IWebElement, bool> waitFunc, IWebElement webElement = null, double timeoutInSec = -1, int retryTimeoutInMSec = -1)
        {
            return GetTimer(timeoutInSec, retryTimeoutInMSec).Wait(() => waitFunc(webElement ?? CheckWebElementIsUnique(SearchElements())));
        }

        public IWebElement WaitElementWithState(Func<IWebElement, bool> waitFunc, IWebElement webElement = null, double timeoutInSec = -1, int retryTimeoutInMSec = -1, string msg = "")
        {
            if (WaitElementState(waitFunc, webElement, timeoutInSec, retryTimeoutInMSec))
                return WebElement;
            throw VISite.Alerting.ThrowError(msg);

        }

        private ReadOnlyCollection<IWebElement> WaitWebElements()
        {
            ReadOnlyCollection<IWebElement> foundElements = null;
            return Timer.Wait(() => (foundElements = SearchElements()).Any())
                ? foundElements
                : null;
        }

        public virtual bool IsPresent
        {
            get
            {
                try
                {
                    WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                    var elements = SearchElements();
                    WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Site.WebDriverTimeouts.WaitWebElementInSec));
                    CheckWebElementIsUnique(elements);
                    return true; 
                }
                catch { return false; }
            }
        }

        public virtual bool IsDisplayed
        {
            get
            {
                try
                {
                    WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                    var elements = SearchElements();
                    WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Site.WebDriverTimeouts.WaitWebElementInSec));
                    return CheckWebElementIsUnique(elements).Displayed;
                }
                catch { return false; }
            }
        }
        
        public int CashDropTimes { get; set; }

        private void IsClearCashNeeded()
        {
            if (Site.SiteSettings.UseCache)
            {
                if (CashDropTimes != Site.SiteSettings.CashDropTimes) 
                    DropCache();
                return;
            }
            _webElement = null;
        }

        private void DropCache()
        {
            CashDropTimes = Site.SiteSettings.CashDropTimes;
            _webElement = null;
        }

        public IWebElement GetWebElement()
        {
            IsClearCashNeeded();
            if (WebElement != null)
                return WebElement;
            var foundElements = WaitWebElements();
            if (foundElements == null)
                VISite.Alerting.ThrowError(CantFindElementMessage);
            WebElement = CheckWebElementIsUnique(foundElements);
            return WaitElementWithState(el => el.Displayed, WebElement, msg: DefaultLogMessage(("Found element stay invisible.")));
        }
        
        public IVIElement GetVIElement()
        {
            WebElement = GetWebElement();
            return this;
        }

        #region Constructors

        public VIElement()
        {
            DefaultNameFunc = () => _typeName + " with by selector " + PrintLocator();
        }

        public VIElement(string name)
        {
            Name = name;
        }

        public VIElement(string name, string cssSelector) : this(name) { Locator = By.CssSelector(cssSelector); }
        public VIElement(string name, By byLocator) : this(name) { Locator = byLocator; }
        public VIElement(By byLocator) : this() { Locator = byLocator; }
        public VIElement(string name, IWebElement webElement) : this(name) { WebElement = webElement; }
        public VIElement(IWebElement webElement) : this("", webElement) { }
        #endregion
        
        public string DefaultLogMessage(string text)
        {
            return text + string.Format(" (Name: '{0}', Type: '{1}', LocatorAttribute: '{2}')", FullName, _typeName, PrintLocator());
        }

        private Func<string, Func<Object>, Func<Object, string>, Object> _defaultViActionR
        {
            get
            {
                return (text, viAction, logResult) =>
                {
                    VISite.Logger.Event(DefaultLogMessage(text));
                    var result = viAction();
                    if (logResult != null)
                        VISite.Logger.Event(logResult(result));
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
            try { return (T)VIActionR(logActionName, () => viAction(), res => logResult != null ? logResult((T)res) : null); }
            catch (Exception ex)
            {
                throw VISite.Alerting.ThrowError(string.Format("Failed to do '{0}' action. Exception: {1}", logActionName, ex));
            }
        }

        //----
        public VIAction<Action<VIElement, string, Action>> DoViAction = new VIAction<Action<VIElement, string, Action>>(
            (viElement, text, viAction) =>
            {
                VISite.Logger.Event(viElement.DefaultLogMessage(text));
                viAction();
            });
        
        protected void DoVIAction(string logActionName, Action viAction)
        {
            try { DoViAction.Action(this, logActionName, viAction); }
            catch (Exception ex)
            {
                throw VISite.Alerting.ThrowError(string.Format("Failed to do '{0}' action. Exception: {1}", logActionName, ex));
            }
        }

        public static readonly Dictionary<Type, Type> InterfaceTypeMap = new Dictionary<Type, Type>
        {
            { typeof(IButton), typeof(Button) },
            { typeof(ICheckbox), typeof(Checkbox) },
            { typeof(ICheckList), typeof(CheckList) },
            { typeof(ILink), typeof(Link) },
            { typeof(ITextField), typeof(TextField) },
            { typeof(ITextArea), typeof(TextArea) },
            { typeof(IText), typeof(TextElement) },
            { typeof(IClickable), typeof(ClickableElement) },
            { typeof(IVIElement), typeof(VIElement) },
            { typeof(IRadioButtons), typeof(RadioButtons) },
            { typeof(IDropDown), typeof(DropDown) },
            { typeof(ITable), typeof(Table) },
        };

        public static readonly Func<string, string, bool> DefaultCompareFunc = (a, e) => a == e;

        protected static string OpenPageName;

        public static void Init(VISite site)
        {
            OpenPageName = null;
            DefaultSite = site;
        }
    }
}
