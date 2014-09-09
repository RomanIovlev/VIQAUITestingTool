using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class ClickableText : ClickableElement, IText
    {
        public TextElement TextElement;

        protected override string _typeName { get { return "Clickable text"; } }

        public ClickableText() { }
        public ClickableText(string name) : base(name) { }
        public ClickableText(string name, string cssSelector) : base(name, cssSelector) { Init(name, By.CssSelector(cssSelector)); }
        public ClickableText(string name, By byLocator) : base(name, byLocator) { Init(name, byLocator); }
        public ClickableText(By byLocator) : base(byLocator) { Init("", byLocator); }
        public ClickableText(By byClickLocator, By byTextLocator) : base(byClickLocator) { Init("", byTextLocator); }
        public ClickableText(string name, IWebElement webElement) : base(name, webElement) { Init(name, webElement); }
        public ClickableText(IWebElement webElement) : base(webElement) { Init("", webElement); }
        
        private void Init(string name, By byLocator) {
            TextElement = new TextElement(name + " text", byLocator); 
        }
        private void Init(string name, IWebElement webElement) {
            TextElement = new TextElement(name + " text", webElement);
        }

        public string Text { get {
            return DoVIActionResult("Get text", () => TextElement.GetTextFunc(), text => text);
        } }
        
    }
}
