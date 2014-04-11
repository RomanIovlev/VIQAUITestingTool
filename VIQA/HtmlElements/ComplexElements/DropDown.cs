using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace VIQA.HtmlElements
{
    public class DropDown : Selector<ClickableText>
    {
        private readonly By _elemntLocator;

        public override Action<string> DefaultSelectAction
        {
            get { return value => new SelectElement(WebDriver.FindElement(_elemntLocator)).SelectByText(value); }
        }

        public override Func<string, bool> DefaultIsSelectedFunc
        {
            get { return value => WebDriver.FindElement(_elemntLocator).Selected; }
        }
        
        protected override string _typeName { get { return "DropDown"; } }

        public DropDown() { }

        public DropDown(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Func<string, string> elementLabelFunc = null, Action<string> selectAction = null, Func<string, bool> isSelectedFunc = null)
            : base(name, listOfValuesFunc, selectAction, elementLabelFunc, isSelectedFunc) { }

        public DropDown(string name, By elementLocator, Func<IWebDriver, List<string>> listOfValuesFunc = null, 
            Func<string, string> elementLabelFunc = null)
            : base(name, listOfValuesFunc, null, elementLabelFunc)
        {
            _elemntLocator = elementLocator;
        }
    }
}
