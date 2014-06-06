using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.BaseClasses;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class Selector<T> : VIList<T>, ISelector<T> where T : ClickableText, ISelected, IHaveValue
    {
        #region Cunstructors
        public Selector() { }

        public Selector(string name) : base(name) { }

        public Selector(string name, By rootCssSelector, Func<T> selectorTemplate)
            : base(name, rootCssSelector, selectorTemplate)
        {
            DefaultSelectAction = (selector, val) =>
            {
                selector.GetWebElement().Click();
                selector.GetVIElementByName(val).Click();
            };
        }
        public Selector(string name, Func<T> selectorTemplate) : base(name, selectorTemplate) { }
        public Selector(string name, By byLocator) : base(name, byLocator) { }
        public Selector(string name, string cssLocator) : base(name, cssLocator) { }

        #endregion

        #region Actions

        public Action<Selector<T>, string> DefaultSelectAction 
            = (selector, name) => selector.GetVIElementByName(name).Click();
        private Action<Selector<T>, string> _selectAction;
        public Action<Selector<T>, string> SelectAction
        {
            set { _selectAction = value; }
            get { return _selectAction ?? DefaultSelectAction; }
        }

        public Func<Selector<T>, string, string> DefaultGetElementLabelFunc 
            = (selector, name) => selector.GetVIElementByName(name).Label;
        private Func<Selector<T>, string, string> _getElementLabelFunc;
        public Func<Selector<T>, string, string> GetElementLabelFunc
        {
            set { _getElementLabelFunc = value; }
            get { return _getElementLabelFunc ?? DefaultGetElementLabelFunc; }
        }

        public Func<Selector<T>, string, bool> DefaultIsSelectedFunc 
            = (selector, name) => selector.GetVIElementByName(name).IsSelected();
        private Func<Selector<T>, string, bool> _isSelectedFunc;
        public Func<Selector<T>, string, bool> IsSelectedFunc
        {
            set { _isSelectedFunc = value; }
            get { return _isSelectedFunc ?? DefaultIsSelectedFunc; }
        }
        #endregion
        
        public List<string> GetListOfValues()
        {
            return DoVIAction(Name + ". GetListOfValues",
                () => GetAllElements().Select(pair => pair.Key).ToList(), 
                values => FullName + " have following values: " + values.Print());
        }

        public void Select(string valueName)
        {
            DoVIAction(Name + ". Select " + valueName,
                () => SelectAction(this, valueName));
        }

        public List<string> SelectedItems { get { 
            return DoVIAction(Name + ". SelectedItems",
                () => GetAllElements().Where(pair => pair.Value.IsSelected()).Select(pair => pair.Key).ToList(),
                values => FullName + " values are selected: " + values.Print());
        } }

        public bool IsSelected(string valueName)
        {
            return GetVIElementByName(valueName).IsSelected();
        }

        public virtual string Value { get { return SelectedItems.Print(); } }

        public virtual void SetValue<T>(T value)
        {
            if (value == null) return;
            Select(value.ToString());
        }
    }
}
