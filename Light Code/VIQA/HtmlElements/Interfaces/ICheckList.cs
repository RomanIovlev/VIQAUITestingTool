using System.Collections.Generic;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ICheckList : ISelector
    {
        void CheckGroup(params string[] values);
        void UncheckGroup(params string[] values);
        void CheckOnly(params string[] values);
        void UncheckOnly(params string[] values);
        List<string> GetListOfChecked();
    }
}
