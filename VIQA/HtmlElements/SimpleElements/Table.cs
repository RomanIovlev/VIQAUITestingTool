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
            for (int rowIndex = 1; rowIndex <= RowCount; rowIndex++)
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
            for (int colIndex = 1; colIndex <= ColCount; colIndex++)
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

        private int? _colCount;
        public int ColCount
        {
            set { _colCount = value; }
            get { return _colCount ?? (int)(_colCount = ColumnNames.Length); }
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
                ColCount = _columnNames.Length;
                if (!HaveColumnNames)
                    _columnNames = GetNumList(ColCount);
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

        private int? _rowCount;
        public int RowCount { 
            set { _rowCount = value; }
            get { return _rowCount ?? (int)(_rowCount = RowNames.Length); }
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
                RowCount = _rowNames.Length;
                if (!HaveRowNames)
                    _rowNames = GetNumList(RowCount);
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
        
        public Cell<T> Cell(int colNum, int rowNum)
        {
            var colIndex = colNum + StartColumnIndex - 1;
            var rowIndex = rowNum + StartRowIndex - 1;
            return CreateCell(colIndex, rowIndex, colNum, rowNum);
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

        private  bool GetCellFromValue(int colIndex, int rowIndex, string value, out Cell<T> cell)
        {
            cell = Cell(colIndex, rowIndex);
            return cell.Value == value;
        }

        public Cell<T> FindFirstCellWithValue(string value)
        {
            Cell<T> cell;
            for (int colIndex = 1; colIndex <= ColCount; colIndex++)
                for (int rowIndex = 1; rowIndex <= RowCount; rowIndex++)
                    if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                        return cell;
            return null;
        }

        public Cell<T> FindCellInColumn(int colIndex, string value)
        {
            Cell<T> cell;
            for (int rowIndex = 1; rowIndex <= RowCount; rowIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInColumn(int colIndex, Regex regex)
        {
            return GetColumn(colIndex).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInColumn(string colName, string value)
        {
            Cell<T> cell;
            var colIndex = Array.IndexOf(ColumnNames, colName) + 1;
            for (int rowIndex = 1; rowIndex <= RowCount; rowIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInColumn(string colname, Regex regex)
        {
            return GetColumn(colname).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInRow(int rowIndex, string value)
        {
            Cell<T> cell;
            for (int colIndex = 1; colIndex <= ColCount; colIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInRow(int rowIndex, Regex regex)
        {
            return GetRow(rowIndex).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInRow(string rowName, string value)
        {
            Cell<T> cell;
            var rowIndex = Array.IndexOf(RowNames, rowName) + 1;
            for (int colIndex = 1; colIndex <= ColCount; colIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInRow(string rowName, Regex regex)
        {
            return GetRow(rowName).Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public List<Cell<T>> FindColumnByRowValue(int rowIndex, string value)
        {
            var columnCell = FindCellInRow(rowIndex, value);
            return columnCell != null ? GetColumn(columnCell.X) : null;
        }

        public List<Cell<T>> FindColumnByRowValue(string rowName, string value)
        {
            var columnCell = FindCellInRow(rowName, value);
            return columnCell != null ? GetColumn(columnCell.X) : null;
        }

        public List<Cell<T>> FindRowByColumnValue(int colIndex, string value)
        {
            var rowCell = FindCellInColumn(colIndex, value);
            return rowCell != null ? GetRow(rowCell.Y) : null;
        }

        public List<Cell<T>> FindRowByColumnValue(string colName, string value)
        {
            var rowCell = FindCellInColumn(colName, value);
            return rowCell != null ? GetRow(rowCell.Y) : null;
        }

        private int GetColumnIndex(string name)
        {
            int nameIndex;
            if (ColumnNames != null && ColumnNames.Contains(name))
                nameIndex = Array.IndexOf(ColumnNames, name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Column: '" + name + "'. " + ((ColumnNames == null) ? "ColumnNames is Null" : ("Available ColumnNames: " + ColumnNames.Print(format: "'{0}'") + ")")));
            return nameIndex + StartColumnIndex;
        }

        private int GetRowIndex(string name)
        {
            int nameIndex;
            if (RowNames != null && RowNames.Contains(name))
                nameIndex = Array.IndexOf(RowNames,name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Row: '" + name + "'. " + ((RowNames == null) ? "RowNames is Null" : ("Available RowNames: " + RowNames.Print(format:"'{0}'") + ")")));
            return nameIndex + StartRowIndex;
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


        private Cell<T> CreateCell(int colIndex, int rowIndex, string colName, string rowName)
        {
            return CreateCell(colIndex, rowIndex, Array.IndexOf(ColumnNames, colName) + 1, Array.IndexOf(RowNames, rowName) + 1, colName, rowName);
        }

        private Cell<T> CreateCell(int colIndex, int rowIndex, int colNum, int rowNum, string colName = "", string rowName = "")
        {
            var cell = CreateCell();

            if (_cellLocatorTemplate == null)
                _cellLocatorTemplate = (cell.HaveLocator())
                    ? cell.Locator
                    : By.XPath(".//tr[{1}]/td[{0}]");
            if (!cell.HaveLocator())
                cell.Locator = (_cellLocatorTemplate ?? By.XPath(".//tr[{1}]/td[{0}]")).FillByTemplate(colIndex, rowIndex);
            cell.InitSubElements();
            return new Cell<T>(cell.GetVIElement(), colNum, rowNum, colName, rowName);
        }

        public Table() { }
        public Table(string name = null, By tableLocator = null, By cellLocatorTemplate = null) : base(name)
        {
            Locator = tableLocator;
            _cellLocatorTemplate = cellLocatorTemplate;
        }

    }

}
