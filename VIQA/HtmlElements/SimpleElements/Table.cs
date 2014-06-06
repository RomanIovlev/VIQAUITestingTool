using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements.SimpleElements
{
    public class Table : Table<TextElement>, ITable { }

    public enum TableElementIndexType { Nums, Names, RowNames, ColNames }
    public enum TableHeadingType { ColumnsOnly, RowsOnly, NoHeadings, RowsAndColumns }

    public class Table<T> : VIElement, ITable<T> where T : VIElement
    {
        protected override string _typeName { get { return "Table"; } }

        private Dictionary<string, Dictionary<string, T>> _allElements;
        public Dictionary<string, Dictionary<string, T>> AllElements
        {
            set { _allElements = value; }
            get
            {
                if (_allElements != null)
                    return _allElements;
                return GetAllElements();
            }
        }

        public Dictionary<string, Dictionary<string, T>> GetAllElements()
        {
            return _allElements = ColumnNames
                .ToDictionary(columnName => columnName, columnName =>
                    RowNames.ToDictionary(rowName => rowName, rowName =>
                        GetVIElementXY(columnName, rowName)));
        }

		private Func<Table<T>, List<string>> _getColumnNamesFunc;
        public Func<Table<T>, List<string>> GetColumnNamesFunc
        {
            set { _getColumnNamesFunc = value; }
            get { return _getColumnNamesFunc ?? 
                (table => table.SearchContext.FindElements(By.XPath(".//th")).Select(el => el.Text).ToList()); }
        }

        private List<string> _columnNames;
        public List<string> ColumnNames
        {
            set { _columnNames = value; }
            get
            {
                if (_columnNames != null)
                    return _columnNames;
                if (HaveColumnNames)
                {
                    _columnNames = DoVIAction("GetColumnNames", () => GetColumnNamesFunc(this));
                    if (_columnNames != null && _columnNames.Any())
                        return _columnNames;
                }
                return TryGenerateNumColumnNames();
            }
        }

        private int GetColCount()
        {
            if (_columnNames != null)
                return _columnNames.Count;
            return ColumnIndex != null
                ? ColumnIndex.Count
                : SearchContext.FindElements(By.XPath(".//th")).Count();
        }

        private List<string> TryGenerateNumColumnNames()
        {
            var colCount = GetColCount();
            if (colCount > 0)
                return _columnNames = GetNumList(colCount);
            throw VISite.Alerting.ThrowError("Table have 0 columns. Please Specify ColumnNames or GetColumnNamesFunc");
        }

        private bool HaveColumnNames { get { return new[] { TableHeadingType.ColumnsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType); } }
        private bool HaveRowNames { get { return new[] { TableHeadingType.RowsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType); } }

        private Func<Table<T>, List<string>> _getRowNamesFunc;
        public Func<Table<T>, List<string>> GetRowNamesFunc
        {
            set { _getRowNamesFunc = value; }
            get
            {
                return _getRowNamesFunc ??
                    (table => table.SearchContext.FindElements(By.XPath(".//tr/td[1]")).Select(el => el.Text).ToList());
            }
        }

        private List<string> _rowNames;
        public List<string> RowNames
        {
            set { _rowNames = value; }
            get
            {
                if (_rowNames != null)
                    return _rowNames;
                if (HaveRowNames)
                {
                    _rowNames = DoVIAction("GetRowNames", () => GetRowNamesFunc(this));
                    if (_rowNames != null && _rowNames.Any())
                        return _rowNames;
                }
                return TryGenerateNumRowNames();
            }
        }

        private int GetRowsCount()
        {
            if (_rowNames != null)
                return _rowNames.Count;
            return RowIndex != null
                ? RowIndex.Count
                : SearchContext.FindElements(By.XPath(".//tr/td[1]")).Count();
        }
        private List<string> TryGenerateNumRowNames()
        {
            var rowCount = GetRowsCount();
            if (rowCount > 0)
                return _rowNames = GetNumList(rowCount);
            throw VISite.Alerting.ThrowError("Table have 0 rows. Please Specify RowNames or GetRowNamesFunc");
        }

        public List<int> ColumnIndex;
        public List<int> RowIndex;

        public TableElementIndexType IndexType = TableElementIndexType.Nums;
        public TableHeadingType HeadingsType = TableHeadingType.ColumnsOnly;
        
        public Func<T> CreateElementFunc = () => (T)Activator.CreateInstance(typeof(T));
        
        public By GetLocator(string col, string row)
        {
            var locatorTemplate = CreateElementFunc().Locator;
            if (locatorTemplate == null)
                locatorTemplate = By.XPath(".//tr[{1}]/td[{0}]");
            var byLocator = locatorTemplate.GetByLocator();
            if (!byLocator.Contains("{0}") && byLocator.Contains("{1}"))
                throw VISite.Alerting.ThrowError(FullName + ". Bad locator template for table element - " + byLocator + ". Locator template should contains {0} and {1}");
            return locatorTemplate.FillByTemplate(col, row);
        }

        public T GetVIElementXY(int colNum, int rowNum)
        {
            return GetVIElementXY(GetColNameByIndex(colNum), GetRowNameByIndex(rowNum));
        }
        
        private string GetColNameByIndex(int index)
        {
            return new[] { TableHeadingType.ColumnsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType) 
                ? ColumnNames[index - 1] 
                : index.ToString();
        }

        private string GetRowNameByIndex(int index)
        {
            return new[] { TableHeadingType.RowsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType)
                ? RowNames[index - 1]
                : index.ToString();
        }

        public T GetVIElementXY(string colName, string rowName)
        {
            var colIndex = GetColumnIndex(colName);
            var rowIndex = GetRowIndex(rowName);
            if (_allElements == null)
                _allElements = new Dictionary<string, Dictionary<string, T>> { { colName, new Dictionary<string, T> { { rowName, CreateElement(colIndex, rowIndex) } } } };
            else 
                if (!_allElements.ContainsKey(colName))
                    _allElements.Add(colName, new Dictionary<string, T> { { rowName, CreateElement(colIndex, rowIndex) } });
                else if (!_allElements[colName].ContainsKey(rowName))
                    _allElements[colName].Add(rowName, CreateElement(colIndex, rowIndex));
            return _allElements[colName][rowName];
        }

        private string GetColumnIndex(string name)
        {
            int nameIndex;
            if (ColumnNames != null && ColumnNames.Contains(name))
                nameIndex = ColumnNames.IndexOf(name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Column named:" + name + ". ColumnNames " + ((ColumnNames == null) ? "is Null" : ("not contains element (" + ColumnNames.Print() + ")")));
            return (ColumnIndex != null)
                ? ColumnIndex[nameIndex].ToString()
                : (nameIndex + 1).ToString();
        }

        private string GetRowIndex(string name)
        {
            int nameIndex;
            if (RowNames != null && RowNames.Contains(name))
                nameIndex = RowNames.IndexOf(name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Row named:" + name + ". RowNames " + ((RowNames == null) ? "is Null" : ("not contains element (" + RowNames.Print() + ")")));
            return (RowIndex != null)
                ? RowIndex[nameIndex].ToString()
                : (nameIndex + 1).ToString();
        }
        
        private List<string> GetNumList(int count)
        {
            var result = new List<string>();
            for (int i = 1; i <= count; i++)
                result.Add(i.ToString());
            return result;
        }

        public string Value { get {
            return "||X|" + ColumnNames.Print("|") + "||".LineBreak() + RowNames
                .Select(rowName => "||" + rowName + "||" + AllElements.Select(col => ((IHaveValue)col.Value[rowName]).Value).ToList().Print("|") + "||").ToList().Print("".LineBreak());
        } }

        public void SetValue<T>(T value) {}

        private T CreateElement(string col, string row)
        {
            var viElement = CreateElementFunc();
            viElement.Locator = GetLocator(col, row);
            viElement.Context = Context;
            return (T)viElement.GetVIElement();
        }

        public Table() { }
        public Table(string name) : base(name) { }
        //public Table(string name, string cssSelector) : base(name, cssSelector) { Init(); }
        //public Table(string name, By byLocator) : base(name, byLocator) { Init(); }
        //public Table(By byLocator) : base(byLocator) { Init(); }
        //public Table(string name, IWebElement webElement) : base(name, webElement) { Init(); }
        //public Table(IWebElement webElement) : base(webElement) { Init(); }

    }
}
