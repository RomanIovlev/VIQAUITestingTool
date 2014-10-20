namespace VIQA.HtmlElements.Interfaces
{
    public interface ILink : IClickable, IText
    {
        string Reference { get; }
    }
}
