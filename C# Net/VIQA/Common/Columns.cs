using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;
using VIQA.SiteClasses;

namespace VIQA.Common
{
    public class Columns<T> : TableLine<T> where T : VIElementsSet, IHaveValue
    {
        public Columns()
        {
            GetHeadersFunc = t => t.FindElements(By.XPath(".//th")).Select(el => el.Text).ToArray();
            HaveHeaders = true;
            ElementIndex = ElementIndexType.Nums;
        }
        private Exception GetRowsException(string colName, Exception ex)
        {
            return VISite.Alerting.ThrowError(string.Format("Can't Get Column '{0}'. Exception: {1}", colName, ex));
        }

        public override List<Cell<T>> this[string name]
        {
            get
            {
                try { return Table.Rows.Headers.Select(rowName => Table.Cell(name, rowName)).ToList(); }
                catch (Exception ex) { throw GetRowsException(name, ex); } 
            }
        }
        
        public override List<Cell<T>> this[int num]
        {
            get
            {
                int? colsCount = null;
                if (_count != null)
                    colsCount = _count;
                else if (_headers != null && _headers.Any())
                    colsCount = _headers.Count();
                if (colsCount != null && colsCount < num)
                    throw VISite.Alerting.ThrowError(string.Format("Can't Get Column '{0}'. [num] > ColumnsCount({1}).", num, colsCount));
                try
                {
                    var result = new List<Cell<T>>();
                    for (int rowNum = 1; rowNum <= Table.Rows.Count; rowNum++)
                        result.Add(Table.Cell(num, rowNum));
                    return result.ToList();
                }
                catch (Exception ex) { throw GetRowsException(num.ToString(), ex); } 
            }
        }
    }
}
