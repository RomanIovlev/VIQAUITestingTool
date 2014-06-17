using System;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class TextElement : VIElement, IText, IHaveValue
    {
        protected override string _typeName { get { return "Text"; } }

        public TextElement() { }
        public TextElement(string name) : base(name) { }
        public TextElement(string name, string cssSelector) : base(name, cssSelector) { }
        public TextElement(string name, By byLocator) : base(name, byLocator) { }
        public TextElement(By byLocator) : base(byLocator) { }
        public TextElement(string name, IWebElement webElement) : base(name, webElement) { }
        public TextElement(IWebElement webElement) : base(webElement) { }

        public virtual Func<string> DefaultGetTextFunc { get { return () => GetWebElement().Text; } }

        private Func<string> _getTextFunc;
        public Func<string> GetTextFunc
        {
            set { _getTextFunc = value; }
            get { return _getTextFunc ?? DefaultGetTextFunc; }
        }


        public Func<Object, Object> FillRule { set; get; }
        public static Func<Object, Object> ToFillRule<T>(Func<T, Object> typeFillRule)
        {
            return o => new Func<T, object>(typeFillRule)((T)o);
        }

        public string Value { get { return Text; } }

        public string Text
        {
            get { return DoVIAction("Get text", GetTextFunc, text => text); }
        }

        public void SetValue<T>(T value) { }
    }
}

