namespace VIQA.HtmlElements.Interfaces
{
    public interface ILink : IClickable, ILabeled
    {
        string Reference { get; }
    }
}
