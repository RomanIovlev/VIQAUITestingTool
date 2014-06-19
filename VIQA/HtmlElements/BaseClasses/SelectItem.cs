using System;
using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements.BaseClasses
{

    public class SelectItem : ClickableText, ISelected, IHaveValue
    {
        public SelectItem() { }
        public SelectItem(string name) : base(name) { }
        public SelectItem(string name, By bySelector) : base(name, bySelector) { }
        public SelectItem(string name, string cssSelector) : base(cssSelector, name) { }
        public SelectItem(By bySelector) : base("", bySelector) { }
        public SelectItem(string name, IWebElement webElement) : base(name, webElement) { }
        public SelectItem(IWebElement webElement) : base(webElement) { }

        private Func<SelectItem, bool> _isSelectedFunc;
        public Func<SelectItem, bool> IsSelectedFunc
        {
            set { _isSelectedFunc = value; }
            get { return _isSelectedFunc ?? (selectItem => selectItem.GetWebElement().Selected); }
        }

        public bool IsSelected()
        {
            return DoVIAction(Name + " isSelected", () => IsSelectedFunc(this), val => val.ToString());
        }

        public string Value { get { return IsSelected().ToString(); } }
        public void SetValue<T>(T value) { Click(); }
    }
}
