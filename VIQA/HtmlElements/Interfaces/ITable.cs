using System.Collections.Generic;
using System.Text.RegularExpressions;
using VIQA.Common;
using VIQA.HtmlElements.SimpleElements;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ITable<T> : IHaveValue where T : VIElementsSet, IHaveValue
    {
        Cell<T> Cell(int colNum, int rowNum);
        Cell<T> Cell(string colName, int rowNum);
        Cell<T> Cell(int colNum, string rowName);
        Cell<T> Cell(string colName, string rowName);
        
        List<Cell<T>> FindCellsWithValue(string value);
        List<Cell<T>> FindCellsWithValue(Regex regex);
        Cell<T> FindFirstCellWithValue(string value);
        Cell<T> FindCellInColumn(int colIndex, string value);
        List<Cell<T>> FindCellsInColumn(int colIndex, Regex regex);
        Cell<T> FindCellInColumn(string colName, string value);
        List<Cell<T>> FindCellsInColumn(string colname, Regex regex);
        Cell<T> FindCellInRow(int rowIndex, string value);
        List<Cell<T>> FindCellsInRow(int rowIndex, Regex regex);
        Cell<T> FindCellInRow(string rowName, string value);
        List<Cell<T>> FindCellsInRow(string rowName, Regex regex);
        List<Cell<T>> FindColumnByRowValue(int rowIndex, string value);
        List<Cell<T>> FindColumnByRowValue(string rowName, string value);
        List<Cell<T>> FindRowByColumnValue(int colIndex, string value);
        List<Cell<T>> FindRowByColumnValue(string colName, string value);

        Columns<T> Columns { get; }
        Rows<T> Rows { get; }
        string[] Footer { get; set; }
    }

    public interface ITable : ITable<TextElement> { }
}
