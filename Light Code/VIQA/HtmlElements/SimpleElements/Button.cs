using System;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class Button : ClickableText, IButton
    {
        private const string LocatorTmpl = "input[type=button][{0}={1}]";
        public static string CommonLocatorById(string id) { return string.Format(LocatorTmpl, "id", id); }
        public static string CommonLocatorByNamed(string id) { return string.Format(LocatorTmpl, "name", id); }
        public static string CommonLocatorByClassName(string id) { return string.Format(LocatorTmpl, "class", id); }

        protected override string _typeName { get { return "Button"; } }

        public Button() { GetLabelFunc.DefaultAction = txt => txt.GetWebElement().GetAttribute("value"); }
        public Button(string name) : base(name) { GetLabelFunc.DefaultAction = txt => txt.GetWebElement().GetAttribute("value"); }
        public Button(string name, string cssSelector) : base(name, cssSelector) { GetLabelFunc.DefaultAction = txt => txt.GetWebElement().GetAttribute("value"); }
        public Button(string name, By byLocator) : base(name, byLocator) { GetLabelFunc.DefaultAction = txt => txt.GetWebElement().GetAttribute("value"); }
        public Button(string name, IWebElement webElement) : base(name, webElement) { GetLabelFunc.DefaultAction = txt => txt.GetWebElement().GetAttribute("value"); }
        public Button(IWebElement webElement) : base(webElement) { GetLabelFunc.DefaultAction = txt => txt.GetWebElement().GetAttribute("value"); }

    }
}
