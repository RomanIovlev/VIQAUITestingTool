namespace VIQA.HtmlElements.Interfaces
{
    public interface ICheckbox : IClickable, ILabeled, IHaveValue
    {
        void Check();
        void Uncheck();
        bool IsChecked();
    }
}
