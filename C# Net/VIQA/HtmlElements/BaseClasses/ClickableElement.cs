using System;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements
{
    public class ClickableElement : VIElement, IClickable
    {
        protected override string _typeName { get { return "Clickable element"; } }

        public ClickableElement() { }
        public ClickableElement(string name) : base(name) { }
        public ClickableElement(string name, string cssSelector) : base(name, cssSelector) { }
        public ClickableElement(string name, By byLocator) : base(name, byLocator) { }
        public ClickableElement(By byLocator) : base(byLocator) { }
        public ClickableElement(string name, IWebElement webElement) : base(name, webElement) { }
        public ClickableElement(IWebElement webElement) : base(webElement) { }
        
        public string ClickLoadsPage { get; set; }

        public Action<ClickableElement> DefaultClickAction = cl => cl.GetWebElement().Click();
        private Action<ClickableElement> _clickAction;
        public Action<ClickableElement> ClickAction
        {
            set { _clickAction = value; }
            get { return _clickAction ?? DefaultClickAction; }
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
            DoVIAction("Click", () => {
                SmartClickAction();
                var windowHandles = WebDriver.WindowHandles;
                if (windowHandles.Count > 1)
                {
                    var windowHandle = windowHandles.Last();
                    Site.Navigate.WindowHandle = windowHandle;
                    WebDriver.SwitchTo().Window(windowHandle);
                }
                if (string.IsNullOrEmpty(ClickLoadsPage)) return;
                OpenPageName = ClickLoadsPage;
                Site.SiteSettings.CashDropTimes ++;
            });
        }

        private void SmartClickAction()
        {
            var clicked = Timer.Wait(() =>
            {
                ClickAction(this);
                VISite.Logger.Event(DefaultLogMessage("Done Click"));
                return true;
            }); 
            if (!clicked)
                throw VISite.Alerting.ThrowError(DefaultLogMessage("Failed to click element"));

        }
        
    }
}
