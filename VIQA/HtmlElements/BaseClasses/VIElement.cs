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
                    : _locator.FillByTemplate(TemplateId);
            }
        }

        public IWebDriver WebDriver { get { return Site.WebDriver; } }
        public IWebDriverTimeouts Timeouts { get { return Site.WebDriverTimeouts; }}
        private IWebElement _webElement;
        private IWebElement WebElement { set
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

        protected int WaitTimeoutInSec { get { return _waitTimeoutInSec ?? _defaultWaitTimeoutInSec; } }
        protected static Action PreviousClickAction;
        
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
            while ((FoundElements == null || !FoundElements.Any()) && !timer.TimeoutPassed(timeout));
            return FoundElements.Any();
        }

        public bool IsPresent
        {
            get
            {
                WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                var elements = SearchContext.FindElements(Locator);
                WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Site.WebDriverTimeouts.WaitWebElementInSec));
                return elements.Count > 0; 
            }
        }

        public bool IsDisplayed
        {
            get
            {
                WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                var elements = SearchContext.FindElements(Locator);
                WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Site.WebDriverTimeouts.WaitWebElementInSec));
                return elements.Count > 0 && CheckWebElementIsUnique(elements).Displayed;
            }
        }
        public bool WaitDisplayed
        {
            get
            {            
                IWebElement webElement;
                var timer = new Timer();
                do { webElement = GetWebElement(); }
                while (!webElement.Displayed && !timer.TimeoutPassed(WaitTimeoutInSec));
                return webElement.Displayed;
            }
        }

        public int CashDropTime { get; set; }

        private void IsClearCashNeeded()
        {
            if (Site.SiteSettings.UseCache)
            {
                if (CashDropTime != Site.SiteSettings.CashDropTimes) 
                    DropCache();
                return;
            }
            _webElement = null;
        }

        private void DropCache()
        {
            CashDropTime = Site.SiteSettings.CashDropTimes;
            _webElement = null;
        }

        public IWebElement GetWebElement()
        {
            IsClearCashNeeded();
            if (WebElement != null)
                return WebElement;
            var timeoutInSec = WaitTimeoutInSec;
            if (!string.IsNullOrEmpty(OpenPageName))
            {
                timeoutInSec = Timeouts.WaitPageToLoadInSec;
                Site.CheckPage(OpenPageName);
                OpenPageName = null;
            }
            if (!WaitWebElement(timeoutInSec * 1000))
            {
                if (PreviousClickAction != null)
                try
                {
                    PreviousClickAction();
                    WaitWebElement(timeoutInSec);
                    VISite.Logger.Event("Used Click Previous action");
                } catch {WaitWebElement(0);}
            }
            return CheckWebElementIsUnique(FoundElements);
        }

        public IVIElement GetVIElement()
        {
            WebElement = GetWebElement();
            return this;
        }

        #region Constructors

        public VIElement()
        {
            DefaultNameFunc = () => _typeName + " with by selector " + Locator;
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
            try { return (T)VIActionR.Invoke(logActionName, () => viAction(), res => logResult != null ? logResult((T)res) : null); }
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
                viAction.Invoke();
            });
        
        protected void DoVIAction(string logActionName, Action viAction)
        {
            try { DoViAction.Action.Invoke(this, logActionName, viAction); }
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
            { typeof(ILabeled), typeof(TextElement) },
            { typeof(IClickable), typeof(ClickableElement) },
            { typeof(IVIElement), typeof(VIElement) },
            { typeof(IRadioButtons), typeof(RadioButtonses) },
            { typeof(IDropDown), typeof(DropDown) },
        };

        public static readonly Func<string, string, bool> DefaultCompareFunc = (a, e) => a == e;

        protected static string OpenPageName;
    }
}
