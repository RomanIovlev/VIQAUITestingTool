
using OpenQA.Selenium;

namespace VIQA.HtmlElements.BaseClasses
{
    public class Frame : VIElement { 
        
        public Frame() {}

        public Frame(string name) : base(name) { }
        public Frame(string name, string cssSelector) : base(name, cssSelector) { }
        public Frame(string name, By byLocator) : base(name, byLocator) { }
        public Frame(By byLocator) : base(byLocator) { }
        public Frame(string name, IWebElement webElement) : base(name, webElement) { }
        public Frame(IWebElement webElement) : base(webElement) { }
    }
}
