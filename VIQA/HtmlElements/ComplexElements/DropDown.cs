using System;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HtmlElements.BaseClasses;

namespace VIQA.HtmlElements
{
    public class DropDown : Selector<SelectItem>
    {
        protected override string _typeName { get { return "DropDown"; } }

        public DropDown() { }

        public DropDown(string name, By rootCssSelector, Func<SelectItem> selectorTemplate) : base(name, rootCssSelector, selectorTemplate) { }
        public DropDown(string name, Func<SelectItem> selectorTemplate) : base(name, selectorTemplate) { }
        public DropDown(string name, By byLocator) : base(name, byLocator) { }
        public DropDown(string name, string cssLocator) : base(name, cssLocator) { }
        
        public new string IsSelected()
        {
            return DoVIAction(Name + ". IsSelected",
                () => GetAllElements().First(pair => pair.Value.IsSelected()).Key,
                value => FullName + " value '" + value + "' is selected: ");
        }

        public override string Value { get { return IsSelected(); } }
    }
}
