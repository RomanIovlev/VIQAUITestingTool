namespace VIQA.HtmlElements.Interfaces
{
    public interface IClickable : IVIElement
    {
        void Click();
        bool WithPageLoadAction { get; set; }
    }
}
