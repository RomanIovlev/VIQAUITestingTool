namespace VIQA.HtmlElements.Interfaces
{
    public interface ICheckbox : IClickable, ILabeled, ISetValue
    {
        void Check();
        void Uncheck();
        bool IsChecked();
    }
}
