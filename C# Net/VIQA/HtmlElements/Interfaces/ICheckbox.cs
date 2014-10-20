namespace VIQA.HtmlElements.Interfaces
{
    public interface ICheckbox : IClickable, IText, IHaveValue
    {
        void Check();
        void Uncheck();
        bool IsChecked();
    }
}
