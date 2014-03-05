using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using VIQA.Common;

namespace VIQA.HtmlElements
{
    public class RadioButtons : Selector
    {
        private const string LocatorTmpl = "input[type=radio][{0}={1}]";
        public static string CommonLocatorById(string id) { return string.Format(LocatorTmpl, "id", id); }
        public static string CommonLocatorByNamed(string id) { return string.Format(LocatorTmpl, "name", id); }
        public static string CommonLocatorByClassName(string id) { return string.Format(LocatorTmpl, "class", id); }

        public override Action<string> DefaultSelectAction
        {
            get { return value => new RadioButton(GetNameByValue(value), CommonLocatorById(value)).Click(); }
        }

        public override Func<string, bool> DefaultIsSelectedFunc
        {
            get { return value => new RadioButton(GetNameByValue(value), CommonLocatorById(value)).GetWebElement().Selected; }
        }

        protected override string _typeName { get { return "RadioButtons"; } }

        public RadioButtons() { }

        public RadioButtons(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Func<string, string> elementLabelFunc = null, Action<string> selectAction = null, Func<string, bool> isSelectedFunc = null)
            : base(name, listOfValuesFunc, selectAction, elementLabelFunc, isSelectedFunc)
        { }

        public RadioButtons(string name, By locatorTemplate, Func<IWebDriver, List<string>> listOfValuesFunc = null, 
            Func<string, string> elementLabelFunc = null)
            : base(name, listOfValuesFunc, null, elementLabelFunc)
        {
            SelectAction = val => new RadioButton(name: GetNameByValue(val), bySelector: locatorTemplate.SetLocatorTemplateValue(val)).Click();
            IsSelectedFunc = val => new RadioButton(name: GetNameByValue(val), bySelector: locatorTemplate.SetLocatorTemplateValue(val)).GetWebElement().Selected;
        }

    }

    public class RadioButton : ClickableElement
    {
        public RadioButton(By bySelector, List<By> byLocators = null, string name = "") : base(name, bySelector, byLocators) { }
        public RadioButton(string cssSelector, string name = "") : base(cssSelector, name) { }
    }
}
