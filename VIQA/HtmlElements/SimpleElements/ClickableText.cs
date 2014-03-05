using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class ClickableText : ClickableElement, ILabeled
    {
        protected override string _typeName { get { return "Clickable label"; } }
        
        public ClickableText(string name = "") : base(name) { }
        public ClickableText(string name, string cssSelector) : base(name, cssSelector) { }
        public ClickableText(string name, By byLocator, List<By> byContext = null) : base(name, byLocator, byContext) { }
        public ClickableText(By byLocator, List<By> byContext = null) : base(byLocator, byContext) { }
        public ClickableText(string name, IWebElement webElement) : base(name, webElement) { }
        public ClickableText(IWebElement webElement) : base(webElement) { }

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
