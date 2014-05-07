using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class Selector<T> : ClickableText, ISelector where T : ClickableText, ISelected
    {
        public T ListElementTemplate;

        public T GetlistElement(string name)
        {
            ListElementTemplate.TemplateId = name;
            ListElementTemplate.TextElement.TemplateId = name;
            ListElementTemplate.Name = Name + " element with value: " + name;
            ListElementTemplate.TextElement.Name = Name + " label with value: " + name;
            return ListElementTemplate;
        }
        
        public Selector() { }

        public Selector(string name, T listElementTemplate) : base(name) {
            ListElementTemplate = listElementTemplate;
        }

        public Selector(string name, string cssSelector) : base(name, cssSelector) {
            ListElementTemplate = (T) Activator.CreateInstance(typeof(T), new object[] { name, cssSelector }); }

        public Selector(string name, By rootCssSelector, string cssOptionTemplateSelector = "input[id={0}]") : base(name, rootCssSelector) {
            ListElementTemplate = (T)Activator.CreateInstance(typeof(T), new[] { name, cssOptionTemplateSelector }); }

        public Selector(string name, By byLocator) : base(name, byLocator) {
            ListElementTemplate = (T)Activator.CreateInstance(typeof(T), new object[] { name, byLocator }); }

        public Selector(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Action<Selector<T>, string> selectAction = null, Func<Selector<T>, string, string> elementLabelFunc = null, 
            Func<Selector<T>, string, bool> isSelectedFunc = null)
            : base(name)
        {
            if (listOfValuesFunc != null)
                GetListOfValuesFunc = listOfValuesFunc;
            if (selectAction != null)
                SelectAction.Action = selectAction;
            if (elementLabelFunc != null)
                GetElementLabelFunc.Action = elementLabelFunc;
            if (isSelectedFunc != null)
                IsSelectedFunc.Action = isSelectedFunc;
        }

        public VIAction<Action<Selector<T>, string>> SelectAction =
            new VIAction<Action<Selector<T>, string>>(
                (selector, name) => selector.GetlistElement(name).Click());

        public VIAction<Func<Selector<T>, string, string>> GetElementLabelFunc =
            new VIAction<Func<Selector<T>, string, string>>(
                (selector, name) => selector.GetlistElement(name).Label);

        public VIAction<Func<Selector<T>, string, bool>> IsSelectedFunc =
            new VIAction<Func<Selector<T>, string, bool>>(
                (selector, name) => selector.GetlistElement(name).IsSelected());
        
        public Func<IWebDriver, List<string>> GetListOfValuesFunc;

        protected IEnumerable<string> GetAllValues
        {
            get
            {
                try { return GetListOfValuesFunc.Invoke(WebDriver); }
                catch { throw new Exception("GetListOfValuesFunc not set for " + Name); }
            }
        }

        public List<string> GetListOfValues()
        {
            return DoVIAction(Name + ". GetListOfValues", 
                () => GetAllValues.Select(val => GetElementLabelFunc.Action(this, val)).ToList(), 
                values => FullName + " have following values: " + values.Print());
        }

        public void Select(string valueName)
        {
            DoVIAction(Name + ". Select " + valueName,
                () => SelectAction.Action(this, valueName));
        }

        public List<string> IsSelected()
        {
            return DoVIAction(Name + ". IsSelected", 
                () => GetAllValues.Where(el => IsSelectedFunc.Action(this, el)).ToList(),
                values => FullName + " have selected following values: " + values.Print());
        }

        public virtual string Value { get { return IsSelected().Print(); } }

        public virtual void SetValue<T>(T value)
        {
            if (value == null) return;
            Select(value.ToString());
        }
    }
}
