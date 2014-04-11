using System;
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

        public ClickableText(string name, string cssSelector) : base(name, cssSelector) { TextElement = new TextElement(name + " label", cssSelector); }
        public ClickableText(string name, By byLocator) : base(name, byLocator) { TextElement = new TextElement(name + " label", byLocator); }
        public ClickableText(string name, IWebElement webElement) : base(name, webElement) { TextElement = new TextElement(name + " label", webElement); }
        public ClickableText(IWebElement webElement) : base(webElement) { TextElement = new TextElement(webElement); }

        public readonly VIAction<Func<TextElement, string>> GetLabelFunc =
            new VIAction<Func<TextElement, string>>(txt => txt.GetWebElement().Text);

        //public virtual Func<string> DefaultGetLabelFunc { get { return () => GetWebElement().Text; } }

        //private Func<string> _getLabelFunc;
        //public Func<string> GetLabelFunc
        //{
        //    set { _getLabelFunc = value; }
        //    get { return _getLabelFunc ?? DefaultGetLabelFunc; }
        //}

        public string Label
        {
            get { return DoVIAction("Get label", GetLabelFunc.Action, text => text); }
        }
    }
}
