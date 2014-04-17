using System;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class ClickableText : ClickableElement, ILabeled
    {
        public TextElement TextElement;

        protected override string _typeName { get { return "Clickable label"; } }

        public ClickableText() { }
        public ClickableText(string name) : base(name) { }

        public ClickableText(string name, string cssSelector) : base(name, cssSelector) { Init(name, By.CssSelector(cssSelector)); }
        public ClickableText(string name, By byLocator) : base(name, byLocator) { Init(name, byLocator); }
        public ClickableText(By byLocator) : base(byLocator) { Init("", byLocator); }
        public ClickableText(string name, IWebElement webElement) : base(name, webElement) { Init(name, webElement); }
        public ClickableText(IWebElement webElement) : base(webElement) { Init("", webElement); }

        public readonly VIAction<Func<TextElement, string>> GetLabelFunc =
            new VIAction<Func<TextElement, string>>(txt => txt.GetWebElement().Text);

        private void Init(string name, By byLocator)
        {
            TextElement = new TextElement(name + " label", byLocator); 
        }
        private void Init(string name, IWebElement webElement)
        {
            TextElement = new TextElement(name + " label", webElement);
        }

        public string Label
        {
            get
            {
                return DoVIAction("Get label", () => GetLabelFunc.Action(TextElement), text => text);
            }
        }

    }
}
