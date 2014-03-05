using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class Selector : VIElement, ISelector
    {
        public string GetNameByValue(string value) { return FullName + " with value " + value; }

        public virtual Action<string> DefaultSelectAction { get { return 
            value => new ClickableElement("", string.Format("input[id={0}]", value)).Click(); } }

        private Action<string> _selectAction;
        public Action<string> SelectAction
        {
            set { _selectAction = value; }
            get { return _selectAction ?? DefaultSelectAction; }
        }

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

        public Selector() { }

        public Selector(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Action<string> selectAction = null, Func<string, string> elementLabelFunc = null, Func<string, bool> isSelectedFunc = null)
            : base(name)
        {
            GetListOfValuesFunc = listOfValuesFunc;
            if (selectAction != null)
                SelectAction = selectAction;
            if (elementLabelFunc != null)
                GetElementLabelFunc = elementLabelFunc;
            if (isSelectedFunc != null)
                IsSelectedFunc = isSelectedFunc;
        }

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
