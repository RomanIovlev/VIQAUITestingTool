using System.Collections.Generic;
using OpenQA.Selenium;
using VIQA.HtmlElements.BaseClasses;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements.SimpleElements
{
    public class Table<T> : VIElement, ITable where T : VIElement
    {
        protected override string _typeName { get { return "Table"; } }

        public Table() {  }
        public Table(string name) : base(name) { }

        public Dictionary<string, List<T>> Columns = new Dictionary<string, List<T>>();
        public string[] ColumnNames;
        public string[] RowNames;

        //public T GetElement(string colName, string rowName)
        //{
        //    for(var i = )
        //    Columns[colName][RowNames[rowName]]
        //}
        //public Table(string name, string cssSelector) : base(name, cssSelector) { Init(); }
        //public Table(string name, By byLocator) : base(name, byLocator) { Init(); }
        //public Table(By byLocator) : base(byLocator) { Init(); }
        //public Table(string name, IWebElement webElement) : base(name, webElement) { Init(); }
        //public Table(IWebElement webElement) : base(webElement) { Init(); }

    }
}
