using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class Selector<T> : ClickableText, ISelector
    {
        public Dictionary<string, ClickableText> ElementsList;

        public ClickableText GetOption(string name)
        {
            if (ElementsList.ContainsKey(name))
                return ElementsList[name];
            var option = new ClickableText("", string.Format("input[id={0}]", name));
            ElementsList.Add(name, option);
            return option;
        }

        public virtual string OptionTemplate { get { return "input[id={0}]"; } }

        public string GetNameByValue(string value) { return FullName + " with value " + value; }

        public string ElementTemplateLocator;
        
        public Selector() { }
        public Selector(string name) : base(name) { }
        public Selector(string name, string cssSelector) : base(name, cssSelector) { }
        public Selector(string name, By byLocator) : base(name, byLocator) { }
        public Selector(string name, IWebElement webElement) : base(name, webElement) { }
        public Selector(IWebElement webElement) : base(webElement) { }


        public Selector(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Action<Selector<T>, string> selectAction = null, Func<string, string> elementLabelFunc = null, Func<string, bool> isSelectedFunc = null)
            : base(name)
        {
            if (listOfValuesFunc != null)
                GetListOfValuesFunc = listOfValuesFunc;
            if (selectAction != null)
                SelectAction.Action = selectAction;
            if (elementLabelFunc != null)
                GetElementLabelFunc = elementLabelFunc;
            if (isSelectedFunc != null)
                IsSelectedFunc = isSelectedFunc;
        }

        public VIAction<Action<Selector<T>, string>> SelectAction =
            new VIAction<Action<Selector<T>, string>>((selector, name) => selector.GetOption(name).Click());

        public virtual Func<string, string> DefaultGetElementLabelFunc { get { return 
            value => new TextElement("", string.Format("label[for={0}]", value)).Label; } }

        private Func<string, string> _getElementLabelFunc;
        public Func<string, string> GetElementLabelFunc
        {
            set { _getElementLabelFunc = value; }
            get { return _getElementLabelFunc ?? DefaultGetElementLabelFunc; }
        }

        public virtual Func<string, bool> DefaultIsSelectedFunc { get { return 
            value => new VIElement("", string.Format("input[id={0}]", value)).GetWebElement().Selected; } }

        private Func<string, bool> _isSelectedFunc;
        public Func<string, bool> IsSelectedFunc
        {
            set { _isSelectedFunc = value; }
            get { return _isSelectedFunc ?? DefaultIsSelectedFunc; }
        }
        
        public Func<IWebDriver, List<string>> GetListOfValuesFunc;
        
        protected IEnumerable<string> GetAllValues { get { return GetListOfValuesFunc.Invoke(WebDriver); } } 

        public List<string> GetListOfValues()
        {
            return DoVIAction(Name + ". GetListOfValues", 
                () => GetAllValues.Select(val => GetElementLabelFunc.Invoke(val)).ToList(), 
                values => FullName + " have following values: " + values.Print());
        }

        public void Select(string valueName)
        {
            DoVIAction(Name + ". Select " + valueName,
                () => SelectAction.Invoke(valueName));
        }

        public List<string> IsSelected()
        {
            return DoVIAction(Name + ". IsSelected", 
                () => GetAllValues.Where(el => IsSelectedFunc.Invoke(el)).Select(el => GetElementLabelFunc.Invoke(el)).ToList(),
                values => FullName + " have selected following values: " + values.Print());
        }

        public virtual void SetValue<T>(T value)
        {
            if (value == null) return;
            Select(value.ToString());
        }
    }
}
