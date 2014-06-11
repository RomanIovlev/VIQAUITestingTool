namespace VIQA.HtmlElements.Interfaces
{
    public interface IClickable : IVIElement
    {
        void Click();
        string ClickOpensPage { get; set; }
    }
}
