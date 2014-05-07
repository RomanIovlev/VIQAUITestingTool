using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HtmlElements.BaseClasses;

namespace VIQA.HtmlElements
{
    public class DropDown : Selector<SelectItem>
    {
        protected override string _typeName { get { return "DropDown"; } }

        public DropDown() { }
        
        public DropDown(string name, SelectItem selectorTemplate) : base(name) {
            ListElementTemplate = selectorTemplate;
        }

        public DropDown(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Action<Selector<SelectItem>, string> selectAction = null, Func<Selector<SelectItem>, string, string> elementLabelFunc = null,
            Func<Selector<SelectItem>, string, bool> isSelectedFunc = null)
            : base(name, listOfValuesFunc, selectAction, elementLabelFunc, isSelectedFunc) { }

        public DropDown(string name, string cssSelector) : base(name, cssSelector) { }

        public DropDown(string name, By rootCssSelector, string cssOptionTemplateSelector)
            : base(name, rootCssSelector, cssOptionTemplateSelector) { }

        public DropDown(string name, By byLocator) : base(name, byLocator) { }

        public new string IsSelected()
        {
            return DoVIAction(Name + ". IsSelected",
                () => GetAllValues.First(el => IsSelectedFunc.Action(this, el)),
                values => values.ToString());
        }

        public override string Value { get { return IsSelected(); } }
    }
}
