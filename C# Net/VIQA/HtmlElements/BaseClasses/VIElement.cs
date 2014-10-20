using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.Common.Pairs;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;
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

        public string Frame { set { Context.Add(ContextType.Frame, By.Id(value));} }
        
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
                if (OpenPageName == null) 
                    return _waitTimeoutInSec ?? DefaultWaitTimeoutInSec;
                if (OpenPageName != "")
                    Site.CheckPage(OpenPageName);
                OpenPageName = null;
                return Timeouts.WaitPageToLoadInSec;
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
            return string.Format("Find {0} elements '{1}' but expected 1. Please correct locator '{2}'", webElements.Count, Name, PrintLocator()); } 
        private string CantFindElementMessage { get {
            return string.Format("Can't find element '{0}' by selector '{1}'. Please correct locator", Name, PrintLocator()); } }
        
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
            return DoVIActionResult("Wait element State", 
                () => GetTimer(timeoutInSec, retryTimeoutInMSec)
                    .Wait(() => waitFunc(webElement ?? CheckWebElementIsUnique(SearchElements()))), 
                result => result.ToString());
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

        public bool IsPresent
        {
            get
            {
                return DoVIActionResult("Is Element Present", () =>
                {
                    try {
                        WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                        var elements = SearchElements();
                        WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Site.WebDriverTimeouts.WaitWebElementInSec));
                        return (elements.Count == 1);
                    }
                    catch { return false; }
                }, result => result.ToString());
            }
        }

        public bool IsDisplayed
        {
            get
            {
                return DoVIActionResult("Is Element Displayed", () =>
                {
                    try {
                        WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                        var elements = DoVIActionResult("Is Displayed Present", () => SearchElements(), els => "Find " + els.Count() + " element(s)");
                        WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Site.WebDriverTimeouts.WaitWebElementInSec));
                        return (elements.Count == 1 && elements.First().Displayed);
                    }
                    catch { return false; }
                }, result => result.ToString());
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

        public VIElement(string name) { Name = name; }
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


        #region ViActions
        // Result Actions
        public static Func<VIElement, string, Func<Object>, Func<Object, string>, Object> DefaultViActionResult = 
            (viElement, text, viAction, logResult) =>
                {
                    VISite.Logger.Event(viElement.DefaultLogMessage(text));
                    var result = viAction();
                    var demoModeSettings = viElement.Site.SiteSettings.DemoSettings;
                    if (demoModeSettings != null)
                        viElement.Highlight(demoModeSettings);
                    var strResult = logResult(result);
                    if (strResult != null)
                        VISite.Logger.Event(strResult);
                    return result;
                }; 

        private Func<VIElement, string, Func<Object>, Func<Object, string>, Object> _viActionResult;
        public Func<VIElement, string, Func<Object>, Func<Object, string>, Object> VIActionResult
        {
            set { _viActionResult = value; }
            get { return _viActionResult ?? DefaultViActionResult; }
        }

        public T DoVIActionResult<T>(string logActionName, Func<T> viAction, Func<T, string> logResult = null)
        {
            try { return (T)VIActionResult(this, logActionName, () => viAction(), res => logResult != null ? logResult((T)res) : null); }
            catch (Exception ex)
            {
                throw VISite.Alerting.ThrowError(string.Format("Failed to do '{0}' action. Exception: {1}", logActionName, ex));
            }
        }

        // NoResult Actions
        public static Action<VIElement, string, Action> DefaultVIAction = 
            (viElement, text, viAction) =>
            {
                VISite.Logger.Event(viElement.DefaultLogMessage(text));
                viAction();
                var demoMode = viElement.Site.SiteSettings.DemoSettings;
                if (demoMode != null)
                    viElement.Highlight(demoMode);
            };

        private Action<VIElement, string, Action> viAction;
        public Action<VIElement, string, Action> VIAction
        {
            set { viAction = value; }
            private get { return viAction ?? DefaultVIAction; }
        }

        public void DoVIAction(string logActionName, Action viAction)
        {
            try { VIAction(this, logActionName, viAction); }
            catch (Exception ex)
            {
                throw VISite.Alerting.ThrowError(string.Format("Failed to do '{0}' action. Exception: {1}", logActionName, ex));
            }
        }
        #endregion

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

        public void SetAttribute(string attributeName, string value)
        {
            var javascript = WebDriver as IJavaScriptExecutor;
            if (javascript != null)
                javascript.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])", GetWebElement(), attributeName, value);
        }

        public void Highlight(string bgColor = "yellow", string frameColor = "red", int timeoutInSec = 1)
        {
            Highlight(new HighlightSettings(bgColor, frameColor, timeoutInSec));
        }

        public void Highlight(HighlightSettings highlightSettings)
        {
            highlightSettings = highlightSettings ?? new HighlightSettings();
            var orig = GetWebElement().GetAttribute("style");
            SetAttribute("style", string.Format("border: 3px solid {0}; background-color: {1};", highlightSettings.FrameColor, highlightSettings.BgColor));
            Thread.Sleep(highlightSettings.TimeoutInSec * 1000);
            SetAttribute("style", orig);
        }
    }
}
