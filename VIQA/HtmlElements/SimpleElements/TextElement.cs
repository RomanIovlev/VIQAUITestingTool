using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class TextElement : VIElement, ILabeled
    {
        protected override string _typeName { get { return "Label"; } }
        
        public TextElement(string name = "") : base(name) { }
        public TextElement(string name, string cssSelector) : base(name, cssSelector) { }
        public TextElement(string name, By byLocator, List<By> byContext = null) : base(name, byLocator, byContext) { }
        public TextElement(By byLocator, List<By> byContext = null) : base(byLocator, byContext) { }
        public TextElement(string name, IWebElement webElement) : base(name, webElement) { }
        public TextElement(IWebElement webElement) : base(webElement) { }

        public virtual Func<string> DefaultGetLabelFunc { get { return () => GetWebElement().Text; } }

        private Func<string> _getLabelFunc;
        public Func<string> GetLabelFunc
        {
            set { _getLabelFunc = value; }
            get { return _getLabelFunc ?? DefaultGetLabelFunc; }
        }

        public string Label
        {
            get { return DoVIAction("Get label", GetLabelFunc, text => text); }
        }
    }
}

