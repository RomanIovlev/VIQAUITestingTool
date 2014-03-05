using VIQA.Common.Interfaces;

namespace VIQA.HtmlElements.Interfaces
{
    public interface ISetValue : IVIElement
    {
        void SetValue<T>(T value);
        IAlerting Alerting { get; }
    }
}
