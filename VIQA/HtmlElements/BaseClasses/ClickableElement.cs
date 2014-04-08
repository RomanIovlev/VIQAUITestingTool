using System;
using System.Threading;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;
using Timer = VIQA.Common.Timer;

namespace VIQA.HtmlElements
{
    public class ClickableElement : VIElement, IClickable
    {
        protected override string _typeName { get { return "Clickable element"; } }

        public ClickableElement() { }
        public ClickableElement(string name) : base(name) { }
        public ClickableElement(string name, string cssSelector) : base(name, cssSelector) { }
        public ClickableElement(string name, By byLocator) : base(name, byLocator) { }
        public ClickableElement(string name, IWebElement webElement) : base(name, webElement) { }
        public ClickableElement(IWebElement webElement) : base(webElement) { }

        private Action _clickAction;
        public Action ClickAction
        {
            set { _clickAction = value; }
            get { return _clickAction ??
                    (() => GetWebElement().Click()); }
        }

        public void ClickOnInvisibleElement()
        {
            const string script = "var object = arguments[0];"
                    + "var theEvent = document.createEvent(\"MouseEvent\");"
                    + "theEvent.initMouseEvent(\"click\", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);"
                    + "object.dispatchEvent(theEvent);";
            ((IJavaScriptExecutor)WebDriver).ExecuteScript(script, GetWebElement());
        }
        
        public void Click()
        {
            DoVIAction("Click",
                () => {
                    SmartClickAction();
                    if (WithPageLoadAction)
                        NextActionNeedWaitPageToLoad = true;
                });
        }

        private void SmartClickAction()
        {
            var clicked = false;
            var timer = new Timer();
            while (!clicked && !timer.TimeoutPassed(WaitTimeoutInSec * Site.WebDriverTimeouts.WaitWebElementInSec))
                try
                {
                    ClickAction.Invoke();
                    clicked = true;
                    PreviousClickAction = ClickAction;
                    VISite.Logger.Event("Done");
                }
                catch
                {
                    Thread.Sleep(Site.WebDriverTimeouts.RetryActionInMsec);
                    VISite.Logger.Event("Done Double Click");
                }
        }
        
    }
}
