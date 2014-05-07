using System;
using System.Collections.Generic;
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
        
        public RadioButtons(string name, RadioButton radioTemplate) : base(name) {
            ListElementTemplate = radioTemplate;
        }

        public RadioButtons(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Action<Selector<RadioButton>, string> selectAction = null, Func<Selector<RadioButton>, string, string> elementLabelFunc = null,
            Func<Selector<RadioButton>, string, bool> isSelectedFunc = null)
            : base(name, listOfValuesFunc, selectAction, elementLabelFunc, isSelectedFunc) { }

        public RadioButtons(string name, string cssSelector = RadioButtonTemplate)
            : base(name, cssSelector) { }


        public RadioButtons(string name, By byLocator)
            : base(name, byLocator) { }

        public new string IsSelected()
        {
            return DoVIAction(Name + ". IsSelected",
                () => GetAllValues.First(el => IsSelectedFunc.Action(this, el)),
                values => values.ToString());
        }

        public override string Value { get { return IsSelected(); } }

    }

    public class RadioButton : SelectItem
    {
        public RadioButton(string name, By bySelector) : base(name, bySelector) { }
        public RadioButton(string name, string cssSelector) : base(cssSelector, name) { }
        public RadioButton(By bySelector) : base("", bySelector) { }
    }
}
