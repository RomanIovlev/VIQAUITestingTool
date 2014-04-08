using System;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ISetValue : IVIElement
    {
        void SetValue<T>(T value);
        Func<Object, Object> FillRule { set; get; }
    }
}
