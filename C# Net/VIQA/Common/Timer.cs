using System;
using System.Diagnostics;
using System.Threading;

namespace VIQA.Common
{
    public class Timer
    {
        private const double DefaultTimeout = 10000;
        private const int DefaultRetryTimeout = 100;

        private readonly Stopwatch _watch = new Stopwatch();
        private readonly double _timeoutInMSec;
        private readonly int _retryTimeoutInMSec = DefaultRetryTimeout;

        public Timer() { _watch = Stopwatch.StartNew(); }

        public Timer(double timoutInMsec = DefaultTimeout, int retryTimeoutInMSec = DefaultRetryTimeout) : this()
        { 
            _timeoutInMSec = timoutInMsec;
            _retryTimeoutInMSec = retryTimeoutInMSec;
        }
        
        public TimeSpan TimePassed()
        {
            return _watch.Elapsed;
        }

        public bool TimeoutPassed()
        {
            return _watch.Elapsed > TimeSpan.FromMilliseconds(_timeoutInMSec);
        }

        public bool Wait(Func<bool> waitFunc)
        {
            while (!TimeoutPassed())
            {
                if (TryGetResult(waitFunc))
                    return true;
                Thread.Sleep(_retryTimeoutInMSec);
            }
            return false;
        }

        private static bool TryGetResult(Func<bool> waitFunc)
        {
            try { return waitFunc(); }
            catch { return false; }
        }
    }
}
