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
    public class Table : Table<TextElement>, ITable
    {
        public Table() { }

        public Table(By tableLocator) : base(tableLocator) { }

        public Table(string name = null, By tableLocator = null, By cellLocatorTemplate = null) : base(name, tableLocator, cellLocatorTemplate) { }
    }
    
    public class Table<T> : VIElement, ITable<T> where T : VIElementsSet, IHaveValue
    {
        protected override string _typeName { get { return "Table"; } }

        private List<Cell<T>> _allCells = new List<Cell<T>>();
        public List<Cell<T>> Cells
        {
            get { return _allCells = Columns.Headers.SelectMany(columnName => Rows.Headers.Select(rowName => Cell(columnName, rowName))).ToList(); }
        }

        private readonly Columns<T> _columns = new Columns<T>();
        public Columns<T> Columns { get { return _columns; } set { _columns.Update(value); } }
        private readonly Rows<T> _rows = new Rows<T>();
        public Rows<T> Rows { get { return _rows; } set { _rows.Update(value); } }

        public string[] ColumnHeaders { set { Columns.Headers = value; } }
        public string[] RowHeaders { set { Rows.Headers = value; } }
        public int ColCount { set { Columns.Count = value; } }
        public int RowCount { set { Rows.Count = value; } }

        public Func<IWebElement, string[]> GetColumnHeadersFunc { set { Columns.GetHeadersFunc = value; } }
        public Func<IWebElement, string[]> GetRowHeadersFunc { set { Rows.GetHeadersFunc = value; } }

        public Func<IWebElement, string[]> GetFooterFunc;
        protected string[] _footer;
        public string[] Footer
        {
            set { _footer = value; }
            get
            {
                if (_footer != null)
                    return _footer;
                _footer = DoVIActionResult("Get Footer", () => GetFooterFunc(GetWebElement()));
                if (_footer == null || !_footer.Any())
                    return default(string[]);
                Columns.Count = _footer.Length;
                return _footer;
            }
        }
        
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
            var colIndex = colNum + Columns.StartIndex - 1;
            var rowIndex = rowNum + Rows.StartIndex - 1;
            return AddCell(colIndex, rowIndex, colNum, rowNum, "", "");
        }

        public Cell<T> Cell(string colName, int rowNum)
        {
            var colIndex = GetColumnIndex(colName);
            var rowIndex = rowNum + Rows.StartIndex - 1;
            return AddCell(colIndex, rowIndex, Array.IndexOf(Columns.Headers, colName) + 1, rowNum, colName, "");
        }

        public Cell<T> Cell(int colNum, string rowName)
        {
            var colIndex = colNum + Columns.StartIndex - 1;
            var rowIndex = GetRowIndex(rowName);
            return AddCell(colIndex, rowIndex, colNum, Array.IndexOf(Rows.Headers, rowName) + 1, "", rowName);
        }

        public Cell<T> Cell(string colName, string rowName)
        {
            var colIndex = GetColumnIndex(colName);
            var rowIndex = GetRowIndex(rowName);
            return AddCell(colIndex, rowIndex, Array.IndexOf(Columns.Headers, colName) + 1, Array.IndexOf(Rows.Headers, rowName) + 1, colName, rowName);
        }

        private Cell<T> AddCell(int colIndex, int rowIndex, int colNum, int rowNum, string colName, string rowName)
        {
            if (!_allCells.Any(cell => cell.ColumnNum == colNum && cell.RowNum == rowNum))
            {
                var cell = CreateCell(colIndex, rowIndex, colNum, rowNum, colName, rowName);
                _allCells.Add(cell);
                return cell;
            }
            return _allCells.First(cell => cell.ColumnNum == colNum && cell.RowNum == rowNum).UpdateData(colName, rowName);
        }

        public List<Cell<T>> FindCellsWithValue(string value)
        {
            return FindCellsWithValue(new Regex("^" + value + "$"));
        }

        public List<Cell<T>> FindCellsWithValue(Regex regex)
        {
            return Cells.Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        private  bool GetCellFromValue(int colIndex, int rowIndex, string value, out Cell<T> cell)
        {
            cell = Cell(colIndex, rowIndex);
            return cell.Value == value;
        }

        public Cell<T> FindFirstCellWithValue(string value)
        {
            Cell<T> cell;
            for (int colIndex = 1; colIndex <= Columns.Count; colIndex++)
                for (int rowIndex = 1; rowIndex <= Rows.Count; rowIndex++)
                    if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                        return cell;
            return null;
        }

        public Cell<T> FindCellInColumn(int colIndex, string value)
        {
            Cell<T> cell;
            for (int rowIndex = 1; rowIndex <= Rows.Count; rowIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInColumn(int colIndex, Regex regex)
        {
            return Columns[colIndex].Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInColumn(string colName, string value)
        {
            Cell<T> cell;
            var colIndex = Array.IndexOf(Columns.Headers, colName) + 1;
            for (int rowIndex = 1; rowIndex <= Rows.Count; rowIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInColumn(string colname, Regex regex)
        {
            return Columns[colname].Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInRow(int rowIndex, string value)
        {
            Cell<T> cell;
            for (int colIndex = 1; colIndex <= Columns.Count; colIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInRow(int rowIndex, Regex regex)
        {
            return Rows[rowIndex].Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public Cell<T> FindCellInRow(string rowName, string value)
        {
            Cell<T> cell;
            var rowIndex = Array.IndexOf(Rows.Headers, rowName) + 1;
            for (int colIndex = 1; colIndex <= Columns.Count; colIndex++)
                if (GetCellFromValue(colIndex, rowIndex, value, out cell))
                    return cell;
            return null;
        }

        public List<Cell<T>> FindCellsInRow(string rowName, Regex regex)
        {
            return Rows[rowName].Where(cell => regex.IsMatch(cell.Value)).ToList();
        }

        public List<Cell<T>> FindColumnByRowValue(int rowIndex, string value)
        {
            var columnCell = FindCellInRow(rowIndex, value);
            return columnCell != null ? Columns[columnCell.ColumnNum] : null;
        }

        public List<Cell<T>> FindColumnByRowValue(string rowName, string value)
        {
            var columnCell = FindCellInRow(rowName, value);
            return columnCell != null ? Columns[columnCell.ColumnNum] : null;
        }

        public List<Cell<T>> FindRowByColumnValue(int colIndex, string value)
        {
            var rowCell = FindCellInColumn(colIndex, value);
            return rowCell != null ? Rows[rowCell.RowNum] : null;
        }

        public List<Cell<T>> FindRowByColumnValue(string colName, string value)
        {
            var rowCell = FindCellInColumn(colName, value);
            return rowCell != null ? Rows[rowCell.RowNum] : null;
        }

        private int GetColumnIndex(string name)
        {
            int nameIndex;
            if (Columns.Headers != null && Columns.Headers.Contains(name))
                nameIndex = Array.IndexOf(Columns.Headers, name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Column: '" + name + "'. " + ((Columns.Headers == null) ? "ColumnHeaders is Null" : ("Available ColumnHeaders: " + Columns.Headers.Print(format: "'{0}'") + ")")));
            return nameIndex + Columns.StartIndex;
        }

        private int GetRowIndex(string name)
        {
            int nameIndex;
            if (Rows.Headers != null && Rows.Headers.Contains(name))
                nameIndex = Array.IndexOf(Rows.Headers, name);
            else
                throw VISite.Alerting.ThrowError("Can't Get Row: '" + name + "'. " + ((Rows.Headers == null) ? "RowHeaders is Null" : ("Available RowHeaders: " + Rows.Headers.Print(format: "'{0}'") + ")")));
            return nameIndex + Rows.StartIndex;
        }
        
        public string Value { get {
            return "||X|" + Columns.Headers.Print("|") + "||".LineBreak() + Rows.Headers
                .Select(rowName => "||" + rowName + "||" + Cells.Where(cell => cell.RowName == rowName).Select(cell => cell.Value).ToList().Print("|") + "||").ToList().Print("".LineBreak());
        } }

        public void SetValue<T1>(T1 value) { }


        private Cell<T> CreateCell(int colIndex, int rowIndex, string colName, string rowName)
        {
            return CreateCell(colIndex, rowIndex, Array.IndexOf(Columns.Headers, colName) + 1, Array.IndexOf(Rows.Headers, rowName) + 1, colName, rowName);
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

        public Table()
        {
            Columns.Table = this;
            Rows.Table = this;
            GetFooterFunc = t => t.FindElements(By.XPath(".//tfoot/tr/td")).Select(el => el.Text).ToArray();
        }

        public Table(By tableLocator) : this()
        {
            Locator = tableLocator;
        }

        public Table(string name = null, By tableLocator = null, By cellLocatorTemplate = null) : this()
        {
            Name = name;
            Locator = tableLocator;
            _cellLocatorTemplate = cellLocatorTemplate;
        }

    }

}
