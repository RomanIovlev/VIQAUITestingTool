using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements.SimpleElements
{
    public class Cell<T> : VIElement, IHaveValue where T : IHaveValue
    {
        public readonly T Element;
        public string Value { get { return Element.Value; } }
        public int ColumnNum;
        public int RowNum;
        public string ColumnName;
        public string RowName;

        public void SetValue<T1>(T1 value) { Element.SetValue(value); }

        public Cell(T element, int columnNum, int rowNum, string colName, string rowName)
        {
            Element = element;
            ColumnNum = columnNum;
            RowNum = rowNum;
            ColumnName = colName;
            RowName = rowName;
        }

        public Cell<T> UpdateData(string colName, string rowName)
        {
            if (string.IsNullOrEmpty(ColumnName) && !string.IsNullOrEmpty(colName))
                ColumnName = colName;
            if (string.IsNullOrEmpty(RowName) && !string.IsNullOrEmpty(rowName))
                RowName = rowName;
            return this;
        }
    }
}
