using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements.BaseClasses
{

    public class SelectItem : ClickableText, ISelected, IHaveValue
    {
        public SelectItem(string name, By bySelector) : base(name, bySelector) { }
        public SelectItem(string name, string cssSelector) : base(cssSelector, name) { }
        public SelectItem(By bySelector) : base("", bySelector) { }
        public SelectItem(string name, IWebElement webElement) : base(name, webElement) { }
        public SelectItem(IWebElement webElement) : base(webElement) { }

        public bool IsSelected()
        {
            return DoVIAction(Name + " isSelected", () => GetWebElement().Selected, val => val.ToString());
        }

        public string Value { get { return IsSelected().ToString(); } }
        public void SetValue<T>(T value) { Click(); }
    }
}
