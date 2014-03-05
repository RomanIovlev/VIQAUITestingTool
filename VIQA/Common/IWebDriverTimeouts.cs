namespace VIQA.Common
{
    public interface IWebDriverTimeouts
    {
        int WaitWebElementInSec { get; }
        int WaitPageToLoadInSec { get; }
        int RetryActionInMsec { get; }
    }
}
