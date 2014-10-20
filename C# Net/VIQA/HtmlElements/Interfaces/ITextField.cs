namespace VIQA.HtmlElements.Interfaces
{
    public interface ITextField : IText, IHaveValue
    {
        void Input(string text);
        void NewInput(string text);
        void Focus();
        void Clear();
    }
}
