using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements.SimpleElements
{
    public class Cell<T> : VIElement, IHaveValue where T : IHaveValue
    {
        public readonly T Element;
        public string Value { get { return Element.Value; } }
        public readonly int X;
        public readonly int Y;
        public readonly string ColumnName;
        public readonly string RowName;

        public void SetValue<T1>(T1 value) { Element.SetValue(value); }

        public Cell(T element, int x, int y, string colName, string rowName)
        {
            Element = element;
            X = x;
            Y = y;
            ColumnName = colName;
            RowName = rowName;
        }
    }
}
