namespace VIQA.Common
{
    public class DefaultWebDriverTimeouts : IWebDriverTimeouts
    {
        public int WaitWebElementInSec { get { return 5; } }
        public int WaitPageToLoadInSec { get { return 20; } }
        public int RetryActionInMsec { get { return 100; } }
    }
}
