using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

    public class Table<T> : VIElement, ITable<T> where T : VIElementsSet, IHaveValue
    {
        protected override string _typeName { get { return "Table"; } }

        private Dictionary<string, Dictionary<string, Cell<T>>> _allCells;
        public Dictionary<string, Dictionary<string, Cell<T>>> Cells
        {
            get
            {
                ColumnNames.ToDictionary(columnName => columnName, columnName =>
                        RowNames.ToDictionary(rowName => rowName, rowName =>
                            Cell(columnName, rowName)));
                return _allCells;
            }
        }

        public List<Cell<T>> GetColumn(int colIndex)
        {
            var result = new List<Cell<T>>();
            for (int rowIndex = 1; rowIndex <= RowNames.Length; rowIndex++)
                result.Add(Cell(colIndex, rowIndex));
            return result.ToList();
        }

        public List<Cell<T>> GetColumn(string colName)
        {
            return RowNames.Select(rowName => Cell(colName, rowName)).ToList();
        }

        public List<Cell<T>> GetRow(int rowIndex)
        {
            var result = new List<Cell<T>>();
            for (int colIndex = 1; colIndex <= ColumnNames.Length; colIndex++)
                result.Add(Cell(colIndex, rowIndex));
            return result.ToList();
        }
        public List<Cell<T>> GetRow(string rowName)
        {
            return ColumnNames.Select(colName => Cell(colName, rowName)).ToList();
        }

		private Func<IWebElement, string[]> _getColumnNamesFunc;
        public Func<IWebElement, string[]> GetColumnNamesFunc
        {
            set { _getColumnNamesFunc = value; }
            get { return _getColumnNamesFunc ??
                (table => table.FindElements(By.XPath(".//th")).Select(el => el.Text).ToArray());
            }
        }

        private string[] _columnNames;
        public string[] ColumnNames
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
                    _columnNames = GetNumList(_columnNames.Length);
                return _columnNames;
            }
        }
        
        private bool HaveColumnNames { get { return new[] { TableHeadingType.ColumnsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType); } }
        private bool HaveRowNames { get { return new[] { TableHeadingType.RowsOnly, TableHeadingType.RowsAndColumns }.Contains(HeadingsType); } }

        private Func<IWebElement, string[]> _getRowNamesFunc;
        public Func<IWebElement, string[]> GetRowNamesFunc
        {
            set { _getRowNamesFunc = value; }
            get
            {
                return _getRowNamesFunc ??
                    (table => table.FindElements(By.XPath(".//tr/td[1]")).Select(el => el.Text).ToArray());
            }
        }
        
        private string[] _rowNames;
        public string[] RowNames
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
                    _rowNames = GetNumList(_rowNames.Length);
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
        
        public Cell<T> Cell(int colNum, int rowNum)
        {
            return Cell(GetColNameByIndex(colNum), GetRowNameByIndex(rowNum));
        }

        public Cell<T> Cell(string colName, string rowName)
        {
            var colIndex = GetColumnIndex(colName);
            var rowIndex = GetRowIndex(rowName);
            if (_allCells == null)
                _allCells = new Dictionary<string, Dictionary<string, Cell<T>>> { { colName, new Dictionary<string, Cell<T>> { { rowName, CreateCell(colIndex, rowIndex, colName, rowName) } } } };
            else 
                if (!_allCells.ContainsKey(colName))
                    _allCells.Add(colName, new Dictionary<string, Cell<T>> { { rowName, CreateCell(colIndex, rowIndex, colName, rowName) } });
                else if (!_allCells[colName].ContainsKey(rowName))
                    _allCells[colName].Add(rowName, CreateCell(colIndex, rowIndex, colName, rowName));
            return _allCells[colName][rowName];
        }
        
        public List<Cell<T>> FindCellsWithValue(string value)
        {
            return FindCellsWithValue(new Regex("^" + value + "$"));
        }

        public List<Cell<T>> FindCellsWithValue(Regex regex)
        {
            return Cells.SelectMany(col => col.Value.Where(row => regex.IsMatch(row.Value.Value)).Select(row => row.Value)).ToList();
        }

        public Cell<T> FindFirstCellWithValue(string value)
        {
            return (from column in Cells from row in column.Value.Where(row => row.Value.Value == value) 
                    select row.Value).FirstOrDefault();
        }

        public Cell<T> FindCellInColumn(int colIndex, string value)
        {
            return GetColumn(colIndex).First(cell => cell.Value == value);
        }

        public List<Cell<T>> FindCellsInColumn(int colIndex, Regex regex)
        {
            return GetColumn(colIndex).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInColumn(string colname, string value)
        {
            return GetColumn(colname).First(cell => cell.Value == value);
        }

        public List<Cell<T>> FindCellsInColumn(string colname, Regex regex)
        {
            return GetColumn(colname).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInRow(int rowIndex, string value)
        {
            return GetRow(rowIndex).First(cell => cell.Value == value);
        }

        public List<Cell<T>> FindCellsInRow(int rowIndex, Regex regex)
        {
            return GetRow(rowIndex).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInRow(string rowName, string value)
        {
            return GetRow(rowName).First(cell => cell.Value == value);
        }

        public List<Cell<T>> FindCellsInRow(string rowName, Regex regex)
        {
            return GetRow(rowName).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public List<Cell<T>> FindColumnByRowValue(int rowIndex, string value)
        {
            var columnCell = GetRow(rowIndex).FirstOrDefault(cell => cell.Value == value);
            return columnCell != null ? GetColumn(columnCell.ColumnName) : null;
        }

        public List<Cell<T>> FindColumnByRowValue(string rowName, string value)
        {
            var columnCell = GetRow(rowName).FirstOrDefault(cell => cell.Value == value);
            return columnCell != null ? GetColumn(columnCell.ColumnName) : null;
        }

        public List<Cell<T>> FindRowByColumnValue(int colIndex, string value)
        {
            var rowCell = GetColumn(colIndex).FirstOrDefault(cell => cell.Value == value);
            return rowCell != null ? GetRow(rowCell.RowName) : null;
        }

        public List<Cell<T>> FindRowByColumnValue(string colName, string value)
        {
            var rowCell = GetColumn(colName).FirstOrDefault(cell => cell.Value == value);
            return rowCell != null ? GetRow(rowCell.RowName) : null;
        }

        private string GetColumnIndex(string name)
        {
            int nameIndex;
            if (ColumnNames != null && ColumnNames.Contains(name))
                nameIndex = Array.IndexOf(ColumnNames, name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Column: '" + name + "'. " + ((ColumnNames == null) ? "ColumnNames is Null" : ("Available ColumnNames: " + ColumnNames.Print(format: "'{0}'") + ")")));
            return (nameIndex + StartColumnIndex).ToString();
        }

        private string GetRowIndex(string name)
        {
            int nameIndex;
            if (RowNames != null && RowNames.Contains(name))
                nameIndex = Array.IndexOf(RowNames,name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Row: '" + name + "'. " + ((RowNames == null) ? "RowNames is Null" : ("Available RowNames: " + RowNames.Print(format:"'{0}'") + ")")));
            return (nameIndex + StartRowIndex).ToString();
        }
        
        private string[] GetNumList(int count, int from = 1)
        {
            var result = new List<string>();
            for (int i = from; i < count + from; i++)
                result.Add(i.ToString());
            return result.ToArray();
        }

        public string Value { get {
            return "||X|" + ColumnNames.Print("|") + "||".LineBreak() + RowNames
                .Select(rowName => "||" + rowName + "||" + _allCells.Select(col => col.Value[rowName].Value).ToList().Print("|") + "||").ToList().Print("".LineBreak());
        } }

        public void SetValue<T1>(T1 value) { }

        private Cell<T> CreateCell(string colIndex, string rowIndex, string colName, string rowName)
        {
            var cell = CreateCell();

            if (_cellLocatorTemplate == null)
                _cellLocatorTemplate = (cell.HaveLocator())
                    ? cell.Locator
                    : By.XPath(".//tr[{1}]/td[{0}]");
            if (!cell.HaveLocator())
                cell.Locator = (_cellLocatorTemplate ?? By.XPath(".//tr[{1}]/td[{0}]")).FillByTemplate(colIndex, rowIndex);
            cell.InitSubElements();
            return new Cell<T>(cell.GetVIElement(), Array.IndexOf(ColumnNames, colName) + 1, Array.IndexOf(RowNames, rowName) + 1, colName, rowName);
        }

        public Table() { }
        public Table(string name = null, By tableLocator = null, By cellLocatorTemplate = null) : base(name)
        {
            Locator = tableLocator;
            _cellLocatorTemplate = cellLocatorTemplate;
        }

    }

}
