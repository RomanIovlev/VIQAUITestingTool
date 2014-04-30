namespace VIQA.HtmlElements.Interfaces
{
    public interface ITextArea : ILabeled, IHaveValue
    {
        void Input(string text);
        void NewInput(string text);
        void Clear();
    }
}
