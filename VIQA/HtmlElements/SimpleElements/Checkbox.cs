using System;
using System.Linq;

using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements
{
    public class Checkbox : ClickableText, ICheckbox, ISelected
    {
        public VIElement CheckSignElement;

        public string Value { get { return IsChecked().ToString(); } }

        //<input type="checkbox" name="vehicle" value="Bike" id="bike">
        //<label for="bike">I have a bike<br></label>
        private const string LocatorTmplate = "input[type=checkbox][{0}={1}]";
        public static string CommonLocatorById(string id) { return string.Format(LocatorTmplate, "id", id); }
        public static string CommonLocatorByNamed(string id) { return string.Format(LocatorTmplate, "name", id); }
        public static string CommonLocatorByClassName(string id) { return string.Format(LocatorTmplate, "class", id); }

        public static string CommonLabelLocator(string id) { return string.Format("label[for='{0}']", id); }

        public virtual Func<bool> DefaultIsSelectedFunc { get { return () => GetWebElement().Selected; } }
        private Func<bool> _isSelectedFunc;
        public Func<bool> IsSelectedFunc
        {
            set { _isSelectedFunc = value; }
            get { return _isSelectedFunc ?? DefaultIsSelectedFunc; }
        }

        protected override string _typeName { get { return "Checkbox"; } }

        public Checkbox() { }
        public Checkbox(string name) : base(name) { }
        public Checkbox(string name, By bySelector) : base(name, bySelector)  { CheckSignElement = new VIElement(name + " label", bySelector); }
        public Checkbox(string name, string cssSelector) : base(cssSelector, name)  { CheckSignElement = new VIElement(name + " label", cssSelector); }
        public Checkbox(By bySelector) : base(bySelector) { CheckSignElement = new VIElement("", bySelector); }
        public Checkbox(string name, IWebElement webElement) : base(name, webElement) { CheckSignElement = new VIElement(name + " label", webElement); }
        public Checkbox(IWebElement webElement) : base(webElement) { CheckSignElement = new VIElement(webElement); }
        
        public Checkbox(string name, ElementId id) : base(name, By.CssSelector(CommonLocatorById(id.ToString())))
        {
        }


        public void Check()
        {
            DoVIAction("Check Checkbox", () =>
            {
                if (!IsChecked())
                    Click();
            });
        }
        public void Uncheck()
        {
            DoVIAction("Uncheck Checkbox", () =>
            {
                if (IsChecked())
                    Click();
            });
        }
        public bool IsChecked()
        {
            return DoVIAction("IsChecked", 
                () => IsSelectedFunc(), 
                isChecked => "Checkbox is " + (isChecked ? "checked" : "unchecked"));
        }

        public void SetValue<T>(T value)
        {
            if (value == null) return;
            var val = value.ToString();
            if (!new [] {"check", "uncheck", "true", "false"}.Contains(val = val.ToLower()))
                throw VISite.Alerting.ThrowError("Wrong Value type. For Checkbox availabel only 'check', 'uncheck', 'true', 'false'values of type String");
            if (val == "check" || val == "true")
                Check();
            else 
                Uncheck();
        }

        public bool IsSelected() { return IsChecked(); }
    }

    public class ElementId
    {
        private readonly string _id;
        public ElementId(string id) { _id = id; }
        public override string ToString() { return _id; }
    }
}
