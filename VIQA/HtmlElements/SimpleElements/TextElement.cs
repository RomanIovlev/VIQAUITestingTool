using System;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class TextElement : VIElement, ILabeled, IHaveValue
    {
        protected override string _typeName { get { return "Label"; } }

        public TextElement() { }
        public TextElement(string name) : base(name) { }
        public TextElement(string name, string cssSelector) : base(name, cssSelector) { }
        public TextElement(string name, By byLocator) : base(name, byLocator) { }
        public TextElement(By byLocator) : base(byLocator) { }
        public TextElement(string name, IWebElement webElement) : base(name, webElement) { }
        public TextElement(IWebElement webElement) : base(webElement) { }

        public virtual Func<string> DefaultGetLabelFunc { get { return () => GetWebElement().Text; } }

        private Func<string> _getLabelFunc;
        public Func<string> GetLabelFunc
        {
            set { _getLabelFunc = value; }
            get { return _getLabelFunc ?? DefaultGetLabelFunc; }
        }

        public string Value { get { return Label; } }

        public string Label
        {
            get { return DoVIAction("Get label", GetLabelFunc, text => text); }
        }
        public void SetValue<T>(T value) { }
    }
}

