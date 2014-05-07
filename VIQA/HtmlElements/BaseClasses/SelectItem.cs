using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements.BaseClasses
{

    public class SelectItem : ClickableText, ISelected
    {
        public SelectItem(string name, By bySelector) : base(name, bySelector) { }
        public SelectItem(string name, string cssSelector) : base(cssSelector, name) { }
        public SelectItem(By bySelector) : base("", bySelector) { }

        public bool IsSelected()
        {
            return DoVIAction(Name + " isSelected", () => GetWebElement().Selected, val => val.ToString());
        }
    }
}
