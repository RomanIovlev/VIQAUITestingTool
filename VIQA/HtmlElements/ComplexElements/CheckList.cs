using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements
{
    public class CheckList : Selector<Checkbox> , ICheckList
    {
        private const string CheckboxTemplate = "input[type=checkbox][id={0}]";
        protected override string _typeName { get { return "Checkboxes"; } }
        private readonly Func<string, Checkbox> _checkBoxTmpl;

        public CheckList() { }
        
        public CheckList(string name, Checkbox checkboxTemplate) : base(name) {
            ListElementTemplate = checkboxTemplate;
        }

        public CheckList(string name, Func<IWebDriver, List<string>> listOfValuesFunc = null,
            Action<Selector<Checkbox>, string> selectAction = null, Func<Selector<Checkbox>, string, string> elementLabelFunc = null,
            Func<Selector<Checkbox>, string, bool> isSelectedFunc = null)
            : base(name, listOfValuesFunc, selectAction, elementLabelFunc, isSelectedFunc) { }

        public CheckList(string name, string cssSelector = CheckboxTemplate)
            : base(name, cssSelector) { }


        public CheckList(string name, By byLocator)
            : base(name, byLocator) { }

        public void CheckGroup(params string[] values)
        {
            DoVIAction("Check Group: " + values.Print(), 
                () => values.ForEach(val => GetlistElement(val).Check()));
        }

        public void UncheckGroup(params string[] values)
        {
            DoVIAction("Uncheck Group: " + values.Print(),
                () => values.ForEach(val => GetlistElement(val).Uncheck()));
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
                    throw VISite.Alerting.ThrowError("Wrong Value type");
            }
        }
    }
}

