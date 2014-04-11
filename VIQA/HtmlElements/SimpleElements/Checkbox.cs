using System;
using System.Linq;

using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements
{
    public class Checkbox : ClickableText, ICheckbox
    {
        public VIElement CheckSignElement;

        //<input type="checkbox" name="vehicle" value="Bike" id="bike">
        //<label for="bike">I have a bike<br></label>
        private const string LocatorTmpl = "input[type=checkbox][{0}={1}]";
        public static string CommonLocatorById(string id) { return string.Format(LocatorTmpl, "id", id); }
        public static string CommonLocatorByNamed(string id) { return string.Format(LocatorTmpl, "name", id); }
        public static string CommonLocatorByClassName(string id) { return string.Format(LocatorTmpl, "class", id); }

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
        public Checkbox(string name, string cssSelector) : base(name, cssSelector) { CheckSignElement = new VIElement(name + " label", cssSelector); }
        public Checkbox(string name, By byLocator) : base(name, byLocator) { CheckSignElement = new VIElement(name + " label", byLocator); }
        public Checkbox(string name, IWebElement webElement) : base(name, webElement) { CheckSignElement = new VIElement(name + " label", webElement); }
        public Checkbox(IWebElement webElement) : base(webElement) { CheckSignElement = new VIElement(webElement); }
        
        public Checkbox(string name, ElementId id) : base(name, By.CssSelector(CommonLocatorById(id.ToString())))
        {
            //GetLabelFunc = txt => new TextElement(FullName + " label", CommonLabelLocator(id.ToString())).Label;
            //IsSelectedFunc = DefaultIsSelectedFunc;
        }

        public void Check()
        {
            DoVIAction("Check Checkbox", () =>
            {
                if (!IsSelectedFunc.Invoke())
                    Click();
            });
        }
        public void Uncheck()
        {
            DoVIAction("Uncheck Checkbox", () =>
            {
                if (IsSelectedFunc.Invoke())
                    Click();
            });
        }
        public bool IsChecked()
        {
            return DoVIAction("IsChecked", 
                () => IsSelectedFunc.Invoke(), 
                isChecked => "Checkbox is " + (isChecked ? "checked" : "unchecked"));
        }

        public void SetValue<T>(T value)
        {
            if (value == null) return;
            var val = value.ToString();
            if (val == null || (!new [] {"check", "uncheck", "true", "false"}.Contains(val = val.ToLower()))) 
                throw VISite.Alerting.ThrowError("Wrong Value type. For Checkbox availabel only 'check', 'uncheck', 'true', 'false'values of type String");
            if (val == "check" || val == "true")
                Check();
            else 
                Uncheck();
        }

    }

    public class ElementId
    {
        private readonly string _id;
        public ElementId(string id) { _id = id; }
        public override string ToString() { return _id; }
    }
}
