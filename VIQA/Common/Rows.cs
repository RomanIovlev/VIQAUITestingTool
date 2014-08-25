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
    public class Rows<T> : TableLine<T> where T : VIElementsSet, IHaveValue
    {
        public Rows()
        {
            GetHeadersFunc = t => t.FindElements(By.XPath(".//tr/td[1]")).Select(el => el.Text).ToArray();
            GetHeadersFunc = t => t.FindElements(By.XPath(".//tr/td[1]")).Select(el => el.Text).ToArray();
            HaveHeaders = false;
            ElementIndex = ElementIndexType.Nums;
        }

        private Exception GetRowsException(string rowName, Exception ex)
        {
            return VISite.Alerting.ThrowError(string.Format("Can't Get Rows '{0}'. Exception: {1}", rowName, ex));
        }

        public override List<Cell<T>> this[string name]
        {
            get
            {
                try { return Table.Columns.Headers.Select(colName => Table.Cell(colName, name)).ToList(); }
                catch (Exception ex) { throw GetRowsException(name, ex); } 
            }
        }
        
        public override List<Cell<T>> this[int num]
        {
            get
            {
                int? rowsCount = null;
                if (_count != null)
                    rowsCount = _count;
                else if (_headers != null && _headers.Any())
                    rowsCount = _headers.Count();
                if (rowsCount != null && rowsCount < num)
                    throw VISite.Alerting.ThrowError(string.Format("Can't Get Row '{0}'. [num] > RowsCount({1}).", num, rowsCount));
                try
                {
                    var result = new List<Cell<T>>();
                    for (int colNum = 1; colNum <= Table.Columns.Count; colNum++)
                        result.Add(Table.Cell(colNum, num));
                    return result.ToList();
                }
                catch (Exception ex) { throw GetRowsException(num.ToString(), ex); } 
            }
        }
    }
}
