using System;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HtmlElements.BaseClasses;

namespace VIQA.HtmlElements
{
    public class RadioButtons : Selector<RadioButton>
    {
        private const string RadioButtonTemplate = "input[type=radio][id={0}]";
        private const string LocatorTmpl = "input[type=radio][{0}={1}]";
        public static string CommonLocatorById(string id) { return string.Format(LocatorTmpl, "id", id); }
        public static string CommonLocatorByNamed(string id) { return string.Format(LocatorTmpl, "name", id); }
        public static string CommonLocatorByClassName(string id) { return string.Format(LocatorTmpl, "class", id); }
        
        protected override string _typeName { get { return "RadioButtons"; } }

        public RadioButtons() { }

        public RadioButtons(string name, By rootLocator, Func<RadioButton> radioTemplate) : base(name, rootLocator, radioTemplate) { }
        public RadioButtons(string name, Func<RadioButton> selectorTemplate) : base(name, selectorTemplate) { }
        public RadioButtons(string name, By byLocator) : base(name, byLocator) { }
        public RadioButtons(string name, string cssLocator) : base(name, cssLocator) { }

        public new string IsSelected()
        {
            return DoVIAction(Name + ". IsSelected",
                () => GetAllElements().First(pair => pair.Value.IsSelected()).Key,
                value => FullName + " value '" + value + "' is selected: ");
        }
        public override string Value { get { return IsSelected(); } }
    }

    public class RadioButton : SelectItem
    {
        public RadioButton(string name, By bySelector) : base(name, bySelector) { }
        public RadioButton(string name, string cssSelector) : base(cssSelector, name) { }
        public RadioButton(By bySelector) : base("", bySelector) { }
        public RadioButton(string name, IWebElement webElement) : base(name, webElement) { }
        public RadioButton(IWebElement webElement) : base(webElement) { }
    }
}
