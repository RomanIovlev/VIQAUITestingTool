using System.Collections.Generic;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ISelector : IHaveValue
    {
        List<string> GetListOfValues();
        void Select(string valueName);
        List<string> IsSelected();
    }
}
