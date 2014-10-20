using System.Collections.Generic;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ICheckList : ISelector<Checkbox>
    {
        void CheckGroup(params string[] values);
        void UncheckGroup(params string[] values);
        void CheckOnly(params string[] values);
        void UncheckOnly(params string[] values);
        List<string> GetListOfChecked();
        List<string> SelectedItems { get; }
    }
}
