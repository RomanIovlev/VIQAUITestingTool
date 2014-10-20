
namespace VIQA.Common.Interfaces
{
    public interface ILogger
    {
        void Event(string msg);
        void Error(string errorMsg);
    }
}
