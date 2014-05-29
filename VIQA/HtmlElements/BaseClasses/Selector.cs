using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.BaseClasses;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class Selector<T> : VIList<T>, ISelector where T : ClickableText, ISelected, IHaveValue
    {
        #region Cunstructors
        public Selector() { }

        public Selector(string name) : base(name) { }

        public Selector(string name, By rootCssSelector, Func<T> selectorTemplate) : base(name, rootCssSelector, selectorTemplate) { }
        public Selector(string name, Func<T> selectorTemplate) : base(name, selectorTemplate) { }
        public Selector(string name, By byLocator) : base(name, byLocator) { }
        public Selector(string name, string cssLocator) : base(name, cssLocator) { }

        #endregion

        #region Actions
        public VIAction<Action<Selector<T>, string>> SelectAction =
            new VIAction<Action<Selector<T>, string>>(
                (selector, name) => selector.GetVIElement(name).Click());

        public VIAction<Func<Selector<T>, string, string>> GetElementLabelFunc =
            new VIAction<Func<Selector<T>, string, string>>(
                (selector, name) => selector.GetVIElement(name).Label);

        public VIAction<Func<Selector<T>, string, bool>> IsSelectedFunc =
            new VIAction<Func<Selector<T>, string, bool>>(
                (selector, name) => selector.GetVIElement(name).IsSelected());
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
                () => SelectAction.Action(this, valueName));
        }

        public List<string> IsSelected()
        {
            return DoVIAction(Name + ". IsSelected",
                () => GetAllElements().Where(pair => pair.Value.IsSelected()).Select(pair => pair.Key).ToList(),
                values => FullName + " values are selected: " + values.Print());
        }

        public virtual string Value { get { return IsSelected().Print(); } }

        public virtual void SetValue<T>(T value)
        {
            if (value == null) return;
            Select(value.ToString());
        }
    }
}
