using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HtmlElements.BaseClasses;
using VIQA.HtmlElements.Interfaces;
using OpenQA.Selenium.Support.UI;
namespace VIQA.HtmlElements
{
    public class DropDown : Selector<SelectItem>, IDropDown
    {
        protected override string _typeName { get { return "DropDown"; } }

        public DropDown() { }

        public DropDown(string name, By byLocator) : base(name, byLocator) { 
            SelectAction = (selector, value) => new SelectElement(selector.GetWebElement()).SelectByText(value); }

        public DropDown(string name, By rootCssSelector, Func<SelectItem> selectorTemplate) : base(name, rootCssSelector, selectorTemplate) { }
        public DropDown(string name, Func<SelectItem> selectorTemplate) : base(name, selectorTemplate) { }
        public DropDown(string name, string cssLocator) : base(name, cssLocator) { }

        private new List<string> SelectedItems() { return null; }

        public string SelectedItem
        {
            get
            {
                return DoVIAction(Name + ". SelectedItems",
                    () => (ListOfValues == null)
                        ? GetAllElements().First(pair => pair.Value.IsSelected()).Key
                        : ListOfValues.First(name => GetVIElementByName(name).IsSelected()),
                    value => FullName + " value '" + value + "' is selected: ");
            }
        }

        public override string Value { get { return SelectedItem; } }
    }
}
