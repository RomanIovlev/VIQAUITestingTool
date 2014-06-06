
using System.Collections.Generic;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ITable<T> : IHaveValue
    {
        Dictionary<string, Dictionary<string, T>> AllElements { get; }
        Dictionary<string, Dictionary<string, T>> GetAllElements();
        T GetVIElementXY(int colNum, int rowNum);
        T GetVIElementXY(string colName, string rowName);
        List<string> ColumnNames { get; }
        List<string> RowNames { get; }
    }

    public interface ITable : ITable<TextElement> { }
}
