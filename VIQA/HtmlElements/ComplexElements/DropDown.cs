using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace VIQA.HtmlElements
{
    public class DropDown : Selector<ClickableText>
    {
        protected override string _typeName { get { return "DropDown"; } }

        public DropDown() { }
        
        public DropDown(string name, DropDown selectorTemplate) : base(name) {
            ListElementTemplate = selectorTemplate;
        }

        public DropDown(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Action<Selector<ClickableText>, string> selectAction = null, Func<Selector<ClickableText>, string, string> elementLabelFunc = null,
            Func<Selector<ClickableText>, string, bool> isSelectedFunc = null)
            : base(name, listOfValuesFunc, selectAction, elementLabelFunc, isSelectedFunc) { }

        public DropDown(string name, string cssSelector) : base(name, cssSelector) { }

        public DropDown(string name, By rootCssSelector, string cssOptionTemplateSelector)
            : base(name, rootCssSelector, cssOptionTemplateSelector) { }

        public DropDown(string name, By byLocator) : base(name, byLocator) { }
    }
}
