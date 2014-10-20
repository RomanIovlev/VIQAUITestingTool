using System.Collections.Generic;
using VIQA.HtmlElements.BaseClasses;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ISelector<T> : IHaveValue, IVIList<T>
    {
        List<string> GetListOfValues();
        void Select(string valueName);
        bool IsSelected(string valueName);
    }

    public interface ISelector : ISelector<SelectItem> { }
}
