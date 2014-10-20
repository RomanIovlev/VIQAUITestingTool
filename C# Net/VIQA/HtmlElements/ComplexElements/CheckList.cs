using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements
{
    public class CheckList : Selector<Checkbox>, ICheckList
    {
        private const string CheckboxTemplate = "input[type=checkbox][id={0}]";
        protected override string _typeName { get { return "Checkboxes"; } }
        private readonly Func<string, Checkbox> _checkBoxTmpl;

        public CheckList() { }

        public CheckList(string name, By rootLocator, Func<Checkbox> checkboxTemplate) : base(name, rootLocator, checkboxTemplate) { }
        public CheckList(string name, Func<Checkbox> checkboxTemplate) : base(name, checkboxTemplate) { }
        public CheckList(string name, By byLocator) : base(name, byLocator) { }
        public CheckList(string name, string cssLocator) : base(name, cssLocator) { }
        public CheckList(By byLocator) : base(byLocator) { }
        public CheckList(string name, IWebElement webElement) : base(name, webElement) { }
        public CheckList(IWebElement webElement) : base(webElement) { }

        public void CheckGroup(params string[] values)
        {
            DoVIAction("Check Group: " + values.Print(),
                () => values.ForEach(val => GetVIElementByTemplate(val).Check()));
        }

        public void UncheckGroup(params string[] values)
        {
            DoVIAction("Uncheck Group: " + values.Print(),
                () => values.ForEach(val => GetVIElementByTemplate(val).Uncheck()));
        }

        public Action<List<Checkbox>> ClearAction;

        public void Clear()
        {
            DoVIAction("Clear cheklist:", () => ClearAction(Elements.Select(pair => pair.Value).ToList()));
        }

        public void CheckOnly(params string[] values)
        {
            DoVIAction("CheckOnly: " + values.Print(),
                () =>
                {
                    Clear();
                    CheckGroup(values);
                });
        }

        public void UncheckOnly(params string[] values)
        {
            DoVIAction("UnheckOnly: " + values.Print(),
                () =>
                {
                    Clear();
                    CheckGroup(ListOfValues.Except(values).ToArray());
                });
        }

        public List<string> GetListOfChecked()
        {
            return DoVIActionResult("GetListOfChecked elements",
                () => SelectedItems, result => "Checkboxes list. GetListOfChecked elements: " + result.Print());
        }

        public List<string> GetListOfNotChecked()
        {
            return DoVIActionResult("GetListOfChecked elements",
                () => ListOfValues.Except(SelectedItems).ToList(), result => "Checkboxes list. GetListOfNotChecked elements: " + result.Print());
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

