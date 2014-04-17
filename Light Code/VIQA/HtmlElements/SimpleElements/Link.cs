using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class Link : ClickableText, ILink
    {
        private const string LocatorTmpl = "a[{0}={1}]";
        public static string CommonLocatorById(string id) { return string.Format(LocatorTmpl, "id", id); }
        public static string CommonLocatorByNamed(string id) { return string.Format(LocatorTmpl, "name", id); }
        public static string CommonLocatorByClassName(string id) { return string.Format(LocatorTmpl, "class", id); }

        protected override string _typeName { get { return "Link"; } }

        public Link() { }
        public Link(string name) : base(name) { }
        public Link(string name, string cssSelector) : base(name, cssSelector) { }
        public Link(string name, By byLocator) : base(name, byLocator) { }
        public Link(string name, IWebElement webElement) : base(name, webElement) { }
        public Link(IWebElement webElement) : base(webElement) { }

        public string Reference
        {
            get { return DoVIAction("Get Reference", () => GetWebElement().GetAttribute("href"), href => "Get href of link '" + href + "'"); }
        }

    }
}
