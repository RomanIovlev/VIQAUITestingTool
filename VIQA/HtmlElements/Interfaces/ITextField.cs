namespace VIQA.HtmlElements.Interfaces
{
    public interface ITextField : ILabeled, IHaveValue
    {
        void Input(string text);
        void NewInput(string text);
        void Clear();
    }
}
