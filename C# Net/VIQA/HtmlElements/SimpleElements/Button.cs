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

        public Button() {  }
        public Button(string name) : base(name) { Init(); }
        public Button(string name, string cssSelector) : base(name, cssSelector) { Init(); }
        public Button(string name, By byLocator) : base(name, byLocator) { Init(); }
        public Button(By byLocator) : base(byLocator) { Init(); }
        public Button(string name, IWebElement webElement) : base(name, webElement) { Init(); }
        public Button(IWebElement webElement) : base(webElement) { Init(); }

        private void Init()
        {
            TextElement.GetTextFunc = () => GetWebElement().GetAttribute("value");
        }

    }
}
