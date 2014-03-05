using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class CheckList : Selector, ICheckList
    {
        public override Action<string> DefaultSelectAction
        {
            get { return value => _checkBoxTmpl.Invoke(value).Click(); }
        }

        public override Func<string, bool> DefaultIsSelectedFunc
        {
            get { return value => _checkBoxTmpl.Invoke(value).IsSelectedFunc.Invoke(); }
        }

        protected override string _typeName { get { return "Checkboxes"; } }
        private readonly Func<string, Checkbox> _checkBoxTmpl;

        public CheckList() { }

        public CheckList(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null, 
            Func<string, string> elementLabelFunc = null, Action<string> selectAction = null, Func<string, bool> isSelectedFunc = null)
            : base(name, listOfValuesFunc, selectAction, elementLabelFunc, isSelectedFunc)
        {
            _checkBoxTmpl = val => new Checkbox(FullName + " option " + val, new ElementId(val)) { ClickAction = () => DefaultSelectAction.Invoke(val) };
        }

        public CheckList(string name, By locatorTemplate, Func<IWebDriver, List<string>> listOfValuesFunc = null, 
            Func<string, string> elementLabelFunc = null)
            : base(name, listOfValuesFunc, null, elementLabelFunc)
        {
            _checkBoxTmpl = val => new Checkbox(GetNameByValue(val), locatorTemplate.SetLocatorTemplateValue(val) );
        }

        public void CheckGroup(params string[] values)
        {
            DoVIAction("Check Group: " + values.Print(), 
                () => values.ForEach(val => _checkBoxTmpl.Invoke(val).Check()));
        }

        public void UncheckGroup(params string[] values)
        {
            DoVIAction("Uncheck Group: " + values.Print(),
                () => values.ForEach(val => _checkBoxTmpl.Invoke(val).Uncheck()));
        }

        public void CheckOnly(params string[] values)
        {
            DoVIAction("CheckOnly: " + values.Print(),
                () =>
                {
                    CheckGroup(values);
                    if (GetListOfValuesFunc != null)
                        //Todo better to Hide logging
                        UncheckGroup(GetAllValues.Except(values).ToArray());
                });
        }

        public void UncheckOnly(params string[] values)
        {
            DoVIAction("UnheckOnly: " + values.Print(),
                () =>
                {
                    UncheckGroup(values);
                    if (GetListOfValuesFunc != null)
                        //Todo better to Hide logging
                        CheckGroup(GetAllValues.Except(values).ToArray());
                });
        }

        public List<string> GetListOfChecked()
        {
            return DoVIAction("GetListOfChecked elements",
                IsSelected, result => "Checkboxes list. GetListOfChecked elements: " + result.Print());
        }

        public List<string> GetListOfNotChecked()
        {
            return DoVIAction("GetListOfChecked elements",
                () => GetAllValues.Except(GetListOfChecked()).ToList(), result => "Checkboxes list. GetListOfNotChecked elements: " + result.Print());
        }

        public override void SetValue<T>(T value)
        {
            if (value == null) return;
            var valAsArray = value as IEnumerable<string>;
            if (valAsArray != null) 
                CheckOnly(valAsArray.ToArray());
            else
            {
                var valAsString = value as string;
                if (valAsString != null) 
                    CheckOnly(valAsString);
                else
                    throw Alerting.ThrowError("Wrong Value type");
            }
        }
    }
}

