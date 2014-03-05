using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.Common.Interfaces;
using VIQA.HAttributes;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;
using Timer = VIQA.Common.Timer;

namespace VIQA.HtmlElements
{
    public class VIElement : IVIElement
    {
        public static void Activate(VISection root, FieldInfo field )
        {
            var name = root.Name + ". " + NameAttribute.GetName(field);
            var locator = LocateAttribute.GetLocator(field) ?? By.CssSelector("");
            var additionalArrgs = root.Context.Any() ? new Object[] { name, locator, root.Context } : new Object[] { name, locator, null };
            var fieldType = (!field.FieldType.IsAbstract) 
                ? field.FieldType
                : field.GetValue(root).GetType();
            var a = Activator.CreateInstance(fieldType, additionalArrgs);
            field.SetValue(root, a);
        }
        
        private string _name;
        public string Name { set { _name = value; } get { return _name ?? (_typeName + "with by selector" + Locator); } }

        public List<By> Context;
        public ISearchContext SearchContext
        {
            get
            {
                ISearchContext searchContext = WebDriver;
                if (Context != null)
                    foreach (var locator in Context)
                    {
                        var el = searchContext.FindElements(locator);
                        if (el.Count() == 1)
                            searchContext = el.First();
                        else 
                            return null;
                    }
                return searchContext;
            }
        }

        private By _locator;
        protected By Locator { 
            set { _locator = value; }
            get
            {
                return string.IsNullOrEmpty(_templateId) 
                    ? _locator 
                    : _locator.GetByFunc().Invoke(string.Format(_locator.GetByLocator(), _templateId));
            }
        }

        public IWebDriver WebDriver { get { return Site.WebDriver; } }
        private VISite _site;
        private static VISite _defaultSite { set; get; }
        public VISite Site { set { _site = value; } get { return _site ?? _defaultSite; } }
        public IAlerting Alerting { get { return Site.Alerting; } }
        public IWebDriverTimeouts Timeouts { get { return Site.WebDriverTimeouts; }}

        protected IWebElement WebElement;

        private int _defaultWaitTimeoutInSec { get { return Timeouts.WaitWebElementInSec; } }
        
        #region Temp Settings
        private int? _waitTimeoutInSec = null;
        private string _templateId;

        private void ClearTempSettings()
        {
            _templateId = null;
            _waitTimeoutInSec = _defaultWaitTimeoutInSec;
        }

        public T WaitTimeout<T>(int timeoutInSec) where T : VIElement
        {
            _waitTimeoutInSec = timeoutInSec;
            return (T)this;
        }
        public VIElement UseAsTemplate(string id)
        {
            _templateId = id;
            return this;
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
        public string FullName { get { return (_name != null) ? (_typeName + " " + Name) : Name;} }

        private IWebElement CheckWebElementIsUnique(ReadOnlyCollection<IWebElement> webElements)
        {
            if (webElements.Count == 1)
                return WebElement = webElements.First();
            if (webElements.Any())
                throw Alerting.ThrowError(string.Format("Found {0} {1} elements but expected. Please specify locator", webElements.Count, FullName));
            throw Alerting.ThrowError((string.Format("Can't found element {0} by selector {1}. Please correct locator", FullName, Locator)));
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

        public IWebElement GetWebElement()
        {
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
                    Site.Logger.Event("Used Click Previous action");
                } catch {WaitWebElement(0);}
            }
            ClearTempSettings();
            return CheckWebElementIsUnique(FoundElements);
        }

        public VIElement(string name = "") { Name = name; }
        public VIElement(string name, string cssSelector) : this(name) { Locator = By.CssSelector(cssSelector); }
        public VIElement(string name, By byLocator, List<By> byContext = null) : this(name) { Locator = byLocator; Context = byContext; }
        public VIElement(By byLocator, List<By> byContext = null) : this("", byLocator, byContext) { }
        public VIElement(string name, IWebElement webElement) : this(name) { WebElement = webElement; }
        public VIElement(IWebElement webElement) : this("", webElement) { }

        public static void Init(VISite site) { _defaultSite = site; }

        public string DefaultLogMessage(string text)
        {
            return text + string.Format(" (Name: {0}, Type: {1}, Locator: {2})", FullName, _typeName, Locator);
        }

        private Func<string, Func<Object>, Func<Object>, Object> _defaultViActionR
        {
            get
            {
                return (text, viAction, logResult) =>
                {
                    var logResultFunc = (Func<Object, string>)logResult.Invoke();
                    Site.Logger.Event(DefaultLogMessage(text));
                    var result = ((Func<Object>)viAction.Invoke()).Invoke();
                    if (logResultFunc != null)
                        Site.Logger.Event(logResultFunc.Invoke(result));
                    return result;
        }; } }

        private Func<string, Func<Object>, Func<Object>, Object> _viActionR;
        public Func<string, Func<Object>, Func<Object>, Object> VIActionR
        {
            set { _viActionR = value; }
            get { return _viActionR ?? _defaultViActionR; }
        }

        protected T DoVIAction<T>(string logActionText, Func<T> viAction, Func<T, string> logResult = null)
        {
            return (T) VIActionR.Invoke(logActionText, () => viAction, () => logResult);
        }

        private Action<VIElement, string, Action> _defaultViAction
        {
            get { return (viElement, text, viAction) => {
                viElement.Site.Logger.Event(viElement.DefaultLogMessage(text));
                viAction.Invoke();
        }; } }

        private Action<VIElement, string, Action> _viAction;
        public Action<VIElement, string, Action> VIAction 
        {
            set { _viAction = value; }
            get { return _viAction ?? _defaultViAction; }
        }

        protected void DoVIAction(string logActionText, Action viAction)
        {
            VIAction.Invoke(this, logActionText, viAction);
        }
    }
}
