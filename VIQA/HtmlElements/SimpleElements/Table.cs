using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.Common.Pairs;
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

        private Dictionary<string, Dictionary<string, T>> _allCells;
        public Dictionary<string, Dictionary<string, T>> AllElements
        {
            set { _allCells = value; }
            get { return _allCells ?? Cells; }
        }

        public Dictionary<string, Dictionary<string, T>> Cells
        {
            get
            {
                return _allCells = ColumnNames
                    .ToDictionary(columnName => columnName, columnName =>
                        RowNames.ToDictionary(rowName => rowName, rowName =>
                            Cell(columnName, rowName)));
            }
        }

		private Func<IWebElement, List<string>> _getColumnNamesFunc;
        public Func<IWebElement, List<string>> GetColumnNamesFunc
        {
            set { _getColumnNamesFunc = value; }
            get { return _getColumnNamesFunc ??
                (table => table.FindElements(By.XPath(".//th")).Select(el => el.Text).ToList());
            }
        }

        private List<string> _columnNames;
        public List<string> ColumnNames
        {
            set { _columnNames = value; }
            get
            {
                if (_columnNames != null)
                    return _columnNames;
                _columnNames = DoVIAction("GetColumnNames", () => GetColumnNamesFunc(GetWebElement()));
                if (_columnNames == null || !_columnNames.Any())
                    throw VISite.Alerting.ThrowError("Table have 0 columns. Please Specify ColumnNames or GetColumnNamesFunc");
                if (!HaveColumnNames)
                    _columnNames = GetNumList(_columnNames.Count);
                return _columnNames;
            }
        }
        
        private bool HaveColumnNames { get { return new[] { TableHeadingType.ColumnsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType); } }
        private bool HaveRowNames { get { return new[] { TableHeadingType.RowsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType); } }

        private Func<IWebElement, List<string>> _getRowNamesFunc;
        public Func<IWebElement, List<string>> GetRowNamesFunc
        {
            set { _getRowNamesFunc = value; }
            get
            {
                return _getRowNamesFunc ??
                    (table => table.FindElements(By.XPath(".//tr/td[1]")).Select(el => el.Text).ToList());
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
                _rowNames = DoVIAction("GetRowNames", () => GetRowNamesFunc(GetWebElement()));
                if (_rowNames == null || !_rowNames.Any())
                    throw VISite.Alerting.ThrowError("Table have 0 rows. Please Specify RowNames or GetRowNamesFunc");
                if (!HaveRowNames)
                    _rowNames = GetNumList(_rowNames.Count);
                return _rowNames;
            }
        }
        
        public int StartColumnIndex = 1;
        public int StartRowIndex = 1;

        public TableElementIndexType IndexType = TableElementIndexType.Nums;
        public TableHeadingType HeadingsType = TableHeadingType.ColumnsOnly;
        
        public Func<T> CellTemplate = () => (T)Activator.CreateInstance(typeof(T));
        private By _cellLocatorTemplate;

        private T CreateCell()
        {
            var cell = CellTemplate();
            cell.Context = new Pairs<ContextType, By>(ContextType.Locator, Locator, Context);
            return cell;
        }
        
        private string GetColNameByIndex(int index)
        {
            return HaveColumnNames 
                ? ColumnNames[index - 1] 
                : index.ToString();
        }

        private string GetRowNameByIndex(int index)
        {
            return HaveRowNames
                ? RowNames[index - 1]
                : index.ToString();
        }
        
        public T Cell(int colNum, int rowNum)
        {
            return Cell(GetColNameByIndex(colNum), GetRowNameByIndex(rowNum));
        }

        public T Cell(string colName, string rowName)
        {
            var colIndex = GetColumnIndex(colName);
            var rowIndex = GetRowIndex(rowName);
            if (_allCells == null)
                _allCells = new Dictionary<string, Dictionary<string, T>> { { colName, new Dictionary<string, T> { { rowName, CreateCell(colIndex, rowIndex) } } } };
            else 
                if (!_allCells.ContainsKey(colName))
                    _allCells.Add(colName, new Dictionary<string, T> { { rowName, CreateCell(colIndex, rowIndex) } });
                else if (!_allCells[colName].ContainsKey(rowName))
                    _allCells[colName].Add(rowName, CreateCell(colIndex, rowIndex));
            return _allCells[colName][rowName];
        }

        private string GetColumnIndex(string name)
        {
            int nameIndex;
            if (ColumnNames != null && ColumnNames.Contains(name))
                nameIndex = ColumnNames.IndexOf(name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Column: '" + name + "'. " + ((ColumnNames == null) ? "ColumnNames is Null" : ("Available ColumnNames: " + ColumnNames.Print(format: "'{0}'") + ")")));
            return (nameIndex + StartColumnIndex).ToString();
        }

        private string GetRowIndex(string name)
        {
            int nameIndex;
            if (RowNames != null && RowNames.Contains(name))
                nameIndex = RowNames.IndexOf(name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Row: '" + name + "'. " + ((RowNames == null) ? "RowNames is Null" : ("Available RowNames: " + RowNames.Print(format:"'{0}'") + ")")));
            return (nameIndex + StartRowIndex).ToString();
        }
        
        private List<string> GetNumList(int count, int from = 1)
        {
            var result = new List<string>();
            for (int i = from; i < count + from; i++)
                result.Add(i.ToString());
            return result;
        }

        public string Value { get {
            return "||X|" + ColumnNames.Print("|") + "||".LineBreak() + RowNames
                .Select(rowName => "||" + rowName + "||" + AllElements.Select(col => ((IHaveValue)col.Value[rowName]).Value).ToList().Print("|") + "||").ToList().Print("".LineBreak());
        } }

        public void SetValue<T>(T value) {}

        private T CreateCell(string col, string row)
        {
            var cell = CreateCell();

            if (_cellLocatorTemplate == null)
                _cellLocatorTemplate = (cell.HaveLocator())
                    ? cell.Locator
                    : By.XPath(".//tr[{1}]/td[{0}]");
            if (!cell.HaveLocator())
                cell.Locator = (_cellLocatorTemplate ?? By.XPath(".//tr[{1}]/td[{0}]")).FillByTemplate(col, row);
            cell.InitSubElements();
            return (T)cell.GetVIElement();
        }

        public Table() { }
        public Table(string name = null, By tableLocator = null, By cellLocatorTemplate = null) : base(name)
        {
            Locator = tableLocator;
            _cellLocatorTemplate = cellLocatorTemplate;
        }

    }
}
