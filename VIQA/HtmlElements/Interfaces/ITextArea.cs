namespace VIQA.HtmlElements.Interfaces
{
    public interface ITextArea : ILabeled, ISetValue
    {
        void Input(string text);
        void NewInput(string text);
        void Clear();
    }
}
