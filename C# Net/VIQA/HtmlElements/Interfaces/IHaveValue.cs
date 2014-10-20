using System;

namespace VIQA.HtmlElements.Interfaces
{
    public interface IHaveValue : IVIElement
    {
        void SetValue<T>(T value);
        Func<Object, Object> FillRule { set; get; }
        string Value { get; }
    }
}
