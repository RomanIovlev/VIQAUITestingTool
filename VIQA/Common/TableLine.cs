using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.HtmlElements.SimpleElements;

namespace VIQA.Common
{
    public abstract class TableLine<T> where T : VIElementsSet, IHaveValue
    {
        public virtual List<Cell<T>> this[string name] { get { return null; } }
        public virtual List<Cell<T>> this[int num] { get { return null; } }
        public Func<IWebElement, string[]> GetHeadersFunc; 
        protected string[] _headers;
        public virtual string[] Headers { get; set; }
        public int StartIndex = 1;
        public bool HaveHeaders;
        public ElementIndexType ElementIndex;

        public Table<T> Table;

        protected int? _count;
        public int Count
        {
            set { _count = value; }
            get
            {
                if (_count != null)
                    return (int)_count;
                return Headers != null ? Headers.Length : 0;
            }
        }

        protected string[] GetNumList(int count, int from = 1)
        {
            var result = new List<string>();
            for (int i = from; i < count + from; i++)
                result.Add(i.ToString());
            return result.ToArray();
        }

        public void Update(TableLine<T> tableLine)
        {
            if (tableLine._count != null)
                Count = tableLine.Count;
            if (tableLine.StartIndex != 1)
                StartIndex = tableLine.StartIndex;
            if (tableLine._headers != null && tableLine._headers.Any())
                Headers = tableLine.Headers;
            if ((tableLine is Columns<T> && !tableLine.HaveHeaders) || (tableLine is Rows<T> && tableLine.HaveHeaders))
                HaveHeaders = tableLine.HaveHeaders;
            if (tableLine.ElementIndex != ElementIndexType.Nums)
                ElementIndex = tableLine.ElementIndex;
            GetHeadersFunc = tableLine.GetHeadersFunc;
        }
    }
    public enum ElementIndexType { Nums, Names }

}
