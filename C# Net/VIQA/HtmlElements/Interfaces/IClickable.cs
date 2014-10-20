namespace VIQA.HtmlElements.Interfaces
{
    public interface IClickable : IVIElement
    {
        void Click();
        string ClickLoadsPage { get; set; }
    }
}
